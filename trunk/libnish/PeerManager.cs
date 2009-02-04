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
    }
}
