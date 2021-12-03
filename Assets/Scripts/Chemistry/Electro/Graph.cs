using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public class Graph
    {
        public string _name;
        private int _powerLevel = 0;
        public int PowerLevel
        {
            get
            {
                return _powerLevel;
            }
            set
            {
                _powerLevel = value;
                Debug.Log("-------------");
                foreach (string item in GraphSystem.baseGraphen.Keys)
                {
                    Debug.Log(item + ": " + GraphSystem.baseGraphen[item]._powerLevel);
                }
                foreach (string item in GraphSystem.combineGraphen.Keys)
                {
                    Debug.Log(item + ": " + GraphSystem.combineGraphen[item]._powerLevel);
                }
            }
        }

        public Graph(string name)
        {
            _name = name;
        }
    }
}
