using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPoolManager : MonoBehaviour
{
    public static UnitPoolManager Instance;
    [SerializeField] private int initialPoolSize = 10; // Starting pool size for each unit type
    private Dictionary<GameObject, ObjectPool> unitPools = new Dictionary<GameObject, ObjectPool>();

    private void Awake()
    {
        Instance = this;
    }

    // Request a pool for a specific unit type (called by TroopSelectorIcon)
    public void RequestPoolForUnit(GameObject unitPrefab)
    {
        if (!unitPools.ContainsKey(unitPrefab))
        {
            // Create a new pool for this unit type if it doesn't exist
            ObjectPool newPool = new ObjectPool(unitPrefab, initialPoolSize);
            unitPools.Add(unitPrefab, newPool);
        }
    }

    // Retrieve a unit from the appropriate pool
    public GameObject GetUnitFromPool(GameObject unitPrefab)
    {
        if (unitPools.TryGetValue(unitPrefab, out ObjectPool pool))
        {
            return pool.GetObject();
        }
        else
        {
            Debug.LogError("No pool exists for unit: " + unitPrefab.name);
            return null;
        }
    }

    // Return a unit back to its pool
    public void ReturnUnitToPool(GameObject unitPrefab, GameObject unitInstance)
    {
        if (unitPools.TryGetValue(unitPrefab, out ObjectPool pool))
        {
            pool.ReturnObject(unitInstance);
        }
        else
        {
            Debug.LogError("No pool exists for unit: " + unitPrefab.name);
        }
    }
}
