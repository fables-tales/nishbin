﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Security.Cryptography;
using libnish.Crypto;
using Mono.Math;
using System.Net.Sockets;

namespace libnish
{
    public abstract class EncryptedConnection
    {
        string IP;
        int Port;

        TcpClient TcpClient;

        protected Limits Limits;

        bool OurEndNeedsToDoTheHandshake = false;
        bool HandsShaken = false;

        BinaryReader br;
        BinaryWriter bw;

        BigInteger key = null;
        BigInteger iv = null;

        aes aes;

        public int Available
        {
            get
            {
                return TcpClient.Available;
            }
        }

        public bool Connected
        {
            get
            {
                return TcpClient.Connected;
            }
        }

        public abstract void Poll();

        protected EncryptedConnection(TcpClient LiveConnection, string IPAddress, int Port, Limits Limits, bool Outward)
        {
            TcpClient = LiveConnection;
            IP = IPAddress;
            this.Port = Port;
            this.Limits = Limits;
            OurEndNeedsToDoTheHandshake = Outward;

            br = new BinaryReader(TcpClient.GetStream());
            bw = new BinaryWriter(TcpClient.GetStream());

            // TODO: Ensure it doesn't timeout!!
            Handshake();

            
        }



        protected void AwaitResponse(byte NumBytesResponseRequired, int TimeoutMs)
        {
            DateTime startTime = DateTime.UtcNow;

            while (TcpClient.Available < NumBytesResponseRequired)
            {
                Thread.Sleep(10); // checking 100x a sec ought to be more than fast enough
                // if you're some bizarro person from the future who needs to do this faster, you can change this bloody const yourself

                if ((DateTime.UtcNow - startTime).TotalMilliseconds >= TimeoutMs)
                    throw new TimeoutException("SendAndAwaitResponse: Peer failed to respond in the given timeout period. Sorry 'bout that...");
            }
        }

        protected void EncryptAndSend(byte[] data)
        {
            bw.Write(aes.encrypt(data));
        }

        protected byte[] ReceiveAndDecrypt(int bytes)
        {
            return aes.decrypt(br.ReadBytes(bytes));
        }

        private void Handshake()
        {
            // If already started/done handshake because remote peer initiated handshake, DO NOT error here!!
            // Need to just fail silently.

            // Now, what to do if both handshake at the same time is something completely different... :<
            if (HandsShaken)
                return;
            else
                HandsShaken = true;

            // Get key. (first DH pass)
            Console.WriteLine("--FIRST PASS--");
            byte[] key = DoDH(false);
            // Get IV! (second DH pass)
            Console.WriteLine("--SECOND PASS--");
            byte[] iv = DoDH(true);

			if (key == null || iv == null)
				throw new Exception("Failed to build key or IV. The encrypted connection cannot be created.");

            Console.WriteLine("\n");
            Console.WriteLine("KEY:: " + new BigInteger(key));
            Console.WriteLine("IV:: " + new BigInteger(iv));

			aes = new aes(ComputeSHA256Hash(key), Convert32To16(new BigInteger(ComputeSHA256Hash(iv))));
        }

