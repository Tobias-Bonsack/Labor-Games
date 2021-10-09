using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventManager : MonoBehaviour
{

    [SerializeField] Animator _animator;
    [SerializeField] ThirdPersonController.PlayerItem _playerItem;
    public void ThrowEvent()
    {
        if (_animator.GetBool("isThrowing"))
        {
            _animator.SetFloat("ThrowSpeedMultiplier", 0f);
        }
    }

    public void ThrowItemEvent()
    {
        _playerItem.ThrowItem();
    }
}