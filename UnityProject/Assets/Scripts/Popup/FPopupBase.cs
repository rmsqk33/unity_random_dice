using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPopupBase : MonoBehaviour
{
    public void Close()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
            animator.SetTrigger("Close");
    }

    public void OnCloseAnimComplete()
    {
        Destroy(this.gameObject);
    }
}
