#pragma once

#include "FPacket.h"

class FPacketParser;

// ��Ŷ ���� ������ C# ��� ���Ϸ� �ۼ��ϴ� Ŭ����
class FPacketWriter_CSharp
{
private:
	const FPacketParser* m_PacketParser;

public:
	FPacketWriter_CSharp(const FPacketParser* InParser);

	void Write(const string& InFilePath);

private:
	string GetPacketStr(const FPacket* InPacket, const string& InIndent = "") const;

	string GetEnumStr(); // ��Ŷ ����ü�� ENUM���� ����
	string GetPacketBaseStr() const; // ��Ŷ Base Ŭ���� ����
	string GetMemberStr(const FPacket* InPacket, const string& InIndent = "") const; // ��Ŷ ��� ���� ����
	string GetConstructor(const FPacket* InPacket, const string& InIndent = "") const;
	string GetSizeFuncStr(const FPacket* InPacket, const string& InIndent = "") const; // ��Ŷ �� ������ Getter
	string GetPacketTypeStr(const FPacket* InPacket, const string& InIndent = "") const; // ��Ŷ Ÿ�� Getter �Լ� ����
	string GetSerializeFuncStr(const FPacket* InPacket, const string& InIndent = "") const; // ��Ŷ Serialize �Լ� ����
	string GetDeserializeFuncStr(const FPacket* InPacket, const string& InIndent = "") const; // ��Ŷ Deserialize �Լ� ����
	string GetBitToValueStr(const string& InType) const;
	string ConvertTypeToCSharpType(const FPacketMember& InMember) const;
};

