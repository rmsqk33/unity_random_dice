#pragma once

class FPacket;

// ��Ŷ �ؽ�Ʈ ���� ��ü �Ľ� Ŭ����
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

