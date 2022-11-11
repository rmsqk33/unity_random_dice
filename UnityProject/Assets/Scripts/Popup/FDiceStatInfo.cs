using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FDiceStatInfo : MonoBehaviour
{
    [SerializeField]
    Image m_Icon;
    [SerializeField]
    TextMeshProUGUI m_Title;
    [SerializeField]
    TextMeshProUGUI m_Value;
    [SerializeField]
    TextMeshProUGUI m_UpgradeValue;

    public Sprite Icon { set { m_Icon.sprite = value; } }
    public string Title { set { m_Title.text = value; } }
    public string Value { set { m_Value.text = value; } }
    public string UpgradeValue { set { m_UpgradeValue.text = value; } }
    public bool Upgradable { set { m_UpgradeValue.gameObject.SetActive(value); } }
}
