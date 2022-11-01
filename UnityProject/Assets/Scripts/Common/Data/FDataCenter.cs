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
    string m_DataPath = Application.dataPath + "/Data";

    [RuntimeInitializeOnLoadMethod]
    static void Initialize() 
    {
        Instance.ParseDirectory(Instance.m_DataPath + "/PreLoadData");
    }

    public bool LoadData()
    {
        ParseDirectory(m_DataPath + "/PostLoadData");
        return true;
    }

    void ParseDirectory(in string InDirPath)
    {
        DirectoryInfo root = new DirectoryInfo(InDirPath);
        foreach(DirectoryInfo dir in root.GetDirectories())
        {
            ParseDirectory(dir.FullName);
        }

        foreach (FileInfo info in root.GetFiles())
        {
            if (info.Extension != ".xml")
                continue;
            
            ParseXML(info.FullName);
        }
    }

    void ParseXML(in string InPath)
    {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(InPath);

        FDataNode newNode = new FDataNode();
        newNode.ParseXmlData(xmlDocument.FirstChild);
        m_RootNode.AddChild(newNode);        
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
