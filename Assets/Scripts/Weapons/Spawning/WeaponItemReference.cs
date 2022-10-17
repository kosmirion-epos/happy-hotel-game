using Autohand;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponItemReference : MonoBehaviour
{
    public WeaponItemInstances ItemInstances;
    public GlobalValue<WeaponType> selectedType;
    public GlobalValue<List<GameObject>> selectedItems;
    public WeaponType type;
    public WeaponItem Item;

    public bool Initialized => ItemInstances != null && Item != null;

    [NonSerialized] public bool registered;

    private void Awake()
    {
        Grabbable g = GetComponent<Grabbable>();
        g.OnGrabEvent += (_, __) =>
        {
            if (selectedType.Value != type)
            {
                selectedType.Value = type;
                selectedItems.Value.Clear();
            }
            else if (ItemInstances.ContainsItem(Item))
            {
                selectedItems.Value = selectedItems.Value.Except(ItemInstances[Item]).ToList();
            }

            selectedItems.Value.Add(gameObject);
        };

        g.transform.parent = null;
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
    }

    private void OnEnable() => Register();
    private void OnDisable() => Deregister();

    public void Register()
    {
        if (!Initialized || registered)
            return;

        ItemInstances.RegisterInstance(Item, gameObject);
        registered = true;
    }

    public void Deregister()
    {
        if (!Initialized || !registered)
            return;

        ItemInstances.DeregisterInstance(Item, gameObject);
        registered = false;
    }
}
