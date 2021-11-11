using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public interface IChemistryReceiver : IChemistry
    {
        public enum Status
        {
            ENTER,
            STAY,
            EXIT
        }

        void RemoveEmitType(ChemistryEmitter emitter, IChemistry.ChemistryTypes type);
        void NewEmitType(ChemistryEmitter emitter, IChemistry.ChemistryTypes type);
    }
}
