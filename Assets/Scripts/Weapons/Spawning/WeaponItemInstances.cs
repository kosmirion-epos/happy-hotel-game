using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Weapon Item Instances")]
public class WeaponItemInstances : ScriptableObject
{
    private Dictionary<WeaponItem, List<GameObject>> itemInstances = new();

    public IReadOnlyList<GameObject> this[WeaponItem item] => itemInstances[item];

    public void RegisterInstance(WeaponItem item, GameObject gameObject)
    {
        if (!itemInstances.ContainsKey(item))
            itemInstances[item] = new();

        itemInstances[item].Add(gameObject);
    }

    public void DeregisterInstance(WeaponItem item, GameObject gameObject)
    {
        itemInstances[item].Remove(gameObject);

        if (!itemInstances[item].Any())
            itemInstances.Remove(item);
    }

    public bool ContainsItem(WeaponItem item) => itemInstances.ContainsKey(item);

    public void DestroyAll() => DestroyAllExcept(null);

    public void DestroyAllExcept(IEnumerable<GameObject> retain)
    {
        var l = itemInstances.Values.SelectMany(o => o).ToList();

        foreach (var o in l)
            if (retain?.Contains(o) != true)
                Destroy(o);
    }

    public void DestroyAllExceptGlobal(GlobalGameObjectList retain)
        => DestroyAllExcept(retain?.Value);

    public void DestroyAllExceptOfType(WeaponType type)
    {
        if (type == null)
            DestroyAll();
        else
            DestroyAllExcept(
                itemInstances.Where(p => type.items.Contains(p.Key))
                .Select(p => p.Value)
                .SelectMany(i => i)
            );
    }

    public bool Contains(GameObject gameObject)
        => itemInstances.Values.Any(l => l.Contains(gameObject));
}
