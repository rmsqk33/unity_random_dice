using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System;
using UnityEngine;
using Packet;
using System.Linq;
using Unity.VisualScripting;

public class FServerManager : Singleton<FServerManager>
{
    const string SERVER_IP = "127.0.0.1";
    const int SERVER_PORT = 7777;
    const int PACKET_MAX = 1024;

    private TcpClient m_TcpClient = null;
    private NetworkStream m_NetStream = null;
    private Thread m_Thread = null;

    private byte[] m_Buffer = new byte[PACKET_MAX];
    private int m_PacketSize = 0;
    private int m_ReadSize = 0;
    private PacketType m_PacketType = PacketType.PACKET_TYPE_NONE;

    public delegate void PacketHandler(in byte[] InBuffer);
    private Dictionary<PacketType, PacketHandler> m_PacketHandlerMap = new Dictionary<PacketType, PacketHandler>();

    struct MessageInfo
    {
        public PacketType type;
        public byte[] buffer;
    }
    private List<MessageInfo> m_MessageQueue = new List<MessageInfo>();

    void Start()
    {
        DontDestroyOnLoad(this);
    }
    
    void Update()
    {
        ExecuteMessage();
    }

    void ExecuteMessage()
    {
        if (m_MessageQueue.Count == 0)
            return;

        MessageInfo msg = m_MessageQueue[0];
        if (!m_PacketHandlerMap.ContainsKey(msg.type))
            return;

        m_PacketHandlerMap[msg.type](msg.buffer);
    }

    void OnApplicationQuit()
    {
        Release();
    }

    void Release()
    {
        if (m_TcpClient != null)
            m_TcpClient.Close();

        if (m_NetStream != null)
            m_NetStream.Close();

        if (m_Thread != null)
            m_Thread.Abort();
    }

    public bool ConnectServer()
    {
        try
        {
            m_TcpClient = new TcpClient(SERVER_IP, SERVER_PORT);
            m_NetStream = m_TcpClient.GetStream();

            m_Thread = new Thread(ReceiveMessage);
            m_Thread.Start();
        }
        catch(Exception e)
        {
            Debug.Log("Server Connect Fail: " + e);
            return false;
        }

        return true;
    }

    void ReceiveMessage()
    {
        while (true)
        {
            if (m_NetStream == null)
                continue;

            try
            {
                if (m_NetStream.CanRead)
                {
                    m_ReadSize += m_NetStream.Read(m_Buffer, m_ReadSize, PACKET_MAX);

                    // 처음 패킷을 받은 경우 사이즈와 패킷 타입 등 필수 데이터를 읽는다.
                    if (m_PacketType == PacketType.PACKET_TYPE_NONE)
                    {
                        m_PacketSize = BitConverter.ToInt32(m_Buffer, 0);
                        m_PacketType = (PacketType)BitConverter.ToInt32(m_Buffer, sizeof(int));
                    }

                    // 패킷 전체를 전부 전달받으면 메시지 큐에 데이터를 담는다.
                    if (m_ReadSize == m_PacketSize)
                    {
                        MessageInfo msg;
                        msg.type = m_PacketType;
                        msg.buffer = new byte[m_PacketSize];
                        Array.Copy(m_Buffer, msg.buffer, m_PacketSize);
                        
                        m_MessageQueue.Add(msg);

                        m_ReadSize = 0;
                        m_PacketSize = 0;
                        m_PacketType = PacketType.PACKET_TYPE_NONE;
                    }
                }
            }
            catch (SocketException e)
            {
                Debug.Log("ReceiveMessage Fail: " + e);
            }
        }
    }

    public void SendMessage(in PacketBase InPacket)
    {
        if(m_NetStream == null)
            return;

        try
        {
            if(m_NetStream.CanWrite)
            {
                List<byte> buffer = new List<byte>();
                InPacket.Serialize(buffer);
                m_NetStream.Write(buffer.ToArray(), 0, buffer.Count());
            }
        }
        catch (SocketException e)
        {
            Debug.Log("SendMessage Fail: " + e);
        }
    }

    public void AddPacketHandler(PacketType InType, PacketHandler InHandler)
    {
        m_PacketHandlerMap.Add(InType, InHandler);
    }

}
