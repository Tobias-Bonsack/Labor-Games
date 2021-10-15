using System;
using System.Collections;
using System.Collections.Generic;
using ChemistryEngine;
using UnityEngine;
using UnityEngine.VFX;

namespace HeatEngine
{
    [RequireComponent(typeof(VisualEffect))]
    public class Smoke : AbstractProperty
    {
        VisualEffect _visualEffect;

        void Awake()
        {
            _visualEffect = GetComponent<VisualEffect>();
            
            _chemistryReceiver._onReceiveHeat += EnterTrigger;
            _chemistryReceiver._onReceiveHeat += ExitTrigger;
        }

        private void EnterTrigger(object sender, ChemistryReceiver.OnReceiveHeatArgs e)
        {
            if (e._status == ChemistryEngine.IChemistryReceiver.Status.ENTER)
            {
                _visualEffect.enabled = true;
            }
        }

        private void ExitTrigger(object sender, ChemistryReceiver.OnReceiveHeatArgs e)
        {
            if (e._status == ChemistryEngine.IChemistryReceiver.Status.EXIT)
            {
                _visualEffect.enabled = false;
            }
        }
    }
}
