#pragma once

enum PacketType
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
};

struct PacketBase
{
	virtual PacketType GetPacketType() const { return PACKET_TYPE_NONE; }
	virtual int Serialize(char* InBuffer) const { return 0; }
};
struct C_GUEST_LOGIN : public PacketBase
{
	wchar_t id[256];

	C_GUEST_LOGIN()
	{
		ZeroMemory(id, sizeof(id));
	}

	C_GUEST_LOGIN(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(int) + sizeof(wchar_t) * wcslen(id);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_C_GUEST_LOGIN;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		int id_length = wcslen(id);
		memcpy(InBuffer + offset, &id_length, sizeof(id_length));
		offset += sizeof(id_length);
		memcpy(InBuffer + offset, id, sizeof(wchar_t) * id_length);
		offset += sizeof(wchar_t) * id_length;
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		int id_length = 0;
		memcpy(&id_length, InBuffer + offset, sizeof(id_length));
		offset += sizeof(id_length);
		memcpy(id, InBuffer + offset, sizeof(wchar_t) * id_length);
		id[id_length] = '\0';
		offset += sizeof(wchar_t) * id_length;
		return offset;
	}

};

struct S_GUEST_LOGIN : public PacketBase
{
	char result = 0;

	S_GUEST_LOGIN()
	{
	}

	S_GUEST_LOGIN(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(result);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_S_GUEST_LOGIN;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		memcpy(InBuffer + offset, &result, sizeof(result));
		offset += sizeof(result);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		memcpy(&result, InBuffer + offset, sizeof(result));
		offset += sizeof(result);
		return offset;
	}

};

struct C_CREATE_GUEST_ACCOUNT : public PacketBase
{

	C_CREATE_GUEST_ACCOUNT()
	{
	}

	C_CREATE_GUEST_ACCOUNT(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_C_CREATE_GUEST_ACCOUNT;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		return offset;
	}

};

struct S_CREATE_GUEST_ACCOUNT : public PacketBase
{
	wchar_t id[256];

	S_CREATE_GUEST_ACCOUNT()
	{
		ZeroMemory(id, sizeof(id));
	}

	S_CREATE_GUEST_ACCOUNT(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(int) + sizeof(wchar_t) * wcslen(id);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_S_CREATE_GUEST_ACCOUNT;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		int id_length = wcslen(id);
		memcpy(InBuffer + offset, &id_length, sizeof(id_length));
		offset += sizeof(id_length);
		memcpy(InBuffer + offset, id, sizeof(wchar_t) * id_length);
		offset += sizeof(wchar_t) * id_length;
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		int id_length = 0;
		memcpy(&id_length, InBuffer + offset, sizeof(id_length));
		offset += sizeof(id_length);
		memcpy(id, InBuffer + offset, sizeof(wchar_t) * id_length);
		id[id_length] = '\0';
		offset += sizeof(wchar_t) * id_length;
		return offset;
	}

};

struct S_USER_DATA : public PacketBase
{
	struct DICE_DATA
	{
		char id = 0;
		char level = 0;
		int count = 0;

		DICE_DATA()
		{
		}

		DICE_DATA(const char* InBuffer)
		{
			Deserialize(InBuffer);
		}

		int GetSize() const
		{
			int size = 0;
			size += sizeof(id);
			size += sizeof(level);
			size += sizeof(count);
			return size;
		}

		int Serialize(char* InBuffer) const 
		{
			int offset = 0;
			memcpy(InBuffer + offset, &id, sizeof(id));
			offset += sizeof(id);
			memcpy(InBuffer + offset, &level, sizeof(level));
			offset += sizeof(level);
			memcpy(InBuffer + offset, &count, sizeof(count));
			offset += sizeof(count);
			return offset;
		}

		int Deserialize(const char* InBuffer)
		{
			int offset = 0;
			memcpy(&id, InBuffer + offset, sizeof(id));
			offset += sizeof(id);
			memcpy(&level, InBuffer + offset, sizeof(level));
			offset += sizeof(level);
			memcpy(&count, InBuffer + offset, sizeof(count));
			offset += sizeof(count);
			return offset;
		}

	};

	wchar_t name[48];
	char level = 0;
	int exp = 0;
	int gold = 0;
	int dia = 0;
	int card = 0;
	DICE_DATA diceDataList[50];
	char dicePreset[5][5];
	int battleFieldIDList[50];
	char battleFieldPreset[5];
	char selectedPresetIndex = 0;

	S_USER_DATA()
	{
		ZeroMemory(name, sizeof(name));
		ZeroMemory(dicePreset, sizeof(dicePreset));
		ZeroMemory(battleFieldIDList, sizeof(battleFieldIDList));
		ZeroMemory(battleFieldPreset, sizeof(battleFieldPreset));
	}

