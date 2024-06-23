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

        //Debug.Log("bow range: " + _weaponRange);
    }
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
        //float arcHeight = Mathf.Clamp(distance / 10.0f, 0.5f, _maxArcHeight); // old
        //float arcHeight = Mathf.Clamp((distance / _weaponRange) * _maxArcHeight, 0.1f, _maxArcHeight); // newer
        float arcHeight = _maxArcHeight * (distance / _weaponRange);
        arcHeight = Mathf.Clamp(arcHeight, 0.1f, _maxArcHeight);

        // Calculate the midpoint for the arc
        Vector3 midpoint = (transform.position + targetLocation) / 2;
        midpoint.y = Mathf.Max(_firingPoint.position.y, targetLocation.y) + arcHeight;

        // Create a parabolic path using a custom tween
        projectileTransform.DOPath(new Vector3[] { transform.position, midpoint, targetLocation }, flightDuration, PathType.CatmullRom)
            .SetEase(Ease.Linear)
            .SetLookAt(0.01f, Vector3.left) // Make the arrow face the direction it's moving
            .OnComplete(() => _projectilePool.ReturnProjectile(_projectileID, projectileObject));
    }
}