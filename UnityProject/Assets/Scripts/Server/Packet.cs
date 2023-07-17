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
		PACKET_TYPE_C_PURCHASE_BATTLEFIELD,
		PACKET_TYPE_S_PURCHASE_BATTLEFIELD,
		PACKET_TYPE_S_ADD_DICE,
		PACKET_TYPE_S_CHANGE_GOLD,
		PACKET_TYPE_S_CHANGE_DIA,
		PACKET_TYPE_S_CHANGE_CARD,
		PACKET_TYPE_C_CHANGE_PRESET,
		PACKET_TYPE_C_CHANGE_PRESET_DICE,
		PACKET_TYPE_C_CHANGE_PRESET_BATTLEFIELD,
		PACKET_TYPE_C_CHANGE_NAME,
		PACKET_TYPE_S_CHANGE_NAME,
		PACKET_TYPE_C_BATTLE_RESULT,
		PACKET_TYPE_S_CHANGE_EXP,
		PACKET_TYPE_S_CHANGE_LEVEL,
		PACKET_TYPE_C_BATTLE_MATCHING,
		PACKET_TYPE_C_BATTLE_MATCHING_CANCEL,
		PACKET_TYPE_S_BATTLE_MATCHING,
		PACKET_TYPE_P2P_PLAYER_DATA,
		PACKET_TYPE_P2P_CHANGE_DICE_LEVEL,
		PACKET_TYPE_P2P_SPAWN_ENEMY,
		PACKET_TYPE_P2P_DAMAGE,
		PACKET_TYPE_P2P_SPAWN_REMOTE_DICE,
		PACKET_TYPE_P2P_C_REQUEST_SPAWN_DICE,
		PACKET_TYPE_P2P_S_REQUEST_SPAWN_DICE,
		PACKET_TYPE_P2P_DESPAWN_OBJECT,
		PACKET_TYPE_P2P_CHANGE_LIFE,
		PACKET_TYPE_P2P_START_BATTLE,
		PACKET_TYPE_P2P_CHANGE_WAVE,
		PACKET_TYPE_P2P_READY_BATTLE,
		PACKET_TYPE_P2P_ON_SKILL,
		PACKET_TYPE_P2P_USE_SKILL_IN_PATH,
		PACKET_TYPE_P2P_OFF_SKILL,
		PACKET_TYPE_P2P_SPAWN_COLLISION_OBJECT,
		PACKET_TYPE_P2P_REQUEST_COLLISION_OBJECT,
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

	public class C_PURCHASE_BATTLEFIELD : PacketBase
	{
		public int id;

		public C_PURCHASE_BATTLEFIELD()
		{
		}

		public C_PURCHASE_BATTLEFIELD(in byte[] InBuffer)
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
			return PacketType.PACKET_TYPE_C_PURCHASE_BATTLEFIELD;
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

	public class S_PURCHASE_BATTLEFIELD : PacketBase
	{
		public int id;

		public int resultType;

		public S_PURCHASE_BATTLEFIELD()
		{
		}

		public S_PURCHASE_BATTLEFIELD(in byte[] InBuffer)
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
			return PacketType.PACKET_TYPE_S_PURCHASE_BATTLEFIELD;
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

	public class C_CHANGE_NAME : PacketBase
	{
		public string name = new string("");

		public C_CHANGE_NAME()
		{
		}

		public C_CHANGE_NAME(in byte[] InBuffer)
		{
			Deserialize(InBuffer);
		}

		public int GetSize()
		{
			int size = 0;
			size += sizeof(int);
			size += sizeof(char) * name.Length; 
			return size;
		}

		public override PacketType GetPacketType()
		{
			return PacketType.PACKET_TYPE_C_CHANGE_NAME;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.AddRange(BitConverter.GetBytes(name.Length));
			InBuffer.AddRange(Encoding.Unicode.GetBytes(name));
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			int name_length = BitConverter.ToInt32(InBuffer, offset) * sizeof(char);
			offset += sizeof(int);
			name = Encoding.Unicode.GetString(InBuffer, offset, name_length);
			offset += name_length;
			return offset;
		}

	}

	public class S_CHANGE_NAME : PacketBase
	{
		public int resultType;

		public string name = new string("");

		public S_CHANGE_NAME()
		{
		}

		public S_CHANGE_NAME(in byte[] InBuffer)
		{
			Deserialize(InBuffer);
		}

		public int GetSize()
		{
			int size = 0;
			size += sizeof(int);
			size += sizeof(int);
			size += sizeof(char) * name.Length; 
			return size;
		}

		public override PacketType GetPacketType()
		{
			return PacketType.PACKET_TYPE_S_CHANGE_NAME;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.AddRange(BitConverter.GetBytes(resultType));
			InBuffer.AddRange(BitConverter.GetBytes(name.Length));
			InBuffer.AddRange(Encoding.Unicode.GetBytes(name));
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			resultType = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			int name_length = BitConverter.ToInt32(InBuffer, offset) * sizeof(char);
			offset += sizeof(int);
			name = Encoding.Unicode.GetString(InBuffer, offset, name_length);
			offset += name_length;
			return offset;
		}

	}

	public class C_BATTLE_RESULT : PacketBase
	{
		public int battleId;

		public int clearWave;

		public C_BATTLE_RESULT()
		{
		}

		public C_BATTLE_RESULT(in byte[] InBuffer)
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
			return PacketType.PACKET_TYPE_C_BATTLE_RESULT;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.AddRange(BitConverter.GetBytes(battleId));
			InBuffer.AddRange(BitConverter.GetBytes(clearWave));
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			battleId = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			clearWave = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			return offset;
		}

	}

	public class S_CHANGE_EXP : PacketBase
	{
		public int exp;

		public S_CHANGE_EXP()
		{
		}

		public S_CHANGE_EXP(in byte[] InBuffer)
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
			return PacketType.PACKET_TYPE_S_CHANGE_EXP;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.AddRange(BitConverter.GetBytes(exp));
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			exp = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			return offset;
		}

	}

	public class S_CHANGE_LEVEL : PacketBase
	{
		public int level;

		public S_CHANGE_LEVEL()
		{
		}

		public S_CHANGE_LEVEL(in byte[] InBuffer)
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
			return PacketType.PACKET_TYPE_S_CHANGE_LEVEL;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.AddRange(BitConverter.GetBytes(level));
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			level = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			return offset;
		}

	}

	public class C_BATTLE_MATCHING : PacketBase
	{
		public C_BATTLE_MATCHING()
		{
		}

		public C_BATTLE_MATCHING(in byte[] InBuffer)
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
			return PacketType.PACKET_TYPE_C_BATTLE_MATCHING;
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

	public class C_BATTLE_MATCHING_CANCEL : PacketBase
	{
		public C_BATTLE_MATCHING_CANCEL()
		{
		}

		public C_BATTLE_MATCHING_CANCEL(in byte[] InBuffer)
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
			return PacketType.PACKET_TYPE_C_BATTLE_MATCHING_CANCEL;
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

	public class S_BATTLE_MATCHING : PacketBase
	{
		public bool isHost;

		public string hostIP = new string("");

		public S_BATTLE_MATCHING()
		{
		}

		public S_BATTLE_MATCHING(in byte[] InBuffer)
		{
			Deserialize(InBuffer);
		}

		public int GetSize()
		{
			int size = 0;
			size += sizeof(bool);
			size += sizeof(int);
			size += sizeof(char) * hostIP.Length; 
			return size;
		}

		public override PacketType GetPacketType()
		{
			return PacketType.PACKET_TYPE_S_BATTLE_MATCHING;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.AddRange(BitConverter.GetBytes(isHost));
			InBuffer.AddRange(BitConverter.GetBytes(hostIP.Length));
			InBuffer.AddRange(Encoding.Unicode.GetBytes(hostIP));
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			isHost = BitConverter.ToBoolean(InBuffer, offset);
			offset += sizeof(bool);
			int hostIP_length = BitConverter.ToInt32(InBuffer, offset) * sizeof(char);
			offset += sizeof(int);
			hostIP = Encoding.Unicode.GetString(InBuffer, offset, hostIP_length);
			offset += hostIP_length;
			return offset;
		}

	}

	public class P2P_PLAYER_DATA : PacketBase
	{
		public class DICE_DATA
		{
			public int id;

			public int level;

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

		public float criticalDamageRate;

		public DICE_DATA[] diceList = new DICE_DATA[5];

		public int instanceId;

		public P2P_PLAYER_DATA()
		{
			for(int i = 0; i < 5; ++i)
			{
				diceList[i] = new DICE_DATA();
			}
		}

		public P2P_PLAYER_DATA(in byte[] InBuffer)
		{
			for(int i = 0; i < 5; ++i)
			{
				diceList[i] = new DICE_DATA();
			}
			Deserialize(InBuffer);
		}

		public int GetSize()
		{
			int size = 0;
			size += sizeof(int);
			size += sizeof(char) * name.Length; 
			size += sizeof(int);
			size += sizeof(float);
			for(int i = 0; i < 5; ++i)
			{
				size += diceList[i].GetSize();
			}
			size += sizeof(int);
			return size;
		}

		public override PacketType GetPacketType()
		{
			return PacketType.PACKET_TYPE_P2P_PLAYER_DATA;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.AddRange(BitConverter.GetBytes(name.Length));
			InBuffer.AddRange(Encoding.Unicode.GetBytes(name));
			InBuffer.AddRange(BitConverter.GetBytes(level));
			InBuffer.AddRange(BitConverter.GetBytes(criticalDamageRate));
			for(int i = 0; i < 5; ++i)
			{
				diceList[i].Serialize(InBuffer); 
			}
			InBuffer.AddRange(BitConverter.GetBytes(instanceId));
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			int name_length = BitConverter.ToInt32(InBuffer, offset) * sizeof(char);
			offset += sizeof(int);
			name = Encoding.Unicode.GetString(InBuffer, offset, name_length);
			offset += name_length;
			level = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			criticalDamageRate = BitConverter.ToSingle(InBuffer, offset);
			offset += sizeof(float);
			for(int i = 0; i < 5; ++i)
			{
				offset = diceList[i].Deserialize(InBuffer, offset);
			}
			instanceId = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			return offset;
		}

	}

	public class P2P_CHANGE_DICE_LEVEL : PacketBase
	{
		public int index;

		public int level;

		public P2P_CHANGE_DICE_LEVEL()
		{
		}

		public P2P_CHANGE_DICE_LEVEL(in byte[] InBuffer)
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
			return PacketType.PACKET_TYPE_P2P_CHANGE_DICE_LEVEL;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.AddRange(BitConverter.GetBytes(index));
			InBuffer.AddRange(BitConverter.GetBytes(level));
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			index = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			level = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			return offset;
		}

	}

	public class P2P_SPAWN_ENEMY : PacketBase
	{
		public int instanceId;

		public int enemyId;

		public int ownerId;

		public P2P_SPAWN_ENEMY()
		{
		}

		public P2P_SPAWN_ENEMY(in byte[] InBuffer)
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
			return PacketType.PACKET_TYPE_P2P_SPAWN_ENEMY;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.AddRange(BitConverter.GetBytes(instanceId));
			InBuffer.AddRange(BitConverter.GetBytes(enemyId));
			InBuffer.AddRange(BitConverter.GetBytes(ownerId));
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			instanceId = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			enemyId = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			ownerId = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			return offset;
		}

	}

	public class P2P_DAMAGE : PacketBase
	{
		public int objectId;

		public int damage;

		public bool critical;

		public P2P_DAMAGE()
		{
		}

		public P2P_DAMAGE(in byte[] InBuffer)
		{
			Deserialize(InBuffer);
		}

		public int GetSize()
		{
			int size = 0;
			size += sizeof(int);
			size += sizeof(int);
			size += sizeof(bool);
			return size;
		}

		public override PacketType GetPacketType()
		{
			return PacketType.PACKET_TYPE_P2P_DAMAGE;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.AddRange(BitConverter.GetBytes(objectId));
			InBuffer.AddRange(BitConverter.GetBytes(damage));
			InBuffer.AddRange(BitConverter.GetBytes(critical));
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			objectId = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			damage = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			critical = BitConverter.ToBoolean(InBuffer, offset);
			offset += sizeof(bool);
			return offset;
		}

	}

	public class P2P_SPAWN_REMOTE_DICE : PacketBase
	{
		public int objectId;

		public int index;

		public int diceId;

		public int eyeCount;

		public P2P_SPAWN_REMOTE_DICE()
		{
		}

		public P2P_SPAWN_REMOTE_DICE(in byte[] InBuffer)
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
			return PacketType.PACKET_TYPE_P2P_SPAWN_REMOTE_DICE;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.AddRange(BitConverter.GetBytes(objectId));
			InBuffer.AddRange(BitConverter.GetBytes(index));
			InBuffer.AddRange(BitConverter.GetBytes(diceId));
			InBuffer.AddRange(BitConverter.GetBytes(eyeCount));
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			objectId = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			index = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			diceId = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			eyeCount = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			return offset;
		}

	}

	public class P2P_C_REQUEST_SPAWN_DICE : PacketBase
	{
		public int index;

		public int diceId;

		public int eyeCount;

		public P2P_C_REQUEST_SPAWN_DICE()
		{
		}

		public P2P_C_REQUEST_SPAWN_DICE(in byte[] InBuffer)
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
			return PacketType.PACKET_TYPE_P2P_C_REQUEST_SPAWN_DICE;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.AddRange(BitConverter.GetBytes(index));
			InBuffer.AddRange(BitConverter.GetBytes(diceId));
			InBuffer.AddRange(BitConverter.GetBytes(eyeCount));
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			index = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			diceId = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			eyeCount = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			return offset;
		}

	}

	public class P2P_S_REQUEST_SPAWN_DICE : PacketBase
	{
		public int objectId;

		public int index;

		public int diceId;

		public int eyeCount;

		public P2P_S_REQUEST_SPAWN_DICE()
		{
		}

		public P2P_S_REQUEST_SPAWN_DICE(in byte[] InBuffer)
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
			return PacketType.PACKET_TYPE_P2P_S_REQUEST_SPAWN_DICE;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.AddRange(BitConverter.GetBytes(objectId));
			InBuffer.AddRange(BitConverter.GetBytes(index));
			InBuffer.AddRange(BitConverter.GetBytes(diceId));
			InBuffer.AddRange(BitConverter.GetBytes(eyeCount));
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			objectId = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			index = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			diceId = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			eyeCount = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			return offset;
		}

	}

	public class P2P_DESPAWN_OBJECT : PacketBase
	{
		public int objectId;

		public P2P_DESPAWN_OBJECT()
		{
		}

		public P2P_DESPAWN_OBJECT(in byte[] InBuffer)
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
			return PacketType.PACKET_TYPE_P2P_DESPAWN_OBJECT;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.AddRange(BitConverter.GetBytes(objectId));
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			objectId = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			return offset;
		}

	}

	public class P2P_CHANGE_LIFE : PacketBase
	{
		public int life;

		public P2P_CHANGE_LIFE()
		{
		}

		public P2P_CHANGE_LIFE(in byte[] InBuffer)
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
			return PacketType.PACKET_TYPE_P2P_CHANGE_LIFE;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.AddRange(BitConverter.GetBytes(life));
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			life = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			return offset;
		}

	}

	public class P2P_START_BATTLE : PacketBase
	{
		public int battleId;

		public P2P_START_BATTLE()
		{
		}

		public P2P_START_BATTLE(in byte[] InBuffer)
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
			return PacketType.PACKET_TYPE_P2P_START_BATTLE;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.AddRange(BitConverter.GetBytes(battleId));
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			battleId = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			return offset;
		}

	}

	public class P2P_CHANGE_WAVE : PacketBase
	{
		public int wave;

		public P2P_CHANGE_WAVE()
		{
		}

		public P2P_CHANGE_WAVE(in byte[] InBuffer)
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
			return PacketType.PACKET_TYPE_P2P_CHANGE_WAVE;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.AddRange(BitConverter.GetBytes(wave));
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			wave = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			return offset;
		}

	}

	public class P2P_READY_BATTLE : PacketBase
	{
		public P2P_READY_BATTLE()
		{
		}

		public P2P_READY_BATTLE(in byte[] InBuffer)
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
			return PacketType.PACKET_TYPE_P2P_READY_BATTLE;
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

	public class P2P_ON_SKILL : PacketBase
	{
		public int objectId;

		public int skillId;

		public int targetId;

		public P2P_ON_SKILL()
		{
		}

		public P2P_ON_SKILL(in byte[] InBuffer)
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
			return PacketType.PACKET_TYPE_P2P_ON_SKILL;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.AddRange(BitConverter.GetBytes(objectId));
			InBuffer.AddRange(BitConverter.GetBytes(skillId));
			InBuffer.AddRange(BitConverter.GetBytes(targetId));
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			objectId = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			skillId = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			targetId = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			return offset;
		}

	}

	public class P2P_USE_SKILL_IN_PATH : PacketBase
	{
		public int objectId;

		public int skillId;

		public int pathIndex;

		public float pathRate;

		public P2P_USE_SKILL_IN_PATH()
		{
		}

		public P2P_USE_SKILL_IN_PATH(in byte[] InBuffer)
		{
			Deserialize(InBuffer);
		}

		public int GetSize()
		{
			int size = 0;
			size += sizeof(int);
			size += sizeof(int);
			size += sizeof(int);
			size += sizeof(float);
			return size;
		}

		public override PacketType GetPacketType()
		{
			return PacketType.PACKET_TYPE_P2P_USE_SKILL_IN_PATH;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.AddRange(BitConverter.GetBytes(objectId));
			InBuffer.AddRange(BitConverter.GetBytes(skillId));
			InBuffer.AddRange(BitConverter.GetBytes(pathIndex));
			InBuffer.AddRange(BitConverter.GetBytes(pathRate));
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			objectId = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			skillId = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			pathIndex = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			pathRate = BitConverter.ToSingle(InBuffer, offset);
			offset += sizeof(float);
			return offset;
		}

	}

	public class P2P_OFF_SKILL : PacketBase
	{
		public int objectId;

		public int skillId;

		public P2P_OFF_SKILL()
		{
		}

		public P2P_OFF_SKILL(in byte[] InBuffer)
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
			return PacketType.PACKET_TYPE_P2P_OFF_SKILL;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.AddRange(BitConverter.GetBytes(objectId));
			InBuffer.AddRange(BitConverter.GetBytes(skillId));
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			objectId = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			skillId = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			return offset;
		}

	}

	public class P2P_SPAWN_COLLISION_OBJECT : PacketBase
	{
		public int objectId;

		public int ownerObjectId;

		public int collisionObjectId;

		public float pathRate;

		public P2P_SPAWN_COLLISION_OBJECT()
		{
		}

		public P2P_SPAWN_COLLISION_OBJECT(in byte[] InBuffer)
		{
			Deserialize(InBuffer);
		}

		public int GetSize()
		{
			int size = 0;
			size += sizeof(int);
			size += sizeof(int);
			size += sizeof(int);
			size += sizeof(float);
			return size;
		}

		public override PacketType GetPacketType()
		{
			return PacketType.PACKET_TYPE_P2P_SPAWN_COLLISION_OBJECT;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.AddRange(BitConverter.GetBytes(objectId));
			InBuffer.AddRange(BitConverter.GetBytes(ownerObjectId));
			InBuffer.AddRange(BitConverter.GetBytes(collisionObjectId));
			InBuffer.AddRange(BitConverter.GetBytes(pathRate));
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			objectId = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			ownerObjectId = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			collisionObjectId = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			pathRate = BitConverter.ToSingle(InBuffer, offset);
			offset += sizeof(float);
			return offset;
		}

	}

	public class P2P_REQUEST_COLLISION_OBJECT : PacketBase
	{
		public int ownerObjectId;

		public int collisionObjectId;

		public float pathRate;

		public P2P_REQUEST_COLLISION_OBJECT()
		{
		}

		public P2P_REQUEST_COLLISION_OBJECT(in byte[] InBuffer)
		{
			Deserialize(InBuffer);
		}

		public int GetSize()
		{
			int size = 0;
			size += sizeof(int);
			size += sizeof(int);
			size += sizeof(float);
			return size;
		}

		public override PacketType GetPacketType()
		{
			return PacketType.PACKET_TYPE_P2P_REQUEST_COLLISION_OBJECT;
		}
		public override void Serialize(List<byte> InBuffer)
		{
			int size = GetSize() + sizeof(int) + sizeof(PacketType);
			InBuffer.AddRange(BitConverter.GetBytes(size));
			InBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));
			InBuffer.AddRange(BitConverter.GetBytes(ownerObjectId));
			InBuffer.AddRange(BitConverter.GetBytes(collisionObjectId));
			InBuffer.AddRange(BitConverter.GetBytes(pathRate));
		}

		public int Deserialize(in byte[] InBuffer, int offset = 0)
		{
			ownerObjectId = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			collisionObjectId = BitConverter.ToInt32(InBuffer, offset);
			offset += sizeof(int);
			pathRate = BitConverter.ToSingle(InBuffer, offset);
			offset += sizeof(float);
			return offset;
		}

	}

}
