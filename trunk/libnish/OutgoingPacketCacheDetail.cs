
using System;

namespace libnish
{
	
	
	public class OutgoingPacketCacheDetail
	{
		public Packet P;
		public DateTime Sent;
		public OutgoingPacketCacheDetail(Packet p){
			this.P = p;
			this.Sent = DateTime.Now;
		}
	}
}
