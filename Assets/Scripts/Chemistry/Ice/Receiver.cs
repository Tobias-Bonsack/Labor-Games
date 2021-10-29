using System;
using System.Collections;
using System.Collections.Generic;
using ChemistryEngine;
using UnityEngine;

namespace FrostEngine
{
    public class Receiver : AbstractReceiver
    {
        void Awake()
        {
            _chemistryReceiver._onReceiveFrost += TriggerStay;
        }

        private void TriggerStay(object sender, ChemistryReceiver.OnReceiveElementArgs e)
        {
            if (e._status == IChemistryReceiver.Status.STAY)
            {
                UpdateElementPercent(e);
            }
        }
    }
}
