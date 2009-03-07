using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using libnish;
using System.Net.Sockets;
using libnish.Crypto;

namespace TestPacketSender
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Test packet sender.");
			Console.WriteLine("-----");

			Console.WriteLine();

			Console.Write("Remote ip, port in [ip]:[port] format: ");
			string ipAndPort = Console.ReadLine();
			string IP = ipAndPort.Split(':')[0];
			int Port = int.Parse(ipAndPort.Split(':')[1]);

			Console.WriteLine("Hit enter to establish connection.");
			Console.ReadLine();


			Console.WriteLine("Establishing...");

			TcpClient tcOutgoing = new TcpClient(IP, Port);

			Console.WriteLine("Connected! Handshaking...");

			Peer p = new Peer(tcOutgoing, IP, Port, Limits.Default, true);

			string uu = UUID.getUUID();
			Console.WriteLine("uu == '" + uu + "'");

			MetaNotifyPacket mnp = new MetaNotifyPacket(uu);

			Console.WriteLine("Sending packet...");

			p.Send(mnp);

			Console.WriteLine("Coolio. We're out.");
			Console.ReadLine();
		}
	}
}
