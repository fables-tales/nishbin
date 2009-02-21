using System;
using System.Collections.Generic;

using System.Text;
using System.Net.Sockets;
using System.IO;
using libnish.Crypto;
using System.Threading;

using Mono.Security.Cryptography;
using Mono.Math;

namespace libnish
{
    public class Peer
    {
        //public float Karma = 1f;

        string IP;
        int Port;

        TcpClient TcpClient;

        Limits Limits;

        bool OurEndNeedsToDoTheHandshake = false;
        bool HandsShaken = false;

        BinaryReader br;
        BinaryWriter bw;

        private Peer(TcpClient LiveConnection, string IPAddress, int Port, Limits Limits, bool Outward)
        {
            TcpClient = LiveConnection;
            IP = IPAddress;
            this.Port = Port;
            this.Limits = Limits;
            OurEndNeedsToDoTheHandshake = Outward;

            br = new BinaryReader(TcpClient.GetStream());
            bw = new BinaryWriter(TcpClient.GetStream());
        }

        private void SendRawAndAwaitResponse(byte[] RawData, byte NumBytesResponseRequired, int TimeoutMs)
        {
            bw.Write(RawData);
            
            DateTime startTime = DateTime.UtcNow;

            while (TcpClient.Available < NumBytesResponseRequired)
            {
                Thread.Sleep(10); // checking 100x a sec ought to be more than fast enough
                                  // if you're some bizarro person from the future who needs to do this faster, you can change this bloody const yourself

                if ((DateTime.UtcNow - startTime).TotalMilliseconds >= TimeoutMs)
                    throw new TimeoutException("SendAndAwaitResponse: Peer failed to respond in the given timeout period. Sorry 'bout that...");
            }
        }

        private void Handshake()
        {
            // If already started/done handshake because remote peer initiated handshake, DO NOT error here!!
            // Need to just fail silently.

            // Now, what to do if both handshake at the same time is something completely different... :<

            if (HandsShaken)
                return;

            /*
	         * The actual dh protocol:
	         * person a: generate g and p, send to person b
	         * person b: accept g and p and respond with an acknowledgement (usually a hash of a + b)
	         * person a: generate a, compute k1 = (g^a) mod p
	         * person b: generate b, compute k2 = (g^b) mod p
	         * preson a: send k1 to person b
	         * person b: send k2 to person a
	         * person a: compute key = (k2^a) mod p
	         * person b: compute key = (k1^b) mod p */

            dh dh = new dh();

            switch (OurEndNeedsToDoTheHandshake)
            {
                case true: // Person A!
                    // person a: generate g and p, send to person b
                    dh.generateGP();
                    bw.Write(dh.g.GetBytes());
                    bw.Write(dh.p.GetBytes());
                    
                    break;

                case false: // Person B!


                    break;
            }
        }


    }
}
