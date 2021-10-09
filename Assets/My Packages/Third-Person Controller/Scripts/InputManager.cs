using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

namespace ThirdPersonController
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] GameObject _player;
        [SerializeField] CinemachineVirtualCamera _zoomCamera;
        [SerializeField] CinemachineFreeLook _suroundCamera;
        private PlayerMovement _movement;
        private PlayerItem _item;
        private Scanner.EnvironmentScanner _scanner;

        private void Awake()
        {
            _movement = _player.GetComponent<PlayerMovement>();
            _item = _player.GetComponent<PlayerItem>();
            _scanner = _player.GetComponent<Scanner.EnvironmentScanner>();
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
            if (context.started)
            {
                _item.OnGrabItem();
            }
        }
        public void OnScanEnvironment(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _scanner.ScanEnvironment();
            }
        }

        public void OnThrow(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _item.OnThrow(true);
            }
            else if (context.canceled)
            {
                _item.OnThrow(false);
            }
        }

        public void OnZoom(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _suroundCamera.enabled = false;
                _zoomCamera.Priority = 2;
            }
            else if (context.canceled)
            {
                _suroundCamera.enabled = true;
                _zoomCamera.Priority = 0;
                _suroundCamera.m_YAxis.Value = 0.4f;
                _suroundCamera.m_XAxis.Value = -10f;
            }
        }
    }
}

