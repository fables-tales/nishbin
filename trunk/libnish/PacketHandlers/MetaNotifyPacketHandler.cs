
using System;
using System.Collections.Generic;

namespace libnish
{
	
	
	public class MetaNotifyPacketHandler:PacketHandler
	{
		public List<string> uuids;
		
		public MetaNotifyPacketHandler()
		{
			this.uuids = new List<string>();
		}
		public override void Handle (Packet p)
		{
			if (p is MetaNotifyPacket){
				MetaNotifyPacket convert = (MetaNotifyPacket)p;
				lock(this.uuids){
					this.uuids.Add(convert.ContainingUUID);
				}
			} else{
				throw new ArgumentException("wrong type of packet passed");
			}
		}

	}
}
