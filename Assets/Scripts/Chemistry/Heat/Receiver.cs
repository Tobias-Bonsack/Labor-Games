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
        void Awake()
        {
            SetAbstractEvents(IChemistry.ChemistryTypes.HEAT);
        }
        protected override void ExtendEnterTrigger(ChemistryReceiver.OnReceiveElementArgs e)
        {
        }

        protected override void ExtendExitTrigger(ChemistryReceiver.OnReceiveElementArgs e)
        {
        }

        protected override void ExtendStayTrigger(ChemistryReceiver.OnReceiveElementArgs e)
        {
        }
    }
}
