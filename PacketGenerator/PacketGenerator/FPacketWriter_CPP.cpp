#include "stdafx.h"
#include "FPacketWriter_CPP.h"
#include "FPacketParser.h"
#include "FUtil.h"

#define START_MULTI_DIMENSIONAL_ARRAY(outStr, indent, count, arrayCountList)\
{\
	string arrayIndent = indent;\
	for (int i = 0; i < count; ++i)\
	{\
		arrayIndent += "\t"; \
		char forAlphabet = 105 + i; \
		outStr += FUtil::SPrintf("%sfor(int %c = 0; %c < %d; ++%c)\n", arrayIndent.c_str(), forAlphabet, forAlphabet, arrayCountList[i], forAlphabet); \
		outStr += FUtil::SPrintf("%s{\n", arrayIndent.c_str()); \
	}\

#define MIDDLE_MULTI_DIMENSIONAL_ARRAY(outStr, indent, count, startFunc, endFunc)\
	outStr += indent + "\t";\
	outStr += startFunc;\
	for (int i = 0; i < count; ++i)\
	{\
		char forAlphabet = 105 + i; \
		outStr += FUtil::SPrintf("[%c]", forAlphabet); \
	}\
	outStr += endFunc;

#define END_MULTI_DIMENSIONAL_ARRAY(outStr, indent, count)\
	for (int i = 0; i < count; ++i)\
	{\
		outStr += FUtil::SPrintf("%s}\n", arrayIndent.c_str()); \
		arrayIndent.pop_back(); \
	}\
}

#define ALL_MULTI_DIMENSIONAL_ARRAY(outStr, indent, count, arrayCountList, startFunc, endFunc)\
{\
	START_MULTI_DIMENSIONAL_ARRAY(outStr, indent, count, arrayCountList)\
	MIDDLE_MULTI_DIMENSIONAL_ARRAY(outStr, arrayIndent, count, startFunc, endFunc)\
	END_MULTI_DIMENSIONAL_ARRAY(outStr, indent, count)\
}

FPacketWriter_CPP::FPacketWriter_CPP(const FPacketParser* InParser)
	: m_PacketParser(InParser)
{
}

void FPacketWriter_CPP::Write(const string& InFilePath)
{
	ofstream writeFile(InFilePath);
	if (!writeFile.is_open())
		return;

	writeFile << "#pragma once\n\n";
	writeFile << GetEnumStr();
	writeFile << GetPacketBaseStr();
	m_PacketParser->ForeachPacket([&](const FPacket* InPacket) {
		writeFile << GetPacketStr(InPacket);
	});

	writeFile.close();
}

string FPacketWriter_CPP::GetPacketStr(const FPacket* InPacket, const string& InIndent/* = ""*/) const
{
	string retVal;

	if (InPacket->IsRoot())
		retVal += FUtil::SPrintf("%sstruct %s : public PacketBase\n", InIndent.c_str(), InPacket->GetName());
	else
		retVal += FUtil::SPrintf("%sstruct %s\n", InIndent.c_str(), InPacket->GetName());

	retVal += FUtil::SPrintf("%s{\n", InIndent.c_str());

	InPacket->ForeachChildPacket([&](const FPacket* InPacket) {
		retVal += GetPacketStr(InPacket, InIndent + "\t");
	});

	retVal += GetMemberStr(InPacket, InIndent + "\t");
	retVal += GetConstructorStr(InPacket, InIndent + "\t");
	retVal += GetSizeFuncStr(InPacket, InIndent + "\t");
	retVal += GetPacketTypeStr(InPacket, InIndent + "\t");
	retVal += GetSerializeFuncStr(InPacket, InIndent + "\t");
	retVal += GetDeserializeFuncStr(InPacket, InIndent + "\t");
	retVal += FUtil::SPrintf("%s};\n\n", InIndent.c_str());

	return retVal;
}

string FPacketWriter_CPP::GetEnumStr() const
{
	string retVal;

	retVal += "enum PacketType\n";
	retVal += "{\n";
	retVal += "\tPACKET_TYPE_NONE,\n";

	m_PacketParser->ForeachPacket([&](const FPacket* InPacket) {
		retVal += FUtil::SPrintf("\tPACKET_TYPE_%s,\n", FUtil::ToUpper(InPacket->GetName()).c_str());
	});

	retVal += "\tPACKET_TYPE_MAX,\n";
	retVal += "};\n\n";

	return retVal;
}