	S_USER_DATA(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(int) + sizeof(wchar_t) * wcslen(name);
		size += sizeof(level);
		size += sizeof(exp);
		size += sizeof(gold);
		size += sizeof(dia);
		size += sizeof(card);
		for(int i = 0; i < 50; ++i)
		{
			size += diceDataList[i].GetSize();
		}
		for(int i = 0; i < 5; ++i)
		{
			for(int j = 0; j < 5; ++j)
			{
				size += sizeof(dicePreset[i][j]);
			}
		}
		for(int i = 0; i < 50; ++i)
		{
			size += sizeof(battleFieldIDList[i]);
		}
		for(int i = 0; i < 5; ++i)
		{
			size += sizeof(battleFieldPreset[i]);
		}
		size += sizeof(selectedPresetIndex);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_S_USER_DATA;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		int name_length = wcslen(name);
		memcpy(InBuffer + offset, &name_length, sizeof(name_length));
		offset += sizeof(name_length);
		memcpy(InBuffer + offset, name, sizeof(wchar_t) * name_length);
		offset += sizeof(wchar_t) * name_length;
		memcpy(InBuffer + offset, &level, sizeof(level));
		offset += sizeof(level);
		memcpy(InBuffer + offset, &exp, sizeof(exp));
		offset += sizeof(exp);
		memcpy(InBuffer + offset, &gold, sizeof(gold));
		offset += sizeof(gold);
		memcpy(InBuffer + offset, &dia, sizeof(dia));
		offset += sizeof(dia);
		memcpy(InBuffer + offset, &card, sizeof(card));
		offset += sizeof(card);
		for(int i = 0; i < 50; ++i)
		{
			offset += diceDataList[i].Serialize(InBuffer + offset);
		}
		memcpy(InBuffer + offset, dicePreset, sizeof(dicePreset));
		offset += sizeof(dicePreset);
		memcpy(InBuffer + offset, battleFieldIDList, sizeof(battleFieldIDList));
		offset += sizeof(battleFieldIDList);
		memcpy(InBuffer + offset, battleFieldPreset, sizeof(battleFieldPreset));
		offset += sizeof(battleFieldPreset);
		memcpy(InBuffer + offset, &selectedPresetIndex, sizeof(selectedPresetIndex));
		offset += sizeof(selectedPresetIndex);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		int name_length = 0;
		memcpy(&name_length, InBuffer + offset, sizeof(name_length));
		offset += sizeof(name_length);
		memcpy(name, InBuffer + offset, sizeof(wchar_t) * name_length);
		name[name_length] = '\0';
		offset += sizeof(wchar_t) * name_length;
		memcpy(&level, InBuffer + offset, sizeof(level));
		offset += sizeof(level);
		memcpy(&exp, InBuffer + offset, sizeof(exp));
		offset += sizeof(exp);
		memcpy(&gold, InBuffer + offset, sizeof(gold));
		offset += sizeof(gold);
		memcpy(&dia, InBuffer + offset, sizeof(dia));
		offset += sizeof(dia);
		memcpy(&card, InBuffer + offset, sizeof(card));
		offset += sizeof(card);
		for(int i = 0; i < 50; ++i)
		{
			offset += diceDataList[i].Deserialize(InBuffer + offset);
		}
		memcpy(dicePreset, InBuffer + offset, sizeof(dicePreset));
		offset += sizeof(dicePreset);
		memcpy(battleFieldIDList, InBuffer + offset, sizeof(battleFieldIDList));
		offset += sizeof(battleFieldIDList);
		memcpy(battleFieldPreset, InBuffer + offset, sizeof(battleFieldPreset));
		offset += sizeof(battleFieldPreset);
		memcpy(&selectedPresetIndex, InBuffer + offset, sizeof(selectedPresetIndex));
		offset += sizeof(selectedPresetIndex);
		return offset;
	}

};

struct S_STORE_DICE_LIST : public PacketBase
{
	struct DICE_DATA
	{
		int id = 0;
		int price = 0;
		int count = 0;
		bool soldOut = 0;

		DICE_DATA()
		{
		}

		DICE_DATA(const char* InBuffer)
		{
			Deserialize(InBuffer);
		}

		int GetSize() const
		{
			int size = 0;
			size += sizeof(id);
			size += sizeof(price);
			size += sizeof(count);
			size += sizeof(soldOut);
			return size;
		}

		int Serialize(char* InBuffer) const 
		{
			int offset = 0;
			memcpy(InBuffer + offset, &id, sizeof(id));
			offset += sizeof(id);
			memcpy(InBuffer + offset, &price, sizeof(price));
			offset += sizeof(price);
			memcpy(InBuffer + offset, &count, sizeof(count));
			offset += sizeof(count);
			memcpy(InBuffer + offset, &soldOut, sizeof(soldOut));
			offset += sizeof(soldOut);
			return offset;
		}

		int Deserialize(const char* InBuffer)
		{
			int offset = 0;
			memcpy(&id, InBuffer + offset, sizeof(id));
			offset += sizeof(id);
			memcpy(&price, InBuffer + offset, sizeof(price));
			offset += sizeof(price);
			memcpy(&count, InBuffer + offset, sizeof(count));
			offset += sizeof(count);
			memcpy(&soldOut, InBuffer + offset, sizeof(soldOut));
			offset += sizeof(soldOut);
			return offset;
		}

	};

	__int64 resetTime = 0;
	int diceCount = 0;
	DICE_DATA diceList[10];

	S_STORE_DICE_LIST()
	{
	}

	S_STORE_DICE_LIST(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(resetTime);
		size += sizeof(diceCount);
		for(int i = 0; i < 10; ++i)
		{
			size += diceList[i].GetSize();
		}
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_S_STORE_DICE_LIST;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		memcpy(InBuffer + offset, &resetTime, sizeof(resetTime));
		offset += sizeof(resetTime);
		memcpy(InBuffer + offset, &diceCount, sizeof(diceCount));
		offset += sizeof(diceCount);
		for(int i = 0; i < 10; ++i)
		{
			offset += diceList[i].Serialize(InBuffer + offset);
		}
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		memcpy(&resetTime, InBuffer + offset, sizeof(resetTime));
		offset += sizeof(resetTime);
		memcpy(&diceCount, InBuffer + offset, sizeof(diceCount));
		offset += sizeof(diceCount);
		for(int i = 0; i < 10; ++i)
		{
			offset += diceList[i].Deserialize(InBuffer + offset);
		}
		return offset;
	}

};

struct C_UPGRADE_DICE : public PacketBase
{
	int id = 0;

	C_UPGRADE_DICE()
	{
	}

	C_UPGRADE_DICE(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(id);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_C_UPGRADE_DICE;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		memcpy(InBuffer + offset, &id, sizeof(id));
		offset += sizeof(id);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		memcpy(&id, InBuffer + offset, sizeof(id));
		offset += sizeof(id);
		return offset;
	}

};

