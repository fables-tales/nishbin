
using System;
using System.Collections.Generic;

namespace libnish
{
	
	
	public class MetaNotifyPacketHandler:PacketHandler
	{
		
		
		public MetaNotifyPacketHandler(DataManager d):base(d)
		{
			
		}
		public override void Handle (Packet p)
		{
			if (p is MetaNotifyPacket)
			{
				MetaNotifyPacket convert = (MetaNotifyPacket)p;
				lock(d){
					bool dontadd =false;
					foreach (MetaData data in d.AllMetaData){
						if (data.UUID == convert.ContainingUUID){
							dontadd = true;
							break;
						}
					}
					if (!dontadd){
						MetaData data = new MetaData(convert.ContainingUUID);
						d.AllMetaData.Add(data);
					}
				}
			} 
			else
			{
				throw new ArgumentException("wrong type of packet passed");
			}
		}

	}
}
