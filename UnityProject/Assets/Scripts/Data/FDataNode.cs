using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class FDataNode
{
    struct FDataAttribute
    {
        public string StrValue;
        public int IntValue;
        public double DoubleValue;
        public bool BoolValue;
        public Color ColorValue;
    }

    Dictionary<string, List<FDataNode>> m_ChildNodes = new Dictionary<string, List<FDataNode>>();
    Dictionary<string, FDataAttribute> m_DataAttributes = new Dictionary<string, FDataAttribute>();

    string m_Name;

    public string Name { get { return m_Name; } } 

    public string GetStringAttr(in string InName)
    {
        if(m_DataAttributes.ContainsKey(InName))
            return m_DataAttributes[InName].StrValue;

        return "";
    }

    public int GetIntAttr(in string InName)
    {
        if (m_DataAttributes.ContainsKey(InName))
            return m_DataAttributes[InName].IntValue;

        return 0;
    }

    public double GetDoubleAttr(in string InName)
    {
        if (m_DataAttributes.ContainsKey(InName))
            return m_DataAttributes[InName].DoubleValue;

        return 0f;
    }

    public bool GetBoolAttr(in string InName)
    {
        if (m_DataAttributes.ContainsKey(InName))
            return m_DataAttributes[InName].BoolValue;

        return false;
    }

    public Color GetColorAttr(in string InName)
    {
        Color color = new Color();
        if (m_DataAttributes.ContainsKey(InName))
            color = m_DataAttributes[InName].ColorValue;

        return color;
    }

    public FDataNode GetDataNodeWithQuery(in string InQuery)
    {
        int nextQueryIndex = InQuery.IndexOf('.');
        string query = nextQueryIndex == -1 ? InQuery : InQuery.Substring(0, nextQueryIndex);

        char[] delimeters = {'[', '@', '=', ']'};
        string[] nodeQuerys = query.Split(delimeters, StringSplitOptions.RemoveEmptyEntries);
        string nodeName = nodeQuerys[0];
        string attrName = 1 < nodeQuerys.Length ? nodeQuerys[1] : null;
        string attrValue = 2 < nodeQuerys.Length ? nodeQuerys[2] : null;

        FDataNode retNode = null;

        if (m_ChildNodes.ContainsKey(nodeName))
        {
            foreach (FDataNode node in m_ChildNodes[nodeName])
            {
                if (attrName != null && attrValue != null)
                {
                    if (node.GetStringAttr(attrName) != attrValue)
                        continue;
                }

                if (nextQueryIndex == -1)
                {
                    retNode = node;
                    break;
                }

                retNode = node.GetDataNodeWithQuery(InQuery.Substring(nextQueryIndex + 1, InQuery.Length - nextQueryIndex - 1));
                if (retNode != null)
                    break;
            }
        }

        return retNode;
    }

    public List<FDataNode> GetDataNodesWithQuery(in string InQuery)
    {
        int nextQueryIndex = InQuery.IndexOf('.');
        string query = nextQueryIndex == -1 ? InQuery : InQuery.Substring(0, nextQueryIndex);

        char[] delimeters = { '@', '=' };
        string[] nodeQuerys = query.Split(delimeters);
        string nodeName = nodeQuerys[0];
        string attrName = 1 < nodeQuerys.Length ? nodeQuerys[1] : null;
        string attrValue = 2 < nodeQuerys.Length ? nodeQuerys[2] : null;

        List<FDataNode> retNodeList = new List<FDataNode>();

        if (m_ChildNodes.ContainsKey(nodeName))
        {
            foreach (FDataNode node in m_ChildNodes[nodeName])
            {
                if (attrName != null)
                {
                    if (node.GetStringAttr(attrName) != attrValue)
                        continue;
                }

                if (nextQueryIndex == -1)
                    retNodeList.Add(node);

                retNodeList.AddRange(node.GetDataNodesWithQuery(InQuery.Substring(nextQueryIndex + 1, InQuery.Length - nextQueryIndex - 1)));
            }
        }

        return retNodeList;
    }

    public delegate void ForeachChildNodesFunc(in FDataNode InNode);
    public void ForeachChildNodes(in string InName, in ForeachChildNodesFunc InFunc)
    {
        if (m_ChildNodes.ContainsKey(InName) == false)
            return;

        foreach (FDataNode node in m_ChildNodes[InName])
        {
            if(node.Name == InName)
            {
                InFunc(node);
            }
        }
    }

    public void AddChild(in FDataNode InChild)
    {
        if (m_ChildNodes.ContainsKey(InChild.Name))
            m_ChildNodes[InChild.Name].Add(InChild);
        else
        {
            List<FDataNode> newDataNodes = new List<FDataNode>();
            newDataNodes.Add(InChild);
            m_ChildNodes.Add(InChild.Name, newDataNodes);
        }
    }

    public void ParseXmlData(in XmlNode InNode)
    {
        m_Name = InNode.Name;

        ParseChild(InNode);
        ParseAttribute(InNode);
    }

    void ParseChild(in XmlNode InNode)
    {
        foreach (XmlNode childNode in InNode.ChildNodes)
        {
            FDataNode newChild = new FDataNode();
            newChild.ParseXmlData(childNode);

            AddChild(newChild);
        }
    }

    void ParseAttribute(in XmlNode InNode)
    {
        foreach (XmlAttribute attr in InNode.Attributes)
        {
            FDataAttribute newAttr = new FDataAttribute();

            if (bool.TryParse(attr.Value, out newAttr.BoolValue)) { }
            else if (int.TryParse(attr.Value, out newAttr.IntValue)) { }
            else if (double.TryParse(attr.Value, out newAttr.DoubleValue)) { }
            else if (ColorUtility.TryParseHtmlString(attr.Value, out newAttr.ColorValue)) { }
            
            newAttr.StrValue = attr.Value;

            m_DataAttributes.Add(attr.Name, newAttr);
        }
    }
}
