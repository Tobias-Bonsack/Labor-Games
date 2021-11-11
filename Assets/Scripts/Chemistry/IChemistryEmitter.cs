using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public interface IChemistryEmitter : IChemistry
    {
        public void RemoveType(IChemistry.ChemistryTypes type);
        public void AddType(IChemistry.ChemistryTypes type, float radiance);

        public void RemoveReceiver(IChemistryReceiver receiver);
    }
}
