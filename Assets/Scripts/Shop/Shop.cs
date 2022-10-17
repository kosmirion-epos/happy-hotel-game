using Autohand;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Shop : MonoBehaviour
{
    [SerializeField] private GlobalInt score;
    [SerializeField] private List<ShopItem> items = new List<ShopItem>();
    [SerializeField] private Transform spawner;
    [SerializeField] private CylinderDispalyRow priceDisplay;
    [SerializeField] private Vector3 spawnForce;
    [SerializeField] private Collider itemBlocker;
    [SerializeField] private Vector3 spawnOffset;

    ShopItem currentItem;

    public UnityEvent BuySuccess;
    public UnityEvent BuyFail;

    void SpawnCurrentItem()
    {
        GameObject o = Instantiate(
            currentItem.prefab,
            spawner.position + spawner.TransformVector(spawnOffset) * items.IndexOf(currentItem) / (items.Count - 1),
            Random.rotation
        );
        o.GetComponentInChildren<Rigidbody>().AddForce(spawner.TransformDirection(spawnForce), ForceMode.Impulse);

        var gs = o.GetComponentsInChildren<Grabbable>();
        var cs = o.GetComponentsInChildren<Collider>();
        HandGrabEvent ignoreCollisions = null;
        ignoreCollisions =
            (_, __) =>
            {
                foreach (var c in cs)
                    Physics.IgnoreCollision(c, itemBlocker);

                foreach (var g in gs)
                    g.OnGrabEvent -= ignoreCollisions;
            };

        foreach (var g in gs)
            g.OnGrabEvent += ignoreCollisions;
    }

    public void SelectItem(int i)
    {
        if (i >= items.Count || i < 0)
        {
            return;
        }
        currentItem = items[i];

        if (priceDisplay)
            priceDisplay.Set(currentItem.price);

    }

    public void BuyItem()
    {
        //score.Value = 9999;
        //Debug.Log(currentItem.price);
        //Debug.Log(score.Value);
        if (!currentItem || score.Value < currentItem.price)
        {
            BuyFail.Invoke();
            return;
        }
        score.Value -= currentItem.price;
        SpawnCurrentItem();
        BuySuccess.Invoke();
    }
}
