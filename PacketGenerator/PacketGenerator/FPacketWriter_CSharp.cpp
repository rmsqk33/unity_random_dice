#include "stdafx.h"
#include "FPacketWriter_CSharp.h"
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

#define MIDDLE_MULTI_DIMENSIONAL_ARRAY(outStr, count, startFunc, endFunc)\
	outStr += arrayIndent + "\t";\
	outStr += startFunc;\
	if(0 < count)\
		outStr += "[";\
	for (int i = 0; i < count; ++i)\
	{\
		char forAlphabet = 105 + i; \
		outStr += FUtil::SPrintf("%c,", forAlphabet); \
	}\
	if (0 < count)\
	{\
		outStr.pop_back();\
		outStr += "]";\
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
	MIDDLE_MULTI_DIMENSIONAL_ARRAY(outStr, count, startFunc, endFunc)\
	END_MULTI_DIMENSIONAL_ARRAY(outStr, indent, count)\
}

FPacketWriter_CSharp::FPacketWriter_CSharp(const FPacketParser* InParser)
	: m_PacketParser(InParser)
{
}

void FPacketWriter_CSharp::Write(const string& InFilePath)
{
	ofstream writeFile(InFilePath);
	if (!writeFile.is_open())
		return;

	writeFile << "using System;\n";
	writeFile << "using System.Text;\n";
	writeFile << "using System.Collections.Generic;\n";
	writeFile << "using System.Linq;\n\n";

	writeFile << "namespace Packet\n";
	writeFile << "{\n";
	writeFile << GetEnumStr();
	writeFile << GetPacketBaseStr();
	
	m_PacketParser->ForeachPacket([&](const FPacket* InPacket) {
		writeFile << GetPacketStr(InPacket, "\t");
		});
	writeFile << "}\n";

	writeFile.close();
}

string FPacketWriter_CSharp::GetPacketStr(const FPacket* InPacket, const string& InIndent/* = ""*/) const
{
	string retVal;

	if (InPacket->IsRoot())
		retVal += FUtil::SPrintf("%spublic class %s : PacketBase\n", InIndent.c_str(), InPacket->GetName());
	else
		retVal += FUtil::SPrintf("%spublic class %s\n", InIndent.c_str(), InPacket->GetName());
	
	retVal += FUtil::SPrintf("%s{\n", InIndent.c_str());

	InPacket->ForeachChildPacket([&](const FPacket* InPacket) {
		retVal += GetPacketStr(InPacket, InIndent + "\t");
		});

	retVal += GetMemberStr(InPacket, InIndent + "\t");
	retVal += GetConstructor(InPacket, InIndent + "\t");
	retVal += GetSizeFuncStr(InPacket, InIndent + "\t");
	retVal += GetPacketTypeStr(InPacket, InIndent + "\t");
	retVal += GetSerializeFuncStr(InPacket, InIndent + "\t");
	retVal += GetDeserializeFuncStr(InPacket, InIndent + "\t");
	retVal += FUtil::SPrintf("%s}\n\n", InIndent.c_str());

	return retVal;
}

string FPacketWriter_CSharp::GetEnumStr()
{
	string retVal;

	retVal += "\tpublic enum PacketType\n";
	retVal += "\t{\n";
	retVal += "\t\tPACKET_TYPE_NONE,\n";

	m_PacketParser->ForeachPacket([&](const FPacket* InPacket) {
		retVal += FUtil::SPrintf("\t\tPACKET_TYPE_%s,\n", InPacket->GetName());
		});

	retVal += "\t\tPACKET_TYPE_MAX,\n";
	retVal += "\t}\n\n";

	return retVal;
}

string FPacketWriter_CSharp::GetPacketBaseStr() const
{
	string retVal;

	retVal += "\tpublic class PacketBase\n";
	retVal += "\t{\n";
	retVal += "\t\tpublic virtual PacketType GetPacketType() { return PacketType.PACKET_TYPE_NONE; }\n";
	retVal += "\t\tpublic virtual void Serialize(List<byte> InBuffer) { }\n";
	retVal += "\t}\n\n";

	return retVal;
}

