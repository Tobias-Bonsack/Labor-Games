using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public abstract class AbstractProperty : MonoBehaviour
    {
        [Header("Generally Parameter")]
        [SerializeField] protected IChemistry.ChemistryTypes _type;
        [SerializeField] protected ChemistryReceiver _chemistryReceiver;
        [SerializeField] protected AbstractReceiver _elementReceiver;
    }
}
