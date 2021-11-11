using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public interface IChemistryEmitter : IChemistry
    {
        void RemoveType(IChemistry.ChemistryTypes type);
        void AddType(IChemistry.ChemistryTypes type, float radiance);

        void RemoveReceiver(IChemistryReceiver receiver);
    }
}
