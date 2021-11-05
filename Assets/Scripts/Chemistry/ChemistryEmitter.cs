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
        public List<IChemistry.ChemistryTypes> _types;

        [Tooltip("Radiance per second, Position mirrors _types"), Range(0f, 1f)]
        public List<float> _radiance;


        [Header("Effect Parameter")]

        [Tooltip("Types of chemistry for the effects")]
        public List<IChemistry.ChemistryTypes> _typeOfEffect;

        [Tooltip("Types of VisualEffects, Position mirrors _typeOfEffect")]
        public List<VisualEffect> _effects;

        public void AddType(IChemistry.ChemistryTypes type, float radiance)
        {
            int position = _types.IndexOf(type);
            if (position == -1)
            {
                _types.Add(type);
                _radiance.Add(radiance);
            }
            else
            {
                _radiance[position] += radiance;
            }

            UpdateVisualEffects(true, type);
        }

        public void RemoveType(IChemistry.ChemistryTypes type)
        {
            int position = _types.IndexOf(type);
            if (position != -1)
            {
                _types.Remove(type);
                _radiance.RemoveAt(position);
            }

            UpdateVisualEffects(false, type);
        }

        private void UpdateVisualEffects(bool toActivate, IChemistry.ChemistryTypes type)
        {
            int pos = _typeOfEffect.IndexOf(type);
            if (pos == -1) return;

            _effects[pos].enabled = toActivate;
        }
    }
}
