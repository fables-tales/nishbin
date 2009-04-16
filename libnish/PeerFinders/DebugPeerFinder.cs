using System;
using System.Collections.Generic;
using System.Text;

namespace libnish.PeerFinders
{
    /// <summary>
    /// Used when debugging to find peers, adds peers manually
    /// </summary>    
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
        
        public override bool TryGetPeer(out List<PotentialPeer> PeersList)
        {
            lock (PotentialPeerList)
            {
                if (PotentialPeerList.Count > 0)
                {
                	Console.WriteLine("DebugPeerFinder got request for peers -- returning " + PotentialPeerList.Count.ToString() + " peers.");
                	PeersList = new List<PotentialPeer>(PotentialPeerList.ToArray());
                	return true;
                }
                else
                {
                	Console.WriteLine("DebugPeerFinder: Got request for peer, but no peers are in the list!");
                	PeersList = null;
                	return false;
                }
        	}
		}

	}
}
