using System;
using System.Collections.Generic;

using System.Text;
using System.Net.Sockets;

namespace libnish
{
    public class Peer
    {
        //public float Karma = 1f;

        string IP;
        int Port;

        TcpClient TcpClient;

        Limits Limits;

        public Peer(TcpClient LiveConnection, string InfoIPAddress, int InfoPort, Limits Limits)
        {
            TcpClient = LiveConnection;
            IP = InfoIPAddress;
            Port = InfoPort;
            this.Limits = Limits;
        }

        private void Send(byte[] RawData)
        {
            // TODOOo~~~
        }

        private void SendAndWait(byte[] RawData, int TimeoutMs)
        {
            // TODO.
        }

        public void Handshake()
        {
            // If already started/done handshake because remote peer initiated handshake, DO NOT error here!!
            // Need to just fail silently.

            // Now, what to do if both handshake at the same time is something completely different... :<

        }
    }
}
