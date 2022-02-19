// /*
//                #########
//               ############
//               #############
//              ##  ###########
//             ###  ###### #####
//             ### #######   ####
//            ###  ########## ####
//           ####  ########### ####
//          ####   ###########  #####
//         #####   ### ########   #####
//        #####   ###   ########   ######
//       ######   ###  ###########   ######
//      ######   #### ##############  ######
//     #######  #####################  ######
//     #######  ######################  ######
//    #######  ###### #################  ######
//    #######  ###### ###### #########   ######
//    #######    ##  ######   ######     ######
//    #######        ######    #####     #####
//     ######        #####     #####     ####
//      #####        ####      #####     ###
//       #####       ###        ###      #
//         ###       ###        ###
//          ##       ###        ###
// __________#_______####_______####______________
//
//                 我们的未来没有BUG
// * ==============================================================================
// * Filename:NetManager.cs
// * Created:2022/2/16
// * Author:  zhouwei
// * Purpose:
// * ==============================================================================
// */
//
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using log4net;
using Newtonsoft.Json.Linq;

namespace Client.Net
{

	public class NetManager
	{


        private ASynKcpUdpClientSocket _socket = null;

        public void Init(int PlayerId)
        {
            NetConfig.PlayerId = PlayerId;
            _socket = new ASynKcpUdpClientSocket(NetConfig.PlayerId, NetConfig.ServerAddress, NetConfig.Port, RecHandler);
            Ping();
        }

        public void RegisterCallBack(string msgid, Action<string, string> regCall)
        {
            if (onMsgIdCallBack.TryGetValue(msgid, out var call))
            {
                call += regCall;
            }
            else
            {
                onMsgIdCallBack[msgid] = regCall;
            }
        }

        Dictionary<string, Action<string, string>> onMsgIdCallBack = new Dictionary<string, Action<string, string>>();
		
        private void RecHandler(byte[] buf)
        {
            //json TODO
            PacketBundle.ToObject(buf, out var packet);
            // LogManager.GetLogger("net").DebugFormat("RecHandler>>>>>> msgId="+ packet.id+ " msg="+ packet.msgJson);
            if (onMsgIdCallBack.TryGetValue(packet.id,out var call))
            {
                call(packet.id, packet.msgJson);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void Update(double deltaTime)
        {
            timer += deltaTime;
            pingTimer += deltaTime;
            if (timer > 5)
            {
                timer = 0;
                Ping();
            }

            _socket.Update();
        }

        public void Release()
        {
            _socket.Dispose();
            _socket = null;
        }

        private double timer = 0;
        private double pingTimer = 0;
        private int pingCounter = 0;
        void Ping()
        {
            JObject jObject = new JObject();
            jObject["counter"] = pingCounter++;
            jObject["timer"] = pingTimer;
            SendMsg("ping",jObject.ToString() );
        }

        public void SendMsg(string msgId,string json)
        {
            // LogManager.GetLogger("net").Debug("SendMsg>>>>>> msgId="+ msgId+ " msg="+ json);
            PacketBundle.ToMsg(msgId, json, out var packet);
            _socket.Send(packet.msgBytes);
        }
    }
}
