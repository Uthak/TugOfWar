using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class handles all unit animations and is called by all other scripts on a 
/// unit to call the various animations.
/// </summary>
public class UnitAnimationController : MonoBehaviour
{
    [Tooltip("Default is a single attack animation, if there are multiple, assign the total here!")]
    [SerializeField] int _nrOfAttackAnimations = 1;

    [Tooltip("Default is a single death animation, if there are multiple, assign the total here!")]
    [SerializeField] int _nrOfDeathAnimations = 1;

    UnitManager _unitManager;
    AnimateProjectile _myProjectileAnimator;
    Animator _unitAnimator;
    AnimatorOverrideController _unitAnimatorOverrideController;

    /// <summary>
    /// Set up a units animations upon spawning it. This is called by the UnitManager-script.
    /// </summary>
    public void InitializeUnitAnimatonController()
    {
        // cache components:
        _unitManager = GetComponent<UnitManager>();

        // check first if there is an animator to begin with:
        if (GetComponent<Animator>())
        {
            _unitAnimator = _unitManager.unitAnimator;
            _unitAnimatorOverrideController = _unitManager.unitAnimationOverrideController;
        }else
        {
            Debug.LogError("ERROR: Trying to access Animator-component that doesn't exist", this);
        }

        // if applicable, assign the projectile animator:
        if (GetComponent<AnimateProjectile>())
        {
            _myProjectileAnimator = GetComponent<AnimateProjectile>();
        }
    }

    public void MoveUnit(float movementSpeed)
    {
        if(!_unitAnimator.GetBool("inCombat"))
        {
            _unitAnimator.SetFloat("MovementSpeed", movementSpeed, 0.15f, Time.deltaTime);

        }else
        {
            _unitAnimator.SetFloat("CombatSpeed", movementSpeed, 0.15f, Time.deltaTime);
        }
    }

    public void EnterCombat()
    {
        _unitAnimator.SetBool("inCombat", true);
    }
    public void ExitCombat()
    {
        _unitAnimator.SetBool("inCombat", false);
    }

    public void AttackAnimation(GameObject enemyUnit ,float attackSpeed, WeaponDataSO.WeaponType weaponType)
    {
        // if the attackspeed is faster than the animation speed up the animation to match:
        /*if (attackSpeed < _attackAnimationDuration)
        {
            _unitAnimator.SetFloat("attackSpeedModifier", 1.0f / attackSpeed);
        }*/

        _unitAnimator.SetLayerWeight(_unitAnimator.GetLayerIndex("UpperBody_Layer"), 1.0f);
        _unitAnimator.SetInteger("attackAnimIndex", Random.Range(0, _nrOfAttackAnimations)); // get random attack animation
        _unitAnimator.SetTrigger("attackTrigger");

        //System.Action<float> callback;
        //StartCoroutine(GetCurrentAnimationLength(callback));
        StartCoroutine(AttackAnimationLength());

        // if ranged, fire a projectile:
        if (weaponType == WeaponDataSO.WeaponType.Bow)
        {
            _myProjectileAnimator.AnimateArrow(enemyUnit.transform.position);
        }
    }
    IEnumerator AttackAnimationLength()
    {
        _unitAnimator.SetBool("inAttackAnimation", true);

        yield return new WaitForEndOfFrame();

        AnimatorStateInfo stateInfo = _unitAnimator.GetCurrentAnimatorStateInfo(0);
        float animationLength = stateInfo.length;

        yield return new WaitForSeconds(animationLength);

        _unitAnimator.SetBool("inAttackAnimation", false);
    }

    public void TakeDamageAnimation()
    {
        _unitAnimator.SetTrigger("takeDamageTrigger"); // unsure why, but this animation plays way after the fact?
    }

    public void DeathAnimation(System.Action<float> callback)
    {
        // Get random death animation:
        int randomIndex = Random.Range(0, _nrOfDeathAnimations);
        _unitAnimator.SetInteger("deathAnimIndex", randomIndex);
        _unitAnimator.SetTrigger("deathTrigger");

        // Start coroutine to get the animation length
        StartCoroutine(GetCurrentAnimationLength(callback));
    }

    private IEnumerator GetCurrentAnimationLength(System.Action<float> callback)
    {
        // Wait until the end of the frame to ensure the animation has started
        yield return new WaitForEndOfFrame();

        // Get the current animator state info
        AnimatorStateInfo stateInfo = _unitAnimator.GetCurrentAnimatorStateInfo(0);

        // Get the length of the current animation
        float lengthOfCurrentAnimation = stateInfo.length;

        // Invoke the callback with the length of the animation
        callback?.Invoke(lengthOfCurrentAnimation);
    }
}
