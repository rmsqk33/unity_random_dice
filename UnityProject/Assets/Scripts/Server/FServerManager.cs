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
    private Thread m_ReceiveMessageThread = null;

    public delegate void PacketHandler(in byte[] InBuffer);
    private Dictionary<PacketType, PacketHandler> m_PacketHandlerMap = new Dictionary<PacketType, PacketHandler>();

    struct MessageData
    {
        public PacketType type;
        public byte[] buffer;
    }
    private List<MessageData> m_MessageQueue = new List<MessageData>();

    void Update()
    {
        ExecuteMessage();
    }

    void ExecuteMessage()
    {
        if (m_MessageQueue.Count == 0)
            return;

        MessageData messageData = m_MessageQueue[0];
        m_MessageQueue.RemoveAt(0);

        if (!m_PacketHandlerMap.ContainsKey(messageData.type))
            return;

        m_PacketHandlerMap[messageData.type](messageData.buffer);
    }

    void OnApplicationQuit()
    {
        DisconnectServer();
    }

    void DisconnectServer()
    {
        if (m_ReceiveMessageThread != null)
            m_ReceiveMessageThread.Abort();

        if (m_TcpClient != null)
            m_TcpClient.Close();

        if (m_NetStream != null)
            m_NetStream.Close();
    }

    public bool ConnectServer()
    {
        try
        {
            m_TcpClient = new TcpClient(SERVER_IP, SERVER_PORT);
            m_NetStream = m_TcpClient.GetStream();

            m_ReceiveMessageThread = new Thread(ReceiveMessage);
            m_ReceiveMessageThread.Start();
        }
        catch (Exception e)
        {
            Debug.Log("Server Connect Fail: " + e);
            return false;
        }

        return true;
    }

    void ReceiveMessage()
    {
        byte[] buffer = new byte[PACKET_MAX];
        int packetSize = 0;
        int readSize = 0;

        while (true)
        {
            try
            {
                if (m_NetStream.CanRead)
                {
                    readSize += m_NetStream.Read(buffer, readSize, PACKET_MAX - readSize);

                    // 한번에 여러 패킷이 오는 경우를 대비하여 반복처리
                    while(true)
                    {             
                        // 처음 패킷을 받은 경우 패킷 사이즈 저장
                        if (packetSize == 0)
                            packetSize = BitConverter.ToInt32(buffer, 0);

                        // 패킷 전체를 전달받지 못한 경우 계속 데이터를 받도록 한다.
                        if (packetSize == 0 || readSize < packetSize)
                            break;

                        // 패킷 전체를 전부 전달받으면 메시지 큐에 데이터를 담는다.
                        MessageData messageData = new MessageData();
                        messageData.type = (PacketType)BitConverter.ToInt32(buffer, sizeof(int));
                        messageData.buffer = new byte[packetSize];

                        int commonDataLength = sizeof(int) + sizeof(PacketType);
                        Array.Copy(buffer, commonDataLength, messageData.buffer, 0, packetSize - commonDataLength);

                        m_MessageQueue.Add(messageData);

                        readSize -= packetSize;
                        
                        // 처리 안된 패킷이 있으면 앞으로 땡기기
                        if (0 < readSize)
                            Array.Copy(buffer, packetSize, buffer, 0, readSize);
                        // 처리할 패킷이 없으면 서버로 부터 패킷을 받도록 반복문 중단
                        else
                        {
                            packetSize = 0;
                            break;
                        }
                    }
                }
            }
            catch (ThreadAbortException e)
            {
                Debug.Log("ThreadAbortException: " + e);
            }
        }
    }

    public void SendMessage(in PacketBase InPacket)
    {
        if (m_NetStream == null)
            return;

        try
        {
            if (m_NetStream.CanWrite)
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
