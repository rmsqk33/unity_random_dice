#pragma once

#include "FPacket.h"

class FPacketParser;

// ��Ŷ ���� ������ C++ ��� ���Ϸ� �ۼ��ϴ� Ŭ����
class FPacketWriter_CPP
{
private:
	const FPacketParser* m_PacketParser;

public:
	FPacketWriter_CPP(const FPacketParser* InParser);

	void Write(const string& InFilePath);

private:
	string GetPacketStr(const FPacket* InPacket, const string& InIndent = "") const;

	string GetEnumStr() const; // ��Ŷ ����ü�� ENUM���� ����
	string GetPacketBaseStr() const; // ��Ŷ Base Ŭ���� ����
	string GetMemberStr(const FPacket* InPacket, const string& InIndent = "") const; // ��Ŷ ��� ���� ����
	string GetConstructorStr(const FPacket* InPacket, const string& InIndent = "") const; // ��Ŷ ������
	string GetSizeFuncStr(const FPacket* InPacket, const string& InIndent = "") const; // ��Ŷ �� ������ Getter
	string GetPacketTypeStr(const FPacket* InPacket, const string& InIndent = "") const; // ��Ŷ Ÿ�� Getter �Լ� ����
	string GetSerializeFuncStr(const FPacket* InPacket, const string& InIndent = "") const; // ��Ŷ Serialize �Լ� ����
	string GetDeserializeFuncStr(const FPacket* InPacket, const string& InIndent = "") const; // ��Ŷ Deserialize �Լ� ����
	string ConvertTypeToCPPType(const FPacketMember& InMember) const;
};

