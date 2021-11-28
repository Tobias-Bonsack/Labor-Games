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

        void RemoveEmitType(AbstractEmitter emitter, IChemistry.ChemistryTypes type);
        void NewEmitType(AbstractEmitter emitter, IChemistry.ChemistryTypes type);
    }
}
