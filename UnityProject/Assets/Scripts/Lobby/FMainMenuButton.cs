using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FMainMenuButton : MonoBehaviour
{
    [SerializeField]
    Sprite NormalSprite;
    [SerializeField]
    Sprite SelectedSprite;

    Animator m_Animator = null;
    Button m_Button = null;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        m_Button = GetComponent<Button>();
    }

    public bool Enable
    {
        get { return m_Button.enabled; }
        set { m_Button.enabled = value; } 
    }

    public Vector2 Position { get { return this.transform.position; } }

    public void SelectTriggerOn()
    {
        m_Animator.SetTrigger("Selected");
        m_Button.image.sprite = SelectedSprite;
    }

    public void NormalTriggerOn()
    {
        m_Animator.SetTrigger("Normal");
        m_Button.image.sprite = NormalSprite;
    }
}
