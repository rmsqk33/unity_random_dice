using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class FDataCenter : FNonObjectSingleton<FDataCenter>
{
    FDataNode rootNode = new FDataNode();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize() 
    {
        string dataPath = Application.streamingAssetsPath + "/Data/";
        Instance.ParseXML(dataPath);

        FDiceDataManager.Instance.Initialize();
        FBattleFieldDataManager.Instance.Initialize();
        FStoreDataManager.Instance.Initialize();
        FBattleDataManager.Instance.Initialize();
        FEffectDataManager.Instance.Initialize();
        FSkillDataManager.Instance.Initialize();
        FAbnormalityDataManager.Instance.Initialize();
        FAbilityDataManager.Instance.Initialize();
        FObjectDataManager.Instance.Initialize();
    }

    void ParseXML(in string InPath)
    {
        if (!Directory.Exists(InPath))
            return;

        string[] allFiles = Directory.GetFiles(InPath, "*.xml", SearchOption.AllDirectories);
        foreach(string file in allFiles)
        {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(file);

                FDataNode newNode = new FDataNode();
                newNode.ParseXmlData(xmlDocument.FirstChild);
                rootNode.AddChild(newNode);
        }
    }

    public FDataNode GetDataNodeWithQuery(in string InQuery)
    {
        return rootNode.GetDataNodeWithQuery(InQuery);
    }

    public List<FDataNode> GetDataNodesWithQuery(in string InQuery)
    {
        return rootNode.GetDataNodesWithQuery(InQuery);
    }

    public int GetIntAttribute(in string InQuery)
    {
        FDataNode node = GetDataNodeWithQuery(InQuery);
        if (node == null)
            return 0;

        string attrName = GetAttrNameInQuery(InQuery);
        return node.GetIntAttr(attrName);
    }

    public bool GetBoolAttribute(in string InQuery)
    {
        FDataNode node = GetDataNodeWithQuery(InQuery);
        if (node == null)
            return false;

        string attrName = GetAttrNameInQuery(InQuery);
        return node.GetBoolAttr(attrName);
    }

    public float GetFloatAttribute(in string InQuery)
    {
        FDataNode node = GetDataNodeWithQuery(InQuery);
        if (node == null)
            return 0f;

        string attrName = GetAttrNameInQuery(InQuery);
        return node.GetFloatAttr(attrName);
    }

    public string GetStringAttribute(in string InQuery)
    {
        FDataNode node = GetDataNodeWithQuery(InQuery);
        if (node == null)
            return "";

        string attrName = GetAttrNameInQuery(InQuery);
        return node.GetStringAttr(attrName);
    }

    string GetAttrNameInQuery(in string InQuery)
    {
        int attrIndex = InQuery.LastIndexOf('@');
        return InQuery.Substring(attrIndex + 1, InQuery.Length - attrIndex - 1);
    }
}
