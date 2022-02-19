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

        Dictionary<string, Action<string, string>> onMsgIdCallBack;
		
        private void RecHandler(byte[] buf)
        {
            //json TODO
            string msgid;
            string pdata;
            PacketBundle.ToObject(buf, out msgid, out pdata);
            if (onMsgIdCallBack.TryGetValue(msgid,out var call))
            {
                call(msgid, pdata);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void Update()
        {
            _socket.Update();
        }

        public void Release()
        {
            _socket.Dispose();
            _socket = null;
        }

        public void SendMsg(string msgId,string json)
        {
            byte[] sendBuf;
            PacketBundle.ToMsg(msgId, json, out sendBuf);
            _socket.Send(sendBuf);
        }
    }
}
