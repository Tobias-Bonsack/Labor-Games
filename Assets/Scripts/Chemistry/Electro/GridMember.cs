using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public class GridMember : AbstractProperty
    {

        public static Dictionary<string, HashSet<GridMember>> grids = new Dictionary<string, HashSet<GridMember>>();
        public static Dictionary<string, int> gridsPowerNodes = new Dictionary<string, int>();

        [SerializeField] string _gridName;
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
            if (!grids.ContainsKey(_gridName)) grids.Add(_gridName, new HashSet<GridMember>());
            if (!gridsPowerNodes.ContainsKey(_gridName)) gridsPowerNodes.Add(_gridName, 0);

            grids[_gridName].Add(this);
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
            if (e._status == IChemistryReceiver.Status.ENTER)
            {
                if (IsGridElement(e._emitterType)) return;
                UpdateAbleToReceive(+1);
            }
        }

        private void ExitTrigger(object sender, ChemistryReceiver.OnReceiveElementArgs e)
        {
            if (e._status == IChemistryReceiver.Status.EXIT)
            {
                if (IsGridElement(e._emitterType)) return;
                UpdateAbleToReceive(-1);
            }
        }

        private bool IsGridElement(IChemistryEmitter.Type type)
        {
            return type == IChemistryEmitter.Type.GRID_MEMBER || type == IChemistryEmitter.Type.GRID_CONNECTOR;
        }

        private void UpdateAbleToReceive(int addValue)
        {
            gridsPowerNodes[_gridName] += addValue;

            foreach (GridMember neighbor in grids[_gridName])
            {
                neighbor.AbleToReceive = gridsPowerNodes[_gridName] > 0;
            }
        }
    }
}
