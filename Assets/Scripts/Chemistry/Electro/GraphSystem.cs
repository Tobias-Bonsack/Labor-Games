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

        public string _name, _parent;
        public int _powerLevel = 0;
        public List<string> _children = new List<string>();


        public GraphSystem(string name, string[] children)
        {
            _name = name;
            _children.AddRange(children);
            if (graphSystems.ContainsKey(_name)) return;
            graphSystems.Add(name, this);
            graphs.Add(_name, new HashSet<GraphMember>());

            foreach (string child in _children)
            {
                _powerLevel += graphSystems[child]._powerLevel;
                graphSystems[child]._parent = _name;
            }
        }

        internal void UpdatePowerLevel(int addValue)
        {
            _powerLevel += addValue;

            if (_parent != null) graphSystems[_parent].UpdatePowerLevel(addValue);
        }

        internal static void DestroySystem(string graphName)
        {
            foreach (string child in graphSystems[graphName]._children)
            {
                string parent = graphSystems[graphName]._parent;
                graphSystems[child]._parent = parent;
                if (parent != null) graphSystems[parent]._children.Add(child);
            }
            graphSystems[graphName]._children.Remove(graphName);

            graphSystems.Remove(graphName);
            graphs.Remove(graphName);
        }

        internal static string FindCurrentGraphName(GraphMember member)
        {
            foreach (string key in graphs.Keys)
            {
                if (graphs[key].Contains(member))
                {
                    return graphSystems[key].getEndName();
                }

            }
            return null;
        }

        private string getEndName()
        {
            if (_parent == null) return _name;
            return graphSystems[_parent].getEndName();
        }
    }
}
