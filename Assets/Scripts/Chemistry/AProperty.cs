using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public abstract class AProperty : MonoBehaviour
    {
        [Header("Generally Parameter")]
        [SerializeField] protected IChemistry.ChemistryTypes _type;
        [SerializeField] protected ChemistryReceiver _chemistryReceiver;
        [SerializeField] protected AElementReceiver _elementReceiver;
    }
}
