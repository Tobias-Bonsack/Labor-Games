using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonController
{
    public class PlayerItem : MonoBehaviour
    {
        [SerializeField] GameObject _holdPlace;

        [Header("SphereCast Parameter")]
        [SerializeField] Transform _camera;
        [SerializeField] Transform _playerHead;
        [SerializeField] float _radius;
        [SerializeField] float _maxDistance;
        [SerializeField] Animator _animator;

        #region throwing parameters
        private Coroutine _ChargeThrow;
        private float _throwPower = 10f, _minThrowPower = 10f, _maxThrowPower = 30f;
        #endregion


        private GameObject _holdItem;

        private int _layerMask = (1 << 8);

        public void OnGrabItem()
        {
            Debug.DrawRay(_playerHead.position, _camera.forward, Color.red, 1f);

            if (Physics.SphereCast(_playerHead.position, _radius, _camera.forward, out RaycastHit hitInfo, _maxDistance, _layerMask))
            {
                _holdItem = hitInfo.collider.gameObject;
                _holdItem.transform.SetParent(_holdPlace.transform);
                _holdItem.transform.localPosition = Vector3.zero;
                _holdItem.transform.localScale = Vector3.one;
                _holdItem.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

                _holdItem.GetComponent<Collider>().enabled = false;
                Destroy(_holdItem.GetComponent<Rigidbody>());
            }

        }

        internal void OnThrow(bool isStarted)
        {
            _animator.SetBool("isThrowing", isStarted);

            if (isStarted)
            {
                _ChargeThrow = StartCoroutine(ChargeThrow());
            }
            else
            {
                StopCoroutine(_ChargeThrow);
                _animator.SetFloat("ThrowSpeedMultiplier", 1f);
            }
        }
        private IEnumerator ChargeThrow()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.01f);
                _throwPower *= 1.01f;
                _throwPower = Mathf.Clamp(_throwPower, _minThrowPower, _maxThrowPower);
            }
        }

        public void ThrowItem()
        {
            if (_holdItem != null)
            {
                _holdItem.GetComponent<Collider>().enabled = true;
                _holdItem.AddComponent<Rigidbody>();
                _holdItem.transform.SetParent(null);
                _holdItem.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * _throwPower, ForceMode.Impulse);
                //TODO: make it in an angle...maybe in charger

                _holdItem = null;
            }

            _throwPower = _minThrowPower;
        }
    }
}
