#include "stdafx.h"
#include "FUtil.h"
#include <cstdarg>

namespace FUtil
{
	vector<string> SplitStringByBlank(const string& InStr)
	{
		char delimeters[] = { ' ', '\t' };
		return SplitStringByDelimeter(InStr, delimeters, 2);
	}

	vector<string> SplitStringByDelimeter(const string& InStr, const char* InDelimeters, int InDelCount)
	{
		vector<string> retList;

		size_t front = 0;
		for (size_t i = 0; i < InStr.length(); ++i)
		{
			for (size_t j = 0; j < InDelCount; ++j)
			{
				if (InStr[i] != InDelimeters[j])
					continue;

				if (front != i)
				{
					string subStr = InStr.substr(front, i - front);
					retList.push_back(subStr);
					front = i + 1;
					break;
				}

				++front;
				break;
			}
		}

		if (front < InStr.length())
		{
			string subStr = InStr.substr(front, InStr.length() - front);
			retList.push_back(subStr);
		}

		return retList;
	}

	string ToUpper(const char* InStr)
	{
		string retVal;

		size_t length = strlen(InStr);
		for (int i = 0; i < length; ++i)
		{
			retVal.push_back(toupper(InStr[i]));
		}

		return retVal;
	}

	string SPrintf(const char* InFormat, ...)
	{
		char buffer[1024];

		va_list args;
		va_start(args, InFormat);
		int length = _vsprintf_s_l(buffer, 1024, InFormat, NULL, args);
		va_end(args);

		return string(buffer);
	}
}