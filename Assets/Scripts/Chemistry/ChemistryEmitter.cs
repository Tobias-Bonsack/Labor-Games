using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace ChemistryEngine
{
    [RequireComponent(typeof(Collider))]
    public class ChemistryEmitter : MonoBehaviour, IChemistryEmitter
    {

        [Header("Emit Parameter")]

        [Tooltip("Types of chemistry this GameObject is able to emit, each type needs an _radiance float")]
        [SerializeField] List<IChemistry.ChemistryTypes> _types;

        [HideInInspector]
        public ReadOnlyCollection<IChemistry.ChemistryTypes> Types
        {
            get
            {
                return new ReadOnlyCollection<IChemistry.ChemistryTypes>(_types);
            }
        }


        [Tooltip("Radiance per second, Position mirrors _types"), Range(0f, 1f)]
        [SerializeField] List<float> _radiance;
        [HideInInspector]
        public ReadOnlyCollection<float> Radiance
        {
            get
            {
                return new ReadOnlyCollection<float>(_radiance);
            }
        }


        [Header("Effect Parameter")]

        [Tooltip("Types of chemistry for the effects")]
        public List<IChemistry.ChemistryTypes> _typeOfEffect;

        [Tooltip("Types of VisualEffects, Position mirrors _typeOfEffect")]
        public List<VisualEffect> _effects;

        private HashSet<IChemistryReceiver> _activeReceiver = new HashSet<IChemistryReceiver>();

        public void AddType(IChemistry.ChemistryTypes type, float radiance)
        {
            int position = Types.IndexOf(type);
            if (position == -1)
            {
                _types.Add(type);
                _radiance.Add(radiance);
                foreach (IChemistryReceiver receiver in _activeReceiver)
                {
                    receiver.NewEmitType(this, type);
                }
            }
            else
            {
                _radiance[position] += radiance;
            }

            UpdateVisualEffects(true, type);
        }

        public void RemoveType(IChemistry.ChemistryTypes type)
        {
            int position = Types.IndexOf(type);
            if (position != -1)
            {
                _types.Remove(type);
                _radiance.RemoveAt(position);
                foreach (IChemistryReceiver receiver in _activeReceiver)
                {
                    receiver.RemovedEmitType(this, type);
                }
            }

            UpdateVisualEffects(false, type);
        }

        private void UpdateVisualEffects(bool toActivate, IChemistry.ChemistryTypes type)
        {
            int pos = _typeOfEffect.IndexOf(type);
            if (pos == -1) return;

            _effects[pos].enabled = toActivate;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<IChemistryReceiver>(out IChemistryReceiver receiver))
            {
                Debug.Log("Add: " + other);
                _activeReceiver.Add(receiver);
            }
        }
        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent<IChemistryReceiver>(out IChemistryReceiver receiver))
            {
                Debug.Log("Remvoe: " + other);
                _activeReceiver.Remove(receiver);
            }
        }

        public void RemoveReceiver(IChemistryReceiver receiver)
        {
            Debug.Log("Remvoe: " + receiver);
            _activeReceiver.Remove(receiver);
        }
    }
}
