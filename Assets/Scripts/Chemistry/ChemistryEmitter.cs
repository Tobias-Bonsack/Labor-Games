using System.Collections.ObjectModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace ChemistryEngine
{
    public class ChemistryEmitter : AbstractEmitter, IChemistryEmitter
    {
        // Standard Emitter

        private void Awake()
        {
            _emitType = IChemistryEmitter.Type.STANDARD;
        }
    }
}
