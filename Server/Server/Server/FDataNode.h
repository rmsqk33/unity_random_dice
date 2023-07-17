#pragma once

class XmlReader;

class FDataNode
{
private:
    enum class AttributeType
    {
        String,
        Int,
        Double,
        Bool,
    };

    struct FNodeAttribute
    {
        string StrValue = "";
        int IntValue = 0;
        double DoubleValue = 0.0;
        bool BoolValue = false;
    };

    map<string, vector<FDataNode*>> m_ChildNodes;
    map<string, FNodeAttribute*> m_Attributes;

    string m_NodeName;

public:
    ~FDataNode();

    void ParseXML(const char* InFilePath);
    void AddChildNode(FDataNode* InNode);

    const char* GetNodeName() const;

    const char* GetStringAttr(const string& InName) const;
    int GetIntAttr(const string& InName) const;
    double GetDoubleAttr(const string& InName) const;
    bool GetBoolAttr(const string& InName) const;

    const FDataNode* FindNode(const string& InQuery) const;
    vector<const FDataNode*> FindNodes(const string& InQuery) const;
    void ForeachChildNodes(const char* InName, const function<void(const FDataNode*)>& InFunc) const;

private:
    void ParseXmlReader(XmlReader* InReader);
    AttributeType GetAttributeType(const string& InAttrValue) const;

    const FNodeAttribute* FindAttribute(const string& InName) const;
};
