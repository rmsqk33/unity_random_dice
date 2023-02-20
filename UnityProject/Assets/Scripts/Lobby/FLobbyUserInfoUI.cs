using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class FLobbyUserInfoUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI m_NameText = null;
    [SerializeField]
    TextMeshProUGUI m_Gold = null;
    [SerializeField]
    TextMeshProUGUI m_Dia = null;
    [SerializeField]
    TextMeshProUGUI m_Exp = null;
    [SerializeField]
    TextMeshProUGUI m_Level = null;
    [SerializeField]
    Image m_ClassIcon = null;
    [SerializeField]
    Transform m_ExpGauge = null;

    int m_CurrentExp = 0;
    int m_MaxExp = 0;

    void Start()
    {
        InitUserInfo();
    }

    public void InitUserInfo()
    {
        Name = FUserDataController.Instance.Name;
        Gold = FUserDataController.Instance.Gold;
        Dia = FUserDataController.Instance.Dia;
        Level = FUserDataController.Instance.Level;
        SetExp(FUserDataController.Instance.Exp, FUserDataController.Instance.MaxExp);
    }

    public string Name { set { m_NameText.text = value; } }
    public int Gold { set { m_Gold.text = value.ToString(); } }
    public int Dia { set { m_Dia.text = value.ToString(); } }
    public int Level
    {
        set
        {
            m_Level.text = value.ToString();
            Sprite classIcon = Resources.Load<Sprite>(FDataCenter.Instance.GetStringAttribute("UserClass.Class[@class=" + m_Level + "]@icon"));
            if (classIcon != null)
                ClassIcon = classIcon;
        }
    }

    public Sprite ClassIcon
    {
        set
        {
            m_ClassIcon.sprite = value;
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

    public void SetExp(int InExp, int InMaxExp)
    {
        m_CurrentExp = Mathf.Min(InExp, InMaxExp);
        m_MaxExp = InMaxExp;
        UpdateExp();
    }

    void UpdateExp()
    {
        m_Exp.text = m_CurrentExp.ToString() + "/" + m_MaxExp.ToString();
        m_ExpGauge.localScale = m_CurrentExp == 0 || m_MaxExp == 0 ? new Vector3(0, 1, 1) : new Vector3(m_CurrentExp / m_MaxExp, 1, 1);
    }
}