struct S_UPGRADE_DICE : public PacketBase
{
	int id = 0;
	int level = 0;
	int count = 0;
	int resultType = 0;

	S_UPGRADE_DICE()
	{
	}

	S_UPGRADE_DICE(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(id);
		size += sizeof(level);
		size += sizeof(count);
		size += sizeof(resultType);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_S_UPGRADE_DICE;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		memcpy(InBuffer + offset, &id, sizeof(id));
		offset += sizeof(id);
		memcpy(InBuffer + offset, &level, sizeof(level));
		offset += sizeof(level);
		memcpy(InBuffer + offset, &count, sizeof(count));
		offset += sizeof(count);
		memcpy(InBuffer + offset, &resultType, sizeof(resultType));
		offset += sizeof(resultType);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		memcpy(&id, InBuffer + offset, sizeof(id));
		offset += sizeof(id);
		memcpy(&level, InBuffer + offset, sizeof(level));
		offset += sizeof(level);
		memcpy(&count, InBuffer + offset, sizeof(count));
		offset += sizeof(count);
		memcpy(&resultType, InBuffer + offset, sizeof(resultType));
		offset += sizeof(resultType);
		return offset;
	}

};

struct C_PURCHASE_DICE : public PacketBase
{
	int id = 0;

	C_PURCHASE_DICE()
	{
	}

	C_PURCHASE_DICE(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(id);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_C_PURCHASE_DICE;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		memcpy(InBuffer + offset, &id, sizeof(id));
		offset += sizeof(id);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		memcpy(&id, InBuffer + offset, sizeof(id));
		offset += sizeof(id);
		return offset;
	}

};

struct S_PURCHASE_DICE : public PacketBase
{
	int id = 0;
	int resultType = 0;

	S_PURCHASE_DICE()
	{
	}

	S_PURCHASE_DICE(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(id);
		size += sizeof(resultType);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_S_PURCHASE_DICE;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		memcpy(InBuffer + offset, &id, sizeof(id));
		offset += sizeof(id);
		memcpy(InBuffer + offset, &resultType, sizeof(resultType));
		offset += sizeof(resultType);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		memcpy(&id, InBuffer + offset, sizeof(id));
		offset += sizeof(id);
		memcpy(&resultType, InBuffer + offset, sizeof(resultType));
		offset += sizeof(resultType);
		return offset;
	}

};

struct C_PURCHASE_BOX : public PacketBase
{
	int id = 0;

	C_PURCHASE_BOX()
	{
	}

	C_PURCHASE_BOX(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(id);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_C_PURCHASE_BOX;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		memcpy(InBuffer + offset, &id, sizeof(id));
		offset += sizeof(id);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		memcpy(&id, InBuffer + offset, sizeof(id));
		offset += sizeof(id);
		return offset;
	}

};

struct S_PURCHASE_BOX : public PacketBase
{
	int resultType = 0;

	S_PURCHASE_BOX()
	{
	}

	S_PURCHASE_BOX(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(resultType);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_S_PURCHASE_BOX;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		memcpy(InBuffer + offset, &resultType, sizeof(resultType));
		offset += sizeof(resultType);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		memcpy(&resultType, InBuffer + offset, sizeof(resultType));
		offset += sizeof(resultType);
		return offset;
	}

};

struct C_PURCHASE_BATTLEFIELD : public PacketBase
{
	int id = 0;

	C_PURCHASE_BATTLEFIELD()
	{
	}

	C_PURCHASE_BATTLEFIELD(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(id);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_C_PURCHASE_BATTLEFIELD;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		memcpy(InBuffer + offset, &id, sizeof(id));
		offset += sizeof(id);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		memcpy(&id, InBuffer + offset, sizeof(id));
		offset += sizeof(id);
		return offset;
	}

};

struct S_PURCHASE_BATTLEFIELD : public PacketBase
{
	int id = 0;
	int resultType = 0;

	S_PURCHASE_BATTLEFIELD()
	{
	}

	S_PURCHASE_BATTLEFIELD(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(id);
		size += sizeof(resultType);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_S_PURCHASE_BATTLEFIELD;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		memcpy(InBuffer + offset, &id, sizeof(id));
		offset += sizeof(id);
		memcpy(InBuffer + offset, &resultType, sizeof(resultType));
		offset += sizeof(resultType);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		memcpy(&id, InBuffer + offset, sizeof(id));
		offset += sizeof(id);
		memcpy(&resultType, InBuffer + offset, sizeof(resultType));
		offset += sizeof(resultType);
		return offset;
	}

};

struct S_ADD_DICE : public PacketBase
{
	struct DICE_DATA
	{
		int id = 0;
		int count = 0;

		DICE_DATA()
		{
		}

		DICE_DATA(const char* InBuffer)
		{
			Deserialize(InBuffer);
		}

		int GetSize() const
		{
			int size = 0;
			size += sizeof(id);
			size += sizeof(count);
			return size;
		}

		int Serialize(char* InBuffer) const 
		{
			int offset = 0;
			memcpy(InBuffer + offset, &id, sizeof(id));
			offset += sizeof(id);
			memcpy(InBuffer + offset, &count, sizeof(count));
			offset += sizeof(count);
			return offset;
		}

		int Deserialize(const char* InBuffer)
		{
			int offset = 0;
			memcpy(&id, InBuffer + offset, sizeof(id));
			offset += sizeof(id);
			memcpy(&count, InBuffer + offset, sizeof(count));
			offset += sizeof(count);
			return offset;
		}

	};

	int diceCount = 0;
	DICE_DATA diceList[10];

	S_ADD_DICE()
	{
	}

