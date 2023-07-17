#include "stdafx.h"
#include "FDataNode.h"
#include "XmlReader.h"

FDataNode::~FDataNode()
{
    for(auto& i : m_ChildNodes)
    {
        for(auto& j : i.second)
        {
            delete j;
        }
    }
    m_ChildNodes.clear();

    for(auto& i : m_Attributes)
    {
        delete i.second;
    }
    m_Attributes.clear();
}

void FDataNode::ParseXML(const char* InFilePath)
{
    XmlReader reader;
    if (reader.Open(InFilePath) == false)
        return;

	while (reader.Read())
	{
		if (reader.GetType() == XmlReaderType::Element)
		{
            ParseXmlReader(&reader);
		}
	}

    reader.Close();
}

void FDataNode::ParseXmlReader(XmlReader* InReader)
{
    m_NodeName = InReader->GetName();
    while(InReader->MoveToNextAttribute())
    {
        FNodeAttribute* newAttr = new FNodeAttribute();
        newAttr->StrValue = InReader->GetValue();
        switch (GetAttributeType(newAttr->StrValue))
        {
        case AttributeType::Int: newAttr->IntValue = FUtil::AtoI(newAttr->StrValue.c_str()); break;
        case AttributeType::Double: newAttr->DoubleValue = FUtil::AtoF(newAttr->StrValue.c_str()); break;
        case AttributeType::Bool: newAttr->BoolValue = newAttr->StrValue == "true" ? true : false; break;
        }
        m_Attributes.insert(pair<string, FNodeAttribute*>(InReader->GetName(), newAttr));
    }

    if (InReader->GetType() == XmlReaderType::EndElement)
        return;

    while(InReader->Read())
    {
        switch(InReader->GetType())
        {
            case XmlReaderType::Element:
            {
                FDataNode* childNode = new FDataNode();
                childNode->ParseXmlReader(InReader);
                AddChildNode(childNode);          
            }
                break;

            case XmlReaderType::EndElement:
                return;
        }
    }
}

FDataNode::AttributeType FDataNode::GetAttributeType(const string& InAttrValue) const
{
    AttributeType attrType = AttributeType::Int;
    for (char ch : InAttrValue)
    {
        if (isdigit(ch))
            continue;

        if (ch == '.' && attrType == AttributeType::Int)
        {
            attrType = AttributeType::Double;
            continue;
        }

        attrType = AttributeType::String;
        break;
    }
    return attrType;
}

void FDataNode::AddChildNode(FDataNode* InNode)
{
    auto iter = m_ChildNodes.find(InNode->GetNodeName());
    if(iter != m_ChildNodes.end())
        iter->second.push_back(InNode);
    else
    {
        vector<FDataNode*> newList;
        newList.push_back(InNode);
        m_ChildNodes.insert(pair<string, vector<FDataNode*>>(InNode->GetNodeName(), newList));
    }
}

const char* FDataNode::GetNodeName() const
{
    return m_NodeName.c_str();
}

const char* FDataNode::GetStringAttr(const string& InName) const
{
    if(const FNodeAttribute* attr = FindAttribute(InName))
        return attr->StrValue.c_str();

    return "";
}

int FDataNode::GetIntAttr(const string& InName) const
{
    if(const FNodeAttribute* attr = FindAttribute(InName))
        return attr->IntValue;
    
    return 0;
}

double FDataNode::GetDoubleAttr(const string& InName) const
{
    if(const FNodeAttribute* attr = FindAttribute(InName))
        return attr->DoubleValue;
    
    return 0.0;
}

bool FDataNode::GetBoolAttr(const string& InName) const
{
    if(const FNodeAttribute* attr = FindAttribute(InName))
        return attr->BoolValue;
    
    return false;
}

const FDataNode::FNodeAttribute* FDataNode::FindAttribute(const string& InName) const
{
    const auto& iter = m_Attributes.find(InName);
    if(iter != m_Attributes.end())
        return iter->second;

    return nullptr;
}

const FDataNode* FDataNode::FindNode(const string& InQuery) const
{
    string query(InQuery);
    int nextQueryIndex = query.find(".");
    query = nextQueryIndex == string::npos ? query : query.substr(0, nextQueryIndex);

    const char* delimeters = "[@=]";
    vector<string> nodeInfo = FUtil::SplitString(query, delimeters);
    const string& nodeName = nodeInfo[0];
    const string& attrName = 1 < nodeInfo.size() ? nodeInfo[1] : "";
    const string& attrValue = 2 < nodeInfo.size() ? nodeInfo[2] : "";

    const auto& iter = m_ChildNodes.find(nodeName);
    if(iter != m_ChildNodes.end())
    {
        for(const FDataNode* node : iter->second)
        {
            if(attrName.size() != 0 
            && attrValue.size() != 0 
            && node->GetStringAttr(attrName.c_str()) != attrValue)
                continue;

            if(nextQueryIndex == string::npos)
                return node;

            return node->FindNode(InQuery.c_str() + nextQueryIndex + 1);
        }
    }

    return nullptr;
}

vector<const FDataNode*> FDataNode::FindNodes(const string& InQuery) const
{
    vector<const FDataNode*> retList;

    string query(InQuery);
    int nextQueryIndex = query.find(".");
    query = nextQueryIndex == string::npos ? query : query.substr(0, nextQueryIndex);

    const char* delimeters = "@=";
    vector<string> nodeInfo = FUtil::SplitString(query, delimeters);
    const string& nodeName = nodeInfo[0];
    const string& attrName = 1 < nodeInfo.size() ? nodeInfo[1] : "";
    const string& attrValue = 2 < nodeInfo.size() ? nodeInfo[2] : "";

    const auto& iter = m_ChildNodes.find(nodeName);
    if(iter != m_ChildNodes.end())
    {
        for(const FDataNode* node : iter->second)
        {
            if(attrName.size() != 0 
            && attrValue.size() != 0 
            && node->GetStringAttr(attrName.c_str()) != attrValue)
                continue;

            if(nextQueryIndex == string::npos)
                retList.push_back(node);
            else
            {
                vector<const FDataNode*> list = node->FindNodes(InQuery.c_str() + nextQueryIndex + 1);
                retList.insert(retList.end(), list.begin(), list.end());
            }
        }
    }

    return retList;
}

void FDataNode::ForeachChildNodes(const char* InName, const function<void(const FDataNode*)>& InFunc) const
{
    const auto& iter = m_ChildNodes.find(InName);
    if(iter != m_ChildNodes.end())
    {
        const vector<FDataNode*>& nodes = iter->second;
        for(const FDataNode* node : nodes)
        {
            InFunc(node);
        }
    }
}