string FPacketWriter_CPP::GetPacketBaseStr() const
{
	string retVal;

	retVal += "struct PacketBase\n";
	retVal += "{\n";
	retVal += "\tvirtual PacketType GetPacketType() const { return PACKET_TYPE_NONE; }\n";
	retVal += "\tvirtual int Serialize(char* InBuffer) const { return 0; }\n";
	retVal += "};\n";

	return retVal;
}

string FPacketWriter_CPP::GetMemberStr(const FPacket* InPacket, const string& InIndent/* = ""*/) const
{
	string retVal;
	const char* indent = InIndent.c_str();

	InPacket->ForeachMember([&](const FPacketMember& InMember) {
		string type = ConvertTypeToCPPType(InMember);
		if (InMember.arrayTotalCount)
		{
			retVal += FUtil::SPrintf("%s%s %s", indent, type.c_str(), InMember.name.c_str());
			for (int i = 0; i < InMember.arrayCountList.size(); ++i)
				retVal += FUtil::SPrintf("[%d]", InMember.arrayCountList[i]);
			retVal += FUtil::SPrintf(";\n");
		}
		else
		{
			retVal += FUtil::SPrintf("%s%s %s", indent, type.c_str(), InMember.name.c_str());
			if (InMember.memberType == PACKET_MEMBER_TYPE_COMMON || InMember.memberType == PACKET_MEMBER_TYPE_BYTE)
				retVal += " = 0";
			retVal += ";\n";
		}
	});

	retVal += "\n";

	return retVal;
}

string FPacketWriter_CPP::GetConstructorStr(const FPacket* InPacket, const string& InIndent) const
{
	string retVal;
	const char* indent = InIndent.c_str();

	retVal += FUtil::SPrintf("%s%s()\n", indent, InPacket->GetName());
	retVal += FUtil::SPrintf("%s{\n", indent);
	InPacket->ForeachMember([&](const FPacketMember& InMember) {
		if (InMember.arrayTotalCount && InMember.memberType != PACKET_MEMBER_TYPE_STRUCT)
		{
			retVal += FUtil::SPrintf("%s\tZeroMemory(%s, sizeof(%s));\n", indent, InMember.name.c_str(), InMember.name.c_str());
		}
		});
	retVal += FUtil::SPrintf("%s}\n\n", indent);


	retVal += FUtil::SPrintf("%s%s(const char* InBuffer)\n", indent, InPacket->GetName());
	retVal += FUtil::SPrintf("%s{\n", indent);
	retVal += FUtil::SPrintf("%s\tDeserialize(InBuffer);\n", indent);
	retVal += FUtil::SPrintf("%s}\n\n", indent);

	return retVal;
}

string FPacketWriter_CPP::GetSizeFuncStr(const FPacket* InPacket, const string& InIndent/* = ""*/) const
{
	string retVal;
	const char* indent = InIndent.c_str();

	retVal += FUtil::SPrintf("%sint GetSize() const\n", indent);
	retVal += FUtil::SPrintf("%s{\n", indent);
	retVal += FUtil::SPrintf("%s\tint size = 0;\n", indent);

	InPacket->ForeachMember([&](const FPacketMember& InMember) {
		if (InMember.arrayTotalCount)
		{
			switch (InMember.memberType)
			{
			case PACKET_MEMBER_TYPE_STRUCT:
				ALL_MULTI_DIMENSIONAL_ARRAY(retVal, indent, InMember.arrayCountList.size(), InMember.arrayCountList, FUtil::SPrintf("size += %s", InMember.name.c_str()), ".GetSize();\n");
				break;

			case PACKET_MEMBER_TYPE_STRING:
				ALL_MULTI_DIMENSIONAL_ARRAY(retVal, indent, InMember.arrayCountList.size() - 1, InMember.arrayCountList, FUtil::SPrintf("size += sizeof(int) + sizeof(wchar_t) * wcslen(%s", InMember.name.c_str()), ");\n");
				break;

			default:
				ALL_MULTI_DIMENSIONAL_ARRAY(retVal, indent, InMember.arrayCountList.size(), InMember.arrayCountList, FUtil::SPrintf("size += sizeof(%s", InMember.name.c_str()), ");\n");
				break;
			}
		}
		else
		{
			if (InMember.memberType == PACKET_MEMBER_TYPE_STRUCT)
				retVal += FUtil::SPrintf("%s\tsize += %s.GetSize();\n", indent, InMember.name.c_str());
			else
				retVal += FUtil::SPrintf("%s\tsize += sizeof(%s);\n", indent, InMember.name.c_str());
		}
	});

	retVal += FUtil::SPrintf("%s\treturn size;\n", indent);
	retVal += FUtil::SPrintf("%s}\n\n", indent);

	return retVal;
}

