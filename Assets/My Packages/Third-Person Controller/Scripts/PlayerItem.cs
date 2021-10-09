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

                _holdItem.GetComponent<Collider>().enabled = false;
                Destroy(_holdItem.GetComponent<Rigidbody>());
            }

        }

        internal void OnThrow(bool isStarted)
        {
            _animator.SetBool("isThrowing", isStarted);

            if (!isStarted)
            {
                _animator.SetFloat("ThrowSpeedMultiplier", 2f);
            }
        }

        public void ThrowItem()
        {
            if (_holdItem != null)
            {
                _holdItem.GetComponent<Collider>().enabled = true;
                _holdItem.AddComponent<Rigidbody>();
                _holdItem.transform.SetParent(null);
                _holdItem.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * 5f, ForceMode.VelocityChange);

                _holdItem = null;
            }
        }
    }
}
