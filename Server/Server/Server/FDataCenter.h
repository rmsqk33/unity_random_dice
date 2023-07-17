#pragma once

#include "FSingleton.h"
#include "FDataNode.h"

class FDataCenter : public FSingleton<FDataCenter>
{
private:
    FDataNode* m_RootNode = nullptr;

public:
    FDataCenter();
    ~FDataCenter();

    void LoadData();

    const FDataNode* FindNode(const string& InQuery) const;
    vector<const FDataNode*> FindNodes(const string& InQuery) const;

    int GetIntAttribute(const string& InQuery) const;

private:
    void ParseXMLInDirectory(const char* InRootDir);

    string GetAttrNameInQuery(const string& InQuery) const;
};