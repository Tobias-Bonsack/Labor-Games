using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ThirdPersonController
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] GameObject _player;
        private PlayerMovement _movement;
        private PlayerItem _item;

        private void Awake()
        {
            _movement = _player.GetComponent<PlayerMovement>();
            _item = _player.GetComponent<PlayerItem>();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _movement._isMoving = !context.canceled;
            //Normalize the vector to have an uniform vector in whichever form it came from (I.E Gamepad, mouse, etc)
            Vector2 moveDirection = context.ReadValue<Vector2>().normalized;
            _movement._direction = new Vector3(moveDirection.x, 0f, moveDirection.y);

        }
        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _movement.Jump(true);
            }
            else if (context.canceled)
            {
                _movement.Jump(false);
            }
        }

        public void OnItemGrab(InputAction.CallbackContext context)
        {

        }
        public void OnScanEnvironment(InputAction.CallbackContext context)
        {

        }
    }
}

