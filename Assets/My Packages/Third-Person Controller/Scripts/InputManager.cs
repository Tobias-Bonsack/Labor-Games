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
        private PlayerEvents _playerEvents;

        #region camera
        private bool _cameraIsMoving = false;
        private Coroutine _cameraRoutine;
        #endregion

        private void Awake()
        {
            _movement = _player.GetComponent<PlayerMovement>();
            _item = _player.GetComponent<PlayerItem>();
            _scanner = _player.GetComponent<Scanner.EnvironmentScanner>();
            _playerEvents = gameObject.GetComponent<PlayerEvents>();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _movement._isMoving = !context.canceled;
            //Normalize the vector to have an uniform vector in whichever form it came from (I.E Gamepad, mouse, etc)
            Vector2 moveDirection = context.ReadValue<Vector2>().normalized;
            _movement._direction = new Vector3(moveDirection.x, 0f, moveDirection.y);

        }
        public void OnMouseDelta(InputAction.CallbackContext context)
        {
            _cameraIsMoving = !context.canceled;
            if (context.started)
            {
                _cameraRoutine = StartCoroutine(CameraMoving(context));
            }
            else if (context.canceled)
            {
                StopCoroutine(_cameraRoutine);
            }
        }
        private IEnumerator CameraMoving(InputAction.CallbackContext context)
        {
            while (_cameraIsMoving)
            {
                yield return new WaitForSeconds(Time.fixedDeltaTime);

                if (StaticProperties._currentCamera == 0)
                {
                    _suroundCamera.gameObject.GetComponent<FreeLookAddOn>().OnLook(context);
                }
                else if (StaticProperties._currentCamera == 1)
                {
                    _movement.Rotate(context.ReadValue<Vector2>());
                }
            }
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

                StaticProperties._currentCamera = 1;
            }
            else if (context.canceled)
            {
                _suroundCamera.enabled = true;
                _zoomCamera.Priority = 0;
                _suroundCamera.m_YAxis.Value = 0.4f;
                _suroundCamera.m_XAxis.Value = -10f;

                StaticProperties._currentCamera = 0;
            }
        }

        public void OnItemDrop(InputAction.CallbackContext context)
        {
            if (context.started) _item.DropItem();
        }
    }
}

