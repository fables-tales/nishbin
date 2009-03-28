using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using libnish;
using libnish.PeerFinders;
using libnish.Crypto;

namespace TestPacketeer
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListenerPeerFinder tlpf = new TcpListenerPeerFinder(new System.Net.IPEndPoint(System.Net.IPAddress.Any, 9001), true);
            DebugPeerFinder dpf = new DebugPeerFinder();

            PeerManager pm = new PeerManager(new Peer[] { }, new PeerFinder[] { tlpf, dpf }, Limits.Default, 10, 10);

            while (true)
            {
                System.Console.Write(":: ");
                string cmd = Console.ReadLine();

                switch (cmd)
                {
                    case "d.a":
                        Console.WriteLine("What address? (Format: ip:port)");
                        string iap = Console.ReadLine();
                        dpf.AddPotentialPeer(iap.Split(':')[0], int.Parse(iap.Split(':')[1]));
                        break;
                    case "m":
                        MetaNotifyPacket mnp = new MetaNotifyPacket(UUID.getUUID());
                        Console.WriteLine("Hurr! Pushing UUID '" + mnp.ContainingUUID + "' inside a MetaNotifyPacket. :)");
                        pm.PushPacketToAll(mnp);
                        break;
                    case "?":
                        Console.WriteLine("Valid commands: d.a (to add to debugpeerfinder), ");
                        break;
                    default:
                        Console.WriteLine("Not a valid command. Use ? to see such things.");
                        break;
                }
            }
        }
    }
}
