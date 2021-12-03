using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public class GraphMember : AProperty
    {
        public string _originalGraph;
        [HideInNormalInspector] public List<GraphMember> _neibhbors = new List<GraphMember>();
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
            if (e._status == IChemistryReceiver.Status.ENTER)
            {
                if (e._emitterType != IChemistryEmitter.Type.GRID_MEMBER)
                    UpdateAbleToReceive(+1);
                else if (sender is GraphMemberEmitter && ((GraphMemberEmitter)sender).MEMBER != null)
                    _neibhbors.Add(((GraphMemberEmitter)sender).MEMBER);
            }
        }

        protected void ExitTrigger(object sender, ChemistryReceiver.OnReceiveElementArgs e)
        {
            if (e._status == IChemistryReceiver.Status.EXIT)
            {
                if (e._emitterType != IChemistryEmitter.Type.GRID_MEMBER)
                    UpdateAbleToReceive(-1);
                else if (sender is GraphMemberEmitter && ((GraphMemberEmitter)sender).MEMBER != null)
                    _neibhbors.Remove(((GraphMemberEmitter)sender).MEMBER);
            }
        }

        protected void UpdateAbleToReceive(int addValue)
        {
            GraphSystem.AddPowerSource(_originalGraph, addValue);
        }
    }
}
