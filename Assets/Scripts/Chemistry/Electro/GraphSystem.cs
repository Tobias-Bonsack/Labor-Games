using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public class GraphSystem
    {
        public static readonly Dictionary<string, GraphSystem> graphSystems = new Dictionary<string, GraphSystem>();
        public static readonly Dictionary<string, HashSet<GraphMember>> graphs = new Dictionary<string, HashSet<GraphMember>>();
        public static readonly Dictionary<string, int> graphs_power_nodes = new Dictionary<string, int>();

        public string _name, _parent;
        public int _powerLevel = 0;
        public string[] _children;


        public GraphSystem(string name, string[] children)
        {
            _name = name;
            _children = children;
            graphSystems.Add(name, this);
            graphs.Add(_name, new HashSet<GraphMember>());
            graphs_power_nodes.Add(_name, 0);

            foreach (string child in _children)
            {
                _powerLevel += graphSystems[child]._powerLevel;
                graphSystems[child]._parent = _name;
            }
        }

        internal void UpdatePowerLevel(int addValue)
        {
            //TODO sich selbst zurechnen und dann seinem parent per recursion
        }

        internal static void DestroySystem(string graphName)
        {
            foreach (string child in graphSystems[graphName]._children)
            {
                graphSystems[child]._parent = null;
            }

            graphSystems.Remove(graphName);
            graphs.Remove(graphName);
            graphs_power_nodes.Remove(graphName);
        }
    }
}
