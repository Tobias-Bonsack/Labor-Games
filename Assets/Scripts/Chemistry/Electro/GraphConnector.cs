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

            new GraphSystem(_originalGraph, new string[0]);
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

        new protected void EnterTrigger(object sender, ChemistryReceiver.OnReceiveElementArgs e)
        {
            if (e._status == IChemistryReceiver.Status.ENTER)
            {
                if (sender is GraphMemberEmitter)
                { // Fusion of graphen
                    GraphMemberEmitter emitter = (GraphMemberEmitter)sender;
                    _neibhbors.Add(emitter.MEMBER);
                    string emitterGraphName = emitter.GRAPH_NAME;
                    string graphName = GraphName;

                    if (emitterGraphName.Equals(graphName)) return;

                    //Valid Fusion
                    GraphSystem graphSystem = new GraphSystem(graphName + emitterGraphName, new string[] { graphName, emitterGraphName });

                    foreach (string child in graphSystem._children)
                    {
                        foreach (GraphMember member in GraphSystem.graphs[child])
                        {
                            GraphSystem.graphs[graphSystem._name].Add(member);
                        }
                    }
                    UpdateAbleToReceive(0);
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
                    string graphName = GraphName;
                    List<string> children = GraphSystem.graphSystems[graphName]._children;

                    if (new GraphDFS(this).startDFS(emitter.MEMBER))
                    { // there is an cycle
                        string toDestroyGraph = "";
                        foreach (string child in children)
                        {
                            if (child.Contains(_originalGraph) && !child.Equals(_originalGraph))
                            { // if child contains, but is not exact the same as the original one
                                toDestroyGraph = child;
                                break;
                            }
                        }
                        if (toDestroyGraph.Length != 0) GraphSystem.DestroySystem(toDestroyGraph);
                        _neibhbors.Remove(emitter.MEMBER);
                        return;
                    }

                    // no cycle found
                    _neibhbors.Remove(emitter.MEMBER);
                    if (children.Contains(graphName)) return;
                    //Valid defusion
                    GraphSystem.DestroySystem(graphName);
                    UpdateAbleToReceive(0);
                }
                else
                { // is PowerSource
                    UpdateAbleToReceive(-1);
                }
            }
        }
    }
}