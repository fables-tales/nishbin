
using System;

namespace libnish
{
	
	
	public abstract class PacketHandler
	{
		protected DataManager d;
		public PacketHandler(DataManager d){
			this.d = d;
		}
		public abstract void Handle(Packet p);
	}
}
