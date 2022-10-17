using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Weapon Type Set")]
public class WeaponTypeSet : ScriptableObject
{
    [Expandable] public List<WeaponType> types;
}
