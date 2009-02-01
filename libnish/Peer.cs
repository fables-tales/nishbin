using System;
using System.Collections.Generic;

using System.Text;
using System.Net.Sockets;
using System.Net;

namespace libnish
{
    public class Peer
    {
        // TODO use this bleh blah woof
        public float Karma = 1f;

        private TcpClient tclient;

        public Peer(TcpClient Connection)
        {
            // bleat
            // (TODO: implement phipper protocol)
            Connection.GetStream().Write(System.Text.Encoding.ASCII.GetBytes("behhh"), 0, 5);
            tclient = Connection;
        }

        public bool Alive
        {
            get
            {
                // TODO: Does this even work?!
                return ((tclient != null) && tclient.Connected && (tclient.Client != null) && tclient.Client.Connected);
            }
        }

        public void SendPacket(Packet Packet)
        {
            byte[] xbuf = Packet.Content;

            // TODO: Encryption hook goes here!

            tclient.GetStream().Write(xbuf, 0, xbuf.Length);
            tclient.GetStream().WriteByte((byte)'\n');
        }

        // TODO: More efficient buffering method?? (that said, it worked on the
        //  android platform for huge bitmaps, eheh...)
        List<byte> Buffer = new List<byte>();

        public bool TryReceivePacket(out Packet ReceivedPacket)
        {
            int available = tclient.Available;

            if (available > 0)
            {
                byte[] xbuf = new byte[available];
                tclient.GetStream().Read(xbuf, 0, available);
                Buffer.AddRange(xbuf);
            }

            // TODO: decryption goes in here somewhere, too...

            int endPos = Buffer.IndexOf((byte)'\n');

            if (endPos < 0)
            {
                ReceivedPacket = null;
                return false;
            }
            else
            {
                byte[] xpacket = new byte[endPos];  // ...so not including the \n at the end

                Buffer.CopyTo(0, xpacket, 0, endPos);
                Buffer.RemoveRange(0, endPos + 1); // +1 so as to pop the \n, too.

                ReceivedPacket = Packet.FromUnencryptedByteArray(xpacket);
                return true;
            }
        }

        // TODO: IP, Port properties
        // TODO: Closedown/clean-up code. destructor, basically...

        /*public string IP
        {
            get
            {
                SocketAddress ipbits = tclient.Client.RemoteEndPoint.Serialize();

                
            }
        }*/
    }
}
