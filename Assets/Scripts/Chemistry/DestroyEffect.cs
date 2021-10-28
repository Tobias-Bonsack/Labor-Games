using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace ChemistryEngine
{
    [RequireComponent(typeof(VisualEffect))]
    public class DestroyEffect : MonoBehaviour
    {
        private VisualEffect _visualEffect;

        private void Awake()
        {
            _visualEffect = GetComponent<VisualEffect>();
            StartCoroutine(WaitToDestroy(_visualEffect.GetFloat("Lifetime B")));
        }

        IEnumerator WaitToDestroy(float maxLifespan)
        {
            yield return new WaitForSeconds(maxLifespan);
            Destroy(gameObject);
        }
    }
}
