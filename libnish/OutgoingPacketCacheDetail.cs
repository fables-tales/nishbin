
using System;

namespace libnish
{
	
	
	public class OutgoingPacketCacheDetail
	{
		private Packet p;
		private DateTime expire;
		public Packet P{
			get{
				return this.p;
			}
		}
		public DateTime Expire{
			get{
				return this.expire;
			}
		}
		
		public OutgoingPacketCacheDetail(Packet p){
			this.p = p;
			this.expire = DateTime.Now+p.KeepInOutGoingCacheFor;
		}
	}
}
