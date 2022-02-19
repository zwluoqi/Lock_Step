using System;
using System.Collections;
using System.Text;
using Newtonsoft.Json.Linq;
using Object = System.Object;


namespace Client.Net
{
    public class NetConfig
    {
        public static string ServerAddress = "10.10.11.81";
        public static int PlayerId = 1; // 玩家id，也是本客户端KCP会话id
        public static int ZoneId = 1; // 
        public static readonly int Port = 8001;
    }

    public class PacketBundle
    {
        public static bool ToMsg(string id, JToken pbwhKBody, out byte[] msg)
        {
            JObject user = new JObject();
            user["id"] = id;
            user["msg"] = pbwhKBody;
            user["pid"] = NetConfig.PlayerId;
            user["zoneid"] = NetConfig.ZoneId;
            msg = System.Text.Encoding.UTF8.GetBytes(user.ToString());
            return true;
        }

        public static bool ToObject(byte[] data, out string id, out string pbData)
        {
            var msg = Encoding.UTF8.GetString(data);
            JObject user = JObject.Parse(msg);
            id = (string) user["id"];
            pbData = user["msg"].ToString();
            // pbData = Encoding.UTF8.(data, data.Length);
            return true;
        }
    }
}
