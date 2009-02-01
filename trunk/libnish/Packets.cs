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

        public Packet FromUnencryptedByteArray(byte[] Contents)
        {
            if (Contents.Length < 4)
                throw new Exception("Invalid packet.");

            switch (System.Text.Encoding.ASCII.GetString(Contents, 0, 4))
            {
                case "PEER":
                    return new PeerNotifyPacket(Contents);
                    
                case "THRD":
                    return new NewThreadPacket(Contents);

                case "POST":
                    return new NewPostPacket(Contents);

                case "META":
                    return new MetaNotifyPacket(Contents);

                default:
                    throw new Exception("Unrecognised packet type '" + System.Text.Encoding.ASCII.GetString(Contents, 0, 4) + "'.");
            }
        }
    }

    public class MetaNotifyPacket : Packet
    {
        // TODO: Verify set UUIDs are valid?
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

        public MetaNotifyPacket(byte[] UnencryptedContent)
        {
            this.Content = UnencryptedContent;
        }

        public override PacketType Type
        {
            get { return PacketType.MetaNotify; }
        }
    }

    public class PeerNotifyPacket : Packet
    {
        /// <summary>
        /// Builds a new PeerNotifyPacket, using the provided peer addresses.
        /// </summary>
        /// <param name="Addresses">Array of peer addresses to be included. Each address should be in the format IP:PORT. (e.g. 127.0.0.1:1336)</param>
        public PeerNotifyPacket(string[] Addresses)
        {
            PeerAddresses = Addresses;
        }

        public PeerNotifyPacket(byte[] UnencryptedContent)
        {
            this.Content = UnencryptedContent;
        }

        public string[] PeerAddresses
        {
            get
            {
                // TODO: More efficient method? (as in, is a more efficient method necessary, or are we good...?)
                string AllAddresses = System.Text.Encoding.ASCII.GetString(Content, 5, Content.Length - 5);
                return AllAddresses.Split(' ');
            }
            set
            {
                // TODO: Validate addresses.
                Content = System.Text.Encoding.ASCII.GetBytes("PEER " + string.Join(" ", value));
            }
        }

        public override PacketType Type
        {
            get { return PacketType.PeerNotify; }
        }
    }

    public class NewThreadPacket : Packet
    {
        string CachedUUID;
        string CachedFirstMessage;

        public NewThreadPacket(byte[] UnencryptedContent)
        {
            this.Content = UnencryptedContent;

            // Ack. gotta do a one-off parsing, b/c the uuid&firstmessage getters just read from the Cached* variables... :<
            // TODO: better parsing, this is crap
            string[] s = System.Text.Encoding.ASCII.GetString(Content).Split(' ');

            if (s.Length != 3)
                throw new Exception("Invalid NewThread packet, expected three args.");

            // can't use properties b/c
            CachedUUID = s[1];
            CachedFirstMessage = System.Web.HttpUtility.UrlDecode(s[2]);
        }

        public NewThreadPacket(string UUID, string FirstMessage)
        {
            this.UUID = UUID;
            this.FirstMessage = FirstMessage;
        }

        public string UUID
        {
            get
            {
                return CachedUUID;
            }
            set
            {
                // TODO: Validate!
                CachedUUID = value;
                RebuildByteArray();
            }
        }

        public string FirstMessage
        {
            get
            {
                return CachedFirstMessage;
            }
            set
            {
                // TODO: Is validation necessary?
                CachedFirstMessage = value;
                RebuildByteArray();
            }
        }

        private void RebuildByteArray()
        {
            // TODO: validation!
            Content = System.Text.Encoding.ASCII.GetBytes("THRD " + CachedUUID + " " + System.Web.HttpUtility.UrlEncode(CachedFirstMessage));
        }

        public override PacketType Type
        {
            get { return PacketType.NewThread; }
        }
    }

    public class NewPostPacket : Packet
    {
        string CachedUUID;
        string CachedMessage;

        public NewPostPacket(byte[] UnencryptedContent)
        {
            this.Content = UnencryptedContent;

            // Ack. gotta do a one-off parsing, b/c the uuid&firstmessage getters just read from the Cached* variables... :<
            // TODO: better parsing, this is crap
            string[] s = System.Text.Encoding.ASCII.GetString(Content).Split(' ');

            if (s.Length != 3)
                throw new Exception("Invalid NewPost packet, expected three args.");

            // can't use properties b/c
            CachedUUID = s[1];
            CachedMessage = System.Web.HttpUtility.UrlDecode(s[2]);
        }

        public NewPostPacket(string UUID, string Message)
        {
            this.UUID = UUID;
            this.Message = Message;
        }

        public string UUID
        {
            get
            {
                return CachedUUID;
            }
            set
            {
                // TODO: Validate!
                CachedUUID = value;
                RebuildByteArray();
            }
        }

        public string Message
        {
            get
            {
                return CachedMessage;
            }
            set
            {
                // TODO: Is validation necessary?
                CachedMessage = value;
                RebuildByteArray();
            }
        }

        private void RebuildByteArray()
        {
            // TODO: validation!
            Content = System.Text.Encoding.ASCII.GetBytes("THRD " + CachedUUID + " " + System.Web.HttpUtility.UrlEncode(CachedMessage));
        }

        public override PacketType Type
        {
            get { return PacketType.NewPost; }
        }
    }


}
