using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FLobbyUserInfoUI : FUIBase
{
    [SerializeField]
    TextMeshProUGUI nameText = null;
    [SerializeField]
    TextMeshProUGUI gold = null;
    [SerializeField]
    TextMeshProUGUI dia = null;
    [SerializeField]
    TextMeshProUGUI exp = null;
    [SerializeField]
    TextMeshProUGUI level = null;
    [SerializeField]
    Image classIcon = null;
    [SerializeField]
    Transform expGauge = null;

    int currentExp = 0;
    int maxExp = 0;

    public string Name { set { nameText.text = value; } }
    public int Gold { set { gold.text = value.ToString(); } }
    public int Dia { set { dia.text = value.ToString(); } }
    public Sprite ClassIcon { set{ classIcon.sprite = value; }}
    
    public int Level
    {
        set
        {
            level.text = value.ToString();
            Sprite classIcon = Resources.Load<Sprite>(FDataCenter.Instance.GetStringAttribute("UserClass.Class[@class=" + value + "]@icon"));
            if (classIcon != null)
                ClassIcon = classIcon;
        }
    }

    public int Exp
    {
        set
        {
            currentExp = value;
            UpdateExp();
        }
    }

    public int MaxExp
    {
        set
        {
            maxExp = value;
            UpdateExp();
        }
    }

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        FLocalPlayerStatController statControler = FLocalPlayer.Instance.FindController<FLocalPlayerStatController>();
        if (statControler != null)
        {
            Name = statControler.Name;
            Level = statControler.Level;
            SetExp(statControler.Exp, statControler.MaxExp);
        }

        FInventoryController inventoryController = FLocalPlayer.Instance.FindController<FInventoryController>();
        if (inventoryController != null)
        {
            Gold = inventoryController.Gold;
            Dia = inventoryController.Dia;
        }
    }

    public void SetExp(int InExp, int InMaxExp)
    {
        currentExp = Mathf.Min(InExp, InMaxExp);
        maxExp = InMaxExp;
        UpdateExp();
    }

    private void UpdateExp()
    {
        exp.text = currentExp.ToString() + "/" + maxExp.ToString();
        expGauge.localScale = currentExp == 0 || maxExp == 0 ? new Vector3(0, 1, 1) : new Vector3(currentExp / maxExp, 1, 1);
    }
}
