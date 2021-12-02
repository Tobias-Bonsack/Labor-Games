using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public class GraphMember : AProperty
    {
        [HideInNormalInspector] public Stack<string> _graphNameStack = new Stack<string>();
        [HideInNormalInspector] public string _originalGraph;
        [SerializeField] protected string _graphName;
        [HideInNormalInspector] public List<GraphMember> _neibhbors = new List<GraphMember>();
        public string GraphName
        {
            get
            {
                return _graphName;
            }
            set
            {
                if (_graphNameStack.Peek().Equals(value))
                { // set to last graph
                    _graphNameStack.Pop();
                    AbleToReceive = GraphSystem.graphs_power_nodes[value] > 0;

                    _graphName = value;
                }
                else
                { // new value
                    _graphNameStack.Push(_graphName);
                    GraphSystem.graphs[value].Add(this);
                    AbleToReceive = GraphSystem.graphs_power_nodes[value] > 0;

                    _graphName = value;
                }
            }
        }
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
            if (!_originalGraph.Equals(_graphName)) GraphSystem.graphs_power_nodes[_originalGraph] += addValue;
            GraphSystem.graphs_power_nodes[_graphName] += addValue;

            LogPowerSources();

            foreach (GraphMember neighbor in GraphSystem.graphs[_graphName])
            {
                neighbor.AbleToReceive = GraphSystem.graphs_power_nodes[_graphName] > 0;
            }
        }

        protected static void LogPowerSources()
        {
            foreach (KeyValuePair<string, int> item in GraphSystem.graphs_power_nodes)
            {
                Debug.Log(item);
            }
            Debug.Log("-----");
        }
    }
}
