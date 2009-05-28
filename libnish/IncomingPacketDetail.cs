
using System;

namespace libnish
{
	
	
	public class IncomingPacketDetail
	{
		private Peer pier;
		private Packet p;
		private DateTime time;

		public Peer Pier {
			get {
				return this.pier;
			}
		}
		public Packet P{
			get{
				return this.p;
			}
		}
		public DateTime Time{
			get{
				return this.time;
			}
		}
		public IncomingPacketDetail(Packet p, Peer pier)
		{
			this.pier = pier;
			this.p = p;
			this.time = DateTime.Now;
			
		}
	}
}
