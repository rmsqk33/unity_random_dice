using UnityEngine;

public class FControllerBase
{
    protected FLocalPlayer Owner { get; set; }

    public FControllerBase(FLocalPlayer InOwner)
    {
        Owner = InOwner;
    }

    public virtual void Initialize() { }
    public virtual void Release() { }
    public virtual void Tick(float InDeltaTime) { }

    protected T FindController<T>() where T : FControllerBase
    {
        return Owner.FindController<T>();
    }
}
