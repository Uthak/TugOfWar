using UnityEngine;
using System.Collections.Generic;

public class ProjectilePool : MonoBehaviour
{
    [SerializeField] GameObject _arrow;
    // additional projectile types

    [SerializeField] int initialPoolSize = 10;

    private Queue<GameObject> _arrowPool = new Queue<GameObject>();
    // additional projectile pools

    void Start()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = Instantiate(_arrow);
            obj.SetActive(false);
            _arrowPool.Enqueue(obj);

            // every other pool
        }
    }

    public GameObject GetProjectile(int _projectileID)
    {
        switch(_projectileID)
        {
            case 0: // arrow
                if (_arrowPool.Count > 0) // if there are enough arrows, give one of those:
                {
                    GameObject obj = _arrowPool.Dequeue();
                    obj.SetActive(true);
                    return obj;
                }else // if there aren't enough arrows, create one:
                {
                    GameObject obj = Instantiate(_arrow);
                    return obj;
                }

            case 1:
                return _arrow; // placeholder!

            case 2:
                return _arrow; // placeholder!

            case 3:
                return _arrow; // placeholder!

            case 4:
                return _arrow; // placeholder!

            default:
                Debug.LogError("ERROR: Projectile-type requested that isn't yet defined!", this);
                return null;
        }
    }

    public void ReturnProjectile(int _projectileID, GameObject obj)
    {
        obj.SetActive(false);

        switch (_projectileID)
        {
            case 0: // arrow
                _arrowPool.Enqueue(obj);
                return;

            case 1:
                return;

            case 2:
                return;

            case 3:
                return;

            case 4:
                return;

            default:
                Debug.LogError("ERROR: Projectile-type requested that isn't yet defined!", this);
                return;
        }
    }
}