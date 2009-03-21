using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace libnish.PeerFinders
{
    public class PotentialPeer
    {
        public string IP;
        public int Port;
        private TcpClient AlreadyEstablishedConnection = null;
        
        ~PotentialPeer()
        {
        	CloseIfConnected();
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == this.GetType())
                if (((PotentialPeer)obj).IP == this.IP)
                    if (((PotentialPeer)obj).Port == this.Port)
                        return true;
            return false;
        }

        public void SetAlreadyEstablishedConnection(TcpClient AEC)
        {
            AlreadyEstablishedConnection = AEC;
        }
        
        public void CloseIfConnected()
        {
        	if (AlreadyEstablishedConnection != null && AlreadyEstablishedConnection.Connected)
            {
                AlreadyEstablishedConnection.Client.Close();
                AlreadyEstablishedConnection.Close();
            }
        }

        public override int GetHashCode()
        {
            // rly rly don't care
            // TODO:: Validate IP, make sure IP is IPv4 address (or fix following code to do v6 too:)
            return (((long)int.Parse(IP.Replace(".", "")) << 32) + Port).GetHashCode();
        }

        public static bool operator ==(PotentialPeer a, PotentialPeer b)
        {
            return ((a.IP == b.IP) && (a.Port == b.Port));
        }

        public static bool operator !=(PotentialPeer a, PotentialPeer b)
        {
            return ((a.IP != b.IP) || (a.Port != b.Port));
        }

        public bool TryConnect(out Peer PeerOut)
        {
            TcpClient Connection = null;

            try
            {
                // Do we need to establish a connection, or...?
                if (AlreadyEstablishedConnection != null)
                    Connection = AlreadyEstablishedConnection;
                else
                    Connection = new TcpClient(IP, Port);
            }
            catch (SocketException se)
            {
                NetEvents.Add(NetEventType.PeerConnectFail, "Failed to connect to peer '" + IP + ":" + Port.ToString() + "'.\nGot socket exception:\n" + se.ToString(), new object[] { se }, this);
                PeerOut = null;
                return false;
            }
            catch (Exception e)
            {
                NetEvents.Add(NetEventType.PeerConnectFail, "Failed to connect to peer '" + IP + ":" + Port.ToString() + "'.\nAn unrecognised exception occurred:\n" + e.ToString(), new object[] { e }, this);
                PeerOut = null;
                return false;
            }

            // Fortunately, a lot of the protocol-related setup stuff lives in Peer.
            // (Considering moving some of this code, too.  It's almost hidden here.)
            Peer p;

		    try
		    {
		        p = new Peer(Connection, IP, Port, Limits.Default, (AlreadyEstablishedConnection == null));
		    }
		    catch (Exception e)
		    {
		        NetEvents.Add(NetEventType.PeerConnectFail, "Failed to negotiate a connection with peer '" + IP + ":" + Port.ToString() + "'.\nAn exception occurred:\n" + e.ToString(), new object[] { e }, this);
		        PeerOut = null;
                if (Connection != null && Connection.Connected)
                {
                    Connection.Client.Close();
                    Connection.Close();
                }
                return false;
		    }
            

            // Success at last!
            PeerOut = p;
            return true;
        }
    }
}
