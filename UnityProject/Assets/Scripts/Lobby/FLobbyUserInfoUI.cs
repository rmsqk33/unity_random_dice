using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class FLobbyUserInfoUI : MonoBehaviour
{
    TextMeshProUGUI m_Name = null;
    TextMeshProUGUI m_Gold = null;
    TextMeshProUGUI m_Dia = null;
    TextMeshProUGUI m_Exp = null;
    TextMeshProUGUI m_Level = null;
    Image m_ClassIcon = null;
    Transform m_ExpGauge = null;

    int m_CurrentExp = 0;
    int m_MaxExp = 0;

    private void Awake()
    {
        Transform Top = transform.Find("Top");
        m_Name = Top.Find("Name").GetComponent<TextMeshProUGUI>();
        m_Gold = Top.Find("Gold").GetComponentInChildren<TextMeshProUGUI>();
        m_Dia = Top.Find("Dia").GetComponentInChildren<TextMeshProUGUI>();

        Transform Exp = transform.Find("Exp");
        m_Exp = Exp.GetComponentInChildren<TextMeshProUGUI>();
        m_ExpGauge = Exp.Find("Gauge");

        Transform Class = transform.Find("Class");
        m_Level = Class.Find("ClassNum").GetComponentInChildren<TextMeshProUGUI>();
        m_ClassIcon = Class.Find("Icon").GetComponentInChildren<Image>();
    }

    public string Name { set { m_Name.text = value; }}
    public int Gold { set { m_Gold.text = value.ToString(); }}
    public int Dia { set { m_Dia.text = value.ToString(); }}
    public int Level 
    {
        set 
        { 
            m_Level.text = value.ToString();
            FDataNode node = FDataCenter.Instance.GetDataNodeWithQuery("UserClass.Class@class=" + value.ToString());
            if (node != null)
                m_ClassIcon.sprite = Resources.Load<Sprite>(node.GetStringAttr("icon"));
        }
    }

    public int CurrentExp
    {
        set
        {
            m_CurrentExp = value;
            UpdateExp();
        }
    }

    public int MaxExp
    {
        set
        {
            m_MaxExp = value;
            UpdateExp();
        }
    }

    void UpdateExp()
    {
        m_Exp.text = m_CurrentExp.ToString() + "/" + m_MaxExp.ToString();
        m_ExpGauge.localScale = new Vector3(m_CurrentExp / m_MaxExp, 1, 1);
    }
}