string FPacketWriter_CSharp::GetMemberStr(const FPacket* InPacket, const string& InIndent/* = ""*/) const
{
	string retVal;
	const char* indent = InIndent.c_str();

	InPacket->ForeachMember([&](const FPacketMember& InMember) {
		string type = ConvertTypeToCSharpType(InMember);
		if (InMember.arrayTotalCount)
		{
			int commaCount = InMember.memberType == PACKET_MEMBER_TYPE_STRING ? InMember.arrayCountList.size() - 2 : InMember.arrayCountList.size() - 1;
			int dimensionCount = InMember.memberType == PACKET_MEMBER_TYPE_STRING ? InMember.arrayCountList.size() - 1 : InMember.arrayCountList.size();

			retVal += FUtil::SPrintf("%spublic %s", indent, type.c_str());
			if (0 < dimensionCount) retVal += "[";
			for (int i = 0; i < commaCount; ++i)
				retVal += ",";
			if (0 < dimensionCount) retVal += "]";
			retVal += FUtil::SPrintf(" %s", InMember.name.c_str());
			retVal += FUtil::SPrintf(" = new %s", type.c_str());
			if (0 < dimensionCount) retVal += "[";
			for (int i = 0; i < dimensionCount; ++i)
				retVal += FUtil::SPrintf("%d,", InMember.arrayCountList[i]);
			if (0 < dimensionCount)
			{
				retVal.pop_back();
				retVal += "]";
			}

			if(InMember.memberType == PACKET_MEMBER_TYPE_STRING && InMember.arrayCountList.size() == 1)
				retVal += "(\"\")";

			retVal += ";\n";
		}
		else
		{
			if (InMember.memberType == PACKET_MEMBER_TYPE_STRUCT)
				retVal += FUtil::SPrintf("%spublic %s %s = new %s();\n", indent, type.c_str(), InMember.name.c_str(), type.c_str());
			else
				retVal += FUtil::SPrintf("%spublic %s %s;\n", indent, type.c_str(), InMember.name.c_str());
		}

		retVal += "\n";
		});

	return retVal;
}

string FPacketWriter_CSharp::GetConstructor(const FPacket* InPacket, const string& InIndent) const
{
	string retVal;
	const char* indent = InIndent.c_str();

	retVal += FUtil::SPrintf("%spublic %s()\n", indent, InPacket->GetName());
	retVal += FUtil::SPrintf("%s{\n", indent);

	InPacket->ForeachMember([&](const FPacketMember& InMember) {
		string type = ConvertTypeToCSharpType(InMember);
		if (InMember.memberType == PACKET_MEMBER_TYPE_STRUCT)
		{
			ALL_MULTI_DIMENSIONAL_ARRAY(retVal, indent, InMember.arrayCountList.size(), InMember.arrayCountList,
				FUtil::SPrintf("%s", InMember.name.c_str()), FUtil::SPrintf(" = new %s();\n", type.c_str()));
		}
		});

	retVal += FUtil::SPrintf("%s}\n\n", indent);

	retVal += FUtil::SPrintf("%spublic %s(in byte[] InBuffer)\n", indent, InPacket->GetName());
	retVal += FUtil::SPrintf("%s{\n", indent);

	InPacket->ForeachMember([&](const FPacketMember& InMember) {
		string type = ConvertTypeToCSharpType(InMember);
		if (InMember.memberType == PACKET_MEMBER_TYPE_STRUCT)
		{
			ALL_MULTI_DIMENSIONAL_ARRAY(retVal, indent, InMember.arrayCountList.size(), InMember.arrayCountList,
				FUtil::SPrintf("%s", InMember.name.c_str()), FUtil::SPrintf(" = new %s();\n", type.c_str()));
		}
		});

	retVal += FUtil::SPrintf("%s\tDeserialize(InBuffer);\n", indent);
	retVal += FUtil::SPrintf("%s}\n\n", indent);

	return retVal;
}

