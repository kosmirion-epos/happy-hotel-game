using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop Item")]
public class ShopItem : ScriptableObject {

    public string itemName;
    public int price;
    public GameObject prefab;

}