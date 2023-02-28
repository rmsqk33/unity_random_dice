using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FLocalPlayer : FSingleton<FLocalPlayer>
{
    private Dictionary<Type, FControllerBase> Controllers = new Dictionary<Type, FControllerBase>();

    protected override void Awake() 
    {
        base.Awake();

        AddController<FInventoryController>();
        AddController<FDiceController>();
        AddController<FBattlefieldController>();
        AddController<FPresetController>();
        AddController<FStatController>();
        AddController<FStoreController>();
    }

    private void AddController<T>()
    {
        Type type = typeof(T);
        Controllers.Add(type, (FControllerBase)Activator.CreateInstance(type, args:Instance));
    }

    public T FindController<T>()
    {
        Type type = typeof(T);
        if (Controllers.ContainsKey(type))
            return (T)Convert.ChangeType(Controllers[type], type);

        return default(T);
    }
}
