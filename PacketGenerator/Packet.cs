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
		PACKET_TYPE_S_USER_DATA,
	}

	public class PacketBase
	{
		public virtual PacketType GetPacketType() { return PacketType.PACKET_TYPE_NONE; }
		public virtual void Serialize(List<byte> InBuffer) { }
	}

	public class C_REQUEST_LOGIN : PacketBase
	{
		public string id = new string("");
		public char[] id = new char]
		public int GetSize()
		{
			int size = 0;
		size += sizeof(int)
		size += sizeof(char) * id.Length; 
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
		InBuffer.AddRange(BitConverter.GetBytes(id.Length))		InBuffer.AddRange(Encoding.Unicode.GetBytes(id))		}

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
		public char[] id = new char]
		public int GetSize()
		{
			int size = 0;
		size += sizeof(int)
		size += sizeof(char) * id.Length; 
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
		InBuffer.AddRange(BitConverter.GetBytes(id.Length))		InBuffer.AddRange(Encoding.Unicode.GetBytes(id))		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
		int id_length = BitConverter.ToInt32(InBuffer, offset) * sizeof(char);
		offset += sizeof(int);
		id = Encoding.Unicode.GetString(InBuffer, offset, id_length);
		offset += id_length;
			return offset;
		}

	}

	public class S_USER_DATA : PacketBase
	{
		public class S_DICE_DATA
		{
			public byte id;

			public byte level;

			public int count;

			public int GetSize()
			{
				int size = 0;
				size += sizeof(byte);
				size += sizeof(byte);
				size += sizeof(int);
				return size;
			}

			public override void Serialize(List<byte> InBuffer)
			{
				if(InBuffer.Count == 0)
				{
					int size = GetSize() + sizeof(int) + sizeof(PacketType);
					InBuffer.AddRange(BitConverter.GetBytes(size));
					InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
				}
				InBuffer.Add(id);
				InBuffer.Add(level);
				InBuffer.AddRange(BitConverter.GetBytes(count));
			}

			public int Deserialize(in byte[] InBuffer, int offset = 0)
			{
				id = InBuffer[offset];
				offset += sizeof(byte);
				level = InBuffer[offset];
				offset += sizeof(byte);
				count = BitConverter.ToInt32(InBuffer, offset);
				offset += sizeof(int);
				return offset;
			}

		}

		public string name = new string("");
		public char[] name = new char]
		public byte level;

		public int dia;

		public int gold;

		public int exp;

		public S_DICE_DATA[,] diceDataList = new S_DICE_DATA[30]
		public byte[,,] dicePreset = new byte[5,5]
		public int GetSize()
		{
			int size = 0;
		size += sizeof(int)
		size += sizeof(char) * name.Length; 
			size += sizeof(byte);
			size += sizeof(int);
			size += sizeof(int);
			size += sizeof(int);
			for(int i = 0; i < 30; ++i)
			{
			size += diceDataList
[i].GetSize();
			}
			size += sizeof(byte) * 25;
			return size;
		}

		public override PacketType GetPacketType()
		{
			return PacketType.PACKET_TYPE_S_USER_DATA;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			if(InBuffer.Count == 0)
			{
				int size = GetSize() + sizeof(int) + sizeof(PacketType);
				InBuffer.AddRange(BitConverter.GetBytes(size));
				InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			}
		InBuffer.AddRange(BitConverter.GetBytes(name.Length))		InBuffer.AddRange(Encoding.Unicode.GetBytes(name))			InBuffer.Add(level);
			InBuffer.AddRange(BitConverter.GetBytes(dia));
			InBuffer.AddRange(BitConverter.GetBytes(gold));
			InBuffer.AddRange(BitConverter.GetBytes(exp));
			for(int i = 0; i < 30; ++i)
			{
								diceDataList[i].Serialize(InBuffer); 
			}
			for(int i = 0; i < 5; ++i)
			{
				for(int j = 0; i < 5; ++i)
				{
				InBuffer.AddRange(dicePreset[i,j].SelectMany(BitConverter.GetBytes).ToArray());
				}
			}
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
		int name_length = BitConverter.ToInt32(InBuffer, offset) * sizeof(char);
		offset += sizeof(int);
		name = Encoding.Unicode.GetString(InBuffer, offset, name_length);
		offset += name_length;
			level = InBuffer[offset];
			offset += sizeof(byte);
			dia = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			gold = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			exp = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			for(int i = 0; i < 30; ++i)
			{
			offset = diceDataList[i].Deserialize(InBuffer, offset);
			}
			Buffer.BlockCopy(InBuffer, offset, dicePreset, 0, sizeof(byte) * 25);
			offset += sizeof(byte) * 25;
			return offset;
		}

	}

}
