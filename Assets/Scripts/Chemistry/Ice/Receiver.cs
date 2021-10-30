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
            SetAbstractEvents(IChemistry.ChemistryTypes.COLD);
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
