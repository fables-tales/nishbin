﻿using System;
using System.Collections.Generic;

using System.Text;
using libnish.PeerFinders;
using System.Threading;

namespace libnish
{
    /// <summary>
    /// this is the main dave for managing all peers in the system, but only on the push network 
    /// </summary>    
    /// <remarks>
    /// peermanager is a dave, that is because he is slightly sentient
    /// this dave ought to be pretty thread safe, most of the variables are locked when you call the methods
    /// </remarks>
    public class PeerManager
    {
        List<Peer> Peers = new List<Peer>();
        List<PeerFinder> PeerFinders = new List<PeerFinder>();
		
		//these badboys are going to replace PacketQueue
		List<OutgoingPacketCacheDetail> OutgoingPacketCache = new List<OutgoingPacketCacheDetail>();
		List<IncomingPacketDetail> IncomingPacketCache = new List<IncomingPacketDetail>();
		Queue<IncomingPacketDetail> IncomingPacketQueue = new Queue<IncomingPacketDetail>();
		
		DateTime LastTryGetMorePeersTime = DateTime.MinValue;
		
        Thread P2PThread;
        Thread PacketProcessingThread;

        int TargetPeerCount;
        int MaxPeerCount;
		//MetaNotifyPacketHandler MNPHandler = new MetaNotifyPacketHandler();
        int BigConnectingCounter = 0;

        bool P2PThreadRun = false;
        bool PacketThreadRun = false;
        
        //why is this not a list of peers?
		List<string> BlacklistedPeers = new List<string>();

        Queue<Packet> PacketQueue = new Queue<Packet>();

        Limits Limits;
        
        public PeerManager(Peer[] InitialPeers, PeerFinder[] InitialPeerFinders, Limits Limits, int TargetPeerNum, int MaxPeerNum)
        {
            Peers.AddRange(InitialPeers);
            PeerFinders.AddRange(InitialPeerFinders);

            TargetPeerCount = TargetPeerNum;
            MaxPeerCount = MaxPeerNum;

            this.Limits = Limits;

            P2PThread = new Thread(new ThreadStart(PeerThreadProc));
            P2PThreadRun = true;
            P2PThread.Start();

            PacketProcessingThread = new Thread(new ThreadStart(PPProc));
            PacketThreadRun = true;
            PacketProcessingThread.Start();
        }
		private void RemoveExpiredPacketsFromIncomingPacketCache(){
			lock(IncomingPacketCache){
				foreach (IncomingPacketDetail da in this.IncomingPacketCache){
					if (da.Time <= DateTime.Now){
						this.IncomingPacketCache.Remove(da);
					}
				}
			}
		}
		
