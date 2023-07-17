#include "stdafx.h"

namespace FUtil
{
    vector<string> SplitString(const string& InStr, const string& InDelimeters)
    {
        vector<string> retList;

        int pivot = 0;
        for (int i = 0; i < InStr.size(); ++i)
        {
            for (int j = 0; j < InDelimeters.size(); ++j)
            {
                if (InStr[i] != InDelimeters[j])
                    continue;
             
                if(i != pivot)
                    retList.push_back(InStr.substr(pivot, i - pivot));

                pivot = i + 1;
            }
        }

        if (pivot != InStr.length())
            retList.push_back(InStr.substr(pivot, InStr.length() - pivot));

        return retList;
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

    string ItoA(int InNum)
    {
        char buffer[1024];
        _itoa_s(InNum, buffer, 1024, 10);
        return string(buffer);
    }

    int AtoI(const char* InStr)
    {
        return atoi(InStr);
    }

    __int64 AtoI64(const char* InStr)
    {
        return _atoi64(InStr);
    }

    double AtoF(const char* InStr)
    {
        return atof(InStr);
    }

    void WCharToChar(char* OutStr, const wchar_t* InWStr)
    {
        int strSize = WideCharToMultiByte(CP_ACP, 0, InWStr, -1, nullptr, 0, nullptr, nullptr);
        WideCharToMultiByte(CP_ACP, 0, InWStr, -1, OutStr, strSize, nullptr, nullptr);
    }

    void WCharToChar(string& OutStr, const wchar_t* InWStr)
    {
        char str[1024];
        int strSize = WideCharToMultiByte(CP_ACP, 0, InWStr, -1, nullptr, 0, nullptr, nullptr);
        WideCharToMultiByte(CP_ACP, 0, InWStr, -1, str, strSize, nullptr, nullptr);
        OutStr = str;
    }
    
    void CharToWChar(wchar_t* OutWStr, const char* InStr)
    {
        int strSize = MultiByteToWideChar(CP_ACP, 0, InStr, -1, nullptr, 0);
        MultiByteToWideChar(CP_ACP, 0, InStr, strlen(InStr) + 1, OutWStr, strSize);
    }

    int Rand(int InMin, int InMax)
    {
        return rand() % (InMax - InMin + 1) + InMin;
    }

}