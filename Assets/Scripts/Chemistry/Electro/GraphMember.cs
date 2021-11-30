using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public class GraphMember : AbstractProperty
    {

        public static Dictionary<string, HashSet<GraphMember>> grids = new Dictionary<string, HashSet<GraphMember>>();
        public static Dictionary<string, int> gridsPowerNodes = new Dictionary<string, int>();

        [SerializeField] string _graphName;
        [SerializeField] GraphMemberEmitter _gridEmitter;
        public bool AbleToReceive
        {
            get
            {
                return _elementReceiver._ableToReceive;
            }
            set
            {
                _elementReceiver._ableToReceive = value;
            }
        }
        private void Awake()
        {
            if (!grids.ContainsKey(_graphName)) grids.Add(_graphName, new HashSet<GraphMember>());
            if (!gridsPowerNodes.ContainsKey(_graphName)) gridsPowerNodes.Add(_graphName, 0);

            _gridEmitter._graphName = _graphName;

            grids[_graphName].Add(this);
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

        private void EnterTrigger(object sender, ChemistryReceiver.OnReceiveElementArgs e)
        {
            if (e._status == IChemistryReceiver.Status.ENTER && !(IsGridElement(e._emitterType)))
            {
                UpdateAbleToReceive(+1);
            }
        }

        private void ExitTrigger(object sender, ChemistryReceiver.OnReceiveElementArgs e)
        {
            if (e._status == IChemistryReceiver.Status.EXIT && !(IsGridElement(e._emitterType)))
            {
                UpdateAbleToReceive(-1);
            }
        }

        private bool IsGridElement(IChemistryEmitter.Type type)
        {
            return type == IChemistryEmitter.Type.GRID_MEMBER || type == IChemistryEmitter.Type.GRID_CONNECTOR;
        }

        private void UpdateAbleToReceive(int addValue)
        {
            gridsPowerNodes[_graphName] += addValue;

            foreach (GraphMember neighbor in grids[_graphName])
            {
                neighbor.AbleToReceive = gridsPowerNodes[_graphName] > 0;
            }
        }
    }
}
