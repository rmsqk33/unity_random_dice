using UnityEngine;

public class FPopupBase : FUIBase
{
    public void Close()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
            animator.SetTrigger("Close");
        else
            Destroy(this.gameObject);
    }

    public void OnCloseAnimComplete()
    {
        Destroy(this.gameObject);
    }
}
