
using System;

namespace libnish
{
	
	
	public class PacketCacheDetail
	{
		public Packet P;
		public DateTime Sent;
		public PacketCacheDetail(Packet p){
			this.P = p;
			this.Sent = DateTime.Now;
		}
	}
}
