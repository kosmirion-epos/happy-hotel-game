using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderDispalyRow : MonoBehaviour
{
    List<CylinderDispalySingle> singles = new List<CylinderDispalySingle>();
    public int value;


    private void Start()
    {
        GetComponentsInChildren(singles);
    }


    public bool test;

    private void Update()
    {
        if (test)
        {
            Set(value);
            test = false;
        }
    }

    public void Set(int value)
    {
        int i = 0;
        foreach(CylinderDispalySingle c in singles) { c.Set(0); }

        foreach(int v in GetIntList(value))
        {
            if (singles.Count <= i) return;
            singles[i].Set(v);
            i++;
        }
    }


    List<int> GetIntList(int num)
    {
        List<int> listOfInts = new List<int>();
        while (num > 0)
        {
            listOfInts.Add(num % 10);
            num = num / 10;
        }
        //listOfInts.Reverse();
        return listOfInts;
    }

}
