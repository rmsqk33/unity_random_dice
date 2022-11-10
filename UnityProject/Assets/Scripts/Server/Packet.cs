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

		public C_REQUEST_LOGIN()
		{
		}
		public int GetSize()
		{
			int size = 0;
			size += sizeof(int);
			size += sizeof(char) * id.Length; 
			return size;
		}

		public override PacketType GetPacketType()
		{
			return PacketType.PACKET_TYPE_C_REQUEST_LOGIN;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
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
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
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
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
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
			size += sizeof(int);
			size += sizeof(char) * id.Length; 
			return size;
		}

		public override PacketType GetPacketType()
		{
			return PacketType.PACKET_TYPE_S_ADD_GUEST_ACCOUNT;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
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

	public class S_USER_DATA : PacketBase
	{
		public class S_DICE_DATA
		{
			public int id;

			public int level;

			public int count;

			public S_DICE_DATA()
			{
			}
			public int GetSize()
			{
				int size = 0;
				size += sizeof(int);
				size += sizeof(int);
				size += sizeof(int);
				return size;
			}

			public void Serialize(List<byte> InBuffer)
			{
				InBuffer.AddRange(BitConverter.GetBytes(id));
				InBuffer.AddRange(BitConverter.GetBytes(level));
				InBuffer.AddRange(BitConverter.GetBytes(count));
			}

			public int Deserialize(in byte[] InBuffer, int offset = 0)
			{
				id = BitConverter.ToInt32(InBuffer, offset);
				offset += sizeof(int);
				level = BitConverter.ToInt32(InBuffer, offset);
				offset += sizeof(int);
				count = BitConverter.ToInt32(InBuffer, offset);
				offset += sizeof(int);
				return offset;
			}

		}

		public class S_BATTLEFIELD_DATA
		{
			public int id;

			public int level;

			public S_BATTLEFIELD_DATA()
			{
			}
			public int GetSize()
			{
				int size = 0;
				size += sizeof(int);
				size += sizeof(int);
				return size;
			}

			public void Serialize(List<byte> InBuffer)
			{
				InBuffer.AddRange(BitConverter.GetBytes(id));
				InBuffer.AddRange(BitConverter.GetBytes(level));
			}

			public int Deserialize(in byte[] InBuffer, int offset = 0)
			{
				id = BitConverter.ToInt32(InBuffer, offset);
				offset += sizeof(int);
				level = BitConverter.ToInt32(InBuffer, offset);
				offset += sizeof(int);
				return offset;
			}

		}

		public string name = new string("");

		public int level;

		public int dia;

		public int gold;

		public int exp;

		public S_DICE_DATA[] diceDataList = new S_DICE_DATA[50];

		public int[,] dicePreset = new int[5,5];

		public S_BATTLEFIELD_DATA[] battleFieldDataList = new S_BATTLEFIELD_DATA[50];

		public int[] battleFieldPreset = new int[5];

		public int selectedPresetIndex;

		public S_USER_DATA()
		{
			for(int i = 0; i < 50; ++i)
			{
				diceDataList[i] = new S_DICE_DATA();
			}
			for(int i = 0; i < 50; ++i)
			{
				battleFieldDataList[i] = new S_BATTLEFIELD_DATA();
			}
		}
		public int GetSize()
		{
			int size = 0;
			size += sizeof(int);
			size += sizeof(char) * name.Length; 
			size += sizeof(int);
			size += sizeof(int);
			size += sizeof(int);
			size += sizeof(int);
			for(int i = 0; i < 50; ++i)
			{
				size += diceDataList[i].GetSize();
			}
			size += sizeof(int) * 25;
			for(int i = 0; i < 50; ++i)
			{
				size += battleFieldDataList[i].GetSize();
			}
			size += sizeof(int) * 5;
			size += sizeof(int);
			return size;
		}

		public override PacketType GetPacketType()
		{
			return PacketType.PACKET_TYPE_S_USER_DATA;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.AddRange(BitConverter.GetBytes(name.Length));
			InBuffer.AddRange(Encoding.Unicode.GetBytes(name));
			InBuffer.AddRange(BitConverter.GetBytes(level));
			InBuffer.AddRange(BitConverter.GetBytes(dia));
			InBuffer.AddRange(BitConverter.GetBytes(gold));
			InBuffer.AddRange(BitConverter.GetBytes(exp));
			for(int i = 0; i < 50; ++i)
			{
				diceDataList[i].Serialize(InBuffer); 
			}
			for(int i = 0; i < 5; ++i)
			{
				for(int j = 0; j < 5; ++j)
				{
					InBuffer.AddRange(BitConverter.GetBytes(dicePreset[i,j]));
				}
			}
			for(int i = 0; i < 50; ++i)
			{
				battleFieldDataList[i].Serialize(InBuffer); 
			}
			for(int i = 0; i < 5; ++i)
			{
				InBuffer.AddRange(BitConverter.GetBytes(battleFieldPreset[i]));
			}
			InBuffer.AddRange(BitConverter.GetBytes(selectedPresetIndex));
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			int name_length = BitConverter.ToInt32(InBuffer, offset) * sizeof(char);
			offset += sizeof(int);
			name = Encoding.Unicode.GetString(InBuffer, offset, name_length);
			offset += name_length;
			level = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			dia = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			gold = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			exp = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			for(int i = 0; i < 50; ++i)
			{
				offset = diceDataList[i].Deserialize(InBuffer, offset);
			}
			Buffer.BlockCopy(InBuffer, offset, dicePreset, 0, sizeof(int) * 25);
			offset += sizeof(int) * 25;
			for(int i = 0; i < 50; ++i)
			{
				offset = battleFieldDataList[i].Deserialize(InBuffer, offset);
			}
			Buffer.BlockCopy(InBuffer, offset, battleFieldPreset, 0, sizeof(int) * 5);
			offset += sizeof(int) * 5;
			selectedPresetIndex = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			return offset;
		}

	}

}
