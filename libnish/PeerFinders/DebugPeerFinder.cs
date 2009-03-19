using System;
using System.Collections.Generic;
using System.Text;

namespace libnish.PeerFinders
{
    public class DebugPeerFinder : PeerFinder
    {
        private List<PotentialPeer> PotentialPeerList = new List<PotentialPeer>();

        public void AddPotentialPeer(PotentialPeer pp)
        {
            lock (PotentialPeerList)
                PotentialPeerList.Add(pp);
        }

        public void AddPotentialPeer(string ip, int port)
        {
            PotentialPeer pp = new PotentialPeer();
            pp.IP = ip;
            pp.Port = port;
            AddPotentialPeer(pp);
        }

        public override bool TryGetPeer(out PotentialPeer PeerDetails)
        {
            lock (PotentialPeerList)
            {
                if (PotentialPeerList.Count > 0)
                {
                    int i = new Random().Next(PotentialPeerList.Count);
                    PeerDetails = PotentialPeerList[i];
                    Console.WriteLine("DebugPeerFinder: Got request for peer. Returning peer #" + i.ToString() + ", '" + PeerDetails.IP + ":" + PeerDetails.Port.ToString() + "'.");

                    if (IsPeerBlacklisted(PeerDetails))
                    {
                        ConsoleColor oldColour = Console.ForegroundColor;
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("WARNING. Peer '" + PeerDetails.IP + ":" + PeerDetails.Port.ToString() + "' has been blacklisted, but the DebugPeerFinder is too dumb to care. Ignoring.");
                        Console.ForegroundColor = oldColour;
                    }

                    NetEvents.Add(NetEventType.FoundPeers, "DebugPeerFinder found peer '" + PeerDetails.IP + ":" + PeerDetails.Port.ToString() + "'.", new object[] { PeerDetails.IP, PeerDetails.Port }, this);
                    return true;
                }
                else
                {
                    Console.WriteLine("DebugPeerFinder: Got request for peer, but no peers are in the list!");
                    NetEvents.Add(NetEventType.FoundPeerFailure, "DebugPeerFinder has no peers in it.", null, this);
                    PeerDetails = null;
                    return false;
                }
            }
        }

        public override bool ArePeersAvailable()
        {
            lock (PotentialPeerList)
            {
                if (PotentialPeerList.Count == 0)
                {
                    Console.WriteLine("DebugPeerFinder: Asked if any peers are available, and none are.");
                    return false;
                }
                else
                    return true;
            }
        }
    }
}
