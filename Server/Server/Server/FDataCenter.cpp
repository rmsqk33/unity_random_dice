#include "stdafx.h"
#include "FDataCenter.h"

#include <filesystem>

#define DATA_PATH "../../../UnityProject/Assets/StreamingAssets/Data/"
#define SERVER_DATA_PATH "../../Data/"

using namespace std::filesystem;

FDataCenter::FDataCenter()
{
    m_RootNode = new FDataNode();
}

FDataCenter::~FDataCenter()
{
    if(m_RootNode != nullptr)
        delete m_RootNode;
}

void FDataCenter::LoadData()
{
    ParseXMLInDirectory(DATA_PATH);
    ParseXMLInDirectory(SERVER_DATA_PATH);
}

void FDataCenter::ParseXMLInDirectory(const char* InRootDir)
{
    for (recursive_directory_iterator iter(InRootDir); iter != recursive_directory_iterator(); ++iter)
    {
        auto& path = iter->path();
        if (path.has_extension() == false)
            continue;

        const string& extention = path.extension().string();
        if (extention != ".xml")
            continue;

        FDataNode* newNode = new FDataNode();
        newNode->ParseXML(path.string().c_str());

        m_RootNode->AddChildNode(newNode);
    }
}

string FDataCenter::GetAttrNameInQuery(const string& InQuery) const
{
    size_t index = InQuery.find_last_of('@');
    return InQuery.substr(index + 1, InQuery.length() - index - 1);
}

const FDataNode* FDataCenter::FindNode(const string& InQuery) const
{
    if (m_RootNode != nullptr)
        return m_RootNode->FindNode(InQuery);

    return nullptr;
}

vector<const FDataNode*> FDataCenter::FindNodes(const string& InQuery) const
{
    vector<const FDataNode*> retList;
    if (m_RootNode != nullptr)
        retList = m_RootNode->FindNodes(InQuery);

    return retList;
}

int FDataCenter::GetIntAttribute(const string& InQuery) const
{
    if (const FDataNode* node = FindNode(InQuery))
    {
        const string attrName = GetAttrNameInQuery(InQuery);
        return node->GetIntAttr(attrName);
    }
    return 0;
}
