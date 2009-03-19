using System;
using System.Collections.Generic;

using System.Text;
using System.Net.Sockets;

namespace libnish.PeerFinders
{
    public abstract class PeerFinder
    {
        private List<string> BlacklistedPeers = new List<string>();

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

        // TODO: CRAP architecturing. So we don't check for repeat peers in peerfinder, but every peerfinder
        // DOES have an individual blacklist?! ugh. not thought through properly. will fix soonish probs...
        internal bool IsPeerBlacklisted(PotentialPeer Details)
        {
            lock (BlacklistedPeers)
            {
                return (BlacklistedPeers.Contains(Details.IP.Trim()));
            }
        }

        public abstract bool TryGetPeer(out PotentialPeer PeerDetails);

        public abstract bool ArePeersAvailable();
    }
}
