using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public class GraphMemberEmitter : AbstractEmitter
    {
        [HideInInspector] public string _graphName;
        private void Awake()
        {
            _emitType = IChemistryEmitter.Type.GRID_MEMBER;
        }
    }
}
