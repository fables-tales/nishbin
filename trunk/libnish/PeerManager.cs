using System;
using System.Collections.Generic;

using System.Text;
using libnish.PeerFinders;
using System.Threading;

namespace libnish
{
    public class PeerManager
    {
        List<Peer> Peers = new List<Peer>();
        List<PeerFinder> PeerFinders = new List<PeerFinder>();

        Thread P2PThread;

        int TargetPeerCount;
        int MaxPeerCount;
        
        List<string> BlacklistedPeers = new List<string>();
        

        public PeerManager(Peer[] InitialPeers, PeerFinder[] InitialPeerFinders, int TargetPeerNum, int MaxPeerNum)
        {
            Peers.AddRange(InitialPeers);
            PeerFinders.AddRange(InitialPeerFinders);

            TargetPeerCount = TargetPeerNum;
            MaxPeerCount = MaxPeerNum;
            
            //P2PThread = new Thread(new ThreadStart(

            //((voidThing)dh.getRandomNumber()).Invoke();
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
                ThreadPool.QueueUserWorkItem(delegate(object o)
                    {
                        ConnectPotentialPeer(fpp);
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
        		if (CheckPotentialPeerOK(pp))
        			OutList.Add(pp);
        			
        	return OutList;
        }

        private bool CheckPotentialPeerOK(PotentialPeer pp)
        {
            // TODO: check not already connected to this ip ('cept for debugging purposes or some shite), etc, etc.
            
            // - Check if peer is already connected!
            // - Check if peer is blacklisted!
            
            // :(
            // there we go.
            // there. reloaded so tab settings should now apply a-ok.  Much betterer.
            lock (Peers)
            {
                foreach (Peer p in Peers){
                    // FIXME: Compare port, too.
                    if (p.IPAddress == pp.IP){
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
            
            pp.TryConnect(out p);

            Peers.Add(p);
        }
    }
}
