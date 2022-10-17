using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefreshScrewable : MonoBehaviour
{
    [SerializeField] private string depletedIdentifier = "Depleted";

    public void Refresh()
    {
        gameObject.name.Replace(" (" + depletedIdentifier + ")", "");
    }
}
