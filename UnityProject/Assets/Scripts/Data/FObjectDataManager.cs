using FEnum;
using System.Collections.Generic;

public class FCollisionData
{
    public readonly int id;
    public readonly int abnormalityID;
    public readonly int size;
    public readonly float duration;
    public readonly string prefab;

    public FCollisionData(FDataNode InNode)
    {
        id = InNode.GetIntAttr("id");
        abnormalityID = InNode.GetIntAttr("abnormalityID");
        size = InNode.GetIntAttr("size");
        duration = InNode.GetFloatAttr("duration");
        prefab = InNode.GetStringAttr("prefab");
    }
}

public class FEnemyData
{
    public readonly int id;
    public readonly int hp;
    public readonly int sp;
    public readonly int hpIncreaseBySpawnCount;
    public readonly int moveSpeed;
    public readonly EnemyType enemyType;
    public readonly string prefabPath;
    public readonly List<int> skillIDList = new List<int>();

    public FEnemyData(FDataNode InNode)
    {
        id = InNode.GetIntAttr("id");
        hp = InNode.GetIntAttr("hp");
        sp = InNode.GetIntAttr("sp");
        hpIncreaseBySpawnCount = InNode.GetIntAttr("hpIncreaseBySpawnCount");
        moveSpeed = InNode.GetIntAttr("moveSpeed");
        enemyType = (EnemyType)InNode.GetIntAttr("type");
        prefabPath = InNode.GetStringAttr("prefab");

        FDataNode skillListNode = InNode.FindChildNode("SkillList");
        if (skillListNode != null)
        {
            skillListNode.ForeachChildNodes("Skill", (in FDataNode InNode) => {
                skillIDList.Add(InNode.GetIntAttr("id"));
            });
        }
    }
}

public class FObjectDataManager : FNonObjectSingleton<FObjectDataManager>
{
    Dictionary<int, FEnemyData> enemyDataMap = new Dictionary<int, FEnemyData>();
    Dictionary<int, FCollisionData> collisionDataMap = new Dictionary<int, FCollisionData>();

    public void Initialize()
    {
        List<FDataNode> enemyDataList = FDataCenter.Instance.GetDataNodesWithQuery("EnemyList.Enemy");
        foreach (FDataNode dataNode in enemyDataList)
        {
            FEnemyData enemyData = new FEnemyData(dataNode);
            enemyDataMap.Add(enemyData.id, enemyData);
        }

        List<FDataNode> collisionDataList = FDataCenter.Instance.GetDataNodesWithQuery("CollisionObjectList.CollisionObject");
        foreach (FDataNode dataNode in collisionDataList)
        {
            FCollisionData collisionData = new FCollisionData(dataNode);
            collisionDataMap.Add(collisionData.id, collisionData);
        }
    }

    public FEnemyData FindEnemyData(int InID)
    {
        if (enemyDataMap.ContainsKey(InID))
            return enemyDataMap[InID];

        return null;
    }

    public FCollisionData FindCollisionData(int InID)
    {
        if (collisionDataMap.ContainsKey(InID))
            return collisionDataMap[InID];

        return null;
    }
}