	S_ADD_DICE(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(diceCount);
		for(int i = 0; i < 10; ++i)
		{
			size += diceList[i].GetSize();
		}
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_S_ADD_DICE;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		memcpy(InBuffer + offset, &diceCount, sizeof(diceCount));
		offset += sizeof(diceCount);
		for(int i = 0; i < 10; ++i)
		{
			offset += diceList[i].Serialize(InBuffer + offset);
		}
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		memcpy(&diceCount, InBuffer + offset, sizeof(diceCount));
		offset += sizeof(diceCount);
		for(int i = 0; i < 10; ++i)
		{
			offset += diceList[i].Deserialize(InBuffer + offset);
		}
		return offset;
	}

};

struct S_CHANGE_GOLD : public PacketBase
{
	int gold = 0;

	S_CHANGE_GOLD()
	{
	}

	S_CHANGE_GOLD(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(gold);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_S_CHANGE_GOLD;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		memcpy(InBuffer + offset, &gold, sizeof(gold));
		offset += sizeof(gold);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		memcpy(&gold, InBuffer + offset, sizeof(gold));
		offset += sizeof(gold);
		return offset;
	}

};

struct S_CHANGE_DIA : public PacketBase
{
	int dia = 0;

	S_CHANGE_DIA()
	{
	}

	S_CHANGE_DIA(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(dia);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_S_CHANGE_DIA;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		memcpy(InBuffer + offset, &dia, sizeof(dia));
		offset += sizeof(dia);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		memcpy(&dia, InBuffer + offset, sizeof(dia));
		offset += sizeof(dia);
		return offset;
	}

};

struct S_CHANGE_CARD : public PacketBase
{
	int card = 0;

	S_CHANGE_CARD()
	{
	}

	S_CHANGE_CARD(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(card);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_S_CHANGE_CARD;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		memcpy(InBuffer + offset, &card, sizeof(card));
		offset += sizeof(card);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		memcpy(&card, InBuffer + offset, sizeof(card));
		offset += sizeof(card);
		return offset;
	}

};

struct C_CHANGE_PRESET : public PacketBase
{
	int presetIndex = 0;

	C_CHANGE_PRESET()
	{
	}

	C_CHANGE_PRESET(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(presetIndex);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_C_CHANGE_PRESET;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		memcpy(InBuffer + offset, &presetIndex, sizeof(presetIndex));
		offset += sizeof(presetIndex);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		memcpy(&presetIndex, InBuffer + offset, sizeof(presetIndex));
		offset += sizeof(presetIndex);
		return offset;
	}

};

struct C_CHANGE_PRESET_DICE : public PacketBase
{
	int presetIndex = 0;
	int slotIndex = 0;
	int diceId = 0;

	C_CHANGE_PRESET_DICE()
	{
	}

	C_CHANGE_PRESET_DICE(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(presetIndex);
		size += sizeof(slotIndex);
		size += sizeof(diceId);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_C_CHANGE_PRESET_DICE;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		memcpy(InBuffer + offset, &presetIndex, sizeof(presetIndex));
		offset += sizeof(presetIndex);
		memcpy(InBuffer + offset, &slotIndex, sizeof(slotIndex));
		offset += sizeof(slotIndex);
		memcpy(InBuffer + offset, &diceId, sizeof(diceId));
		offset += sizeof(diceId);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		memcpy(&presetIndex, InBuffer + offset, sizeof(presetIndex));
		offset += sizeof(presetIndex);
		memcpy(&slotIndex, InBuffer + offset, sizeof(slotIndex));
		offset += sizeof(slotIndex);
		memcpy(&diceId, InBuffer + offset, sizeof(diceId));
		offset += sizeof(diceId);
		return offset;
	}

};

struct C_CHANGE_PRESET_BATTLEFIELD : public PacketBase
{
	int presetIndex = 0;
	int battlefieldId = 0;

	C_CHANGE_PRESET_BATTLEFIELD()
	{
	}

	C_CHANGE_PRESET_BATTLEFIELD(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(presetIndex);
		size += sizeof(battlefieldId);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_C_CHANGE_PRESET_BATTLEFIELD;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		memcpy(InBuffer + offset, &presetIndex, sizeof(presetIndex));
		offset += sizeof(presetIndex);
		memcpy(InBuffer + offset, &battlefieldId, sizeof(battlefieldId));
		offset += sizeof(battlefieldId);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		memcpy(&presetIndex, InBuffer + offset, sizeof(presetIndex));
		offset += sizeof(presetIndex);
		memcpy(&battlefieldId, InBuffer + offset, sizeof(battlefieldId));
		offset += sizeof(battlefieldId);
		return offset;
	}

};

struct C_CHANGE_NAME : public PacketBase
{
	wchar_t name[48];

	C_CHANGE_NAME()
	{
		ZeroMemory(name, sizeof(name));
	}

	C_CHANGE_NAME(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(int) + sizeof(wchar_t) * wcslen(name);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_C_CHANGE_NAME;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		int name_length = wcslen(name);
		memcpy(InBuffer + offset, &name_length, sizeof(name_length));
		offset += sizeof(name_length);
		memcpy(InBuffer + offset, name, sizeof(wchar_t) * name_length);
		offset += sizeof(wchar_t) * name_length;
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		int name_length = 0;
		memcpy(&name_length, InBuffer + offset, sizeof(name_length));
		offset += sizeof(name_length);
		memcpy(name, InBuffer + offset, sizeof(wchar_t) * name_length);
		name[name_length] = '\0';
		offset += sizeof(wchar_t) * name_length;
		return offset;
	}

};

struct S_CHANGE_NAME : public PacketBase
{
	int resultType = 0;
	wchar_t name[48];

	S_CHANGE_NAME()
	{
		ZeroMemory(name, sizeof(name));
	}

