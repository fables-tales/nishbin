using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Security;
using System.IO;

namespace libnish
{
    public class TrapdoorConnection : EncryptedConnection
    {
		
		
		public TrapdoorConnection(TcpClient LiveConnection, string IPAddress, int Port, Limits Limits, bool Outward)
            : base(LiveConnection, IPAddress, Port, Limits, Outward)
		{
		
			
		}
		public override void Poll ()
		{
			throw new NotImplementedException ();
		}

    }
}
