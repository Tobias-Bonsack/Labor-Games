using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public class ElementReceiver : AElementReceiver
    {
        void Awake()
        {
            SetAbstractEvents(_type);
        }
        protected override void ExtendEnterTrigger(ChemistryReceiver.OnReceiveElementArgs e)
        {
            //TODO place for more general enter effect
        }
        protected override void ExtendStayTrigger(ChemistryReceiver.OnReceiveElementArgs e)
        {
            //TODO place for more general stay effect
        }
        protected override void ExtendExitTrigger(ChemistryReceiver.OnReceiveElementArgs e)
        {
            //TODO place for more general exit effect
        }

    }
}
