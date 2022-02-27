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
        private static ServerUDPMgr _instance = new ServerUDPMgr();

        public static ServerUDPMgr Instance
		{
			get
			{
                return _instance;
			}
		}

        private ASynKcpUdpServerSocket _socket;

        public void Init()
        {
            _socket = new ASynKcpUdpServerSocket(NetConfig.Port, RecHandler);
        }

        public void RegisterCallBack(string msgid, OnReceiveCall regCall)
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

        public void UnRegisterCallBack(string msgid, OnReceiveCall regCall)
        {
            if (onMsgIdCallBack.TryGetValue(msgid, out var call))
            {
                call -= regCall;
                if (call == null)
                {
                    onMsgIdCallBack.Remove(msgid);
                }
                else
                {
                    onMsgIdCallBack[msgid] = call;
                }
            }
            else
            {
                LogManager.GetLogger("net").Error(msgid + " not callback");
            }
        }

        public delegate void OnReceiveCall(string s1, string s2);
        Dictionary<string, OnReceiveCall> onMsgIdCallBack = new Dictionary<string, OnReceiveCall>();

        private Queue<Packet> RecieveDatas = new Queue<Packet>();
        object _RecieveLock = new object();

        private void RecHandler(byte[] buf, ASynServerKcp serverKcp)
        {

            PacketBundle.ToObject(buf, out var packet);
            serverKcp.PlayerId = packet.pid;

            InputReceiveDatas(packet);

        }

        public void Update()
        {
            _socket.Update();
            ProcessReceiveDatas();
        }

        private Packet[] packets = new Packet[16];

        private void InputReceiveDatas(Packet packet)
        {
			LogManager.GetLogger("net").Debug("RecHandler>>>>>> msgId=" + packet.id + " msg=" + packet.msgJson);
			lock (_RecieveLock)
            {
                RecieveDatas.Enqueue(packet);
            }
        }

        private void ProcessReceiveDatas()
        {
            int counter = 0;
            Packet packet = null;
            lock (_RecieveLock)
            {
                while (RecieveDatas.Count > 0 && counter < 16)
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
                if (onMsgIdCallBack.TryGetValue(packet.id, out var call))
                {
                    call(packet.id, packet.msgJson);
                }
                else
                {
                    LogManager.GetLogger("net").Error(packet.id + " not callback");
                }
            }
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