// TrapDoorPackets.cs created with MonoDevelop
// User: sam at 15:01 28/03/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using System.IO;

namespace libnish
{
	
	
	public enum TrapDoorPackets
	{
		HavePacket,
		HerePacket,
		NopePacket,
		GrabPacket,
		RatePacket,
	}
	public abstract class TrapDoorPacket
    {
        internal byte[] Content;

        public abstract TrapDoorPackets Type { get; }

        public byte[] ToUnencryptedByteArray()
        {
            return Content;
        }

        public static TrapDoorPacket FromUnencryptedByteArray(byte[] Contents)
        {
            if (Contents.Length < 4)
                throw new Exception("Invalid packet.");

            switch (System.Text.Encoding.ASCII.GetString(Contents, 0, 4))
            {
			case "HAVE":
				return new HavePacket(Contents);
			case "HERE":
				return new HerePacket(Contents);
			default:
                    // FIXME: Proliferate, don't throw an exception!!
                    throw new Exception("Unrecognised packet type '" + System.Text.Encoding.ASCII.GetString(Contents, 0, 4) + "'.");
            }
        }
    }
	public class HavePacket : TrapDoorPacket{
		
		private string[] uuids;
		public override TrapDoorPackets Type {
			get { return TrapDoorPackets.HavePacket; }
		}

		public HavePacket(byte[] UnencryptedByteArray){
			this.Content = UnencryptedByteArray;
			string compareuuid = Crypto.UUID.getUUID();
			string thispacket = System.Text.Encoding.ASCII.GetString(this.Content).Substring(5);
			string[] uuids = thispacket.Split(' ');
			foreach (string uuid in uuids){
				if (!Crypto.UUID.verifyuuid(uuid)){
					for(int i=0;i<this.Content.Length;i++){
						this.Content[i] = Crypto.Math.getRandom(8).GetBytes()[0];
						
					}
					throw new InvalidDataException("A recieved uuid was made of fail");
				}
			}
			this.uuids = uuids;
			
		}
		public HavePacket(string[] UUIDS){
			List<byte> content = new List<byte>();
			content.AddRange(System.Text.Encoding.ASCII.GetBytes("HAVE"));
			foreach (string uuid in UUIDS){
				content.Add((byte)' ');
				if (Crypto.UUID.verifyuuid(uuid)){
					content.AddRange(System.Text.Encoding.ASCII.GetBytes(uuid));
				} else {
					throw new InvalidDataException("invalid uuid passed");
				}
			}
			this.Content = content.ToArray();
			
			
		}
		
	}
	public class HerePacket: TrapDoorPacket{
		public override TrapDoorPackets Type {
			get { return TrapDoorPackets.HerePacket; }
		}
		public byte[] chunk;
		public HerePacket(byte[] UnencryptedContent){
			
			string[] data = System.Text.Encoding.ASCII.GetString(UnencryptedContent).Substring(5).Split(' ');
			if (data.Length == 3){
				string uuid = data[0];
				if (Crypto.UUID.verifyuuid(uuid)){
					int length = int.Parse(data[1]);
					string sdata = data[2];
					byte[] binarydata = System.Text.Encoding.ASCII.GetBytes(sdata);
					this.chunk = binarydata;
					this.Content = UnencryptedContent;
				}
				else{
					throw new InvalidDataException("the uuid is not valid");
				}
			}
		}
		public HerePacket(string UUID,byte[] content){
			if (!(Crypto.UUID.verifyuuid(UUID))){
				throw new InvalidDataException("the uuid is not valid");
			}
			List<byte> bcontent = new List<byte>();
			bcontent.AddRange(System.Text.Encoding.ASCII.GetBytes("HERE "));
			bcontent.AddRange(System.Text.Encoding.ASCII.GetBytes(UUID + " "));
			bcontent.AddRange(System.Text.Encoding.ASCII.GetBytes(content.Length.ToString() + " "));
			bcontent.AddRange(content);
			this.Content = bcontent.ToArray();
			
		}
	}
	
}

