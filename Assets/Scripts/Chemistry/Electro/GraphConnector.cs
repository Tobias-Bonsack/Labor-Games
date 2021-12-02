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
            _chemistryReceiver = _elementReceiver.ChemistryReceiver;

            _originalGraph = _graphName;

            if (!GraphSystem.graphs.ContainsKey(_graphName)) GraphSystem.graphs.Add(_graphName, new HashSet<GraphMember>());
            if (!GraphSystem.graphs_power_nodes.ContainsKey(_graphName)) GraphSystem.graphs_power_nodes.Add(_graphName, 0);

            GraphSystem.graphs[_graphName].Add(this);
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
                    _neibhbors.Add(emitter.MEMBER);
                    string emitterGraphName = emitter.GRAPH_NAME;

                    if (emitterGraphName.Equals(_graphName)) return;

                    //Valid Fusion
                    GraphSystem graphSystem = new GraphSystem(_graphName + emitterGraphName, new string[] { _graphName, emitterGraphName });
                    LogPowerSources();

                    foreach (GraphMember member in GraphSystem.graphs[_graphName])
                    {
                        member.GraphName = _graphName;
                    }
                    foreach (GraphMember member in GraphSystem.graphs[emitterGraphName])
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
                { // Defusion of graphen
                    GraphMemberEmitter emitter = (GraphMemberEmitter)sender;

                    if (new GraphDFS(this).startDFS(emitter.MEMBER))
                    {
                        _neibhbors.Remove(emitter.MEMBER);
                        Debug.Log("Cycle found to: " + transform.parent.transform.parent.name);
                        return;
                    }
                    _neibhbors.Remove(emitter.MEMBER);

                    string emitterGraphName = emitter.GRPAH_STACK.Peek();
                    if (emitterGraphName.Equals(_graphName)) return;

                    //Valid defusion
                    LogPowerSources();

                    foreach (GraphMember member in GraphSystem.graphs[_graphName])
                    {
                        member.GraphName = member._graphNameStack.Peek();
                    }
                    GraphSystem.DestroySystem(_graphName);
                }
                else
                { // is PowerSource
                    UpdateAbleToReceive(-1);
                }
            }
        }
    }
}