#pragma once

namespace FUtil
{
	vector<string> SplitStringByBlank(const string& InStr);
	vector<string> SplitStringByDelimeter(const string& InStr, const char* InDelimiter, int InDelCount);
	string ToUpper(const char* InStr);
	string SPrintf(const char* InFormat, ...);
}