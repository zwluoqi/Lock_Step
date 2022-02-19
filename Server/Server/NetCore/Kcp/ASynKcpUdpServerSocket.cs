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
    private Dictionary<string, ASynServerKcp> _aSynKcpDic;
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
        foreach (ASynServerKcp aSynKcp in _aSynKcpDic.Values)
        {
            aSynKcp.Send(buff);
        }
    }

    public void Update()
    {
        UInt32 current = GetMilliseconds();
        foreach (ASynServerKcp aSynKcp in _aSynKcpDic.Values)
        {
            aSynKcp.Update(current);
        }
    }

    public void RemovePlayer(int playerId)
    {
        foreach (ASynServerKcp aSynKcp in _aSynKcpDic.Values)
        {
            if(aSynKcp.PlayerId == playerId)
            {
                string epKey = aSynKcp.RemoteEP.Address + ":" + aSynKcp.RemoteEP.Port;
                _aSynKcpDic.Remove(epKey);
                break;
            }
        }
    }

    public void Dispose()
    {
        _socket.Close();
        _socket = null;
        foreach (ASynServerKcp aSynKcp in _aSynKcpDic.Values)
        {
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
        LogManager.GetLogger("kcp_server").Debug("ASynKcpUdpServerSocket:ReceiveAsyn Message receive from "+ epKey);

        ASynServerKcp aSynKcp;
        if (_aSynKcpDic.ContainsKey(epKey))
        {
            aSynKcp = _aSynKcpDic[epKey];
        }
        else
        {
            //这个ID应该通过tcp分配给Client,然后服务器创建对应的ASynServerKcp而不是在这个地方处理,这里简化
            UInt32 conv_ =0;
            KCP.ikcp_decode32u(rcvBuf, 0, ref conv_);
            aSynKcp = new ASynServerKcp(conv_, _socket, _remoteEP, _recHandler);
            _aSynKcpDic.Add(epKey, aSynKcp);
        }
        aSynKcp.Input(rcvBuf);
        _socket.BeginReceive(ReceiveAsyn, this);
    }

    private uint GetMilliseconds()
    {
        return (uint)(Convert.ToInt64(DateTime.UtcNow.Subtract(UTC_TIME).TotalMilliseconds) & 0xffffffff);
    }
}