string FPacketWriter_CSharp::GetSizeFuncStr(const FPacket* InPacket, const string& InIndent/* = ""*/) const
{
	string retVal;
	const char* indent = InIndent.c_str();

	retVal += FUtil::SPrintf("%spublic int GetSize()\n", indent);
	retVal += FUtil::SPrintf("%s{\n", indent);
	retVal += FUtil::SPrintf("%s\tint size = 0;\n", indent);

	InPacket->ForeachMember([&](const FPacketMember& InMember) {
		string type = ConvertTypeToCSharpType(InMember);
		if (InMember.arrayTotalCount)
		{
			switch (InMember.memberType)
			{
			case PACKET_MEMBER_TYPE_STRUCT:
				ALL_MULTI_DIMENSIONAL_ARRAY(retVal, indent, InMember.arrayCountList.size(), InMember.arrayCountList,
					FUtil::SPrintf("size += %s", InMember.name.c_str()),
					".GetSize();\n");
				break;
			case PACKET_MEMBER_TYPE_STRING:
				START_MULTI_DIMENSIONAL_ARRAY(retVal, indent, InMember.arrayCountList.size() - 1, InMember.arrayCountList);
				MIDDLE_MULTI_DIMENSIONAL_ARRAY(retVal, 0, "size += sizeof(int);\n", "");
				MIDDLE_MULTI_DIMENSIONAL_ARRAY(retVal, 0, FUtil::SPrintf("size += sizeof(char) * %s", InMember.name.c_str()), ".Length; \n");
				END_MULTI_DIMENSIONAL_ARRAY(retVal, indent, InMember.arrayCountList.size() - 1);
				break;
			default:
				retVal += FUtil::SPrintf("%s\tsize += sizeof(%s) * %d;\n", indent, type.c_str(), InMember.arrayTotalCount);
			}
		}
		else
		{
			if (InMember.memberType == PACKET_MEMBER_TYPE_STRUCT)
				retVal += FUtil::SPrintf("%s\tsize += %s.GetSize();\n", indent, InMember.name.c_str());
			else
				retVal += FUtil::SPrintf("%s\tsize += sizeof(%s);\n", indent, type.c_str());
		}
	});

	retVal += FUtil::SPrintf("%s\treturn size;\n", indent);
	retVal += FUtil::SPrintf("%s}\n\n", indent);

	return retVal;
}

string FPacketWriter_CSharp::GetPacketTypeStr(const FPacket* InPacket, const string& InIndent) const
{
	string retVal;

	if (InPacket->IsRoot())
	{
		retVal += FUtil::SPrintf("%spublic override PacketType GetPacketType()\n", InIndent.c_str());
		retVal += FUtil::SPrintf("%s{\n", InIndent.c_str());
		retVal += FUtil::SPrintf("%s\treturn PacketType.PACKET_TYPE_%s;\n", InIndent.c_str(), InPacket->GetName());
		retVal += FUtil::SPrintf("%s}\n", InIndent.c_str());
	}

	return retVal;
}

