using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public class GraphMember : AProperty
    {
        public string _originalGraph;
        public bool AbleToReceive
        {
            get
            {
                return _elementReceiver.AbleToReceive;
            }
            set
            {
                _elementReceiver.AbleToReceive = value;
            }
        }
        protected override void Awake()
        {
            base.Awake();

            GraphSystem.AddBaseGraph(_originalGraph);
            GraphSystem.graphs[_originalGraph].Add(this);

            switch (_type)
            {
                case IChemistry.ChemistryTypes.ELECTRICITY:
                    _chemistryReceiver._onReceiveElectricity += EnterTrigger;
                    _chemistryReceiver._onReceiveElectricity += ExitTrigger;
                    break;
                default:
                    break;
            }
        }
        protected void EnterTrigger(object sender, ChemistryReceiver.OnReceiveElementArgs e)
        {
            if (e._status == IChemistryReceiver.Status.ENTER && e._emitterType != IChemistryEmitter.Type.GRID_MEMBER)
            {
                UpdateAbleToReceive(+1);
                NewMethod();
            }

        }
        protected void ExitTrigger(object sender, ChemistryReceiver.OnReceiveElementArgs e)
        {
            if (e._status == IChemistryReceiver.Status.EXIT && e._emitterType != IChemistryEmitter.Type.GRID_MEMBER)
            {
                UpdateAbleToReceive(-1);
                NewMethod();
            }

        }
        protected void UpdateAbleToReceive(int addValue)
        {
            GraphSystem.AddPowerSource(_originalGraph, addValue);
        }

        protected static void NewMethod()
        {
            Debug.Log("-------------");
            foreach (string item in GraphSystem.baseGraphen.Keys)
            {
                Debug.Log(item + ": " + GraphSystem.baseGraphen[item].PowerLevel + "-" + GraphSystem.baseGraphen[item]._connections.Count);
            }

            foreach (string item in GraphSystem.combineGraphen.Keys)
            {
                Debug.Log(item + ": " + GraphSystem.combineGraphen[item].PowerLevel + "-" + GraphSystem.combineGraphen[item]._connections.Count);
            }
        }
    }
}
