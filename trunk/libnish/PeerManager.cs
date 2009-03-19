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
        public int ConnectMorePeers(int ParallelAttempts)
        {
            List<PotentialPeer> Found = new List<PotentialPeer>();

            lock (PeerFinders)
            {
                foreach (PeerFinder pf in PeerFinders)
                {
                    PotentialPeer pp;

                    while (Found.Count < ParallelAttempts)
                    {
                        if (pf.TryGetPeer(out pp))
                            Found.Add(pp);
                        else
                            break;
                    }
                }
            }
            
            int SuccessfulAttempts = 0;

            foreach (PotentialPeer fpp in Found)
            {
                if (CheckPotentialPeerOK(fpp))
                {
                    ThreadPool.QueueUserWorkItem(delegate(object o)
                        {
                            ConnectPotentialPeer(fpp);
                        });
                    
                    SuccessfulAttempts++;
                }
                else
                    NetEvents.Add(NetEventType.BannedPeer, "Peer '" + fpp.IP + ":" + fpp.Port.ToString() + "' failed validation for some reason. Not connecting.", new object[] { fpp }, this);
            }

            if (SuccessfulAttempts == 0)
                NetEvents.Add(NetEventType.FoundPeerFailure, "No PeerFinders found any suitable peers that passed validation. (Requested " + ParallelAttempts.ToString() + " peers)", new object[] { 0, ParallelAttempts }, this);
            else
                NetEvents.Add(NetEventType.FoundPeers, "PeerFinders found us " + SuccessfulAttempts.ToString() + " suitable peers that passed validation, out of " + ParallelAttempts.ToString() + " requested peers.", new object[] { SuccessfulAttempts, ParallelAttempts }, this);

            return SuccessfulAttempts;
        }

        private bool CheckPotentialPeerOK(PotentialPeer pp)
        {
            // TODO: check not already connected to this ip ('cept for debugging purposes or some shite), etc, etc.
            return true;
        }

        private void ConnectPotentialPeer(PotentialPeer pp)
        {
            Peer p;
            
            pp.TryConnect(out p);

            Peers.Add(p);
        }
    }
}
