#include "stdafx.h"
#include "FPacket.h"
#include "FUtil.h"

FPacket::FPacket(ifstream& InFile, const string& InName, bool InRoot/* = false*/)
{
	m_Name = FUtil::ToUpper(InName.c_str());
	m_Root = InRoot;
	Parse(InFile);
}

void FPacket::Parse(ifstream& InFile)
{
	string readLine;
	while (getline(InFile, readLine))
	{
		// 구조체 내에 있는 구조체 파싱
		size_t findStrIndex = readLine.find("struct");
		if (findStrIndex != string::npos)
		{
			vector<string> splitList = FUtil::SplitStringByBlank(readLine);
			if (splitList.size() < 2)
				continue;

			m_ChildPacketList.push_back(FPacket(InFile, splitList[1]));
		}
		else
		{
			// 구조체 종료 체크
			findStrIndex = readLine.find("}");
			if (findStrIndex != string::npos)
			{
				break;
			}
			else
			{
				// 멤버 변수 파싱, 타입 이름[배열 크기]
				char delimiters[] = { ' ', '\t', '[', ']' };
				vector<string> splitList = FUtil::SplitStringByDelimeter(readLine, delimiters, 4);
				if (2 <= splitList.size())
				{
					FPacketMember newMember;
					newMember.type = splitList[0];
					newMember.name = splitList[1];

					if (2 < splitList.size())
					{
						newMember.arrayTotalCount = 1;
						for (int i = 2; i < splitList.size(); ++i)
						{
							int arrayCount = atoi(splitList[2].c_str());
							newMember.arrayTotalCount *= arrayCount;
							newMember.arrayCountList.push_back(arrayCount);
						}

						if (newMember.type == "char")
							newMember.memberType = PACKET_MEMBER_TYPE_STRING;
					}

					if (newMember.type == "byte")
						newMember.memberType = PACKET_MEMBER_TYPE_BYTE;

					m_MemberList.push_back(newMember);
				}
			}
		}
	}

	for (int i = 0; i < m_MemberList.size(); ++i)
	{
		if (IsUserDefineType(m_MemberList[i].type))
		{
			m_MemberList[i].memberType = PACKET_MEMBER_TYPE_STRUCT;
		}
	}
}

void FPacket::ForeachChildPacket(const function<void(const FPacket*)>& InFunc) const
{
	for (int i = 0; i < m_ChildPacketList.size(); ++i)
	{
		InFunc(&m_ChildPacketList[i]);
	}
}

void FPacket::ForeachMember(const function<void (const FPacketMember&)>& InFunc) const
{
	for (int i = 0; i < m_MemberList.size(); ++i)
	{
		InFunc(m_MemberList[i]);
	}
}

const char* FPacket::GetName() const
{
	return m_Name.c_str();
}

bool FPacket::IsRoot() const
{
	return m_Root;
}

bool FPacket::IsUserDefineType(const string& InType) const
{
	if (m_Name == InType)
		return true;

	for (int i = 0; i < m_ChildPacketList.size(); ++i)
	{
		if (m_ChildPacketList[i].IsUserDefineType(InType))
			return true;
	}

	return false;
}
