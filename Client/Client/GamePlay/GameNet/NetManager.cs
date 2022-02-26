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
        private static NetManager _instance = new NetManager();

        public static NetManager Instance
        {
            get
            {
                return _instance;
            }
        }

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
        
        public void UnRegisterCallBack(string msgid, Action<string, string> regCall)
        {
            if (onMsgIdCallBack.TryGetValue(msgid, out var call))
            {
                call -= regCall;
            }
            else
            {
                
            }
        }

        Dictionary<string, Action<string, string>> onMsgIdCallBack = new Dictionary<string, Action<string, string>>();

        private Queue<Packet> RecieveDatas = new Queue<Packet>();
        object _RecieveLock = new object();

        private void RecHandler(byte[] buf)
        {
            //json TODO
            PacketBundle.ToObject(buf, out var packet);
            RecieveDatas.Enqueue(packet);
            // LogManager.GetLogger("net").DebugFormat("RecHandler>>>>>> msgId="+ packet.id+ " msg="+ packet.msgJson);

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
            ProcessReceiveDatas();
        }

        private Packet[] packets = new Packet[16];
        private void ProcessReceiveDatas()
        {
            int counter = 0;
            Packet packet = null;
            lock (_RecieveLock)
            {
                while (RecieveDatas.Count>0 && counter<16)
                {
                    packet = RecieveDatas.Dequeue();
                    if (packet != null)
                    {
                        packets[counter++] = (packet);
                    }
                }
            }

            if (counter == 0)
            {
                return;
            }

            for (int i = 0; i < counter; i++)
            {
                packet = packets[i];
                if (onMsgIdCallBack.TryGetValue(packet.id,out var call))
                {
                    call(packet.id, packet.msgJson);
                }
                else
                {
                    throw new NotImplementedException();
                } 
            }
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
