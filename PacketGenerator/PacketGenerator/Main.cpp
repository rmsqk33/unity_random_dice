#include "stdafx.h"
#include "io.h"

#include <iostream>
#include <Windows.h>

#include "FPacketWriter_CPP.h"
#include "FPacketWriter_CSharp.h"
#include "FPacketParser.h"
#include "FUtil.h"


int main(int InArgCount, char** InArgValues)
{
#ifdef _DEBUG
	string packetFilePath = "E:\\Unity Projects\\random_dice_simple_server\\Server\\Packet.txt";
	string writeFilePath_CPP = "C:\\Users\\rkdrb\\source\\repos\\Test\\Test\\Packet.h";
	string writeFilePath_CSharp = "C:\\Users\\rkdrb\\source\\repos\\TestCSharp\\TestCSharp\\Packet.cs"; 
#else
	if (InArgCount < 4)
		return 0;

	string packetFilePath = InArgValues[1];
	string writeFilePath_CPP = InArgValues[2];
	string writeFilePath_CSharp = InArgValues[3];
#endif

	// ================================== C++ 처리 ==================================
	size_t findIndex = writeFilePath_CPP.find_last_of('\\');
	string withoutFileName = writeFilePath_CPP.substr(0, findIndex + 1);
	string withoutFilePath = writeFilePath_CPP.substr(findIndex + 1, writeFilePath_CPP.find_last_of('.') - findIndex - 1);
	string checkFilePath_CPP = FUtil::SPrintf("%scheckCPP_%s.fcheck", withoutFileName.c_str(), withoutFilePath.c_str());

	WIN32_FIND_DATAA packetFileInfo;
	HANDLE packetFileHandle = FindFirstFileA(packetFilePath.c_str(), &packetFileInfo);

	bool needWriteFile_CPP = _access(writeFilePath_CPP.c_str(), 0) == -1;
	if (!needWriteFile_CPP)
	{
		ifstream checkFileStream(checkFilePath_CPP.c_str());
		needWriteFile_CPP = !checkFileStream.is_open();

		if (!needWriteFile_CPP)
		{
			string readLine;
			getline(checkFileStream, readLine);
			needWriteFile_CPP = FUtil::SPrintf("%lu%lu", packetFileInfo.ftLastWriteTime.dwHighDateTime, packetFileInfo.ftLastWriteTime.dwLowDateTime) != readLine;
		}

		checkFileStream.close();
	}
	
	FPacketParser parser;
	parser.ParsePacketFile(packetFilePath);
	//if (needWriteFile_CPP)
	{
		FPacketWriter_CPP packetWriter(&parser);
		packetWriter.Write(writeFilePath_CPP);

		ofstream checkFileStream(checkFilePath_CPP.c_str());
		checkFileStream << FUtil::SPrintf("%lu%lu", packetFileInfo.ftLastWriteTime.dwHighDateTime, packetFileInfo.ftLastWriteTime.dwLowDateTime);
		checkFileStream.close();
	}

	// ================================== C# 처리 ==================================
	findIndex = writeFilePath_CSharp.find_last_of('\\');
	withoutFileName = writeFilePath_CSharp.substr(0, findIndex + 1);
	withoutFilePath = writeFilePath_CSharp.substr(findIndex + 1, writeFilePath_CSharp.find_last_of('.') - findIndex - 1);
	string checkFilePath_CSharp = FUtil::SPrintf("%scheckCS_%s.fcheck", withoutFileName.c_str(), withoutFilePath.c_str());

	bool needWriteFile_CSharp = _access(writeFilePath_CSharp.c_str(), 0) == -1;
	if (!needWriteFile_CSharp)
	{
		ifstream checkFileStream(checkFilePath_CSharp.c_str());
		needWriteFile_CSharp = !checkFileStream.is_open();

		if (!needWriteFile_CSharp)
		{
			string readLine;
			getline(checkFileStream, readLine);
			needWriteFile_CSharp = FUtil::SPrintf("%lu%lu", packetFileInfo.ftLastWriteTime.dwHighDateTime, packetFileInfo.ftLastWriteTime.dwLowDateTime) != readLine;
		}

		checkFileStream.close();
	}

	//if (needWriteFile_CSharp)
	{
		FPacketWriter_CSharp packetWriter(&parser);
		packetWriter.Write(writeFilePath_CSharp);

		ofstream checkFileStream(checkFilePath_CSharp.c_str());
		checkFileStream << FUtil::SPrintf("%lu%lu", packetFileInfo.ftLastWriteTime.dwHighDateTime, packetFileInfo.ftLastWriteTime.dwLowDateTime);
		checkFileStream.close();
	}
	
	FindClose(packetFileHandle);

	OutputDebugString(L"Packet Generate Complete");

	return 0;
}
