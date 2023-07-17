
public class FLocalPlayer : FObjectBase
{
    protected override void Awake()
    {
        AddController<FInventoryController>();
        AddController<FDiceController>();
        AddController<FBattlefieldController>();
        AddController<FPresetController>();
        AddController<FLocalPlayerStatController>();
        AddController<FStoreController>();
    }
}
