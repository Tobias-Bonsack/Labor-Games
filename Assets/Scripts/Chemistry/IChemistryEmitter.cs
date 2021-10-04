using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public interface IChemistryEmitter<T> : IChemistry
    {
        void Emit(T[] chemistryType);
    }
}
