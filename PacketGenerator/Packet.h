#pragma once

enum PacketType
{
	PACKET_TYPE_NONE,
	PACKET_TYPE_C_REQUEST_LOGIN,
	PACKET_TYPE_S_REQUEST_LOGIN,
	PACKET_TYPE_C_ADD_GUEST_ACCOUNT,
	PACKET_TYPE_S_ADD_GUEST_ACCOUNT,
	PACKET_TYPE_S_USER_DATA,
	PACKET_TYPE_MAX,
};

struct PacketBase
{
	virtual PacketType GetPacketType() const { return PACKET_TYPE_NONE; }
	virtual int Serialize(char* InBuffer) const { return 0; }
};
struct C_REQUEST_LOGIN : public PacketBase
{
	char id[256];

	int GetSize() const
	{
		int size = 0;
		size += sizeof(int) + sizeof(wchar_t) * wcslen(id);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_C_REQUEST_LOGIN;
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
		offset += sizeof(wchar_t) * id_length;
		memcpy(id, InBuffer + offset, sizeof(wchar_t) * id_length);
		id[id_length] = '\0';
	}
		return offset;
	}

};

struct S_REQUEST_LOGIN : public PacketBase
{

	int GetSize() const
	{
		int size = 0;
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_S_REQUEST_LOGIN;
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

struct C_ADD_GUEST_ACCOUNT : public PacketBase
{

	int GetSize() const
	{
		int size = 0;
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_C_ADD_GUEST_ACCOUNT;
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

struct S_ADD_GUEST_ACCOUNT : public PacketBase
{
	char id[256];

	int GetSize() const
	{
		int size = 0;
		size += sizeof(int) + sizeof(wchar_t) * wcslen(id);
		return size;
	}

	PacketType GetPacketType() const override
	{
		return PACKET_TYPE_S_ADD_GUEST_ACCOUNT;
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
		offset += sizeof(wchar_t) * id_length;
		memcpy(id, InBuffer + offset, sizeof(wchar_t) * id_length);
		id[id_length] = '\0';
	}
		return offset;
	}

};

struct S_USER_DATA : public PacketBase
{
	struct S_DICE_DATA
	{
		byte id;
		byte class;
		int count;

		int GetSize() const
		{
			int size = 0;
			size += sizeof(id);
			size += sizeof(class);
			size += sizeof(count);
			return size;
		}

		int Serialize(char* InBuffer) const 
		{
			int offset = 0;
			memcpy(InBuffer + offset, &id, sizeof(id));
			offset += sizeof(id);
			memcpy(InBuffer + offset, &class, sizeof(class));
			offset += sizeof(class);
			memcpy(InBuffer + offset, &count, sizeof(count));
			offset += sizeof(count);
			return offset;
		}

		int Deserialize(const char* InBuffer)
		{
			int offset = 0;
			memcpy(&id, InBuffer + offset, sizeof(id));
			offset += sizeof(id);
			memcpy(&class, InBuffer + offset, sizeof(class));
			offset += sizeof(class);
			memcpy(&count, InBuffer + offset, sizeof(count));
			offset += sizeof(count);
			return offset;
		}

	};

	char name[48];
	byte class;
	int dia;
	int gold;
	int exp;
	S_DICE_DATA diceDataList[30];

	int GetSize() const
	{
		int size = 0;
		size += sizeof(int) + sizeof(wchar_t) * wcslen(name);
		size += sizeof(class);
		size += sizeof(dia);
		size += sizeof(gold);
		size += sizeof(exp);
		for(int i = 0; i < 30; ++i)
		{
			size += diceDataList[i].GetSize();
		}
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
		memcpy(InBuffer + offset, &class, sizeof(class));
		offset += sizeof(class);
		memcpy(InBuffer + offset, &dia, sizeof(dia));
		offset += sizeof(dia);
		memcpy(InBuffer + offset, &gold, sizeof(gold));
		offset += sizeof(gold);
		memcpy(InBuffer + offset, &exp, sizeof(exp));
		offset += sizeof(exp);
		for(int i = 0; i < 30; ++i)
		{
			offset += diceDataList[i].Serialize(InBuffer + offset);
		}
		return offset;
	}

	int Deserialize(const char* InBuffer)
	{
		int offset = 0;
		int name_length = 0;
		memcpy(&name_length, InBuffer + offset, sizeof(name_length));
		offset += sizeof(name_length);
		offset += sizeof(wchar_t) * name_length;
		memcpy(name, InBuffer + offset, sizeof(wchar_t) * name_length);
		name[name_length] = '\0';
	}
		memcpy(&class, InBuffer + offset, sizeof(class));
		offset += sizeof(class);
		memcpy(&dia, InBuffer + offset, sizeof(dia));
		offset += sizeof(dia);
		memcpy(&gold, InBuffer + offset, sizeof(gold));
		offset += sizeof(gold);
		memcpy(&exp, InBuffer + offset, sizeof(exp));
		offset += sizeof(exp);
		for(int i = 0; i < 30; ++i)
		{
			offset += diceDataList[i].Deserialize(InBuffer + offset);
		}
		return offset;
	}

};

