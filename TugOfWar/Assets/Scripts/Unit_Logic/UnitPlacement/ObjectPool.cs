using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private GameObject prefab;
    private Queue<GameObject> availableObjects;
    private int poolSize;

    public ObjectPool(GameObject prefab, int initialSize)
    {
        this.prefab = prefab;
        this.poolSize = initialSize;
        availableObjects = new Queue<GameObject>();

        // Pre-instantiate objects and disable them
        for (int i = 0; i < poolSize; i++)
        {
            GameObject newObj = Object.Instantiate(prefab);
            newObj.SetActive(false);
            availableObjects.Enqueue(newObj);
        }
    }

    public GameObject GetObject()
    {
        if (availableObjects.Count > 0)
        {
            GameObject obj = availableObjects.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            // If the pool is empty, create a new object and add it to the pool
            GameObject newObj = Object.Instantiate(prefab);
            return newObj;
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        availableObjects.Enqueue(obj);
    }
}
