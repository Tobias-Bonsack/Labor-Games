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
            _originalGraph = _graphName;

            if (!GRAPHS.ContainsKey(_graphName)) GRAPHS.Add(_graphName, new HashSet<GraphMember>());
            if (!GRAPHS_POWER_NODES.ContainsKey(_graphName)) GRAPHS_POWER_NODES.Add(_graphName, 0);

            GRAPHS[_graphName].Add(this);
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
            if (e._status == IChemistryReceiver.Status.ENTER)
            {
                if (sender is GraphMemberEmitter)
                { // Fusion of graphen
                    GraphMemberEmitter emitter = (GraphMemberEmitter)sender;
                    string emitterGraphName = emitter.GRAPH_NAME;

                    GRAPHS_POWER_NODES[_graphName] += GRAPHS_POWER_NODES[emitterGraphName];
                    foreach (GraphMember member in GRAPHS[_graphName])
                    {
                        member.AbleToReceive = GRAPHS_POWER_NODES[_graphName] > 0;
                    }
                    foreach (GraphMember member in GRAPHS[emitterGraphName])
                    {
                        member.GraphName = _graphName;
                    }
                }
                else
                { // is PowerSource
                    UpdateAbleToReceive(+1);
                }
            }
        }

        new protected void ExitTrigger(object sender, ChemistryReceiver.OnReceiveElementArgs e)
        {
            if (e._status == IChemistryReceiver.Status.EXIT)
            {
                if (sender is GraphMemberEmitter)
                { // Fusion of graphen
                    GraphMemberEmitter emitter = (GraphMemberEmitter)sender;
                    string emitterGraphName = emitter.GRPAH_STACK.Peek();

                    GRAPHS_POWER_NODES[_graphName] -= GRAPHS_POWER_NODES[emitterGraphName];
                    foreach (GraphMember member in GRAPHS[emitterGraphName])
                    {
                        member.GraphName = emitterGraphName;
                    }
                    foreach (GraphMember member in GRAPHS[_graphName])
                    {
                        member.AbleToReceive = GRAPHS_POWER_NODES[_graphName] > 0;
                    }
                }
                else
                { // is PowerSource
                    UpdateAbleToReceive(-1);
                }
            }
        }
    }
}