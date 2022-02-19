using System;
using System.Collections;
using System.Text;
using Newtonsoft.Json.Linq;
using Object = System.Object;


namespace Server.Net
{
    public class NetConfig
    {
        public static string ServerAddress = "127.0.0.1";
        public static readonly int Port = 8001;
    }


	public class Packet
	{
        public string id;
        public byte[] msgBytes;
        public string msgJson;
        public int pid;
        public int zoneid;
    }

    public class PacketBundle
    {
        public static bool ToMsg(string id, JToken pbwhKBody, out Packet packet)
        {
            packet = new Packet();
            packet.id = id;
            JObject user = new JObject();
            user["id"] = id;
            user["msg"] = pbwhKBody;
            user["pid"] = 0;
            user["zoneid"] = 0;
            packet.msgBytes = System.Text.Encoding.UTF8.GetBytes(user.ToString());
            return true;
        }

        public static bool ToObject(byte[] data, out Packet packet)
        {
            packet = new Packet();
            var msg = Encoding.UTF8.GetString(data);
            JObject user = JObject.Parse(msg);
            packet.id = (string) user["id"];
            packet.msgJson = user["msg"].ToString();
            packet.pid = (int)user["pid"];
            packet.zoneid = (int)user["zoneid"];
            return true;
        }
    }
}
