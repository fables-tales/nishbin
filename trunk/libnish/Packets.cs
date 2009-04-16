using System;
using System.Collections.Generic;

using System.Text;
using System.Threading;

/// <summary>
/// a series of packets used in nishbin
/// </summary>
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

    public class BadPacketException : Exception
    {
        string desc;

        public BadPacketException(string Description)
        {
            desc = Description;
        }

        public override string Message
        {
            get
            {
                return desc;
            }
        }
    }

    public abstract class Packet
    {
        internal byte[] Content;

        public abstract PacketType Type { get; }

        public byte[] ToUnencryptedByteArray()
        {
            return Content;
        }

        public static Packet FromUnencryptedByteArray(byte[] Contents)
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
                    // FIXME: Proliferate, don't throw an exception!!
                    throw new Exception("Unrecognised packet type '" + System.Text.Encoding.ASCII.GetString(Contents, 0, 4) + "'.");
            }
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
                if (!Crypto.UUID.verifyuuid(value))
                    throw new BadPacketException("Invalid UUID set.");

                Content = System.Text.Encoding.ASCII.GetBytes("META " + value);
            }
        }

        public MetaNotifyPacket(string UUID)
        {
            if (!Crypto.UUID.verifyuuid(UUID))
                throw new BadPacketException("Invalid UUID");

            ContainingUUID = UUID;
        }

        public MetaNotifyPacket(byte[] UnencryptedContent)
        {
            this.Content = UnencryptedContent;

            if (!Crypto.UUID.verifyuuid(ContainingUUID))
                throw new BadPacketException("Bad packet - contains invalid UUID!");
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

            // can't use properties b/c they always rebuild the byte array...which would cause problems with unset vars.
            if (!Crypto.UUID.verifyuuid(s[1]))
                throw new BadPacketException("Bad packet -- invalid UUID!");

            CachedUUID = s[1];
            CachedFirstMessage = System.Web.HttpUtility.UrlDecode(s[2]);
        }

        public NewThreadPacket(string UUID, string FirstMessage)
        {
            if (!Crypto.UUID.verifyuuid(UUID))
                throw new BadPacketException("Invalid UUID");

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
                if (!Crypto.UUID.verifyuuid(value))
                    throw new BadPacketException("Invalid UUID set.");

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
            if (!Crypto.UUID.verifyuuid(CachedUUID))
                throw new BadPacketException("Bad packet, invalid UUID!");

            // TODO: msg validation

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
            if (!Crypto.UUID.verifyuuid(s[1]))
                throw new BadPacketException("Bad packet -- invalid UUID!");

            CachedUUID = s[1];
            CachedMessage = System.Web.HttpUtility.UrlDecode(s[2]);
        }

        public NewPostPacket(string UUID, string Message)
        {
            if (!Crypto.UUID.verifyuuid(UUID))
                throw new BadPacketException("Invalid UUID");

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
                if (!Crypto.UUID.verifyuuid(value))
                    throw new BadPacketException("Invalid UUID set.");

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
            if (!Crypto.UUID.verifyuuid(CachedUUID))
                throw new BadPacketException("Bad packet, invalid UUID!");

            // TODO: msg validation

            Content = System.Text.Encoding.ASCII.GetBytes("THRD " + CachedUUID + " " + System.Web.HttpUtility.UrlEncode(CachedMessage));
        }

        public override PacketType Type
        {
            get { return PacketType.NewPost; }
        }
    }


}
