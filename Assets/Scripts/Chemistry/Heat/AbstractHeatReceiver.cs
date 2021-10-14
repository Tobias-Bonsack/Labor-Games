using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace ChemistryEngine
{
    [RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public abstract class AbstractHeatReceiver : MonoBehaviour, IChemistryReceiver
    {

        protected VisualEffect _visualEffect;
        [SerializeField, Tooltip("0f = Full Resistance, 1f = Zero Resistance"), Range(0f, 1f)] protected float _heatSusceptibility;
        protected float _burnPercent = 0f;
        protected int _positionOfHeat;

        void Awake()
        {
            _visualEffect = GetComponent<VisualEffect>();
        }
        void OnTriggerEnter(Collider other)
        {
            UpdateIndex(other);
            if (_positionOfHeat != -1) ExtendHeatTriggerEnter(other);
        }
        void OnTriggerStay(Collider other)
        {
            UpdateIndex(other);

            if (_positionOfHeat != -1)
            {
                ChemistryEmitter chemistryEmitter = other.gameObject.GetComponent<ChemistryEmitter>();
                float radiance = chemistryEmitter._radiance[_positionOfHeat];
                _burnPercent += _heatSusceptibility * radiance * Time.fixedDeltaTime;
                _burnPercent = Mathf.Clamp(_burnPercent, 0f, 1f);

                ExtendHeatTriggerStay(other);
            }

        }
        void OnTriggerExit(Collider other)
        {
            UpdateIndex(other);
            if (_positionOfHeat != -1) ExtendHeatTriggerExit(other);
        }
        private void UpdateIndex(Collider other)
        {
            ChemistryEmitter chemistryEmitter = other.gameObject.GetComponent<ChemistryEmitter>();
            _positionOfHeat = Array.IndexOf(chemistryEmitter._types, IChemistry.ChemistryTypes.HEAT);
        }

        protected abstract void ExtendHeatTriggerEnter(Collider other);
        protected abstract void ExtendHeatTriggerStay(Collider other);
        protected abstract void ExtendHeatTriggerExit(Collider other);
    }
}
