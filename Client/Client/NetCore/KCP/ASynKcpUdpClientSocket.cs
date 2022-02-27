using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using log4net;

public class ASynKcpUdpClientSocket
{
    private readonly DateTime UTC_TIME = new DateTime(1970, 1, 1);

    private Action<byte[]> _recHandler;
    private UdpClient _socket;
    private KCP _kcp;
    private IPEndPoint _remoteEP;
    private IPEndPoint _listenEP;

    public ASynKcpUdpClientSocket(int conv, string host, int port, Action<byte[]> recHandler)
    {
        _recHandler = recHandler;
        createSocket(conv, host, port);
    }

    public void Send(byte[] buff)
    {
        _kcp.Send(buff);
    }

    public void Update()
    {
        _kcp.Update(GetMilliseconds());
    }

    public void Dispose()
    {
        _socket.Close();
        _socket = null;
        _kcp = null;
        _recHandler = null;
    }

    private void createSocket(int conv, string host, int port)
    {
        _remoteEP = new IPEndPoint(IPAddress.Parse(host), port);
        _listenEP = new IPEndPoint(IPAddress.Any, port);
        // 主要一定要设置port为0，不然无法接收到服务器的消息
        _socket = new UdpClient(0);
        _socket.Connect(_remoteEP);
        _kcp = new KCP((uint)conv, (byte[] buf, int size) =>
        {
            if (size == 24)
            {
            }
            else
            {
                LogManager.GetLogger("kcp_client").Debug("ASynKcpUdpClientSocket:send Message send size=" + size);
            }

            _socket.Send(buf, size);
        });
        // fast mode.
        _kcp.NoDelay(1, 10, 2, 1);
        _kcp.WndSize(128, 128);
        _socket.BeginReceive(ReceiveAsyn, this);
    }

    private void ReceiveAsyn(IAsyncResult arg)
    {
        //byte[] rcvBuf = _receiveEP == null ? _socket.Receive(ref _receiveEP) : _socket.EndReceive(arg, ref _receiveEP);
        try
        {
            byte[] rcvBuf = _socket.EndReceive(arg, ref _listenEP);
            if (rcvBuf != null)
            {
                // LogManager.GetLogger("kcp_client")
                //     .Debug("ASynKcpUdpClientSocket:ReceiveAsyn Message receive from " + _listenEP.ToString()+" length:"+rcvBuf.Length);
                _kcp.Input(rcvBuf);
                for (var size = _kcp.PeekSize(); size > 0; size = _kcp.PeekSize())
                {
                    byte[] buf = new byte[size];
                    if (_kcp.Recv(buf) > 0)
                    {
                        _recHandler(buf);
                    }
                }
            }
        }
        catch (Exception e)
        {
            LogManager.GetLogger("kcp_client")
                .Error(e.Message);
        }
        finally
        {

            if (_socket != null)
            {
                _socket.BeginReceive(ReceiveAsyn, this);
            }
        }
    }

    private uint GetMilliseconds()
    {
        return (uint)(Convert.ToInt64(DateTime.UtcNow.Subtract(UTC_TIME).TotalMilliseconds) & 0xffffffff);
    }
}
