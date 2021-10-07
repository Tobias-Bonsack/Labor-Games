using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public class HeatReceiver : MonoBehaviour, IChemistryReceiver
    {
        [SerializeField] GameObject _heatVFX;

        [Tooltip("0f = Full Resistance, 1f = Zero Resistance")]
        [Range(0f, 1f)]
        [SerializeField] float _heatSusceptibility;
        private float _burnPercent = 0f;
        private Material _material;

        private void Awake()
        {
            _material = gameObject.GetComponent<Renderer>().material;
        }

        private void OnTriggerEnter(Collider other)
        {
            ChemistryEmitter chemistryEmitter = other.gameObject.GetComponent<ChemistryEmitter>();
            int positionOfHeat = Array.IndexOf(chemistryEmitter._types, IChemistry.ChemistryTypes.HEAT);
            if (positionOfHeat != -1)
            {
                _heatVFX.SetActive(true);
            }
        }
        private void OnTriggerStay(Collider other)
        {
            ChemistryEmitter chemistryEmitter = other.gameObject.GetComponent<ChemistryEmitter>();
            int positionOfHeat = Array.IndexOf(chemistryEmitter._types, IChemistry.ChemistryTypes.HEAT);
            if (positionOfHeat != -1)
            {
                float radiance = chemistryEmitter._radiance[positionOfHeat];
                _burnPercent += _heatSusceptibility * radiance * Time.fixedDeltaTime;
                _burnPercent = Mathf.Clamp(_burnPercent, 0f, 1f);

                Color color = Color.black;
                color.r = 1f - _burnPercent;
                color.b = 1f - _burnPercent;
                color.g = 1f - _burnPercent;

                _material.SetColor("_BaseColor", color);
            }
        }
        void OnTriggerExit(Collider other)
        {
            ChemistryEmitter chemistryEmitter = other.gameObject.GetComponent<ChemistryEmitter>();
            int positionOfHeat = Array.IndexOf(chemistryEmitter._types, IChemistry.ChemistryTypes.HEAT);
            if (positionOfHeat != -1)
            {
                _heatVFX.SetActive(false);
            }
        }
    }
}
