using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scanner
{

    public class EnvironmentScanner : MonoBehaviour
    {
        [Header("Overlapshere Parameter")]
        [SerializeField] float _radius;

        private int _layerMask;

        private void Awake()
        {
            // 8 == Grabbable, 9 == Outline
            _layerMask = (1 << 8) | (1 << 9);
        }

        public void ScanEnvironment()
        {
            Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, _radius, _layerMask);
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.TryGetComponent<Interactive>(out Interactive interactive))
                {
                    interactive.ActivateOutline();
                }
            }
        }
    }
}
