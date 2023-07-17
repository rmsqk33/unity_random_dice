#include "stdafx.h"
#include "FPacketParser.h"
#include "FPacket.h"
#include "FUtil.h"

FPacketParser::FPacketParser()
{
}

FPacketParser::~FPacketParser()
{
	for (int i = 0; i < m_PacketList.size(); ++i)
	{
		delete m_PacketList[i];
	}
	m_PacketList.clear();
}

void FPacketParser::ParsePacketFile(const string& InFilePath)
{
	ifstream readFile(InFilePath);
	if (!readFile.is_open())
		return;

	string readLine;
	while (getline(readFile, readLine))
	{
		size_t findStrIndex = readLine.find("struct");
		if (findStrIndex == string::npos)
			continue;
		
		vector<string> splitList = FUtil::SplitStringByBlank(readLine);
		if (splitList.size() < 2)
			continue;

		m_PacketList.push_back(new FPacket(readFile, splitList[1], true));
	}

	readFile.close();
}

void FPacketParser::ForeachPacket(const function<void(const FPacket*)>& InFunc) const
{
	for (int i = 0; i < m_PacketList.size(); ++i)
	{
		InFunc(m_PacketList[i]);
	}
}