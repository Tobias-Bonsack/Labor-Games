using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public class ChemistryEmitter : MonoBehaviour, IChemistryEmitter<IChemistry.ChemistryTypes>
    {

        [SerializeField] Transform _origin;
        [SerializeField] float _radius;
        [SerializeField] LayerMask _layerMask;
        [SerializeField] IChemistry.ChemistryTypes[] _types;

        [Tooltip("Time between Overlapsphere cast, nagetiv value lead to Time.fixedDeltaTime")]
        [SerializeField] float _timer;

        // Start is called before the first frame update
        void Start()
        {
            _timer = _timer < 0f ? Time.fixedDeltaTime : _timer;
            StartCoroutine(CheckForReceiver());
        }

        IEnumerator CheckForReceiver()
        {
            while (true)
            {
                yield return new WaitForSeconds(_timer);
                Emit(_types);
            }
        }

        void FixedUpdate()
        {

        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        public void Emit(IChemistry.ChemistryTypes[] chemistryType)
        {
            Debug.Log("Emit");
            Collider[] colliders = Physics.OverlapSphere(_origin.position, _radius, _layerMask);
            foreach (Collider collider in colliders)
            {
                IChemistryReceiver[] chemistryReceivers = collider.gameObject.GetComponents<IChemistryReceiver>();
                Debug.Log("Receiver: " + chemistryReceivers.Length);
                foreach (IChemistryReceiver receiver in chemistryReceivers)
                {
                    receiver.receiv(_types, _timer);
                }
            }
        }
    }
}
