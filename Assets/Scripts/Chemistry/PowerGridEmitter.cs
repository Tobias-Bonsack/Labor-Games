using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public class PowerGridEmitter : AbstractEmitter
    {
        private void Awake()
        {
            _emitType = IChemistryEmitter.Type.GRID_MEMBER;
        }
    }
}
