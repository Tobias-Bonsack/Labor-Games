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

        public void RemovedEmitType(ChemistryEmitter emitter, IChemistry.ChemistryTypes type);
        public void NewEmitType(ChemistryEmitter emitter, IChemistry.ChemistryTypes type);
    }
}
