using System;
using System.Collections;
using System.Collections.Generic;
using ChemistryEngine;
using UnityEngine;
using UnityEngine.VFX;

namespace ChemistryEngine
{
    [RequireComponent(typeof(VisualEffect))]
    public class ReceiveEffect : AProperty
    {
        [Header("Propertie-Parameter")]
        VisualEffect _visualEffect;

        protected override void Awake()
        {
            base.Awake();
            _visualEffect = GetComponent<VisualEffect>();
            _elementReceiver._onActiveTriggerChange += Triggerchange;
        }

        private void Triggerchange(object sender, EventArgs e)
        {
            _visualEffect.enabled = _elementReceiver.ActiveTriggers != 0;
        }
    }
}
