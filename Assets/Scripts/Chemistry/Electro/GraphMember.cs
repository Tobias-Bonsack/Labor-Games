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

        [SerializeField] protected string _originalGraphName;
        protected string _currentGraphName;
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
            _currentGraphName = _originalGraphName;

            if (!grids.ContainsKey(_currentGraphName)) grids.Add(_currentGraphName, new HashSet<GraphMember>());
            if (!gridsPowerNodes.ContainsKey(_currentGraphName)) gridsPowerNodes.Add(_currentGraphName, 0);

            _gridEmitter._graphName = _currentGraphName;

            grids[_currentGraphName].Add(this);
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
            }
        }

        protected void ExitTrigger(object sender, ChemistryReceiver.OnReceiveElementArgs e)
        {
            if (e._status == IChemistryReceiver.Status.EXIT && e._emitterType != IChemistryEmitter.Type.GRID_MEMBER)
            {
                UpdateAbleToReceive(-1);
            }
        }

        private void UpdateAbleToReceive(int addValue)
        {
            if (_originalGraphName != _currentGraphName) gridsPowerNodes[_originalGraphName] += addValue;
            gridsPowerNodes[_currentGraphName] += addValue;

            foreach (GraphMember neighbor in grids[_currentGraphName])
            {
                neighbor.AbleToReceive = gridsPowerNodes[_currentGraphName] > 0;
            }
        }
    }
}