		private void RemoveExpiredPacketsFromOutgoingPacketCache(){
			lock(OutgoingPacketCache){
				foreach (OutgoingPacketCacheDetail cd in this.OutgoingPacketCache){
					if (cd.Expire <= DateTime.Now){
						this.OutgoingPacketCache.Remove(cd);
					}
				}
			}
		}
        private void PPProc()
        {
            // todo write this
            // shouldn't this stuff only print stuff to console if debugging flags are enabled?
			// also, shouldn't we push packets we recieve to everyone?
			// and lock the packetqueue?
			// and lock peers
            while (PacketThreadRun)
            {
                this.RemoveExpiredPacketsFromIncomingPacketCache();
				this.RemoveExpiredPacketsFromOutgoingPacketCache();	
				lock (this.Peers){
					lock (this.IncomingPacketQueue){
						if (this.IncomingPacketQueue.Count > 0){
							IncomingPacketDetail da = IncomingPacketQueue.Dequeue();
							Packet p = da.P;
                    		if (p is MetaNotifyPacket)
                    		{
								MetaNotifyPacket mnp = (MetaNotifyPacket)p;
								Console.WriteLine("got mnp");
								Console.WriteLine("uuid is " + mnp.ContainingUUID+"\n");
                    		} else {
								Console.ForegroundColor = ConsoleColor.Cyan;
                        		Console.WriteLine("Got packet of type '" + p.Type.ToString() + "'. Not doing anything rly.");
                        		Console.ForegroundColor = ConsoleColor.Gray;
							}
							
							foreach (Peer peer in this.Peers){
								if (peer != da.SendingPeer){
									peer.Send(p);
								}
							}
							lock(this.OutgoingPacketCache){
								this.OutgoingPacketCache.Add(new OutgoingPacketCacheDetail(da.P));
							}	
							
						}
					}					
				}
                Thread.Sleep(100);
            }
        }
        public void IsThisProgramSkyNetYet(){
            throw new libnish.Debug.SkynetException("This program has become skynet, please destroy it");
        }
        public void PushPacketToAll(Packet p)
        {
            lock (Peers)
            {
				//wat?
				foreach (Peer pier in Peers)
                    pier.Send(p);
            }
			lock (OutgoingPacketCache){
				OutgoingPacketCacheDetail cache = new OutgoingPacketCacheDetail(p);
				this.OutgoingPacketCache.Add(cache);
				
			}
        }
		public void AddIncomingPacket(Packet p,Peer sendingpeer){
			lock(IncomingPacketQueue){
				lock(IncomingPacketCache){
					bool dontadd=false;
					foreach (IncomingPacketDetail da in this.IncomingPacketCache)
					{
						if (da.P.ToUnencryptedByteArray() == p.ToUnencryptedByteArray())
						{
							dontadd = true;
							break;
						}
					}
					if (!dontadd)
					{
						IncomingPacketDetail da = new IncomingPacketDetail(p,sendingpeer);
						this.IncomingPacketQueue.Enqueue(da);
						this.IncomingPacketCache.Add(da);
					}
				}
			}
		}
		private void TryGetMorePeers(){
			if (DateTime.UtcNow < LastTryGetMorePeersTime)
                    LastTryGetMorePeersTime = DateTime.MinValue;
			if (BigConnectingCounter == 0 && ((DateTime.UtcNow - LastTryGetMorePeersTime).TotalMilliseconds > Limits.MsBetweenConnectionAttempts))
            {
                lock (Peers)
                    if (Peers.Count < TargetPeerCount)
                        this.ConnectMorePeers((TargetPeerCount - Peers.Count) + 2); // TODO: Expose as a setting...?

                LastTryGetMorePeersTime = DateTime.UtcNow;
            }

            Thread.Sleep(50);
		}
        private void PeerThreadProc()
        {
            while (P2PThreadRun)
            {
                this.TryGetMorePeers();
                lock (Peers)
                {
                    foreach (Peer p in Peers)
                    {
                        p.Poll();

                        // TODO: poll() needs a hard limit to stop 10000000000 small packets being processed & murdering the system
                        // this loop should probably have a timeout
                        // is it in a seperate thread from the main one?
                        while (p.PacketAvailable)
                        {
                            Packet pa;

                            pa = p.TryGetPacket();

                            if (pa != null){
								this.AddIncomingPacket(pa,p);
							}
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Attempts to find and begin connecting to (ParallelAttempts) peers.
        /// </summary>
        /// <param name="ParallelAttempts">Numbers of peers to connect to simultaneously.</param>
        /// <remarks>Remember that a large quantity of connection attempts can and will fail, due to outdated information and peers who have filled their
        /// personal connection quota.
        /// Also, the function does NOT return how many connections were actually successful.</remarks>
        /// <returns>Number of peer connection attempts that have successfully bebgun.</returns>
        public int ConnectMorePeers(int AdditionalPeerCount)
        {
            List<PotentialPeer> Found = new List<PotentialPeer>();

            lock (PeerFinders)
            {
                foreach (PeerFinder pf in PeerFinders)
                {
                    List<PotentialPeer> pp; // = new List<PotentialPeer>(); 
                    
                    // TODO: Rename TryGetPeer->TryGetPeers
                    if (pf.TryGetPeer(out pp))
                    	Found.AddRange(pp);
                    	
                    if (Found.Count >= AdditionalPeerCount)
                    	break;
                }
            }
            
            Found = FilterPotentialPeerList(Found);
            
            if (Found.Count > AdditionalPeerCount)
            	Found.RemoveRange(AdditionalPeerCount, Found.Count - AdditionalPeerCount);
            
            int SuccessfulAttempts = 0;

            foreach (PotentialPeer fpp in Found)
            {
                BigConnectingCounter++;
                ThreadPool.QueueUserWorkItem(delegate(object o)
                    {
                        try
                        {
                            ConnectPotentialPeer(fpp);
                        }
                        finally
                        {
                            BigConnectingCounter--;
                        }
                    });
                
                SuccessfulAttempts++;
            }

            if (SuccessfulAttempts == 0)
                NetEvents.Add(NetEventType.FoundPeerFailure, "No PeerFinders found any suitable peers that passed validation. (Requested " + AdditionalPeerCount.ToString() + " peers)", new object[] { 0, AdditionalPeerCount }, this);
            else
                NetEvents.Add(NetEventType.FoundPeers, "PeerFinders found us " + SuccessfulAttempts.ToString() + " suitable peers that passed validation, out of " + AdditionalPeerCount.ToString() + " requested peers.", new object[] { SuccessfulAttempts, AdditionalPeerCount }, this);

            return SuccessfulAttempts;
        }
        
        private List<PotentialPeer> FilterPotentialPeerList(List<PotentialPeer> InList)
        {
        	List<PotentialPeer> OutList = new List<PotentialPeer>();
        	
        	foreach (PotentialPeer pp in InList)
        	{
        		if (CheckPotentialPeerOK(pp))
        			OutList.Add(pp);
        		else
        			pp.CloseIfConnected();
        	}
        			
        	return OutList;
        }

        private bool CheckPotentialPeerOK(PotentialPeer pp)
        {
            // TODO: check not already connected to this ip ('cept for debugging purposes or some shite), etc, etc.
            
            // - Check if peer is already connected!
            // - Check if peer is blacklisted!
            lock (Peers)
            {
                foreach (Peer p in Peers){
                    // FIXME: Compare port, too.
                    // fixed??
                    if (p.RemoteIP == pp.IP && pp.Port == p.RemotePort){
                    	NetEvents.Add(NetEventType.PeerConnectFail, "Peer already connected.", new object[] { pp.IP, pp.Port }, this);
				        return false;
			        }
		        }
		        
            }
            
            // Next up: Fixing TLPF, DebugPeerFinder.
            // LEAD THE WAY MON AMI(s?)! no just one me. to TLPF
            if (IsPeerBlacklisted(pp))
            {
            	NetEvents.Add(NetEventType.BannedPeer, "This peer is blacklisted, not using.", new object[] { pp.IP, pp.Port }, this);
                return false;
            }
            
            return true;
        }

        public void BlacklistPeer(string IP)
        {
            lock (BlacklistedPeers)
            {
                BlacklistedPeers.Add(IP.Trim());
            }
        }

        public void UnBlacklistPeer(string IP)
        {
            lock (BlacklistedPeers)
            {
                BlacklistedPeers.Remove(IP.Trim());
            }
        }
        
        private bool IsPeerBlacklisted(PotentialPeer Details)
        {
            lock (BlacklistedPeers)
            {
                return (BlacklistedPeers.Contains(Details.IP.Trim()));
            }
        }

        private void ConnectPotentialPeer(PotentialPeer pp)
        {
            Peer p;
            if (pp.TryConnect(out p))
			{
				lock (Peers)
				{
                    if (Peers.Count < MaxPeerCount)
                    {
                        NetEvents.Add(NetEventType.NewPeer, "New peer '" + p.RemoteIP + ":" + p.RemotePort.ToString() + "' added.", new object[] { p.RemoteIP, p.RemotePort }, this);
                        Peers.Add(p);
						lock (OutgoingPacketCache){
							foreach (OutgoingPacketCacheDetail d in OutgoingPacketCache){
								p.Send(d.P);
							}
						}
                    }
                    else
                    {
                        NetEvents.Add(NetEventType.RejectedPeerBecauseFull, "Rejected peer '" + p.RemoteIP + ":" + p.RemotePort.ToString() + "' as we've already got more peers than you can shake a big thing at.", new object[] { p.RemoteIP, p.RemotePort }, this);
                        p.Close();
                    }
		       	}
           	}
        }
    }
}
