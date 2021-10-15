using System;
using System.Collections;
using System.Collections.Generic;
using ChemistryEngine;
using UnityEngine;

namespace HeatEngine
{
    public class ChangeMaterialColor : AbstractProperty
    {
        [SerializeField] Renderer _renderer;
        void Awake()
        {
            _chemistryReceiver.OnReceiveHeat += EnterStay;
        }

        private void EnterStay(object sender, ChemistryReceiver.OnReceiveHeatArgs e)
        {
            if (e._status == ChemistryEngine.IChemistryReceiver.Status.STAY)
            {
                Color color = Color.black;
                color.r = 1f - _heatReceiver._burnPercent;
                color.b = 1f - _heatReceiver._burnPercent;
                color.g = 1f - _heatReceiver._burnPercent;

                _renderer.material.SetColor("_BaseColor", color);
            }
        }
    }
}
