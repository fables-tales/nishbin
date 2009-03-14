using System;
using System.Collections.Generic;
using System.Text;
using libnish;
using System.Net.Sockets;
using libnish.Crypto;
using System.Threading;

namespace TestPacketReceiver
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Test packet receiver.");
			Console.WriteLine("-----");

			Console.WriteLine();

			Console.Write("Listening on port 23132.");

			TcpListener tListener = new TcpListener(23132);
			tListener.Start();

			while (!tListener.Pending())
			{
				Console.WriteLine("Waiting.");
				Thread.Sleep(500);
			}

			TcpClient tcIncoming = tListener.AcceptTcpClient();

			Console.WriteLine("Got one! Handshaking...");

			Peer p = new Peer(tcIncoming, "DONTCARE", 0, Limits.Default, false);

			while (!p.PacketAvailable)
			{
				p.Poll();
				Thread.Sleep(500);
				Console.WriteLine("Waiting for a packet.");
			}

			MetaNotifyPacket pa = (MetaNotifyPacket)(p.TryGetPacket());

			Console.WriteLine("Got uuid == " + pa.ContainingUUID);

			Console.WriteLine("+++ THE END +++");
			Console.ReadLine();
		}
	}
}
