﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace libnish.PeerFinders
{
    // TODO: UPnP.

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

        public override bool TryGetPeer(out PotentialPeer PeerDetails)
        {
            TcpClient tclient;

            if (!listenerStarted)
            {
                PeerDetails = null;
                return false;
            }

            if (tListener.Pending())
            {
                try
                {
                    tclient = tListener.AcceptTcpClient();
                }
                catch (Exception e)
                {
                    NetEvents.Add(NetEventType.PeerConnectFail, "Failed to accept incoming TCP connection in TcpListenerPeerFinder.\nException was:\n" + e.ToString(), new object[] { e }, this);
                    PeerDetails = null;
                    return false;
                }
            }
            else
            {
                PeerDetails = null;
                return false;
            }

            // Ok, we successfully accepted the TcpClient.
            // Time to fill in the appropriate boxes.
            PotentialPeer pp = new PotentialPeer();
            IPEndPoint iep = (IPEndPoint)(tclient.Client.RemoteEndPoint);

            pp.IP = iep.Address.ToString();
            pp.Port = iep.Port;
            pp.SetAlreadyEstablishedConnection(tclient);

            // TODO: Some way to check if blacklisted without actually having to accept the connection first?!
            if (IsPeerBlacklisted(pp))
            {
                try
                {
                    tclient.Client.Close();
                    tclient.Close();
                }
                finally
                {
                    NetEvents.Add(NetEventType.PeerConnectFail, "Failed to accept incoming TCP connection in TcpListenerPeerFinder, as the peer has been blacklisted.", new object[] { pp.IP, pp.Port }, this);
                    PeerDetails = null;
                }
                return false;
            }

            PeerDetails = pp;
            return true;
        }

        public override bool ArePeersAvailable()
        {
            if (!listenerStarted)
                return false;

            return tListener.Pending();
        }
    }
}
