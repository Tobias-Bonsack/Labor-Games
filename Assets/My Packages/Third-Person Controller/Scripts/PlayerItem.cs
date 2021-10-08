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

        private int _layerMask = (1 << 8);

        public void OnGrabItem()
        {
            Debug.DrawRay(_playerHead.position, _camera.forward, Color.red, 1f);

            if (Physics.SphereCast(_playerHead.position, _radius, _camera.forward, out RaycastHit hitInfo, _maxDistance, _layerMask))
            {
                GameObject hittedObject = hitInfo.collider.gameObject;
                hittedObject.transform.SetParent(_holdPlace.transform);
                hittedObject.transform.localPosition = Vector3.zero;
                hittedObject.transform.localScale = Vector3.one;

                hittedObject.GetComponent<Collider>().enabled = false;
                Destroy(hittedObject.GetComponent<Rigidbody>());
            }

        }
    }
}