	S_CHANGE_NAME(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(resultType);
		size += sizeof(int) + sizeof(wchar_t) * wcslen(name);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_S_CHANGE_NAME;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		memcpy(InBuffer + offset, &resultType, sizeof(resultType));
		offset += sizeof(resultType);
		int name_length = wcslen(name);
		memcpy(InBuffer + offset, &name_length, sizeof(name_length));
		offset += sizeof(name_length);
		memcpy(InBuffer + offset, name, sizeof(wchar_t) * name_length);
		offset += sizeof(wchar_t) * name_length;
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		memcpy(&resultType, InBuffer + offset, sizeof(resultType));
		offset += sizeof(resultType);
		int name_length = 0;
		memcpy(&name_length, InBuffer + offset, sizeof(name_length));
		offset += sizeof(name_length);
		memcpy(name, InBuffer + offset, sizeof(wchar_t) * name_length);
		name[name_length] = '\0';
		offset += sizeof(wchar_t) * name_length;
		return offset;
	}

};

struct C_BATTLE_RESULT : public PacketBase
{
	int battleId = 0;
	int clearWave = 0;

	C_BATTLE_RESULT()
	{
	}

	C_BATTLE_RESULT(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(battleId);
		size += sizeof(clearWave);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_C_BATTLE_RESULT;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		memcpy(InBuffer + offset, &battleId, sizeof(battleId));
		offset += sizeof(battleId);
		memcpy(InBuffer + offset, &clearWave, sizeof(clearWave));
		offset += sizeof(clearWave);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		memcpy(&battleId, InBuffer + offset, sizeof(battleId));
		offset += sizeof(battleId);
		memcpy(&clearWave, InBuffer + offset, sizeof(clearWave));
		offset += sizeof(clearWave);
		return offset;
	}

};

struct S_CHANGE_EXP : public PacketBase
{
	int exp = 0;

	S_CHANGE_EXP()
	{
	}

	S_CHANGE_EXP(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(exp);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_S_CHANGE_EXP;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		memcpy(InBuffer + offset, &exp, sizeof(exp));
		offset += sizeof(exp);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		memcpy(&exp, InBuffer + offset, sizeof(exp));
		offset += sizeof(exp);
		return offset;
	}

};

struct S_CHANGE_LEVEL : public PacketBase
{
	int level = 0;

	S_CHANGE_LEVEL()
	{
	}

	S_CHANGE_LEVEL(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(level);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_S_CHANGE_LEVEL;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		memcpy(InBuffer + offset, &level, sizeof(level));
		offset += sizeof(level);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		memcpy(&level, InBuffer + offset, sizeof(level));
		offset += sizeof(level);
		return offset;
	}

};

struct C_BATTLE_MATCHING : public PacketBase
{

	C_BATTLE_MATCHING()
	{
	}

	C_BATTLE_MATCHING(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_C_BATTLE_MATCHING;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		return offset;
	}

};

struct C_BATTLE_MATCHING_CANCEL : public PacketBase
{

	C_BATTLE_MATCHING_CANCEL()
	{
	}

	C_BATTLE_MATCHING_CANCEL(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_C_BATTLE_MATCHING_CANCEL;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		return offset;
	}

};

struct S_BATTLE_MATCHING : public PacketBase
{
	bool isHost = 0;
	wchar_t hostIP[48];

	S_BATTLE_MATCHING()
	{
		ZeroMemory(hostIP, sizeof(hostIP));
	}

	S_BATTLE_MATCHING(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(isHost);
		size += sizeof(int) + sizeof(wchar_t) * wcslen(hostIP);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_S_BATTLE_MATCHING;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		memcpy(InBuffer + offset, &isHost, sizeof(isHost));
		offset += sizeof(isHost);
		int hostIP_length = wcslen(hostIP);
		memcpy(InBuffer + offset, &hostIP_length, sizeof(hostIP_length));
		offset += sizeof(hostIP_length);
		memcpy(InBuffer + offset, hostIP, sizeof(wchar_t) * hostIP_length);
		offset += sizeof(wchar_t) * hostIP_length;
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		memcpy(&isHost, InBuffer + offset, sizeof(isHost));
		offset += sizeof(isHost);
		int hostIP_length = 0;
		memcpy(&hostIP_length, InBuffer + offset, sizeof(hostIP_length));
		offset += sizeof(hostIP_length);
		memcpy(hostIP, InBuffer + offset, sizeof(wchar_t) * hostIP_length);
		hostIP[hostIP_length] = '\0';
		offset += sizeof(wchar_t) * hostIP_length;
		return offset;
	}

};

struct P2P_PLAYER_DATA : public PacketBase
{
	struct DICE_DATA
	{
		int id = 0;
		int level = 0;

		DICE_DATA()
		{
		}

		DICE_DATA(const char* InBuffer)
		{
			Deserialize(InBuffer);
		}

		int GetSize() const
		{
			int size = 0;
			size += sizeof(id);
			size += sizeof(level);
			return size;
		}

		int Serialize(char* InBuffer) const 
		{
			int offset = 0;
			memcpy(InBuffer + offset, &id, sizeof(id));
			offset += sizeof(id);
			memcpy(InBuffer + offset, &level, sizeof(level));
			offset += sizeof(level);
			return offset;
		}

		int Deserialize(const char* InBuffer)
		{
			int offset = 0;
			memcpy(&id, InBuffer + offset, sizeof(id));
			offset += sizeof(id);
			memcpy(&level, InBuffer + offset, sizeof(level));
			offset += sizeof(level);
			return offset;
		}

	};

	wchar_t name[48];
	int level = 0;
	float criticalDamageRate = 0;
	DICE_DATA diceList[5];
	int instanceId = 0;

	P2P_PLAYER_DATA()
	{
		ZeroMemory(name, sizeof(name));
	}

