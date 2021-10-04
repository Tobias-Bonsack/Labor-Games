using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public interface IChemistryReceiver : IChemistry
    {
        void receiv(IChemistry.ChemistryTypes[] chemistryType);
    }
}
