using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public class HeatReceiver : AbstractHeatReceiver
    {
        [SerializeField] GameObject _heatVFX;
        private Material _material;

        private void Awake()
        {
            _material = gameObject.GetComponent<Renderer>().material;
        }

        protected override void ExtendHeatTriggerEnter(Collider other)
        {
            _heatVFX.SetActive(true);
        }
        protected override void ExtendHeatTriggerStay(Collider other)
        {
            Color color = Color.black;
            color.r = 1f - _burnPercent;
            color.b = 1f - _burnPercent;
            color.g = 1f - _burnPercent;

            _material.SetColor("_BaseColor", color);
        }
        protected override void ExtendHeatTriggerExit(Collider other)
        {
            _heatVFX.SetActive(false);
        }
    }
}
