using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonController
{
    public class PlayerItem : MonoBehaviour
    {
        [SerializeField] GameObject _holdPlace;
        [SerializeField] GameObject _throwPosition;

        [Header("SphereCast Parameter")]
        [SerializeField] Transform _camera;
        [SerializeField] Transform _playerHead;
        [SerializeField] float _radius;
        [SerializeField] float _maxDistance;
        [SerializeField] Animator _animator;

        #region throwing parameters
        private Coroutine _ChargeThrow;
        private float _throwPower = 5f, _minThrowPower = 5f, _maxThrowPower = 30f;
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

            Trajectory.SetParams(20, 0.1f, _throwPosition.GetComponent<LineRenderer>());
            Trajectory.SetPoints(_throwPosition.transform.position, _throwPosition.transform.forward * _throwPower);
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
                yield return new WaitForSeconds(0.02f);
                _throwPower *= 1.01f;
                _throwPower = Mathf.Clamp(_throwPower, _minThrowPower, _maxThrowPower);
                Trajectory.SetPoints(_throwPosition.transform.position, _throwPosition.transform.forward * _throwPower);

            }
        }

        public void ThrowItem()
        {
            if (_holdItem != null)
            {
                _holdItem.GetComponent<Collider>().enabled = true;
                _holdItem.AddComponent<Rigidbody>();
                _holdItem.transform.SetParent(null);
                _holdItem.GetComponent<Rigidbody>().AddForce(_throwPosition.transform.forward * _throwPower, ForceMode.Impulse);
                //TODO: make it in an angle...maybe in charger


                _holdItem = null;
            }

            _throwPosition.GetComponent<LineRenderer>().positionCount = 0;
            _throwPower = _minThrowPower;
        }
    }
}
