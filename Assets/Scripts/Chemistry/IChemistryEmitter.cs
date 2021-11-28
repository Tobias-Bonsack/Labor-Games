using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public interface IChemistryEmitter : IChemistry
    {
        public enum Type
        {
            STANDARD,
            GRID_MEMBER,
            GRID_CONNECTOR
        }
        void RemoveType(IChemistry.ChemistryTypes type, float radiance);
        void AddType(IChemistry.ChemistryTypes type, float radiance);

        void RemoveReceiver(IChemistryReceiver receiver);
    }
}
