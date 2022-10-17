using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Weapon Type")]
public class WeaponType : ScriptableObject
{
    [Expandable] public List<WeaponItem> items;
}