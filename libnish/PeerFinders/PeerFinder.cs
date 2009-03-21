using System;
using System.Collections.Generic;

using System.Text;
using System.Net.Sockets;

namespace libnish.PeerFinders
{
    public abstract class PeerFinder
    {
        public abstract bool TryGetPeer(out List<PotentialPeer> PeersList);

    }
}
