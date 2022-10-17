using Autohand;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public partial class WeaponSpawner : ExtendedBehaviour
{
    [Expandable][SerializeField] private WeaponTypeSet weaponTypes;
    [Tooltip(
        "The number of items as well as each element's item field cannot be edited " +
        "here and are automatically generated from the items present in the above Weapon " +
        "Types. To update manually, press the button at the bottom of the Component."
    )]
    [SerializeField]
    private List<WeaponItemSpawnInfo> spawnInfo;

    [SerializeField] private EjectSpace ejectSpace;

    [Tooltip("Do not modify this field's local value!")]
    [SerializeField]
    private ScopedValue<List<GameObject>> currentWeaponItems;

    [SerializeField] private bool respawnAutomatically;
    [ShowIf(nameof(respawnAutomatically))][SerializeField] private ScopedValue<float> weaponRespawnDelay;

    [SerializeField] private ScopedValue<float> activationDelay;
    [SerializeField] private ScopedValue<float> deactivationDelay;
    [SerializeField] private bool startActive = true;

    [SerializeField] private GlobalEvent destroyWeapons;

    public event UnityAction<Grabbable> OnSpawnWeaponEvent;

    private Dictionary<WeaponItem, WeaponItemSpawnInfo> weaponItemMap;
    private List<GameObject> displayedWeaponItems;
    private int displayedWeaponTypeIndex;
    private State activeState;

    public int DisplayedWeaponTypeIndex
    {
        get => displayedWeaponTypeIndex;
        set => displayedWeaponTypeIndex = (value % WeaponTypeCount + WeaponTypeCount) % WeaponTypeCount;
    }
    public WeaponType DisplayedWeaponType
    {
        get => weaponTypes.types[DisplayedWeaponTypeIndex];
        set => displayedWeaponTypeIndex = weaponTypes.types.IndexOf(value);
    }

    public void SetDisplayedWeaponTypeByGlobal(GlobalWeaponType type)
        => DisplayedWeaponType = type.Value;

    public int WeaponTypeCount => weaponTypes.types.Count;

    [Serializable]
    public struct WeaponItemSpawnInfo
    {
        [Expandable] public WeaponItem item;
        public Transform spawnTransform;
        public Vector3 ejectForce;
    }

    public enum State { Inactive, Activating, Active, Deactivating }

    private enum EjectSpace
    {
        SpawnerTransform, ItemSpawnTransform, World
    }

    private void Awake()
    {
        weaponItemMap = new();
        displayedWeaponItems = new();

        foreach (var info in spawnInfo)
            weaponItemMap[info.item] = info;

        spawnInfo = null;

        if (startActive)
        {
            SpawnWeapons();
            activeState = State.Active;
        }
    }

    private void OnValidate()
    {
        if (weaponTypes == null)
        {
            spawnInfo.Clear();
            return;
        }

        spawnInfo = spawnInfo.FindAll(info => weaponTypes.types.Any(t => t.items.Contains(info.item)));

        foreach (var t in weaponTypes.types)
            foreach (var i in t.items)
                if (!spawnInfo.Any(info => info.item == i))
                    spawnInfo.Add(new WeaponItemSpawnInfo { item = i });
    }

    [Button]
    private void _updateSpawnInfo() => OnValidate();

    private void _onTakeOutWeapon(Hand _, Grabbable takenOut) => EjectWeapons(takenOut);

    public void SpawnWeapons()
    {
        if (displayedWeaponItems.Any())
            return;

        foreach (var item in DisplayedWeaponType.items)
        {
            var info = weaponItemMap[item];
            var w = Instantiate(
                item.prefab,
                info.spawnTransform.position,
                info.spawnTransform.rotation
            );

            var g = w.GetComponent<Grabbable>();
            var r = w.GetComponent<WeaponItemReference>();

            g.OnGrabEvent += _onTakeOutWeapon;
            r.Item = item;
            r.Register();
            w.GetComponent<Rigidbody>().isKinematic = true;
            w.GetComponent<DistanceGrabbable>().OnPull.AddListener(_onTakeOutWeapon);

            //We have to do it this way, since Grabbable only sets originalParent once and it's in Awake ¯\_(?)_/¯
            w.transform.parent = info.spawnTransform;

            displayedWeaponItems.Add(w);

            OnSpawnWeaponEvent?.Invoke(g);
        }
    }

    public void DespawnWeapons()
    {
        foreach (var w in displayedWeaponItems)
            Destroy(w);

        displayedWeaponItems.Clear();
    }

    public void EjectWeapons() => EjectWeapons(null);

    public void EjectWeapons(Grabbable ignore)
    {
        foreach (var w in displayedWeaponItems)
        {
            var g = w.GetComponent<Grabbable>();

            g.OnGrabEvent -= _onTakeOutWeapon;
            w.GetComponent<DistanceGrabbable>().OnPull.RemoveListener(_onTakeOutWeapon);

            Rigidbody b = w.GetComponent<Rigidbody>();
            b.isKinematic = false;

            if (g != ignore)
            {
                w.transform.parent = null;

                var info = weaponItemMap[w.GetComponent<WeaponItemReference>().Item];
                var force = info.ejectForce;
                b.AddForce(
                    ejectSpace switch
                    {
                        EjectSpace.SpawnerTransform => transform.TransformDirection(force),
                        EjectSpace.ItemSpawnTransform => info.spawnTransform.TransformDirection(force),
                        _ => force,
                    },
                    ForceMode.Impulse
                );
            }
        }

        currentWeaponItems.Value = new(displayedWeaponItems);
        displayedWeaponItems.Clear();

        destroyWeapons.Invoke();

        if (respawnAutomatically)
            WithDelay(weaponRespawnDelay.Value, SpawnWeapons);
    }

    public void Activate()
    {
        if (activeState != State.Inactive)
            return;

        SpawnWeapons();

        foreach (var w in displayedWeaponItems)
            w.GetComponent<Grabbable>().enabled = false;

        WithDelay(
            activationDelay.Value,
            () =>
            {
                foreach (var w in displayedWeaponItems)
                    w.GetComponent<Grabbable>().enabled = true;

                activeState = State.Active;
            }
        );

        activeState = State.Activating;
    }

    public void Deactivate()
    {
        if (activeState != State.Active)
            return;

        foreach (var w in displayedWeaponItems)
            Destroy(w.GetComponent<Grabbable>());

        WithDelay(
            deactivationDelay.Value, 
            () =>
            {
                DespawnWeapons();
                activeState = State.Inactive;
            }
        );

        activeState = State.Deactivating;
    }

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    public void GoToPreviousWeapon() => CycleDisplayedWeapons(-1);

    [Button(enabledMode: EButtonEnableMode.Playmode)]
    public void GoToNextWeapon() => CycleDisplayedWeapons(1);

    public void JumpToWeapon(int newIndex)
    {
        DespawnWeapons();
        DisplayedWeaponTypeIndex = newIndex;
        SpawnWeapons();
    }

    public void CycleDisplayedWeapons(int add) => JumpToWeapon(DisplayedWeaponTypeIndex + add);
}
