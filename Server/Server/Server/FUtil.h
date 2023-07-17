#pragma once

namespace FUtil
{
    vector<string> SplitString(const string& InStr, const string& InDelimeters);
    string SPrintf(const char* InFormat, ...);

    string ItoA(int InNum);
    int AtoI(const char* InStr);
    __int64 AtoI64(const char* InStr);
    double AtoF(const char* InStr);

    void CharToWChar(wchar_t* OutWStr, const char* InStr);
    void WCharToChar(char* OutStr, const wchar_t* InWStr);
    void WCharToChar(string& OutStr, const wchar_t* InWStr);

    int Rand(int InMin, int InMax);

    template<typename T1, typename T2>
    vector<pair<T1, T2>> ConvertMapToList(const map<T1, T2>& InMap)
    {
        vector<pair<T1, T2>> list;
        for (auto& iter : InMap)
        {
            list.push_back(pair<T1, T2>(iter.first, iter.second));
        }
        return list;
    }
};