using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Holds a list of different values that can be assigned weights in the inspector. Exposes a method to retrieve a random element affected by those weights.
/// 
/// Devised by u/spidergrisen on Reddit.
/// See: https://github.com/s164031/UnityPercentageObjectSpawner
///      https://www.reddit.com/r/Unity3D/comments/v124s7/very_satisfying_custom_editor_for_object_spawner/
/// </summary>
/// <typeparam name="TObj"></typeparam>
[Serializable]
[HelpURL("https://github.com/s164031/UnityPercentageObjectSpawner")]
public class WeightedRandomSelector<TObj> : ISerializationCallbackReceiver
{
    [Serializable]
    public class SpawnObject
    {
        public TObj spawnObject;
        [Range(0.00f, 1.00f)]
        public float chance;
        internal float prevChance;
        public bool locked;
        internal bool prevLocked;
    }

    [SerializeField]
    private List<SpawnObject> spawnObjects;
    private int lengthObjects;
    private List<int> lockedList = new List<int>();
    private bool initialized;

    public List<SpawnObject> SpawnObjects { get { return spawnObjects; } }

    public WeightedRandomSelector()
    {
        //spawnObjects = new List<SpawnObject>();
	}

    public WeightedRandomSelector(List<SpawnObject> spawns)
	{
        spawnObjects = spawns;
	}

    public void OnBeforeSerialize()
    {
        
    }

    public void OnAfterDeserialize()
    {
        if (initialized)
            return;

        foreach (var o in SpawnObjects)
        {
            o.prevLocked = o.locked;
            o.prevChance = o.chance;
        }

        lengthObjects = SpawnObjects.Count;

        for (var i = 0; i < SpawnObjects.Count; ++i)
            if (SpawnObjects[i].locked)
                lockedList.Add(i);

        initialized = true;
    }

    public void ValidateFields()
    {
        //Check if the list is empty/not initialized
        if (spawnObjects == null)
		{
            return;
		}
        //Check if list entry is added
        if (lengthObjects != spawnObjects.Count)
        {
            lengthObjects = spawnObjects.Count;
            lockedList = new List<int>();
            for (int i = 0; i < spawnObjects.Count; i++)
            {
                spawnObjects[i].chance = 1f / lengthObjects;
                spawnObjects[i].prevChance = spawnObjects[i].chance;
                if (spawnObjects[i].locked)
                    lockedList.Add(i);
            }
        }
        else
        {
            List<int> indexList = new List<int>(lockedList);
            int index = 0;
            bool change = false;
            float changeAmount = 0.000f;
            float changeSum;
            foreach (SpawnObject spawnObject in spawnObjects)
            {
                //Check if it's a change in probability (the slider)
                if (spawnObject.prevChance != spawnObject.chance)
                {
                    if (spawnObject.locked)
                    {
                        spawnObject.locked = false;
                        spawnObject.prevLocked = false;
                        lockedList.Remove(index);
                    }
                    else
                    {
                        indexList.Add(index);
                    }
                    //Check if it is the only object which is unlocked
                    if (spawnObjects.Count > 1 && lockedList.Count >= spawnObjects.Count - 1)
                    {
                        spawnObject.chance = spawnObject.prevChance;
                        break;
                    }

                    change = true;
                    changeAmount = (spawnObject.chance - spawnObject.prevChance) / (spawnObjects.Count - indexList.Count);
                    spawnObject.prevChance = spawnObject.chance;
                    break;
                }
                //Check if a lock has changed
                if (spawnObject.prevLocked != spawnObject.locked)
                {
                    if (spawnObject.locked)
                        lockedList.Add(index);
                    else
                        lockedList.Remove(index);
                    spawnObject.prevLocked = spawnObject.locked;
                    break;
                }
                index++;
            }
            //Adjust all elements with the changed probability
            while (change)
            {
                change = false;
                float sum = 0.000f;
                for (int i = 0; i < spawnObjects.Count; i++)
                {
                    if (indexList.Contains(i))
                    {
                        sum += spawnObjects[i].chance;
                        continue;
                    }
                    spawnObjects[i].chance -= changeAmount;
                    if (spawnObjects[i].chance < 0.000f)
                    {
                        spawnObjects[i].chance = 0.000f;
                        change = true;
                        indexList.Add(i);
                    }
                    else if (spawnObjects[i].chance > 1.000f)
                    {
                        spawnObjects[i].chance = 1.000f;
                        change = true;
                        indexList.Add(i);
                    }
					else if (Mathf.Approximately(spawnObjects[i].chance, 0))
					{
                        spawnObjects[i].chance = 0;
					}
                    sum += spawnObjects[i].chance;
                    spawnObjects[i].prevChance = spawnObjects[i].chance;
                }

                changeSum = sum - 1.000f;
                if (change && spawnObjects.Count - indexList.Count > 0)
                {
                    changeAmount = changeSum / (spawnObjects.Count - indexList.Count);
                }
                if (indexList.Count == spawnObjects.Count && spawnObjects.Count > 1)
                {
                    spawnObjects[index].chance -= changeSum;
                    spawnObjects[index].prevChance = spawnObjects[index].chance;
                    break;
                }
            }
        }
    }

    public TObj GetRandomValue()
    {
        float rand = UnityEngine.Random.Range(0.000f, 1.000f);
        float chanceSum = 0.00f;
        foreach (SpawnObject obj in spawnObjects)
        {
            if (obj.chance + chanceSum > rand)
            {
                return obj.spawnObject;
            }
            chanceSum += obj.chance;
        }
        return default(TObj);
    }
}