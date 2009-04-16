using System;
using System.Collections.Generic;

using System.Text;
using System.Net.Sockets;
using System.IO;
using libnish.Crypto;
using System.Threading;

using Mono.Security.Cryptography;
using Mono.Math;
using System.Security.Cryptography;

namespace libnish
{
    public class Peer : EncryptedConnection
    {
        //public float Karma = 1f;

        public Peer(TcpClient LiveConnection, string IPAddress, int Port, Limits Limits, bool Outward)
            : base(LiveConnection, IPAddress, Port, Limits, Outward)
        {
            // todo: simplify constructor...?
            // oh god yes


        }

        public void Send(Packet p)
        {
            EncryptAndSend(p.ToUnencryptedByteArray());
        }

        Queue<byte> RecvBuffer = new Queue<byte>();
        bool MsgAvailable = false;

        public bool PacketAvailable
        {
            get
            {
                lock (RecvBuffer)
                {
                    return MsgAvailable;
                }
            }
        }

        public Packet TryGetPacket()
        {
            lock (RecvBuffer)
            {
                Packet p;

                Poll();

                if (PacketAvailable)
                {
                    List<byte> packbuff = new List<byte>();

                    while (true)
                    {
                        byte b = RecvBuffer.Dequeue();

                        if (b == (byte)'\n')
                        {
                            while (RecvBuffer.Count > 0 && RecvBuffer.Peek() == (byte)'\n')
                                RecvBuffer.Dequeue();

                            break;
                        }
                        else
                            packbuff.Add(b);
                    }

                    p = Packet.FromUnencryptedByteArray(packbuff.ToArray());

                    return p;
                }
                else
                    return null;

            }
        }

        public override void Poll()
        {
            lock (RecvBuffer)
            {
                if (Available > 0)
                {
                    // FIXME: might take us too long to react to being pumped with loads of junk data > limit setting. (not til next if statement!)
                    // buffer a load of data, but only parse it in 4096 byte chunks, or bigger, but something like that
                    byte[] somebytes = ReceiveAndDecrypt(Available);
                    for (int i = 0; i < somebytes.Length; i++)
					{
						
                        RecvBuffer.Enqueue(somebytes[i]);
						
					}
                }

                if (RecvBuffer.Contains((byte)'\n'))
                    MsgAvailable = true;
                else
                {
                    // we failin' a limit here?
                    if (RecvBuffer.Count > this.Limits.PushPacketSizeHardLimit)
                        throw new InvalidDataException("PushPacketSizeHardLimit exceeded.  No valid push packets found within the limit.");
                }
            }
        }



    }
}
