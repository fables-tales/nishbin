using System;
using System.Collections.Generic;

using System.Text;
using System.Net.Sockets;

namespace libnish.PeerFinders
{
    /// <summary>
    /// the base class for peerfinders
    /// </summary>    
    public abstract class PeerFinder
    {
        public abstract bool TryGetPeer(out List<PotentialPeer> PeersList);

    }
}