	P2P_PLAYER_DATA(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(int) + sizeof(wchar_t) * wcslen(name);
		size += sizeof(level);
		size += sizeof(criticalDamageRate);
		for(int i = 0; i < 5; ++i)
		{
			size += diceList[i].GetSize();
		}
		size += sizeof(instanceId);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_P2P_PLAYER_DATA;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		int name_length = wcslen(name);
		memcpy(InBuffer + offset, &name_length, sizeof(name_length));
		offset += sizeof(name_length);
		memcpy(InBuffer + offset, name, sizeof(wchar_t) * name_length);
		offset += sizeof(wchar_t) * name_length;
		memcpy(InBuffer + offset, &level, sizeof(level));
		offset += sizeof(level);
		memcpy(InBuffer + offset, &criticalDamageRate, sizeof(criticalDamageRate));
		offset += sizeof(criticalDamageRate);
		for(int i = 0; i < 5; ++i)
		{
			offset += diceList[i].Serialize(InBuffer + offset);
		}
		memcpy(InBuffer + offset, &instanceId, sizeof(instanceId));
		offset += sizeof(instanceId);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		int name_length = 0;
		memcpy(&name_length, InBuffer + offset, sizeof(name_length));
		offset += sizeof(name_length);
		memcpy(name, InBuffer + offset, sizeof(wchar_t) * name_length);
		name[name_length] = '\0';
		offset += sizeof(wchar_t) * name_length;
		memcpy(&level, InBuffer + offset, sizeof(level));
		offset += sizeof(level);
		memcpy(&criticalDamageRate, InBuffer + offset, sizeof(criticalDamageRate));
		offset += sizeof(criticalDamageRate);
		for(int i = 0; i < 5; ++i)
		{
			offset += diceList[i].Deserialize(InBuffer + offset);
		}
		memcpy(&instanceId, InBuffer + offset, sizeof(instanceId));
		offset += sizeof(instanceId);
		return offset;
	}

};

struct P2P_CHANGE_DICE_LEVEL : public PacketBase
{
	int index = 0;
	int level = 0;

	P2P_CHANGE_DICE_LEVEL()
	{
	}

	P2P_CHANGE_DICE_LEVEL(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(index);
		size += sizeof(level);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_P2P_CHANGE_DICE_LEVEL;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		memcpy(InBuffer + offset, &index, sizeof(index));
		offset += sizeof(index);
		memcpy(InBuffer + offset, &level, sizeof(level));
		offset += sizeof(level);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		memcpy(&index, InBuffer + offset, sizeof(index));
		offset += sizeof(index);
		memcpy(&level, InBuffer + offset, sizeof(level));
		offset += sizeof(level);
		return offset;
	}

};

struct P2P_SPAWN_ENEMY : public PacketBase
{
	int instanceId = 0;
	int enemyId = 0;
	int ownerId = 0;

	P2P_SPAWN_ENEMY()
	{
	}

	P2P_SPAWN_ENEMY(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(instanceId);
		size += sizeof(enemyId);
		size += sizeof(ownerId);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_P2P_SPAWN_ENEMY;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		memcpy(InBuffer + offset, &instanceId, sizeof(instanceId));
		offset += sizeof(instanceId);
		memcpy(InBuffer + offset, &enemyId, sizeof(enemyId));
		offset += sizeof(enemyId);
		memcpy(InBuffer + offset, &ownerId, sizeof(ownerId));
		offset += sizeof(ownerId);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		memcpy(&instanceId, InBuffer + offset, sizeof(instanceId));
		offset += sizeof(instanceId);
		memcpy(&enemyId, InBuffer + offset, sizeof(enemyId));
		offset += sizeof(enemyId);
		memcpy(&ownerId, InBuffer + offset, sizeof(ownerId));
		offset += sizeof(ownerId);
		return offset;
	}

};

struct P2P_DAMAGE : public PacketBase
{
	int objectId = 0;
	int damage = 0;
	bool critical = 0;

	P2P_DAMAGE()
	{
	}

	P2P_DAMAGE(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(objectId);
		size += sizeof(damage);
		size += sizeof(critical);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_P2P_DAMAGE;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		memcpy(InBuffer + offset, &objectId, sizeof(objectId));
		offset += sizeof(objectId);
		memcpy(InBuffer + offset, &damage, sizeof(damage));
		offset += sizeof(damage);
		memcpy(InBuffer + offset, &critical, sizeof(critical));
		offset += sizeof(critical);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		memcpy(&objectId, InBuffer + offset, sizeof(objectId));
		offset += sizeof(objectId);
		memcpy(&damage, InBuffer + offset, sizeof(damage));
		offset += sizeof(damage);
		memcpy(&critical, InBuffer + offset, sizeof(critical));
		offset += sizeof(critical);
		return offset;
	}

};

struct P2P_SPAWN_REMOTE_DICE : public PacketBase
{
	int objectId = 0;
	int index = 0;
	int diceId = 0;
	int eyeCount = 0;

	P2P_SPAWN_REMOTE_DICE()
	{
	}

	P2P_SPAWN_REMOTE_DICE(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(objectId);
		size += sizeof(index);
		size += sizeof(diceId);
		size += sizeof(eyeCount);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_P2P_SPAWN_REMOTE_DICE;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		memcpy(InBuffer + offset, &objectId, sizeof(objectId));
		offset += sizeof(objectId);
		memcpy(InBuffer + offset, &index, sizeof(index));
		offset += sizeof(index);
		memcpy(InBuffer + offset, &diceId, sizeof(diceId));
		offset += sizeof(diceId);
		memcpy(InBuffer + offset, &eyeCount, sizeof(eyeCount));
		offset += sizeof(eyeCount);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		memcpy(&objectId, InBuffer + offset, sizeof(objectId));
		offset += sizeof(objectId);
		memcpy(&index, InBuffer + offset, sizeof(index));
		offset += sizeof(index);
		memcpy(&diceId, InBuffer + offset, sizeof(diceId));
		offset += sizeof(diceId);
		memcpy(&eyeCount, InBuffer + offset, sizeof(eyeCount));
		offset += sizeof(eyeCount);
		return offset;
	}

};

