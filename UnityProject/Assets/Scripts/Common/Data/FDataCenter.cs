using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using UnityEngine;

public class FDataCenter : FNonObjectSingleton<FDataCenter>
{
    FDataNode m_RootNode = new FDataNode();

    [RuntimeInitializeOnLoadMethod]
    static void Initialize() 
    {
        Instance.ParseXML("Data/PreLoadData");
    }

    public bool LoadData()
    {
        ParseXML("Data/PostLoadData");
        return true;
    }

    void ParseXML(in string InPath)
    {
        TextAsset[] textAssetList = Resources.LoadAll<TextAsset>(InPath);
        foreach(TextAsset textAsset in textAssetList)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(textAsset.text);

            FDataNode newNode = new FDataNode();
            newNode.ParseXmlData(xmlDocument.FirstChild);
            m_RootNode.AddChild(newNode);
        }
    }

    public FDataNode GetDataNodeWithQuery(in string InQuery)
    {
        return m_RootNode.GetDataNodeWithQuery(InQuery);
    }

    public List<FDataNode> GetDataNodesWithQuery(in string InQuery)
    {
        return m_RootNode.GetDataNodesWithQuery(InQuery);
    }
}
