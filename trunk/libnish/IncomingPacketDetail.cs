
using System;

namespace libnish
{
	
	
	public class IncomingPacketDetail
	{
		private Peer sendingpeer;
		private Packet p;
		private DateTime time;

		public Peer SendingPeer {
			get {
				return this.sendingpeer;
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
		public IncomingPacketDetail(Packet p, Peer sendingpeer)
		{
			this.sendingpeer = sendingpeer;
			this.p = p;
			this.time = DateTime.Now+p.IgnoreIfReReceivedWithin;
			
		}
	}
}
