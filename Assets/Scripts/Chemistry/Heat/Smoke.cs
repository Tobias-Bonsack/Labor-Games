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
        int _numberOfTrigger = 0;

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
                _numberOfTrigger++;
            }
        }

        private void ExitTrigger(object sender, ChemistryReceiver.OnReceiveHeatArgs e)
        {
            if (e._status == ChemistryEngine.IChemistryReceiver.Status.EXIT)
            {
                if(--_numberOfTrigger == 0) _visualEffect.enabled = false;
            }
        }
    }
}
