using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public class ChemistryEmitter : MonoBehaviour, IChemistryEmitter
    {

        [Header("Emit Parameter")]

        [Tooltip("Types of chemistry this GameObject is able to emit, each type needs an _radiance float")]
        public IChemistry.ChemistryTypes[] _types;

        [Tooltip("Radiance per second, Position mirrors _types")]
        [Range(0f, 1f)]
        public float[] _radiance;
    }
}