using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    private Animator _animator;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetIdle()
    {
        DeactivateEveryBool();
        _animator.SetBool("IsIdle", true);
    }

    public void SetRunning()
    {
        DeactivateEveryBool();
        _animator.SetBool("IsRunning", true);
    }

    public void SetJumping()
    {
        DeactivateEveryBool();
        _animator.SetTrigger("IsJumping");
    }

    public void SetRolling()
    {
        DeactivateEveryBool();
        _animator.SetBool("IsRolling", true);
    }

    private void DeactivateEveryBool()
    {
        _animator.SetBool("IsIdle", false);
        _animator.SetBool("IsRunning", false);
        _animator.SetBool("IsRolling", false);
    }
}
