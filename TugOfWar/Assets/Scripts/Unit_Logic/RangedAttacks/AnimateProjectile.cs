using UnityEngine;
using DG.Tweening;

public class AnimateProjectile : MonoBehaviour
{
    [Header("Projectile Animation Setup:")]
    [SerializeField] int _projectileID = 0; // 0 arrow, 1 crossbow-bolt, 2 bullet, 3 boulder, 4 fire ball

    [SerializeField] Transform _firingPoint; // the point where the arrow leaves the bow, etc.

    [SerializeField] float _projectileSpeed = 50.0f; // Duration for the arrow to reach the target
    [SerializeField] float _maxArcHeight = 5.0f; // Height of the arc
    
    float _weaponRange = 0.0f; // Maximum range of the weapon
    private ProjectilePool _projectilePool;

    public void InitializeProjectileAnimator()
    {
        _projectilePool = FindAnyObjectByType<ProjectilePool>();
        _weaponRange = GetComponent<UnitManager>().baseAttackRange;

        Debug.Log("bow range: " + _weaponRange);
    }

    //public void AnimateArrow(Vector3 targetLocation)
    public void AnimateArrow(GameObject targetUnit)
    {
        // get a projectile from the pool:
        GameObject projectileObject = _projectilePool.GetProjectile(_projectileID);
        Transform projectileTransform = projectileObject.transform;

        // Set the starting position and rotation of the projectile:
        projectileTransform.position = _firingPoint.position;

        // target the center of mass of enemy units:
        Collider enemyCollider = targetUnit.GetComponent<Collider>();
        Vector3 targetPoint = new Vector3(0,0,0);
        if (enemyCollider != null)
        {
            targetPoint = enemyCollider.bounds.center;
        }else
        {
            targetPoint = targetUnit.transform.position;
        }

        // Calculate the distance and the flight duration based on speed
        float distance = Vector3.Distance(_firingPoint.position, targetPoint);
        float flightDuration = distance / _projectileSpeed;

        #region new arc calc
        // Calculate the dynamic arc height based on the distance
        float arcHeight;
        float halfRange = _weaponRange / 2.0f;

        if (distance <= halfRange)
        {
            //arcHeight = Mathf.Lerp(0f, (_maxArcHeight/10.0f), distance / halfRange);
            arcHeight = 0.0f;
        }
        else
        {
            arcHeight = Mathf.Lerp((_maxArcHeight / 10.0f), _maxArcHeight, (distance - halfRange) / halfRange);
        }

        arcHeight = Mathf.Clamp(arcHeight, (_maxArcHeight / 10.0f), _maxArcHeight);
        #endregion

        // Calculate the dynamic arc height based on the distance
        //float arcHeight = _maxArcHeight * (distance / _weaponRange); // 2 * (10 / 10) = 2; 2 * (2 / 10)
        //arcHeight = Mathf.Clamp(arcHeight, 0.1f, _maxArcHeight);

        // Calculate the midpoint for the arc
        Vector3 midpoint = (_firingPoint.position + targetPoint) / 2;
        
        midpoint.y = Mathf.Max(_firingPoint.position.y, targetPoint.y) + arcHeight;
        //midpoint.y = Mathf.Min(_firingPoint.position.y, targetPoint.y) + arcHeight;

        // Create a parabolic path using a custom tween
        //projectileTransform.DOPath(new Vector3[] { transform.position, midpoint, targetLocation }, flightDuration, PathType.CatmullRom)
        projectileTransform.DOPath(new Vector3[] { _firingPoint.position, midpoint, targetPoint }, flightDuration, PathType.CatmullRom)
            .SetEase(Ease.Linear)
            .SetLookAt(0.01f, Vector3.right) // Make the arrow face the direction it's moving
            .OnComplete(() => _projectilePool.ReturnProjectile(_projectileID, projectileObject));
    }

    /*
    public void AnimateArrow(Vector3 targetLocation)
    {
        // Get an arrow from the pool
        GameObject projectileObject = _projectilePool.GetProjectile(_projectileID);
        Transform projectileTransform = projectileObject.transform;

        // Set the starting position and rotation of the arrow
        projectileTransform.position = _firingPoint.position;

        // Calculate the distance and the flight duration based on speed
        float distance = Vector3.Distance(transform.position, targetLocation);
        float flightDuration = distance / _projectileSpeed;

        // Calculate the dynamic arc height based on the distance
        float arcHeight = _maxArcHeight * (distance / _weaponRange);
        arcHeight = Mathf.Clamp(arcHeight, 0.1f, _maxArcHeight);

        // Calculate the midpoint for the arc
        Vector3 midpoint = (transform.position + targetLocation) / 2;
        midpoint.y = Mathf.Max(_firingPoint.position.y, targetLocation.y) + arcHeight;

        // Create a parabolic path using a custom tween
        //projectileTransform.DOPath(new Vector3[] { transform.position, midpoint, targetLocation }, flightDuration, PathType.CatmullRom)
        projectileTransform.DOPath(new Vector3[] { _firingPoint.position, midpoint, targetLocation }, flightDuration, PathType.CatmullRom)
            .SetEase(Ease.Linear)
            .SetLookAt(0.01f, Vector3.right) // Make the arrow face the direction it's moving
            .OnComplete(() => _projectilePool.ReturnProjectile(_projectileID, projectileObject));
    }*/
}