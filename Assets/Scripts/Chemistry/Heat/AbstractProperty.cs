using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeatEngine
{
    public abstract class AbstractProperty : MonoBehaviour
    {
        [Header("Generally Parameter")]
        [SerializeField] protected ChemistryEngine.ChemistryReceiver _chemistryReceiver;
        [SerializeField] protected Receiver _heatReceiver;
    }
}
