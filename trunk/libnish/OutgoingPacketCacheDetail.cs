
using System;

namespace libnish
{
	
	
	public class OutgoingPacketCacheDetail
	{
		private Packet p;
		private DateTime sent;
		public Packet P{
			get{
				return this.p;
			}
		}
		public DateTime Sent{
			get{
				return this.sent;
			}
		}
		
		public OutgoingPacketCacheDetail(Packet p){
			this.p = p;
			this.sent = DateTime.Now;
		}
	}
}