struct P2P_C_REQUEST_SPAWN_DICE : public PacketBase
{
	int index = 0;
	int diceId = 0;
	int eyeCount = 0;

	P2P_C_REQUEST_SPAWN_DICE()
	{
	}

	P2P_C_REQUEST_SPAWN_DICE(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(index);
		size += sizeof(diceId);
		size += sizeof(eyeCount);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_P2P_C_REQUEST_SPAWN_DICE;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		memcpy(InBuffer + offset, &index, sizeof(index));
		offset += sizeof(index);
		memcpy(InBuffer + offset, &diceId, sizeof(diceId));
		offset += sizeof(diceId);
		memcpy(InBuffer + offset, &eyeCount, sizeof(eyeCount));
		offset += sizeof(eyeCount);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		memcpy(&index, InBuffer + offset, sizeof(index));
		offset += sizeof(index);
		memcpy(&diceId, InBuffer + offset, sizeof(diceId));
		offset += sizeof(diceId);
		memcpy(&eyeCount, InBuffer + offset, sizeof(eyeCount));
		offset += sizeof(eyeCount);
		return offset;
	}

};

struct P2P_S_REQUEST_SPAWN_DICE : public PacketBase
{
	int objectId = 0;
	int index = 0;
	int diceId = 0;
	int eyeCount = 0;

	P2P_S_REQUEST_SPAWN_DICE()
	{
	}

	P2P_S_REQUEST_SPAWN_DICE(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(objectId);
		size += sizeof(index);
		size += sizeof(diceId);
		size += sizeof(eyeCount);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_P2P_S_REQUEST_SPAWN_DICE;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		memcpy(InBuffer + offset, &objectId, sizeof(objectId));
		offset += sizeof(objectId);
		memcpy(InBuffer + offset, &index, sizeof(index));
		offset += sizeof(index);
		memcpy(InBuffer + offset, &diceId, sizeof(diceId));
		offset += sizeof(diceId);
		memcpy(InBuffer + offset, &eyeCount, sizeof(eyeCount));
		offset += sizeof(eyeCount);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		memcpy(&objectId, InBuffer + offset, sizeof(objectId));
		offset += sizeof(objectId);
		memcpy(&index, InBuffer + offset, sizeof(index));
		offset += sizeof(index);
		memcpy(&diceId, InBuffer + offset, sizeof(diceId));
		offset += sizeof(diceId);
		memcpy(&eyeCount, InBuffer + offset, sizeof(eyeCount));
		offset += sizeof(eyeCount);
		return offset;
	}

};

struct P2P_DESPAWN_OBJECT : public PacketBase
{
	int objectId = 0;

	P2P_DESPAWN_OBJECT()
	{
	}

	P2P_DESPAWN_OBJECT(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(objectId);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_P2P_DESPAWN_OBJECT;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		memcpy(InBuffer + offset, &objectId, sizeof(objectId));
		offset += sizeof(objectId);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		memcpy(&objectId, InBuffer + offset, sizeof(objectId));
		offset += sizeof(objectId);
		return offset;
	}

};

struct P2P_CHANGE_LIFE : public PacketBase
{
	int life = 0;

	P2P_CHANGE_LIFE()
	{
	}

	P2P_CHANGE_LIFE(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(life);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_P2P_CHANGE_LIFE;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		memcpy(InBuffer + offset, &life, sizeof(life));
		offset += sizeof(life);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		memcpy(&life, InBuffer + offset, sizeof(life));
		offset += sizeof(life);
		return offset;
	}

};

struct P2P_START_BATTLE : public PacketBase
{
	int battleId = 0;

	P2P_START_BATTLE()
	{
	}

	P2P_START_BATTLE(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(battleId);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_P2P_START_BATTLE;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		memcpy(InBuffer + offset, &battleId, sizeof(battleId));
		offset += sizeof(battleId);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		memcpy(&battleId, InBuffer + offset, sizeof(battleId));
		offset += sizeof(battleId);
		return offset;
	}

};

struct P2P_CHANGE_WAVE : public PacketBase
{
	int wave = 0;

	P2P_CHANGE_WAVE()
	{
	}

	P2P_CHANGE_WAVE(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(wave);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_P2P_CHANGE_WAVE;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		memcpy(InBuffer + offset, &wave, sizeof(wave));
		offset += sizeof(wave);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		memcpy(&wave, InBuffer + offset, sizeof(wave));
		offset += sizeof(wave);
		return offset;
	}

};

struct P2P_READY_BATTLE : public PacketBase
{

	P2P_READY_BATTLE()
	{
	}

	P2P_READY_BATTLE(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_P2P_READY_BATTLE;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		return offset;
	}

};

struct P2P_ON_SKILL : public PacketBase
{
	int objectId = 0;
	int skillId = 0;
	int targetId = 0;

	P2P_ON_SKILL()
	{
	}

	P2P_ON_SKILL(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(objectId);
		size += sizeof(skillId);
		size += sizeof(targetId);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_P2P_ON_SKILL;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		memcpy(InBuffer + offset, &objectId, sizeof(objectId));
		offset += sizeof(objectId);
		memcpy(InBuffer + offset, &skillId, sizeof(skillId));
		offset += sizeof(skillId);
		memcpy(InBuffer + offset, &targetId, sizeof(targetId));
		offset += sizeof(targetId);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		memcpy(&objectId, InBuffer + offset, sizeof(objectId));
		offset += sizeof(objectId);
		memcpy(&skillId, InBuffer + offset, sizeof(skillId));
		offset += sizeof(skillId);
		memcpy(&targetId, InBuffer + offset, sizeof(targetId));
		offset += sizeof(targetId);
		return offset;
	}

};