string FPacketWriter_CSharp::GetSerializeFuncStr(const FPacket* InPacket, const string& InIndent/* = ""*/) const
{
	string retVal;
	const char* indent = InIndent.c_str();

	if (InPacket->IsRoot())
		retVal += FUtil::SPrintf("%spublic override void Serialize(List<byte> InBuffer)\n", indent);
	else
		retVal += FUtil::SPrintf("%spublic void Serialize(List<byte> InBuffer)\n", indent);

	retVal += FUtil::SPrintf("%s{\n", indent);

	// Buffer 제일 앞에 패킷 총 사이즈와 패킷 Enum 값 저장
	if (InPacket->IsRoot())
	{
		retVal += FUtil::SPrintf("%s\tint size = GetSize() + sizeof(int) + sizeof(PacketType);\n", indent);
		retVal += FUtil::SPrintf("%s\tInBuffer.AddRange(BitConverter.GetBytes(size));\n", indent);
		retVal += FUtil::SPrintf("%s\tInBuffer.AddRange(BitConverter.GetBytes((int)GetPacketType()));\n", indent);
	}

	InPacket->ForeachMember([&](const FPacketMember& InMember) {
		if (InMember.arrayTotalCount)
		{
			switch (InMember.memberType)
			{
			case PACKET_MEMBER_TYPE_STRUCT:
				ALL_MULTI_DIMENSIONAL_ARRAY(retVal, indent, InMember.arrayCountList.size(), InMember.arrayCountList,
					FUtil::SPrintf("%s", InMember.name.c_str()),
					".Serialize(InBuffer); \n");
				break;

			case PACKET_MEMBER_TYPE_STRING:
				START_MULTI_DIMENSIONAL_ARRAY(retVal, indent, InMember.arrayCountList.size() - 1, InMember.arrayCountList);
				MIDDLE_MULTI_DIMENSIONAL_ARRAY(retVal, InMember.arrayCountList.size() - 1, FUtil::SPrintf("InBuffer.AddRange(BitConverter.GetBytes(%s", InMember.name.c_str()), ".Length));\n");
				MIDDLE_MULTI_DIMENSIONAL_ARRAY(retVal, InMember.arrayCountList.size() - 1, FUtil::SPrintf("InBuffer.AddRange(Encoding.Unicode.GetBytes(%s", InMember.name.c_str()),"));\n");
				END_MULTI_DIMENSIONAL_ARRAY(retVal, indent, InMember.arrayCountList.size() - 1);
				break;

			case PACKET_MEMBER_TYPE_BYTE:
				ALL_MULTI_DIMENSIONAL_ARRAY(retVal, indent, InMember.arrayCountList.size(), InMember.arrayCountList,
					FUtil::SPrintf("InBuffer.Add(%s", InMember.name.c_str()), ");\n");
				break;

			default:
				ALL_MULTI_DIMENSIONAL_ARRAY(retVal, indent, InMember.arrayCountList.size(), InMember.arrayCountList,
					FUtil::SPrintf("InBuffer.AddRange(BitConverter.GetBytes(%s", InMember.name.c_str()), "));\n");
			}
		}
		else
		{
			switch (InMember.memberType)
			{
			case PACKET_MEMBER_TYPE_STRUCT:
				retVal += FUtil::SPrintf("%s\t%s.Serialize(InBuffer);\n", indent, InMember.name.c_str());
				break;

			case PACKET_MEMBER_TYPE_BYTE:
				retVal += FUtil::SPrintf("%s\tInBuffer.Add(%s);\n", indent, InMember.name.c_str());
				break;

			default:
				retVal += FUtil::SPrintf("%s\tInBuffer.AddRange(BitConverter.GetBytes(%s));\n", indent, InMember.name.c_str());
			}
		}
		});

	retVal += FUtil::SPrintf("%s}\n\n", indent);

	return retVal;
}

