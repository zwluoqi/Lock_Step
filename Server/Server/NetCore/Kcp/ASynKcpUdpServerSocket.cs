using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using log4net;

public class ASynKcpUdpServerSocket
{
    private readonly DateTime UTC_TIME = new DateTime(1970, 1, 1);

    private Action<byte[], ASynServerKcp> _recHandler;
    private UdpClient _socket;
    ASynServerKcp[] tmps = new ASynServerKcp[1024];
    private Dictionary<string, ASynServerKcp> _aSynKcpDic;
    private object __aSynKcpDicLock = new object();
    private IPEndPoint _remoteEP;
    private IPEndPoint _listenEP;

    public ASynKcpUdpServerSocket(int port, Action<byte[], ASynServerKcp> recHandler)
    {
        _recHandler = recHandler;
        _aSynKcpDic = new Dictionary<string, ASynServerKcp>();
        createSocket(port);
    }

    public void SendToAll(byte[] buff)
    {
        Dictionary<string, ASynServerKcp>.ValueCollection tmps;

        lock (__aSynKcpDicLock)
        {
            tmps = _aSynKcpDic.Values;
        }
        foreach (ASynServerKcp aSynKcp in tmps)
        {
            aSynKcp.Send(buff);
        }
    }

    public void Update()
    {
        UInt32 current = GetMilliseconds();
        int kcpCounter = 0;
        lock (__aSynKcpDicLock)
        {
            _aSynKcpDic.Values.CopyTo(tmps, 0);
            kcpCounter = _aSynKcpDic.Count;

        }

        for(int i=0;i< kcpCounter;i++)
        {
            var aSynKcp = tmps[i];
            aSynKcp.Update(current);
			//这里是简单掉线判断处理,合理情况应该用额外的tcp做在线或掉线处理
			if (aSynKcp.IsTimeOut)
			{
                string epKey = aSynKcp.RemoteEP.Address + ":" + aSynKcp.RemoteEP.Port;
                aSynKcp.Dispose();
                _aSynKcpDic.Remove(epKey);
                LogManager.GetLogger("kcp_server").Debug("用户超时掉线:" + aSynKcp.PlayerId);
            }
        }
    }

    public void RemovePlayer(int playerId)
    {
        int kcpCounter = 0;
        lock (__aSynKcpDicLock)
        {
            _aSynKcpDic.Values.CopyTo(tmps, 0);
            kcpCounter = _aSynKcpDic.Count;

        }

        for (int i = 0; i < kcpCounter; i++)
        {
            var aSynKcp = tmps[i];
            if (aSynKcp.PlayerId == playerId)
            {
                string epKey = aSynKcp.RemoteEP.Address + ":" + aSynKcp.RemoteEP.Port;
                ASynServerKcp aSynServerKcp = null;
                lock (__aSynKcpDicLock)
                {
                    if (_aSynKcpDic.TryGetValue(epKey, out aSynServerKcp))
                    {
                        _aSynKcpDic.Remove(epKey);
                    }
                }
                if (aSynServerKcp != null)
                {
                    aSynServerKcp.Dispose();
                }
                break;
            }
        }
    }

    public void Dispose()
    {
        _socket.Close();
        _socket = null;
        int kcpCounter = 0;
        lock (__aSynKcpDicLock)
        {
            _aSynKcpDic.Values.CopyTo(tmps, 0);
            kcpCounter = _aSynKcpDic.Count;

        }

        for (int i = 0; i < kcpCounter; i++)
        {
            var aSynKcp = tmps[i];
            aSynKcp.Dispose();
        }
        _aSynKcpDic = null;
        _recHandler = null;
    }

    private void createSocket(int port)
    {
        _remoteEP = new IPEndPoint(IPAddress.Any, port);
        _listenEP = new IPEndPoint(IPAddress.Any, port);
        _socket = new UdpClient(_listenEP);
       
        _socket.BeginReceive(ReceiveAsyn, this);
    }

    private void ReceiveAsyn(IAsyncResult arg)
    {
        byte[] rcvBuf = _socket.EndReceive(arg, ref _remoteEP);
        string epKey = _remoteEP.Address + ":" + _remoteEP.Port;
        if (rcvBuf.Length == 24)
        {
			//KCP ping
        }
        else
        {
            LogManager.GetLogger("kcp_server").Debug("ASynKcpUdpServerSocket:ReceiveAsyn Message receive from " + epKey + " size:" + rcvBuf.Length);
        }

        ASynServerKcp aSynKcp = null;
        lock (__aSynKcpDicLock)
        {
            if (_aSynKcpDic.ContainsKey(epKey))
            {
                aSynKcp = _aSynKcpDic[epKey];
            }
            else
            {
                //这个ID应该通过tcp分配给Client,然后服务器创建对应的ASynServerKcp而不是在这个地方处理,这里简化
                UInt32 conv_ = 0;
                KCP.ikcp_decode32u(rcvBuf, 0, ref conv_);
                aSynKcp = new ASynServerKcp(conv_, _socket, _remoteEP, _recHandler);
                _aSynKcpDic.Add(epKey, aSynKcp);
                LogManager.GetLogger("kcp_server").Debug("用户连接:" + aSynKcp.PlayerId);
            }
        }
        aSynKcp.Input(rcvBuf);
        _socket.BeginReceive(ReceiveAsyn, this);
    }

    private uint GetMilliseconds()
    {
        return (uint)(Convert.ToInt64(DateTime.UtcNow.Subtract(UTC_TIME).TotalMilliseconds) & 0xffffffff);
    }
}
