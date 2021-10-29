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
        int _numberOfTrigger = 0;

        void Awake()
        {
            _visualEffect = GetComponent<VisualEffect>();

            switch (_type)
            {
                case IChemistry.ChemistryTypes.HEAT:
                    _chemistryReceiver._onReceiveHeat += EnterTrigger;
                    _chemistryReceiver._onReceiveHeat += ExitTrigger;
                    break;
                case IChemistry.ChemistryTypes.COLD:
                    _chemistryReceiver._onReceiveFrost += EnterTrigger;
                    _chemistryReceiver._onReceiveFrost += ExitTrigger;
                    break;
                default:
                    Debug.LogError("UnknownType");
                    break;
            }
        }

        private void EnterTrigger(object sender, ChemistryReceiver.OnReceiveElementArgs e)
        {
            if (e._status == ChemistryEngine.IChemistryReceiver.Status.ENTER)
            {
                _visualEffect.enabled = true;
                _numberOfTrigger++;
            }
        }

        private void ExitTrigger(object sender, ChemistryReceiver.OnReceiveElementArgs e)
        {
            if (e._status == ChemistryEngine.IChemistryReceiver.Status.EXIT)
            {
                if (--_numberOfTrigger == 0) _visualEffect.enabled = false;
            }
        }
    }
}
