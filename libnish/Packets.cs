using System;
using System.Collections.Generic;

using System.Text;

namespace libnish
{
    public enum PacketType
    {
        PeerNotify,
        NewThread,
        NewPost,
        MetaNotify,
        RequestChunks
    }

    public abstract class Packet
    {
        internal byte[] Content;

        public abstract PacketType Type { get; }

        public byte[] ToUnencryptedByteArray()
        {
            return Content;
        }
    }

    public class MetaNotifyPacket : Packet
    {
        public string ContainingUUID
        {
            get
            {
                return System.Text.Encoding.ASCII.GetString(Content, 5, Content.Length - 5);
            }
            set
            {
                Content = System.Text.Encoding.ASCII.GetBytes("META " + value);
            }
        }

        public MetaNotifyPacket(string UUID)
        {
            ContainingUUID = UUID;
        }

        public override PacketType Type
        {
            get { return PacketType.MetaNotify; }
        }
    }
}
