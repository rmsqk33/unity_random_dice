using UnityEngine;
using UnityEngine.UI;

public class FLobbyMenuButton : MonoBehaviour
{
    [SerializeField]
    Sprite normalSprite;
    [SerializeField]
    Sprite selectedSprite;

    Animator animator = null;
    Button button = null;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        button = GetComponent<Button>();
    }

    public bool Enable
    {
        get { return button.enabled; }
        set { button.enabled = value; } 
    }

    public Vector2 Position { get { return this.transform.position; } }

    public void SelectTriggerOn()
    {
        animator.SetTrigger("Selected");
        button.image.sprite = selectedSprite;
    }

    public void NormalTriggerOn()
    {
        animator.SetTrigger("Normal");
        button.image.sprite = normalSprite;
    }
}
