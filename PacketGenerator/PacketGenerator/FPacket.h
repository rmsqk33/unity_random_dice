#pragma once


// 패킷 텍스트 파일 내 구조체 하나의 정보를 파싱하는 클래스
enum FPacketMemberType
{
	PACKET_MEMBER_TYPE_COMMON,
	PACKET_MEMBER_TYPE_STRING,
	PACKET_MEMBER_TYPE_STRUCT,
	PACKET_MEMBER_TYPE_BYTE,
};

struct FPacketMember
{
	string type;
	string name;
	int arrayTotalCount = 0;
	vector<int> arrayCountList;
	FPacketMemberType memberType = PACKET_MEMBER_TYPE_COMMON;
};

class FPacket
{
private:
	string m_Name;
	vector<FPacket> m_ChildPacketList;
	vector<FPacketMember> m_MemberList;
	bool m_Root;

public:
	FPacket(ifstream& InFile, const string& InName, bool InRoot = false);

	void Parse(ifstream& InFile);
	
	void ForeachChildPacket(const function<void(const FPacket*)>& InFunc) const;
	void ForeachMember(const function<void(const FPacketMember&)>& InFunc) const;

	const char* GetName() const;
	bool IsRoot() const;

private:
	bool IsUserDefineType(const string& InType) const; // 패킷 구조체 내에 있는 구조체인지 체크

};

