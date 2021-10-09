using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventManager : MonoBehaviour
{

    [SerializeField] Animator _animator;

    public void ThrowEvent()
    {
        if (_animator.GetBool("isThrowing"))
        {
            _animator.SetFloat("ThrowSpeedMultiplier", 0f);
        }
    }
}