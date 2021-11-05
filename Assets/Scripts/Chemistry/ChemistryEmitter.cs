using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace ChemistryEngine
{
    [RequireComponent(typeof(Collider))]
    public class ChemistryEmitter : MonoBehaviour, IChemistryEmitter
    {

        [Header("Emit Parameter")]

        [Tooltip("Types of chemistry this GameObject is able to emit, each type needs an _radiance float")]
        public List<IChemistry.ChemistryTypes> _types;

        [Tooltip("Radiance per second, Position mirrors _types"), Range(0f, 1f)]
        public List<float> _radiance;
    }
}
