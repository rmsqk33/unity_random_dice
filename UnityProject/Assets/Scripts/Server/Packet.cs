using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace Packet
{
	public enum PacketType
	{
		PACKET_TYPE_NONE,
		PACKET_TYPE_C_GUEST_LOGIN,
		PACKET_TYPE_S_GUEST_LOGIN,
		PACKET_TYPE_C_CREATE_GUEST_ACCOUNT,
		PACKET_TYPE_S_CREATE_GUEST_ACCOUNT,
		PACKET_TYPE_S_USER_DATA,
		PACKET_TYPE_S_STORE_DICE_LIST,
		PACKET_TYPE_C_UPGRADE_DICE,
		PACKET_TYPE_S_UPGRADE_DICE,
		PACKET_TYPE_C_PURCHASE_DICE,
		PACKET_TYPE_S_PURCHASE_DICE,
		PACKET_TYPE_C_PURCHASE_BOX,
		PACKET_TYPE_S_PURCHASE_BOX,
		PACKET_TYPE_S_ADD_DICE,
		PACKET_TYPE_S_CHANGE_GOLD,
		PACKET_TYPE_S_CHANGE_DIA,
		PACKET_TYPE_S_CHANGE_CARD,
		PACKET_TYPE_C_CHANGE_PRESET,
		PACKET_TYPE_C_CHANGE_PRESET_DICE,
		PACKET_TYPE_C_CHANGE_PRESET_BATTLEFIELD,
		PACKET_TYPE_MAX,
	}

	public class PacketBase
	{
		public virtual PacketType GetPacketType() { return PacketType.PACKET_TYPE_NONE; }
		public virtual void Serialize(List<byte> InBuffer) { }
	}

	public class C_GUEST_LOGIN : PacketBase
	{
		public string id = new string("");

		public C_GUEST_LOGIN()
		{
		}

		public C_GUEST_LOGIN(in byte[] InBuffer)
		{
			Deserialize(InBuffer);
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
			return PacketType.PACKET_TYPE_C_GUEST_LOGIN;
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

	public class S_GUEST_LOGIN : PacketBase
	{
		public byte result;

		public S_GUEST_LOGIN()
		{
		}

		public S_GUEST_LOGIN(in byte[] InBuffer)
		{
			Deserialize(InBuffer);
		}

		public int GetSize()
		{
			int size = 0;
			size += sizeof(byte);
			return size;
		}

		public override PacketType GetPacketType()
		{
			return PacketType.PACKET_TYPE_S_GUEST_LOGIN;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.Add(result);
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			result = InBuffer[offset];
			offset += sizeof(byte);
			return offset;
		}

	}

	public class C_CREATE_GUEST_ACCOUNT : PacketBase
	{
		public C_CREATE_GUEST_ACCOUNT()
		{
		}

		public C_CREATE_GUEST_ACCOUNT(in byte[] InBuffer)
		{
			Deserialize(InBuffer);
		}

		public int GetSize()
		{
			int size = 0;
			return size;
		}

		public override PacketType GetPacketType()
		{
			return PacketType.PACKET_TYPE_C_CREATE_GUEST_ACCOUNT;
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

	public class S_CREATE_GUEST_ACCOUNT : PacketBase
	{
		public string id = new string("");

		public S_CREATE_GUEST_ACCOUNT()
		{
		}

		public S_CREATE_GUEST_ACCOUNT(in byte[] InBuffer)
		{
			Deserialize(InBuffer);
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
			return PacketType.PACKET_TYPE_S_CREATE_GUEST_ACCOUNT;
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
		public class DICE_DATA
		{
			public byte id;

			public byte level;

			public int count;

			public DICE_DATA()
			{
			}

			public DICE_DATA(in byte[] InBuffer)
			{
				Deserialize(InBuffer);
			}

			public int GetSize()
			{
				int size = 0;
				size += sizeof(byte);
				size += sizeof(byte);
				size += sizeof(int);
				return size;
			}

			public void Serialize(List<byte> InBuffer)
			{
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

		public byte level;

		public int exp;

		public int gold;

		public int dia;

		public int card;

		public DICE_DATA[] diceDataList = new DICE_DATA[50];

		public byte[,] dicePreset = new byte[5,5];

		public int[] battleFieldIDList = new int[50];

		public byte[] battleFieldPreset = new byte[5];

		public byte selectedPresetIndex;

		public S_USER_DATA()
		{
			for(int i = 0; i < 50; ++i)
			{
				diceDataList[i] = new DICE_DATA();
			}
		}

		public S_USER_DATA(in byte[] InBuffer)
		{
			for(int i = 0; i < 50; ++i)
			{
				diceDataList[i] = new DICE_DATA();
			}
			Deserialize(InBuffer);
		}

		public int GetSize()
		{
			int size = 0;
			size += sizeof(int);
			size += sizeof(char) * name.Length; 
			size += sizeof(byte);
			size += sizeof(int);
			size += sizeof(int);
			size += sizeof(int);
			size += sizeof(int);
			for(int i = 0; i < 50; ++i)
			{
				size += diceDataList[i].GetSize();
			}
			size += sizeof(byte) * 25;
			size += sizeof(int) * 50;
			size += sizeof(byte) * 5;
			size += sizeof(byte);
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
			InBuffer.Add(level);
			InBuffer.AddRange(BitConverter.GetBytes(exp));
			InBuffer.AddRange(BitConverter.GetBytes(gold));
			InBuffer.AddRange(BitConverter.GetBytes(dia));
			InBuffer.AddRange(BitConverter.GetBytes(card));
			for(int i = 0; i < 50; ++i)
			{
				diceDataList[i].Serialize(InBuffer); 
			}
			for(int i = 0; i < 5; ++i)
			{
				for(int j = 0; j < 5; ++j)
				{
					InBuffer.Add(dicePreset[i,j]);
				}
			}
			for(int i = 0; i < 50; ++i)
			{
				InBuffer.AddRange(BitConverter.GetBytes(battleFieldIDList[i]));
			}
			for(int i = 0; i < 5; ++i)
			{
				InBuffer.Add(battleFieldPreset[i]);
			}
			InBuffer.Add(selectedPresetIndex);
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			int name_length = BitConverter.ToInt32(InBuffer, offset) * sizeof(char);
			offset += sizeof(int);
			name = Encoding.Unicode.GetString(InBuffer, offset, name_length);
			offset += name_length;
			level = InBuffer[offset];
			offset += sizeof(byte);
			exp = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			gold = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			dia = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			card = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			for(int i = 0; i < 50; ++i)
			{
				offset = diceDataList[i].Deserialize(InBuffer, offset);
			}
			Buffer.BlockCopy(InBuffer, offset, dicePreset, 0, sizeof(byte) * 25);
			offset += sizeof(byte) * 25;
			Buffer.BlockCopy(InBuffer, offset, battleFieldIDList, 0, sizeof(int) * 50);
			offset += sizeof(int) * 50;
			Buffer.BlockCopy(InBuffer, offset, battleFieldPreset, 0, sizeof(byte) * 5);
			offset += sizeof(byte) * 5;
			selectedPresetIndex = InBuffer[offset];
			offset += sizeof(byte);
			return offset;
		}

	}

	public class S_STORE_DICE_LIST : PacketBase
	{
		public class DICE_DATA
		{
			public int id;

			public int price;

			public int count;

			public bool soldOut;

			public DICE_DATA()
			{
			}

			public DICE_DATA(in byte[] InBuffer)
			{
				Deserialize(InBuffer);
			}

			public int GetSize()
			{
				int size = 0;
				size += sizeof(int);
				size += sizeof(int);
				size += sizeof(int);
				size += sizeof(bool);
				return size;
			}

			public void Serialize(List<byte> InBuffer)
			{
				InBuffer.AddRange(BitConverter.GetBytes(id));
				InBuffer.AddRange(BitConverter.GetBytes(price));
				InBuffer.AddRange(BitConverter.GetBytes(count));
				InBuffer.AddRange(BitConverter.GetBytes(soldOut));
			}

			public int Deserialize(in byte[] InBuffer, int offset = 0)
			{
				id = BitConverter.ToInt32(InBuffer, offset);
				offset += sizeof(int);
				price = BitConverter.ToInt32(InBuffer, offset);
				offset += sizeof(int);
				count = BitConverter.ToInt32(InBuffer, offset);
				offset += sizeof(int);
				soldOut = BitConverter.ToBoolean(InBuffer, offset);
				offset += sizeof(bool);
				return offset;
			}

		}

		public Int64 resetTime;

		public int diceCount;

		public DICE_DATA[] diceList = new DICE_DATA[10];

		public S_STORE_DICE_LIST()
		{
			for(int i = 0; i < 10; ++i)
			{
				diceList[i] = new DICE_DATA();
			}
		}

		public S_STORE_DICE_LIST(in byte[] InBuffer)
		{
			for(int i = 0; i < 10; ++i)
			{
				diceList[i] = new DICE_DATA();
			}
			Deserialize(InBuffer);
		}

		public int GetSize()
		{
			int size = 0;
			size += sizeof(Int64);
			size += sizeof(int);
			for(int i = 0; i < 10; ++i)
			{
				size += diceList[i].GetSize();
			}
			return size;
		}

		public override PacketType GetPacketType()
		{
			return PacketType.PACKET_TYPE_S_STORE_DICE_LIST;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.AddRange(BitConverter.GetBytes(resetTime));
			InBuffer.AddRange(BitConverter.GetBytes(diceCount));
			for(int i = 0; i < 10; ++i)
			{
				diceList[i].Serialize(InBuffer); 
			}
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			resetTime = BitConverter.ToInt64(InBuffer, offset);
			offset += sizeof(Int64);
			diceCount = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			for(int i = 0; i < 10; ++i)
			{
				offset = diceList[i].Deserialize(InBuffer, offset);
			}
			return offset;
		}

	}

	public class C_UPGRADE_DICE : PacketBase
	{
		public int id;

		public C_UPGRADE_DICE()
		{
		}

		public C_UPGRADE_DICE(in byte[] InBuffer)
		{
			Deserialize(InBuffer);
		}

		public int GetSize()
		{
			int size = 0;
			size += sizeof(int);
			return size;
		}

		public override PacketType GetPacketType()
		{
			return PacketType.PACKET_TYPE_C_UPGRADE_DICE;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.AddRange(BitConverter.GetBytes(id));
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			id = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			return offset;
		}

	}

	public class S_UPGRADE_DICE : PacketBase
	{
		public int id;

		public int level;

		public int count;

		public int resultType;

		public S_UPGRADE_DICE()
		{
		}

		public S_UPGRADE_DICE(in byte[] InBuffer)
		{
			Deserialize(InBuffer);
		}

		public int GetSize()
		{
			int size = 0;
			size += sizeof(int);
			size += sizeof(int);
			size += sizeof(int);
			size += sizeof(int);
			return size;
		}

		public override PacketType GetPacketType()
		{
			return PacketType.PACKET_TYPE_S_UPGRADE_DICE;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.AddRange(BitConverter.GetBytes(id));
			InBuffer.AddRange(BitConverter.GetBytes(level));
			InBuffer.AddRange(BitConverter.GetBytes(count));
			InBuffer.AddRange(BitConverter.GetBytes(resultType));
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			id = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			level = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			count = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			resultType = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			return offset;
		}

	}

	public class C_PURCHASE_DICE : PacketBase
	{
		public int id;

		public C_PURCHASE_DICE()
		{
		}

		public C_PURCHASE_DICE(in byte[] InBuffer)
		{
			Deserialize(InBuffer);
		}

		public int GetSize()
		{
			int size = 0;
			size += sizeof(int);
			return size;
		}

		public override PacketType GetPacketType()
		{
			return PacketType.PACKET_TYPE_C_PURCHASE_DICE;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.AddRange(BitConverter.GetBytes(id));
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			id = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			return offset;
		}

	}

	public class S_PURCHASE_DICE : PacketBase
	{
		public int id;

		public int resultType;

		public S_PURCHASE_DICE()
		{
		}

		public S_PURCHASE_DICE(in byte[] InBuffer)
		{
			Deserialize(InBuffer);
		}

		public int GetSize()
		{
			int size = 0;
			size += sizeof(int);
			size += sizeof(int);
			return size;
		}

		public override PacketType GetPacketType()
		{
			return PacketType.PACKET_TYPE_S_PURCHASE_DICE;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.AddRange(BitConverter.GetBytes(id));
			InBuffer.AddRange(BitConverter.GetBytes(resultType));
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			id = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			resultType = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			return offset;
		}

	}

	public class C_PURCHASE_BOX : PacketBase
	{
		public int id;

		public C_PURCHASE_BOX()
		{
		}

		public C_PURCHASE_BOX(in byte[] InBuffer)
		{
			Deserialize(InBuffer);
		}

		public int GetSize()
		{
			int size = 0;
			size += sizeof(int);
			return size;
		}

		public override PacketType GetPacketType()
		{
			return PacketType.PACKET_TYPE_C_PURCHASE_BOX;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.AddRange(BitConverter.GetBytes(id));
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			id = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			return offset;
		}

	}

	public class S_PURCHASE_BOX : PacketBase
	{
		public int resultType;

		public S_PURCHASE_BOX()
		{
		}

		public S_PURCHASE_BOX(in byte[] InBuffer)
		{
			Deserialize(InBuffer);
		}

		public int GetSize()
		{
			int size = 0;
			size += sizeof(int);
			return size;
		}

		public override PacketType GetPacketType()
		{
			return PacketType.PACKET_TYPE_S_PURCHASE_BOX;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.AddRange(BitConverter.GetBytes(resultType));
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			resultType = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			return offset;
		}

	}

	public class S_ADD_DICE : PacketBase
	{
		public class DICE_DATA
		{
			public int id;

			public int count;

			public DICE_DATA()
			{
			}

			public DICE_DATA(in byte[] InBuffer)
			{
				Deserialize(InBuffer);
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
				InBuffer.AddRange(BitConverter.GetBytes(count));
			}

			public int Deserialize(in byte[] InBuffer, int offset = 0)
			{
				id = BitConverter.ToInt32(InBuffer, offset);
				offset += sizeof(int);
				count = BitConverter.ToInt32(InBuffer, offset);
				offset += sizeof(int);
				return offset;
			}

		}

		public int diceCount;

		public DICE_DATA[] diceList = new DICE_DATA[10];

		public S_ADD_DICE()
		{
			for(int i = 0; i < 10; ++i)
			{
				diceList[i] = new DICE_DATA();
			}
		}

		public S_ADD_DICE(in byte[] InBuffer)
		{
			for(int i = 0; i < 10; ++i)
			{
				diceList[i] = new DICE_DATA();
			}
			Deserialize(InBuffer);
		}

		public int GetSize()
		{
			int size = 0;
			size += sizeof(int);
			for(int i = 0; i < 10; ++i)
			{
				size += diceList[i].GetSize();
			}
			return size;
		}

		public override PacketType GetPacketType()
		{
			return PacketType.PACKET_TYPE_S_ADD_DICE;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.AddRange(BitConverter.GetBytes(diceCount));
			for(int i = 0; i < 10; ++i)
			{
				diceList[i].Serialize(InBuffer); 
			}
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			diceCount = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			for(int i = 0; i < 10; ++i)
			{
				offset = diceList[i].Deserialize(InBuffer, offset);
			}
			return offset;
		}

	}

	public class S_CHANGE_GOLD : PacketBase
	{
		public int gold;

		public S_CHANGE_GOLD()
		{
		}

		public S_CHANGE_GOLD(in byte[] InBuffer)
		{
			Deserialize(InBuffer);
		}

		public int GetSize()
		{
			int size = 0;
			size += sizeof(int);
			return size;
		}

		public override PacketType GetPacketType()
		{
			return PacketType.PACKET_TYPE_S_CHANGE_GOLD;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.AddRange(BitConverter.GetBytes(gold));
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			gold = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			return offset;
		}

	}

	public class S_CHANGE_DIA : PacketBase
	{
		public int dia;

		public S_CHANGE_DIA()
		{
		}

		public S_CHANGE_DIA(in byte[] InBuffer)
		{
			Deserialize(InBuffer);
		}

		public int GetSize()
		{
			int size = 0;
			size += sizeof(int);
			return size;
		}

		public override PacketType GetPacketType()
		{
			return PacketType.PACKET_TYPE_S_CHANGE_DIA;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.AddRange(BitConverter.GetBytes(dia));
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			dia = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			return offset;
		}

	}

	public class S_CHANGE_CARD : PacketBase
	{
		public int card;

		public S_CHANGE_CARD()
		{
		}

		public S_CHANGE_CARD(in byte[] InBuffer)
		{
			Deserialize(InBuffer);
		}

		public int GetSize()
		{
			int size = 0;
			size += sizeof(int);
			return size;
		}

		public override PacketType GetPacketType()
		{
			return PacketType.PACKET_TYPE_S_CHANGE_CARD;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.AddRange(BitConverter.GetBytes(card));
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			card = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			return offset;
		}

	}

	public class C_CHANGE_PRESET : PacketBase
	{
		public int presetIndex;

		public C_CHANGE_PRESET()
		{
		}

		public C_CHANGE_PRESET(in byte[] InBuffer)
		{
			Deserialize(InBuffer);
		}

		public int GetSize()
		{
			int size = 0;
			size += sizeof(int);
			return size;
		}

		public override PacketType GetPacketType()
		{
			return PacketType.PACKET_TYPE_C_CHANGE_PRESET;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.AddRange(BitConverter.GetBytes(presetIndex));
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			presetIndex = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			return offset;
		}

	}

	public class C_CHANGE_PRESET_DICE : PacketBase
	{
		public int presetIndex;

		public int slotIndex;

		public int diceId;

		public C_CHANGE_PRESET_DICE()
		{
		}

		public C_CHANGE_PRESET_DICE(in byte[] InBuffer)
		{
			Deserialize(InBuffer);
		}

		public int GetSize()
		{
			int size = 0;
			size += sizeof(int);
			size += sizeof(int);
			size += sizeof(int);
			return size;
		}

		public override PacketType GetPacketType()
		{
			return PacketType.PACKET_TYPE_C_CHANGE_PRESET_DICE;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.AddRange(BitConverter.GetBytes(presetIndex));
			InBuffer.AddRange(BitConverter.GetBytes(slotIndex));
			InBuffer.AddRange(BitConverter.GetBytes(diceId));
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			presetIndex = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			slotIndex = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			diceId = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			return offset;
		}

	}

	public class C_CHANGE_PRESET_BATTLEFIELD : PacketBase
	{
		public int presetIndex;

		public int battlefieldId;

		public C_CHANGE_PRESET_BATTLEFIELD()
		{
		}

		public C_CHANGE_PRESET_BATTLEFIELD(in byte[] InBuffer)
		{
			Deserialize(InBuffer);
		}

		public int GetSize()
		{
			int size = 0;
			size += sizeof(int);
			size += sizeof(int);
			return size;
		}

		public override PacketType GetPacketType()
		{
			return PacketType.PACKET_TYPE_C_CHANGE_PRESET_BATTLEFIELD;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.AddRange(BitConverter.GetBytes(presetIndex));
			InBuffer.AddRange(BitConverter.GetBytes(battlefieldId));
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			presetIndex = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			battlefieldId = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			return offset;
		}

	}

}
