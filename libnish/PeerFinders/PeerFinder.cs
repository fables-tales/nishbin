using System;
using System.Collections.Generic;

using System.Text;

namespace libnish.PeerFinders
{
    public struct PotentialPeer
    {
        public string IP;
        public int Port;

        public bool TryConnect(out Peer Peer)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class PeerFinder
    {
        private List<Peer> PotentialPeers;

        
    }
}