string FPacketWriter_CPP::GetPacketTypeStr(const FPacket* InPacket, const string& InIndent) const
{
	string retVal;
	
	if (InPacket->IsRoot())
	{
		retVal += FUtil::SPrintf("%sPacketType GetPacketType() const override\n", InIndent.c_str());
		retVal += FUtil::SPrintf("%s{\n", InIndent.c_str());
		retVal += FUtil::SPrintf("%s\treturn PACKET_TYPE_%s;\n", InIndent.c_str(), InPacket->GetName());
		retVal += FUtil::SPrintf("%s}\n\n", InIndent.c_str());
	}

	return retVal;
}

string FPacketWriter_CPP::GetSerializeFuncStr(const FPacket* InPacket, const string& InIndent/* = ""*/) const
{
	string retVal;
	const char* indent = InIndent.c_str();

	retVal += FUtil::SPrintf("%sint Serialize(char* InBuffer) const \n", indent);
	retVal += FUtil::SPrintf("%s{\n", indent);
	retVal += FUtil::SPrintf("%s\tint offset = 0;\n", indent);

	// Buffer 제일 앞에 패킷 총 사이즈와 패킷 Enum 값 저장
	if (InPacket->IsRoot())
	{
		retVal += FUtil::SPrintf("%s\tint size = GetSize() + sizeof(int) + sizeof(PacketType);\n", indent);
		retVal += FUtil::SPrintf("%s\tmemcpy(InBuffer + offset, &size, sizeof(size));\n", indent);
		retVal += FUtil::SPrintf("%s\toffset += sizeof(size);\n", indent);
		retVal += FUtil::SPrintf("%s\tPacketType packetType = GetPacketType();\n", indent);
		retVal += FUtil::SPrintf("%s\tmemcpy(InBuffer + offset, &packetType, sizeof(PacketType));\n", indent);
		retVal += FUtil::SPrintf("%s\toffset += sizeof(PacketType);\n", indent);
	}

	InPacket->ForeachMember([&](const FPacketMember& InMember) {
		if (InMember.arrayTotalCount)
		{
			switch (InMember.memberType)
			{
			case PACKET_MEMBER_TYPE_STRUCT:
				ALL_MULTI_DIMENSIONAL_ARRAY(retVal, indent, InMember.arrayCountList.size(), InMember.arrayCountList,
					FUtil::SPrintf("offset += %s", InMember.name.c_str()),
					".Serialize(InBuffer + offset);\n");
				break;

			case PACKET_MEMBER_TYPE_STRING:
				START_MULTI_DIMENSIONAL_ARRAY(retVal, indent, InMember.arrayCountList.size() - 1, InMember.arrayCountList);
				MIDDLE_MULTI_DIMENSIONAL_ARRAY(retVal, arrayIndent, InMember.arrayCountList.size() - 1, FUtil::SPrintf("int %s_length = wcslen(%s", InMember.name.c_str(), InMember.name.c_str()), ");\n");
				retVal += FUtil::SPrintf("%s\tmemcpy(InBuffer + offset, &%s_length, sizeof(%s_length));\n", arrayIndent.c_str(), InMember.name.c_str(), InMember.name.c_str());
				retVal += FUtil::SPrintf("%s\toffset += sizeof(%s_length);\n", arrayIndent.c_str(), InMember.name.c_str());
				MIDDLE_MULTI_DIMENSIONAL_ARRAY(retVal, arrayIndent, InMember.arrayCountList.size() - 1, FUtil::SPrintf("memcpy(InBuffer + offset, %s", InMember.name.c_str()), FUtil::SPrintf(", sizeof(wchar_t) * %s_length);\n", InMember.name.c_str()));
				retVal += FUtil::SPrintf("%s\toffset += sizeof(wchar_t) * %s_length;\n", arrayIndent.c_str(), InMember.name.c_str());
				END_MULTI_DIMENSIONAL_ARRAY(retVal, indent, InMember.arrayCountList.size() - 1);
				break;

			default:
				retVal += FUtil::SPrintf("%s\tmemcpy(InBuffer + offset, %s, sizeof(%s));\n", indent, InMember.name.c_str(), InMember.name.c_str());
				retVal += FUtil::SPrintf("%s\toffset += sizeof(%s);\n", indent, InMember.name.c_str());
			}
		}
		else
		{
			if (InMember.memberType == PACKET_MEMBER_TYPE_STRUCT)
				retVal += FUtil::SPrintf("%s\toffset += %s.Serialize(InBuffer + offset);\n", indent, InMember.name.c_str());
			else
			{
				retVal += FUtil::SPrintf("%s\tmemcpy(InBuffer + offset, &%s, sizeof(%s));\n", indent, InMember.name.c_str(), InMember.name.c_str());
				retVal += FUtil::SPrintf("%s\toffset += sizeof(%s);\n", indent, InMember.name.c_str());
			}
		}
		});

	retVal += FUtil::SPrintf("%s\treturn offset;\n", indent);
	retVal += FUtil::SPrintf("%s}\n\n", indent);

	return retVal;
}

