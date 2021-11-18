using System;
using System.Collections;
using System.Collections.Generic;
using ChemistryEngine;
using UnityEngine;
using UnityEngine.VFX;

namespace ChemistryEngine
{
    [RequireComponent(typeof(VisualEffect))]
    public class ReceiveEffect : AbstractProperty
    {
        [Header("Propertie-Parameter")]
        VisualEffect _visualEffect;

        void Awake()
        {
            _visualEffect = GetComponent<VisualEffect>();
            _elementReceiver._onActiveTriggerChange += Triggerchange;
        }

        private void Triggerchange(object sender, EventArgs e)
        {
            _visualEffect.enabled = _elementReceiver.ActiveTriggers != 0;
        }
    }
}
