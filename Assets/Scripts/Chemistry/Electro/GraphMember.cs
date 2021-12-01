using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public class GraphMember : AbstractProperty
    {

        public static readonly Dictionary<string, HashSet<GraphMember>> GRAPHS = new Dictionary<string, HashSet<GraphMember>>();
        public static readonly Dictionary<string, int> GRAPHS_POWER_NODES = new Dictionary<string, int>();

        [HideInNormalInspector] public Stack<string> _graphNameStack = new Stack<string>();
        [HideInNormalInspector] public string _originalGraph;
        [SerializeField] protected string _graphName;
        public string GraphName
        {
            get
            {
                return _graphName;
            }
            set
            {
                if (_graphNameStack.Count != 0 && _graphNameStack.Peek().Equals(value))
                { // set to last graph
                    _graphNameStack.Pop();
                    GRAPHS[_graphName].Remove(this);
                    AbleToReceive = GRAPHS_POWER_NODES[value] > 0;

                    _graphName = value;
                }
                else
                { // new value
                    _graphNameStack.Push(_graphName);
                    GRAPHS[value].Add(this);
                    AbleToReceive = GRAPHS_POWER_NODES[value] > 0;

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

        protected void UpdateAbleToReceive(int addValue)
        {
            if (!_originalGraph.Equals(_graphName)) GRAPHS_POWER_NODES[_originalGraph] += addValue;
            GRAPHS_POWER_NODES[_graphName] += addValue;

            foreach (GraphMember neighbor in GRAPHS[_graphName])
            {
                neighbor.AbleToReceive = GRAPHS_POWER_NODES[_graphName] > 0;
            }
        }
    }
}
