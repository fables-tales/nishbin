using System;
using System.Collections.Generic;

using System.Text;

namespace libnish.PeerFinders
{
    public struct PotentialPeer
    {
        public string IP;
        public int Port;

        public override bool Equals(object obj)
        {
            if (obj.GetType() == this.GetType())
                if (((PotentialPeer)obj).IP == this.IP)
                    if (((PotentialPeer)obj).Port == this.Port)
                        return true;
            return false;
        }

        public override int GetHashCode()
        {
            // rly rly don't care
            return base.GetHashCode();
        }

        public static bool operator == (PotentialPeer a, PotentialPeer b)
        {
            return ((a.IP == b.IP) && (a.Port == b.Port));
        }

        public static bool operator != (PotentialPeer a, PotentialPeer b)
        {
            return ((a.IP != b.IP) || (a.Port != b.Port));
        }

        public bool TryConnect(out Peer Peer)
        {
			Peer = null;
			return true;
        }
    }

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

        internal bool IsPeerBlacklisted(PotentialPeer Details)
        {
            lock (BlacklistedPeers)
            {
                return (BlacklistedPeers.Contains(Details.IP.Trim()));
            }
        }

        public abstract bool TryGetPeer(out PotentialPeer PeerDetails);
    }
}
