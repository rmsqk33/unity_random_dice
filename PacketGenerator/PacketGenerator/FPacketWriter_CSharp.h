#pragma once

#include "FPacket.h"

class FPacketParser;

// 패킷 정의 파일을 C# 언어 파일로 작성하는 클래스
class FPacketWriter_CSharp
{
private:
	const FPacketParser* m_PacketParser;

public:
	FPacketWriter_CSharp(const FPacketParser* InParser);

	void Write(const string& InFilePath);

private:
	string GetPacketStr(const FPacket* InPacket, const string& InIndent = "") const;

	string GetEnumStr(); // 패킷 구조체를 ENUM으로 정의
	string GetPacketBaseStr() const; // 패킷 Base 클래스 정의
	string GetMemberStr(const FPacket* InPacket, const string& InIndent = "") const; // 패킷 멤버 변수 정의
	string GetConstructor(const FPacket* InPacket, const string& InIndent = "") const;
	string GetSizeFuncStr(const FPacket* InPacket, const string& InIndent = "") const; // 패킷 총 사이즈 Getter
	string GetPacketTypeStr(const FPacket* InPacket, const string& InIndent = "") const; // 패킷 타입 Getter 함수 정의
	string GetSerializeFuncStr(const FPacket* InPacket, const string& InIndent = "") const; // 패킷 Serialize 함수 정의
	string GetDeserializeFuncStr(const FPacket* InPacket, const string& InIndent = "") const; // 패킷 Deserialize 함수 정의
	string GetBitToValueStr(const string& InType) const;
	string ConvertTypeToCSharpType(const FPacketMember& InMember) const;
};

