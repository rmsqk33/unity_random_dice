#pragma once

class FPacket;

// 패킷 텍스트 파일 전체 파싱 클래스
class FPacketParser
{
private:
	vector<FPacket*> m_PacketList;

public:
	FPacketParser();
	~FPacketParser();

	void ParsePacketFile(const string& InFilePath);
	void ForeachPacket(const function<void (const FPacket*)>& InFunc) const;
};

