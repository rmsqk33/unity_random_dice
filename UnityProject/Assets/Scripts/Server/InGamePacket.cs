using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InGamePacket
{
    // - InGamePacketType
    public enum PacketType
    {
        PACKET_TYPE_NONE,
        PACKET_TYPE_C_MOVE,
        PACKET_TYPE_S_MOVE,
        PACKET_TYPE_END
    }

    public class PacketBase
    {
        public virtual PacketType GetPacketType() { return PacketType.PACKET_TYPE_END; }
        public virtual void Serialize(List<byte> InBuffer) { }
    }

    public class C_MOVE : PacketBase
    {
        public KeyCode _input;

        public C_MOVE() { }
        public C_MOVE(in byte[] _buffer, int offset = 0)
        {
            offset += sizeof(int) + sizeof(PacketType);
            _input = (KeyCode)BitConverter.ToInt32(_buffer, offset);
        }
        public int GetSize()
        {
            int size = sizeof(KeyCode);
            return size;
        }

        public override PacketType GetPacketType()
        {
            return PacketType.PACKET_TYPE_C_MOVE;
        }

        public override void Serialize(List<byte> InBuffer)
        {
            int size = GetSize() + sizeof(KeyCode) + sizeof(PacketType);
            InBuffer.AddRange(BitConverter.GetBytes(size));
            InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
            InBuffer.AddRange(BitConverter.GetBytes((int)_input));
        }

        public int Deserialize(in byte[] _buffer, int offset = 0)
        {
            offset += sizeof(int);
            _input = (KeyCode)BitConverter.ToInt32(_buffer, offset);
            return offset;
        }
    }

    public class S_MOVE : PacketBase
    {
        public KeyCode _input;

        public S_MOVE() { }
        public S_MOVE(in byte[] _buffer, int offset = 0)
        {
            offset += sizeof(int) + sizeof(PacketType);
            _input = (KeyCode)BitConverter.ToInt32(_buffer, offset);
        }
        public int GetSize()
        {
            int size = sizeof(KeyCode);
            return size;
        }

        public override PacketType GetPacketType()
        {
            return PacketType.PACKET_TYPE_S_MOVE;
        }

        public override void Serialize(List<byte> InBuffer)
        {
            int size = GetSize() + sizeof(KeyCode) + sizeof(PacketType);
            InBuffer.AddRange(BitConverter.GetBytes(size));
            InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
            InBuffer.AddRange(BitConverter.GetBytes((int)_input));
        }

        public int Deserialize(in byte[] _buffer, int offset = 0)
        {
            offset += sizeof(int);
            _input = (KeyCode)BitConverter.ToInt32(_buffer, offset);
            return offset;
        }
    }
}