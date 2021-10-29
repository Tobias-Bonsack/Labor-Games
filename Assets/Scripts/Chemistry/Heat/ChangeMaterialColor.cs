using System;
using System.Collections;
using System.Collections.Generic;
using ChemistryEngine;
using UnityEngine;

namespace HeatEngine
{
    public class ChangeMaterialColor : AbstractProperty
    {
        [Header("Propertie-Parameter")]
        [SerializeField] Renderer _renderer;
        [SerializeField] Color _endColor;
        private Color _materialColor, _stepColor;
        void Awake()
        {
            _chemistryReceiver._onReceiveHeat += EnterStay;
            _elementReceiver._onBurnPercentChange += OnBurnPercentChange;
            _materialColor = _renderer.material.GetColor("_BaseColor");

            _stepColor = _endColor - _materialColor;
        }
        private void EnterStay(object sender, ChemistryReceiver.OnReceiveElementArgs e)
        {
            if (e._status == ChemistryEngine.IChemistryReceiver.Status.STAY)
            {
                Color color = _materialColor + (_stepColor * _elementReceiver._elementPercent);
                _renderer.material.SetColor("_BaseColor", color);
            }
        }
        private void OnBurnPercentChange(object sender, EventArgs e)
        {
            Color color = _materialColor + (_stepColor * _elementReceiver._elementPercent);
            _renderer.material.SetColor("_BaseColor", color);
        }
    }
}
