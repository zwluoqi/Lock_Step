using System;
using System.Net.Sockets;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json.Linq;
using log4net;

namespace Server.Net
{
    public class ServerUDPMgr
    {
        private ASynKcpUdpServerSocket _socket;

        public void Init()
        {
            _socket = new ASynKcpUdpServerSocket(NetConfig.Port, RecHandler);
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


        private void RecHandler(byte[] buf, ASynServerKcp serverKcp)
        {

            PacketBundle.ToObject(buf, out var packet);
            //LogManager.GetLogger("net").DebugFormat($"SimulateServerUDP:RecHandler>>>>>> msgId= {packet.id} ,msg= {packet.msgJson}");
            serverKcp.PlayerId = packet.pid;
            if (onMsgIdCallBack.TryGetValue(packet.id, out var call))
            {
                call(packet.id, packet.msgJson);
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

        public void RemovePlayer(int playerId)
        {
            _socket.RemovePlayer(playerId);
        }

        public void SendMsgToAll(string msgId, string msg)
        {
            //LogManager.GetLogger("net").Debug($"SimulateServerUDP:SendMsgToAll>>>>>> msgId= {msgId} ,msg= {msg}");
            PacketBundle.ToMsg(msgId, msg, out var packet);
            _socket.SendToAll(packet.msgBytes);
        }
    }
}