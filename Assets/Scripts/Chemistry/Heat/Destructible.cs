using System;
using System.Collections;
using System.Collections.Generic;
using ChemistryEngine;
using UnityEngine;
using UnityEngine.VFX;

namespace HeatEngine
{

    [RequireComponent(typeof(VisualEffect))]
    public class Destructible : AbstractProperty
    {
        [Header("Parameter")]
        [SerializeField] GameObject _objectToDestroy;
        [SerializeField, Range(0f, 1f)] float _pointToDestroy = 1f;
        private VisualEffect _visualEffect;

        private void Awake()
        {
            _visualEffect = GetComponent<VisualEffect>();
            _chemistryReceiver._onReceiveHeat += StayTrigger;
        }

        private void StayTrigger(object sender, ChemistryReceiver.OnReceiveHeatArgs e)
        {
            if (e._status == IChemistryReceiver.Status.STAY && _heatReceiver._burnPercent >= _pointToDestroy)
            {
                transform.parent = null;
                StartCoroutine(WaitToDestroy(_visualEffect.GetFloat("Lifetime B")));
                StartCoroutine(MoveBevoreDestroy());
            }
        }

        private IEnumerator MoveBevoreDestroy()
        {
            _objectToDestroy.transform.position -= new Vector3(0f, -1000f, 0f);
            yield return new WaitForSecondsRealtime(0.1f);
            Destroy(_objectToDestroy);
        }

        IEnumerator WaitToDestroy(float maxLifespan)
        {
            _visualEffect.enabled = true;
            yield return new WaitForSeconds(maxLifespan);
            Destroy(gameObject);
        }

        void OnDestroy()
        {
            _chemistryReceiver._onReceiveHeat -= StayTrigger;
        }
    }
}
