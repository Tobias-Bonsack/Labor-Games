using System;
using System.Collections;
using System.Collections.Generic;
using ChemistryEngine;
using UnityEngine;
using UnityEngine.VFX;

namespace HeatEngine
{
    public class Receiver : AbstractReceiver
    {
        [Header("Heat Properties")]
        [HideInNormalInspector] public int _activeTriggers = 0;

        void Awake()
        {
            _chemistryReceiver._onReceiveHeat += EnterTrigger;
            _chemistryReceiver._onReceiveHeat += ExitTrigger;
            _chemistryReceiver._onReceiveHeat += TriggerStay;
        }

        private void EnterTrigger(object sender, ChemistryReceiver.OnReceiveElementArgs e)
        {
            if (e._status == IChemistryReceiver.Status.ENTER) _activeTriggers++;
        }
        private void TriggerStay(object sender, ChemistryReceiver.OnReceiveElementArgs e)
        {
            if (e._status == IChemistryReceiver.Status.STAY)
            {
                UpdateElementPercent(e);
            }
        }

        private void ExitTrigger(object sender, ChemistryReceiver.OnReceiveElementArgs e)
        {
            if (e._status == IChemistryReceiver.Status.EXIT) _activeTriggers--;
        }
    }
}