struct P2P_USE_SKILL_IN_PATH : public PacketBase
{
	int objectId = 0;
	int skillId = 0;
	int pathIndex = 0;
	float pathRate = 0;

	P2P_USE_SKILL_IN_PATH()
	{
	}

	P2P_USE_SKILL_IN_PATH(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(objectId);
		size += sizeof(skillId);
		size += sizeof(pathIndex);
		size += sizeof(pathRate);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_P2P_USE_SKILL_IN_PATH;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		memcpy(InBuffer + offset, &objectId, sizeof(objectId));
		offset += sizeof(objectId);
		memcpy(InBuffer + offset, &skillId, sizeof(skillId));
		offset += sizeof(skillId);
		memcpy(InBuffer + offset, &pathIndex, sizeof(pathIndex));
		offset += sizeof(pathIndex);
		memcpy(InBuffer + offset, &pathRate, sizeof(pathRate));
		offset += sizeof(pathRate);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		memcpy(&objectId, InBuffer + offset, sizeof(objectId));
		offset += sizeof(objectId);
		memcpy(&skillId, InBuffer + offset, sizeof(skillId));
		offset += sizeof(skillId);
		memcpy(&pathIndex, InBuffer + offset, sizeof(pathIndex));
		offset += sizeof(pathIndex);
		memcpy(&pathRate, InBuffer + offset, sizeof(pathRate));
		offset += sizeof(pathRate);
		return offset;
	}

};

struct P2P_OFF_SKILL : public PacketBase
{
	int objectId = 0;
	int skillId = 0;

	P2P_OFF_SKILL()
	{
	}

	P2P_OFF_SKILL(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(objectId);
		size += sizeof(skillId);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_P2P_OFF_SKILL;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		memcpy(InBuffer + offset, &objectId, sizeof(objectId));
		offset += sizeof(objectId);
		memcpy(InBuffer + offset, &skillId, sizeof(skillId));
		offset += sizeof(skillId);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		memcpy(&objectId, InBuffer + offset, sizeof(objectId));
		offset += sizeof(objectId);
		memcpy(&skillId, InBuffer + offset, sizeof(skillId));
		offset += sizeof(skillId);
		return offset;
	}

};

struct P2P_SPAWN_COLLISION_OBJECT : public PacketBase
{
	int objectId = 0;
	int ownerObjectId = 0;
	int collisionObjectId = 0;
	float pathRate = 0;

	P2P_SPAWN_COLLISION_OBJECT()
	{
	}

	P2P_SPAWN_COLLISION_OBJECT(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(objectId);
		size += sizeof(ownerObjectId);
		size += sizeof(collisionObjectId);
		size += sizeof(pathRate);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_P2P_SPAWN_COLLISION_OBJECT;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		memcpy(InBuffer + offset, &objectId, sizeof(objectId));
		offset += sizeof(objectId);
		memcpy(InBuffer + offset, &ownerObjectId, sizeof(ownerObjectId));
		offset += sizeof(ownerObjectId);
		memcpy(InBuffer + offset, &collisionObjectId, sizeof(collisionObjectId));
		offset += sizeof(collisionObjectId);
		memcpy(InBuffer + offset, &pathRate, sizeof(pathRate));
		offset += sizeof(pathRate);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		memcpy(&objectId, InBuffer + offset, sizeof(objectId));
		offset += sizeof(objectId);
		memcpy(&ownerObjectId, InBuffer + offset, sizeof(ownerObjectId));
		offset += sizeof(ownerObjectId);
		memcpy(&collisionObjectId, InBuffer + offset, sizeof(collisionObjectId));
		offset += sizeof(collisionObjectId);
		memcpy(&pathRate, InBuffer + offset, sizeof(pathRate));
		offset += sizeof(pathRate);
		return offset;
	}

};

struct P2P_REQUEST_COLLISION_OBJECT : public PacketBase
{
	int ownerObjectId = 0;
	int collisionObjectId = 0;
	float pathRate = 0;

	P2P_REQUEST_COLLISION_OBJECT()
	{
	}

	P2P_REQUEST_COLLISION_OBJECT(const char* InBuffer)
	{
		Deserialize(InBuffer);
	}

	int GetSize() const
	{
		int size = 0;
		size += sizeof(ownerObjectId);
		size += sizeof(collisionObjectId);
		size += sizeof(pathRate);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_P2P_REQUEST_COLLISION_OBJECT;
	}

	int Serialize(char* InBuffer) const 
	{
		int offset = 0;
		int size = GetSize() + sizeof(int) + sizeof(PacketType);
		memcpy(InBuffer + offset, &size, sizeof(size));
		offset += sizeof(size);
		PacketType packetType = GetPacketType();
		memcpy(InBuffer + offset, &packetType, sizeof(PacketType));
		offset += sizeof(PacketType);
		memcpy(InBuffer + offset, &ownerObjectId, sizeof(ownerObjectId));
		offset += sizeof(ownerObjectId);
		memcpy(InBuffer + offset, &collisionObjectId, sizeof(collisionObjectId));
		offset += sizeof(collisionObjectId);
		memcpy(InBuffer + offset, &pathRate, sizeof(pathRate));
		offset += sizeof(pathRate);
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		memcpy(&ownerObjectId, InBuffer + offset, sizeof(ownerObjectId));
		offset += sizeof(ownerObjectId);
		memcpy(&collisionObjectId, InBuffer + offset, sizeof(collisionObjectId));
		offset += sizeof(collisionObjectId);
		memcpy(&pathRate, InBuffer + offset, sizeof(pathRate));
		offset += sizeof(pathRate);
		return offset;
	}

};

