using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace libnish.PeerFinders
{
    // TODO: UPnP.
    // down thar, to trygetpeer()
    /// <summary>
    /// This one is a little weird, it finds peers, by connecting peers that are already connected
    /// </summary>
    public class TcpListenerPeerFinder : PeerFinder
    {
        TcpListener tListener;
        bool listenerStarted = false;

        public TcpListenerPeerFinder(string ListenAddress, int ListenPort, bool StartNow)
            : this(new IPEndPoint(IPAddress.Parse(ListenAddress), ListenPort), StartNow)
        {
        }

        public TcpListenerPeerFinder(IPEndPoint ListenEndPoint, bool StartNow)
        {
            tListener = new TcpListener(ListenEndPoint);

            if (StartNow)
                Start();
        }

        public void Start()
        {
            listenerStarted = true;
            tListener.Start();
        }

        public void Stop()
        {
            listenerStarted = false;
            tListener.Stop();
        }

        public override bool TryGetPeer(out List<PotentialPeer> PeersList)
        {
            if (!listenerStarted || !tListener.Pending())
            {
                PeersList = null;
                return false;
            }
            
            PeersList = new List<PotentialPeer>();

            while (tListener.Pending())
            {
                try
                {
                	TcpClient tclient;
                	
                    tclient = tListener.AcceptTcpClient();
                    
                    // Ok, we successfully accepted the TcpClient.
				    // Time to fill in the appropriate boxes.
				    PotentialPeer pp = new PotentialPeer();
				    IPEndPoint iep = (IPEndPoint)(tclient.Client.RemoteEndPoint);

				    pp.IP = iep.Address.ToString();
				    pp.Port = iep.Port;
				    pp.SetAlreadyEstablishedConnection(tclient);
				    
				    PeersList.Add(pp);
                }
                catch (Exception e)
                {
                    NetEvents.Add(NetEventType.PeerConnectFail, "Failed to accept incoming TCP connection in TcpListenerPeerFinder.\nException was:\n" + e.ToString(), new object[] { e }, this);
                    PeersList = null;
                    return false;
                }
            }
            return true;
        }
    }
}
