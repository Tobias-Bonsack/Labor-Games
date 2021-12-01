using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public class GraphMemberEmitter : AbstractEmitter
    {
        [SerializeField] GraphMember _member;
        public Queue<string> GRPAH_QUEUE
        {
            get
            {
                return _member._graphNameQueue;
            }
        }
        public string GRAPH_NAME
        {
            get
            {
                return _member.GraphName;
            }
        }
        private void Awake()
        {
            _emitType = IChemistryEmitter.Type.GRID_MEMBER;
        }
    }
}
