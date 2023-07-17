
public class FControllerBase
{
    protected FObjectBase Owner { get; set; }

    public FControllerBase(FObjectBase InOwner)
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

    public T FindChildComponent<T>(string InName)
    {
        return Owner.FindChildComponent<T>(InName);
    }
}