string FPacketWriter_CPP::GetDeserializeFuncStr(const FPacket* InPacket, const string& InIndent/* = ""*/) const
{
	string retVal;
	const char* indent = InIndent.c_str();

	retVal += FUtil::SPrintf("%sint Deserialize(const char* InBuffer)\n", indent);
	retVal += FUtil::SPrintf("%s{\n", indent);
	retVal += FUtil::SPrintf("%s\tint offset = 0;\n", indent);

	InPacket->ForeachMember([&](const FPacketMember& InMember) {
		if (InMember.arrayTotalCount)
		{
			switch (InMember.memberType)
			{
			case PACKET_MEMBER_TYPE_STRUCT:
				ALL_MULTI_DIMENSIONAL_ARRAY(retVal, indent, InMember.arrayCountList.size(), InMember.arrayCountList,
					FUtil::SPrintf("offset += %s", InMember.name.c_str()),
					".Deserialize(InBuffer + offset);\n");
				break;

			case PACKET_MEMBER_TYPE_STRING:
				START_MULTI_DIMENSIONAL_ARRAY(retVal, indent, InMember.arrayCountList.size() - 1, InMember.arrayCountList);
				retVal += FUtil::SPrintf("%s\tint %s_length = 0;\n", arrayIndent.c_str(), InMember.name.c_str());
				retVal += FUtil::SPrintf("%s\tmemcpy(&%s_length, InBuffer + offset, sizeof(%s_length));\n", arrayIndent.c_str(), InMember.name.c_str(), InMember.name.c_str());
				retVal += FUtil::SPrintf("%s\toffset += sizeof(%s_length);\n", arrayIndent.c_str(), InMember.name.c_str());
				MIDDLE_MULTI_DIMENSIONAL_ARRAY(retVal, arrayIndent, InMember.arrayCountList.size() - 1, FUtil::SPrintf("memcpy(%s", InMember.name.c_str()), FUtil::SPrintf(", InBuffer + offset, sizeof(wchar_t) * %s_length);\n", InMember.name.c_str()));
				MIDDLE_MULTI_DIMENSIONAL_ARRAY(retVal, arrayIndent, InMember.arrayCountList.size() - 1, FUtil::SPrintf("%s", InMember.name.c_str()), FUtil::SPrintf("[% s_length] = \'\\0\';\n", InMember.name.c_str()));
				retVal += FUtil::SPrintf("%s\toffset += sizeof(wchar_t) * %s_length;\n", arrayIndent.c_str(), InMember.name.c_str());
				END_MULTI_DIMENSIONAL_ARRAY(retVal, indent, InMember.arrayCountList.size() - 1);
				break;

			default:
				retVal += FUtil::SPrintf("%s\tmemcpy(%s, InBuffer + offset, sizeof(%s));\n", indent, InMember.name.c_str(), InMember.name.c_str());
				retVal += FUtil::SPrintf("%s\toffset += sizeof(%s);\n", indent, InMember.name.c_str());
			}
		}
		else
		{
			if (InMember.memberType == PACKET_MEMBER_TYPE_STRUCT)
				retVal += FUtil::SPrintf("%s\toffset += %s.Deserialize(InBuffer + offset);\n", indent, InMember.name.c_str());
			else
			{
				retVal += FUtil::SPrintf("%s\tmemcpy(&%s, InBuffer + offset, sizeof(%s));\n", indent, InMember.name.c_str(), InMember.name.c_str());
				retVal += FUtil::SPrintf("%s\toffset += sizeof(%s);\n", indent, InMember.name.c_str());
			}
		}
		});

	retVal += FUtil::SPrintf("%s\treturn offset;\n", indent);
	retVal += FUtil::SPrintf("%s}\n\n", indent);

	return retVal;
}

string FPacketWriter_CPP::ConvertTypeToCPPType(const FPacketMember& InMember) const
{
	switch (InMember.memberType)
	{
	case PACKET_MEMBER_TYPE_STRING: return "wchar_t";
	case PACKET_MEMBER_TYPE_BYTE: return "char";
	default:
		if (InMember.type == "int64")
			return "__int64";
	}

	return InMember.type;
}
