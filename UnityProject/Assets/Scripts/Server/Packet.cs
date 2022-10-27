using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace Packet
{
	public enum PacketType
	{
		PACKET_TYPE_NONE,
		PACKET_TYPE_C_REQUEST_LOGIN,
		PACKET_TYPE_S_REQUEST_LOGIN,
		PACKET_TYPE_C_ADD_GUEST_ACCOUNT,
		PACKET_TYPE_S_ADD_GUEST_ACCOUNT,
	}

	public class PacketBase
	{
		public virtual PacketType GetPacketType() { return PacketType.PACKET_TYPE_NONE; }
		public virtual void Serialize(List<byte> InBuffer) { }
	}

	public class C_REQUEST_LOGIN : PacketBase
	{
		public string id = new string("");

		public C_REQUEST_LOGIN()
		{
		}

		public int GetSize()
		{
			int size = 0;
			size += sizeof(int) + sizeof(char) * id.Length;
			return size;
		}

		public override PacketType GetPacketType()
		{
			return PacketType.PACKET_TYPE_C_REQUEST_LOGIN;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			if(InBuffer.Count == 0)
			{
				int size = GetSize() + sizeof(int) + sizeof(PacketType);
				InBuffer.AddRange(BitConverter.GetBytes(size));
				InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			}
			InBuffer.AddRange(BitConverter.GetBytes(id.Length));
			InBuffer.AddRange(Encoding.Unicode.GetBytes(id));
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			int id_length = BitConverter.ToInt32(InBuffer, offset) * sizeof(char);
			offset += sizeof(int);
			id = Encoding.Unicode.GetString(InBuffer, offset, id_length);
			offset += id_length;
			return offset;
		}

	}

	public class S_REQUEST_LOGIN : PacketBase
	{

		public S_REQUEST_LOGIN()
		{
		}

		public int GetSize()
		{
			int size = 0;
			return size;
		}

		public override PacketType GetPacketType()
		{
			return PacketType.PACKET_TYPE_S_REQUEST_LOGIN;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			if(InBuffer.Count == 0)
			{
				int size = GetSize() + sizeof(int) + sizeof(PacketType);
				InBuffer.AddRange(BitConverter.GetBytes(size));
				InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			}
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			return offset;
		}

	}

	public class C_ADD_GUEST_ACCOUNT : PacketBase
	{

		public C_ADD_GUEST_ACCOUNT()
		{
		}

		public int GetSize()
		{
			int size = 0;
			return size;
		}

		public override PacketType GetPacketType()
		{
			return PacketType.PACKET_TYPE_C_ADD_GUEST_ACCOUNT;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			if(InBuffer.Count == 0)
			{
				int size = GetSize() + sizeof(int) + sizeof(PacketType);
				InBuffer.AddRange(BitConverter.GetBytes(size));
				InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			}
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			return offset;
		}

	}

	public class S_ADD_GUEST_ACCOUNT : PacketBase
	{
		public string id = new string("");

		public S_ADD_GUEST_ACCOUNT()
		{
		}

		public int GetSize()
		{
			int size = 0;
			size += sizeof(int) + sizeof(char) * id.Length;
			return size;
		}

		public override PacketType GetPacketType()
		{
			return PacketType.PACKET_TYPE_S_ADD_GUEST_ACCOUNT;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			if(InBuffer.Count == 0)
			{
				int size = GetSize() + sizeof(int) + sizeof(PacketType);
				InBuffer.AddRange(BitConverter.GetBytes(size));
				InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			}
			InBuffer.AddRange(BitConverter.GetBytes(id.Length));
			InBuffer.AddRange(Encoding.Unicode.GetBytes(id));
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			int id_length = BitConverter.ToInt32(InBuffer, offset) * sizeof(char);
			offset += sizeof(int);
			id = Encoding.Unicode.GetString(InBuffer, offset, id_length);
			offset += id_length;
			return offset;
		}

	}

}
