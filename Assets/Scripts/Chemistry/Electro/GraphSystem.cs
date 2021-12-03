using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public class GraphSystem
    {
        public static readonly Dictionary<string, Graph> baseGraphen = new Dictionary<string, Graph>();
        public static readonly Dictionary<string, Graph> combineGraphen = new Dictionary<string, Graph>();

        public static void AddBaseGraph(string name)
        {
            if (baseGraphen.ContainsKey(name)) return;
            baseGraphen.Add(name, new Graph(name));
            graphs.Add(name, new HashSet<GraphMember>());
        }
        public static void AddCombineGraph(string[] couple)
        {
            string combinedName = couple[0] + ":" + couple[1];
            if (combineGraphen.ContainsKey(combinedName)) return;

            combineGraphen.Add(combinedName, new Graph(combinedName));

            foreach (string item in combineGraphen.Keys)
            {
                UpdatePowerLevel(item);
            }
            UpdateAllAbleToReceive();
        }
        public static void RemoveCombineGraph(string combineName)
        {
            combineGraphen.Remove(combineName);
            foreach (string item in combineGraphen.Keys)
            {
                UpdatePowerLevel(item);
            }
            UpdateAllAbleToReceive();
        }

        private static void UpdatePowerLevel(string combineName)
        {
            List<string> visited = new List<string>();
            combineGraphen[combineName].PowerLevel = CalculatePowerLevel(visited, combineName);
        }

        private static int CalculatePowerLevel(List<string> visited, string combinedGraph)
        {
            int result = 0;
            visited.Add(combinedGraph);

            string[] vs = combinedGraph.Split(':');
            foreach (string baseGraph in vs)
            {
                if (!visited.Contains(baseGraph))
                {
                    visited.Add(baseGraph);
                    result += baseGraphen[baseGraph].PowerLevel;
                    List<string> cGs = GetCombinedGraphen(baseGraph);
                    foreach (string cG in cGs)
                    {
                        if (!visited.Contains(cG))
                        {
                            result += CalculatePowerLevel(visited, cG);
                        }
                    }
                }
            }
            return result;
        }
        private static void UpdateAllAbleToReceive()
        {
            foreach (string key in baseGraphen.Keys)
            {
                UpdateAbleToReceive(key);
            }
        }
        private static void UpdateAbleToReceive(string baseName)
        {
            foreach (GraphMember member in graphs[baseName])
            {
                bool ableToReceive = false;
                if (baseGraphen[baseName].PowerLevel > 0) ableToReceive = true;
                else
                {
                    foreach (string cG in GetCombinedGraphen(baseName))
                    {
                        if (combineGraphen[cG].PowerLevel > 0) ableToReceive = true;
                    }
                }
                member.AbleToReceive = ableToReceive;
            }
        }

        private static List<string> GetCombinedGraphen(string baseGraph)
        {
            List<string> result = new List<string>();

            foreach (string key in combineGraphen.Keys)
            {
                if (key.Contains(baseGraph)) result.Add(key);
            }

            return result;
        }

        public static void AddPowerSource(string baseName, int addValue)
        {
            baseGraphen[baseName].PowerLevel += addValue;
            foreach (string item in combineGraphen.Keys)
            {
                UpdatePowerLevel(item);
            }
            UpdateAllAbleToReceive();
        }

        //OLD ones
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
