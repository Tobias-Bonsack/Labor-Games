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

        void RemoveEmitType(AChemistryEmitter emitter, IChemistry.ChemistryTypes type);
        void NewEmitType(AChemistryEmitter emitter, IChemistry.ChemistryTypes type);
    }
}