string FPacketWriter_CSharp::GetDeserializeFuncStr(const FPacket* InPacket, const string& InIndent/* = ""*/) const
{
	string retVal;
	const char* indent = InIndent.c_str();

	retVal += FUtil::SPrintf("%spublic int Deserialize(in byte[] InBuffer, int offset = 0)\n", indent);
	retVal += FUtil::SPrintf("%s{\n", indent);

	InPacket->ForeachMember([&](const FPacketMember& InMember) {
		string type = ConvertTypeToCSharpType(InMember);
		if (InMember.arrayTotalCount)
		{
			switch (InMember.memberType)
			{
			case PACKET_MEMBER_TYPE_STRUCT:
				ALL_MULTI_DIMENSIONAL_ARRAY(retVal, indent, InMember.arrayCountList.size(), InMember.arrayCountList, FUtil::SPrintf("offset = %s", InMember.name.c_str()), ".Deserialize(InBuffer, offset);\n");
				break;

			case PACKET_MEMBER_TYPE_STRING:
				START_MULTI_DIMENSIONAL_ARRAY(retVal, indent, InMember.arrayCountList.size() - 1, InMember.arrayCountList);
				retVal += FUtil::SPrintf("%s\tint %s_length = %s * sizeof(char);\n", arrayIndent.c_str(), InMember.name.c_str(), GetBitToValueStr("int").c_str());
				retVal += FUtil::SPrintf("%s\toffset += sizeof(int);\n", arrayIndent.c_str());
				MIDDLE_MULTI_DIMENSIONAL_ARRAY(retVal, InMember.arrayCountList.size() - 1, FUtil::SPrintf("%s", InMember.name.c_str()), FUtil::SPrintf(" = Encoding.Unicode.GetString(InBuffer, offset, %s_length);\n", InMember.name.c_str()));
				retVal += FUtil::SPrintf("%s\toffset += %s_length;\n", arrayIndent.c_str(), InMember.name.c_str());
				END_MULTI_DIMENSIONAL_ARRAY(retVal, indent, InMember.arrayCountList.size() - 1);
				break;

			default:
				retVal += FUtil::SPrintf("%s\tBuffer.BlockCopy(InBuffer, offset, %s, 0, sizeof(%s) * %d);\n", indent, InMember.name.c_str(), type.c_str(), InMember.arrayTotalCount);
				retVal += FUtil::SPrintf("%s\toffset += sizeof(%s) * %d;\n", indent, type.c_str(), InMember.arrayTotalCount);
			}
		}
		else
		{
			if (InMember.memberType == PACKET_MEMBER_TYPE_STRUCT)
				retVal += FUtil::SPrintf("%s\toffset = %s.Deserialize(InBuffer, offset);\n", indent, InMember.name.c_str());
			else
			{
				retVal += FUtil::SPrintf("%s\t%s = %s;\n", indent, InMember.name.c_str(), GetBitToValueStr(InMember.type).c_str());
				retVal += FUtil::SPrintf("%s\toffset += sizeof(%s);\n", indent, type.c_str());
			}
		}
		});

	retVal += FUtil::SPrintf("%s\treturn offset;\n", indent);
	retVal += FUtil::SPrintf("%s}\n\n", indent);

	return retVal;
}

string FPacketWriter_CSharp::GetBitToValueStr(const string& InType) const
{
	string retVal;

	if (InType == "byte")
		retVal = FUtil::SPrintf("InBuffer[offset]");
	else if (InType == "bool")
		retVal = FUtil::SPrintf("BitConverter.ToBoolean(InBuffer, offset)");
	else if (InType == "short")
		retVal = FUtil::SPrintf("BitConverter.ToInt16(InBuffer, offset)"); 
	else if (InType == "int")
		retVal = FUtil::SPrintf("BitConverter.ToInt32(InBuffer, offset)");
	else if (InType == "int64")
		retVal = FUtil::SPrintf("BitConverter.ToInt64(InBuffer, offset)");
	else if (InType == "float")
		retVal = FUtil::SPrintf("BitConverter.ToSingle(InBuffer, offset)");
	else if (InType == "double")
		retVal = FUtil::SPrintf("BitConverter.ToDouble(InBuffer, offset)");

	return retVal;
}

string FPacketWriter_CSharp::ConvertTypeToCSharpType(const FPacketMember& InMember) const
{
	if (InMember.type == "int64")
		return "Int64";
	else if (InMember.memberType == PACKET_MEMBER_TYPE_STRING)
		return "string";

	return InMember.type;
}
