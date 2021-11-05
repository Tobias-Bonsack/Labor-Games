using System;
using System.Collections;
using System.Collections.Generic;
using ChemistryEngine;
using UnityEngine;
using UnityEngine.VFX;

namespace ChemistryEngine
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

            switch (_type)
            {
                case IChemistry.ChemistryTypes.HEAT:
                    _chemistryReceiver._onReceiveHeat += StayTrigger;
                    break;
                case IChemistry.ChemistryTypes.COLD:
                    _chemistryReceiver._onReceiveFrost += StayTrigger;
                    break;
                default:
                    break;
            }
        }

        private void StayTrigger(object sender, ChemistryReceiver.OnReceiveElementArgs e)
        {
            if (e._status == IChemistryReceiver.Status.STAY && _elementReceiver._elementPercent >= _pointToDestroy)
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
            switch (_type)
            {
                case IChemistry.ChemistryTypes.HEAT:
                    _chemistryReceiver._onReceiveHeat -= StayTrigger;
                    break;
                case IChemistry.ChemistryTypes.COLD:
                    _chemistryReceiver._onReceiveFrost -= StayTrigger;
                    break;
                default:
                    break;
            }
        }
    }
}