        private byte[] DoDH(bool IVNotKey)
        {
            /*
	         * The actual dh protocol:
	         * person a: generate g and p, send to person b
	         * person b: accept g and p and respond with an acknowledgement (usually a hash of a + b)
	         * person a: generate a, compute k1 = (g^a) mod p
	         * person b: generate b, compute k2 = (g^a) mod p
	         * person a: send k1 to person b
	         * person b: send k2 to person a
	         * person a: compute key = (k2^b) mod p
	         * person b: compute key = (k1^b) mod p */

            dh dh = new dh();

            byte[] hash;

            switch (OurEndNeedsToDoTheHandshake)
            {
                case true: // Person A!
                    // person a: generate g and p, send to person b
                    dh.generateGP();
			    Console.Out.WriteLine(dh.p);
				Console.Out.WriteLine(dh.g);
                    bw.Write(dh.g.GetBytes());
                    bw.Write(dh.p.GetBytes());
                    
                    // Compute the hash ourselves, to compare with the data we sure are gonna receive right about now
                    hash = ComputeSHA256Hash(dh.g.GetBytes(), dh.p.GetBytes());
                    byte[] bsHash = br.ReadBytes(32);

                    for (int i = 0; i < 32; i++)
                        if (bsHash[i] != hash[i])
                            throw new InvalidDataException("Person B failed to compute the hash. Computer error. Virus = very yes");

                    // person a: generate a, compute k1 = (g^a) mod p
                    dh.generateA();
                    dh.computeK1();

                    // person a: send k1 to person b
                    bw.Write(MakeIt32Bytes(dh.k1.GetBytes()));
                    // (get their k1, or 'k2' as we like to call it.)
                    dh.k2 = new BigInteger(br.ReadBytes(32));

                    // compute.
                    dh.computeKey();

					if (IVNotKey)
                        return Convert32To16(dh.key);
					else
						return dh.key.GetBytes();

                case false: // Person B!
                    // person b: accept g and p and respond with an acknowledgement (usually a hash of a + b)
                    dh.g = new BigInteger(br.ReadBytes(32));
                    dh.p = new BigInteger(br.ReadBytes(32));
				Console.Out.WriteLine(dh.p);
				Console.Out.WriteLine(dh.g);
                    hash = ComputeSHA256Hash(dh.g.GetBytes(), dh.p.GetBytes());
                    bw.Write(hash);

                    // person b: generate a, compute k2 (our k1) = (g^a) mod p
                    dh.generateA();
                    dh.computeK1();

                    // person b: send k2 (our k1) to person a
                    bw.Write(MakeIt32Bytes(dh.k1.GetBytes()));
                    dh.k2 = new BigInteger(br.ReadBytes(32));

                    // compute.
                    dh.computeKey();

					if (IVNotKey)
					{
                        return Convert32To16(dh.key);

                        // TODO: If you can read this, delete the below. keeping temporarily for safekeeping...
						/*byte[] kl = new byte[16];
						byte[] kh = new byte[16];

						kh = MakeIt16Bytes((dh.key >> 128).GetBytes());
						kl = MakeIt16Bytes((dh.key % (BigInteger)(1 << 128)).GetBytes());

						byte[] result = new byte[16];

						for (int i = 0;i<result.Length;i++)
						{
							result[i] = (byte) (kl[i] ^ kh[i]);
						}

						return result;*/
					}
					else
						return dh.key.GetBytes();
            }

            throw new NotSupportedException("FILE_NOT_FOUND");
        }

        private byte[] Convert32To16(BigInteger thirtytwo)
        {
            byte[] kl = new byte[16];
            byte[] kh = new byte[16];

            kh = MakeIt16Bytes((thirtytwo >> 128).GetBytes());
            kl = MakeIt16Bytes((thirtytwo % (BigInteger)(1 << 128)).GetBytes());

            byte[] result = new byte[16];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (byte)(kl[i] ^ kh[i]);
            }

            return result;
        }



        private byte[] MakeIt32Bytes(byte[] LessThan32Bytes)
        {
            //List<byte> output = new List<byte>();
            byte[] output = new byte[32];

            for (int i = 0; i < 32; i++)
                output[i] = 0;
            for (int i = 0; i < LessThan32Bytes.Length; i++)
                output[i + (32 - LessThan32Bytes.Length)] = LessThan32Bytes[i];

            return output;
        }

        private byte[] MakeIt16Bytes(byte[] LessThan16Bytes)
        {
            byte[] output = new byte[16];

            for (int i = 0; i < 16; i++)
                output[i] = 0;
            for (int i = 0; i < LessThan16Bytes.Length; i++)
                output[i + (16 - LessThan16Bytes.Length)] = LessThan16Bytes[i];

            return output;
        }

        private byte[] ComputeSHA256Hash(byte[] firstHalfOfInput, byte[] secondHalfOfInput)
        {
            List<byte> bl = new List<byte>();
            bl.AddRange(firstHalfOfInput);
            bl.AddRange(secondHalfOfInput);

            return ComputeSHA256Hash(bl.ToArray());
        }

        private byte[] ComputeSHA256Hash(byte[] input)
        {
            SHA256 sha = new SHA256Managed();
            byte[] hash = sha.ComputeHash(input);
            return hash;
        }
    }
}