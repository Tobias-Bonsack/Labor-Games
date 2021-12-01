using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public class GraphConnector : GraphMember
    {

        protected override void Awake()
        {
            base.Awake();
        }

        new protected void EnterTrigger(object sender, ChemistryReceiver.OnReceiveElementArgs e)
        {

        }

        new protected void ExitTrigger(object sender, ChemistryReceiver.OnReceiveElementArgs e)
        {

        }
    }
}