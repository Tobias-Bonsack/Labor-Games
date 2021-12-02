using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public class GraphDFS
    {
        private List<GraphMember> _visited = new List<GraphMember>();
        private GraphMember _origin;

        public GraphDFS(GraphMember origin)
        {
            _origin = origin;
            _visited.Add(_origin);
        }

        public bool startDFS(GraphMember direction)
        {
            return dfs(_origin, direction);
        }

        private bool dfs(GraphMember parent, GraphMember currentMember)
        {
            _visited.Add(currentMember);

            foreach (GraphMember neibhor in currentMember._neibhbors)
            {
                if (!_visited.Contains(neibhor))
                {
                    if (dfs(currentMember, neibhor)) return true;
                }
                else if (parent != _origin && neibhor == _origin) return true;
            }
            return false;
        }
    }
}
