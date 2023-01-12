using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Linq;
using System;
using UnityEngine;
using InGamePacket;

public class PTPServerManager : Singleton<PTPServerManager>
{
// - Member Variable
    // - ishost, ip, port, packet
    private bool            _isHost         = false;
    const string            SERVER_IP       = "127.0.0.1"; // - temp ip/my
    const int               SERVER_PORT     = 50001;
    const int               PACKET_MAX      = 1024;

    // - tcp listener, client, netstream
    private TcpListener     _tcplistener    = null;
    private TcpClient       _tcpclient      = null;
    private NetworkStream   _netstream      = null;
    private Thread          _thread         = null;

    // - buffer, packet
    private byte[]          _buffer         = new byte[PACKET_MAX];
    private int             _packetsize     = 0;
    private int             _readsize       = 0;
    private PacketType      _packettype     = PacketType.PACKET_TYPE_NONE;

    // - delegate, handlermap
    public delegate void PacketHandler(in byte[] inbuffer);
    private Dictionary<PacketType, PacketHandler> _packethandlermap = new Dictionary<PacketType, PacketHandler>();


    struct MessageInfo
    {
        public PacketType type;
        public byte[] buffer;
    }
    private List<MessageInfo> _messagequeue = new List<MessageInfo>();

    // - Method
    // - Base
    protected override void Awake()
    {
        base.Awake();
        // - if host, set listener
        if (_isHost)
        {
            _thread = new Thread(new ThreadStart(ListenForIncommingRequest));
            _thread.IsBackground = true;
            _thread.Start();
        }
        // - else set client
        else
        {
            ConnectServer();
        }
    }

    private void Start()
    {
    }

    private void Update()
    {
        ExecuteMessage();
    }

    // - Listening
    private void ListenForIncommingRequest()
    {
        try
        {
            _tcplistener = new TcpListener(IPAddress.Parse(SERVER_IP), SERVER_PORT);
            _tcplistener.Start();
            Debug.Log("Server is listening");

            while(true)
            {
                using (_tcpclient = _tcplistener.AcceptTcpClient())
                {
                    using (_netstream = _tcpclient.GetStream())
                    {
                        // - To do
                        ReceiveMessage();
                    }
                }
            }
        }
        catch(SocketException e)
        {
            Debug.LogError("SocketException : " + e);
        }
    }

    // - 
    private void ExecuteMessage()
    {
        lock (_messagequeue)
        {
            if (_messagequeue.Count == 0)
                return;

            MessageInfo msg = _messagequeue[0];
            if (!_packethandlermap.ContainsKey(msg.type))
                return;

            _packethandlermap[msg.type](msg.buffer);
            _messagequeue.RemoveAt(0);
        }
    }

    public bool ConnectServer()
    {
        try
        {
            _tcpclient = new TcpClient(SERVER_IP, SERVER_PORT);
            _netstream = _tcpclient.GetStream();

            _thread = new Thread(ReceiveMessage);
            _thread.Start();
            Debug.Log("Server is connecting");
        }
        catch (Exception e)
        {
            Debug.Log("Server Connect Fail : " + e);
            return false;
        }

        return true;
    }

    private void ReceiveMessage()
    {
        while (true)
        {
            try
            {
                if (_netstream.CanRead)
                {
                    _readsize += _netstream.Read(_buffer, _readsize, PACKET_MAX);

                    // - first receive packet, read data
                    if (_packettype == PacketType.PACKET_TYPE_NONE)
                    {
                        _packetsize = BitConverter.ToInt32(_buffer, 0);
                        _packettype = (PacketType)BitConverter.ToInt32(_buffer, sizeof(int));
                    }

                    // - All packect receive, input data in Messagequeue
                    if (_readsize == _packetsize)
                    {
                        MessageInfo msg;
                        msg.type = _packettype;
                        msg.buffer = new byte[_packetsize];
                        Array.Copy(_buffer, msg.buffer, _packetsize);

                        lock (_messagequeue)
                        {
                            _messagequeue.Add(msg);
                        }

                        _readsize = 0;
                        _packetsize = 0;
                        _packettype = PacketType.PACKET_TYPE_NONE;
                    }
                }
            }
            catch (SocketException e)
            {
                Debug.Log("ReceiveMessage Fail : " + e);
            }
        }
    }

    public void SendMessage(in PacketBase InPacket)
    {
        if (_netstream == null)
            return;

        try
        {
            if (_netstream.CanWrite)
            {
                List<byte> _buffer = new List<byte>();
                InPacket.Serialize(_buffer);
                _netstream.Write(_buffer.ToArray(), 0, _buffer.Count());
            }
        }
        catch(SocketException e)
        {
            Debug.Log("SendMessage Fail : " + e);
        }
    }

    public void AddPacketHandler(PacketType intype, PacketHandler inhandler)
    {
        _packethandlermap.Add(intype, inhandler);
    }

    // - If Application Quit, Memory Release
    private void OnApplicationQuit()
    {
        DisConnectServer();
    }

    public void DisConnectServer()
    {
        if (_tcplistener != null)
        { _tcplistener.Stop(); }

        if (_tcpclient != null)
            _tcpclient.Close();

        if (_netstream != null)
            _netstream.Close();

        if (_thread != null)
            _thread.Abort();
    }
}