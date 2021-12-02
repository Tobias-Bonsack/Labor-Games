using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public class GraphMember : AProperty
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
        protected override void Awake()
        {
            base.Awake();

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
            if (_graphNameStack.Count != 0)
            {
                foreach (string item in _graphNameStack)
                {
                    GRAPHS_POWER_NODES[item] += addValue;
                }
            }
            GRAPHS_POWER_NODES[_graphName] += addValue;

            LogPowerSources();

            foreach (GraphMember neighbor in GRAPHS[_graphName])
            {
                neighbor.AbleToReceive = GRAPHS_POWER_NODES[_graphName] > 0;
            }
        }

        protected static void LogPowerSources()
        {
            foreach (KeyValuePair<string, int> item in GRAPHS_POWER_NODES)
            {
                Debug.Log(item);
            }
            Debug.Log("-----");
        }
    }
}
