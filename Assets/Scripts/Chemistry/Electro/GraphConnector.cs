using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public class GraphConnector : GraphMember
    {
        private void Awake()
        {
            _currentGraphName = _originalGraphName;

            if (!grids.ContainsKey(_currentGraphName)) grids.Add(_currentGraphName, new HashSet<GraphMember>());
            if (!gridsPowerNodes.ContainsKey(_currentGraphName)) gridsPowerNodes.Add(_currentGraphName, 0);

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

        new protected void EnterTrigger(object sender, ChemistryReceiver.OnReceiveElementArgs e)
        {
            throw new NotImplementedException();
        }

        new protected void ExitTrigger(object sender, ChemistryReceiver.OnReceiveElementArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
