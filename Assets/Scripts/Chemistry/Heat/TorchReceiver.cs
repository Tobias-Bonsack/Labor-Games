using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace ChemistryEngine
{
    public class TorchReceiver : AbstractHeatReceiver
    {
        [SerializeField] GameObject _emitter;

        protected override void ExtendHeatTriggerEnter(Collider other)
        {
            _visualEffect.enabled = true;
        }

        protected override void ExtendHeatTriggerStay(Collider other)
        {
            if (_burnPercent == 1f)
            {
                _emitter.SetActive(true);
                Destroy(gameObject);
            }
        }
        protected override void ExtendHeatTriggerExit(Collider other)
        {
            _visualEffect.enabled = false;
        }

    }
}
