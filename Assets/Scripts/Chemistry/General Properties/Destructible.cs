using System;
using System.Collections;
using System.Collections.Generic;
using ChemistryEngine;
using UnityEngine;
using UnityEngine.VFX;

namespace ChemistryEngine
{

    [RequireComponent(typeof(VisualEffect))]
    public class Destructible : AProperty
    {
        [Header("Parameter")]
        [SerializeField] GameObject _objectToDestroy;
        [SerializeField, Range(0f, 1f)] float _pointToDestroy = 1f;
        private VisualEffect _visualEffect;

        protected override void Awake()
        {
            base.Awake();
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
            if (e._status == IChemistryReceiver.Status.STAY && _elementReceiver.ElementPercent >= _pointToDestroy)
            {
                transform.parent = null;
                Destroy(_objectToDestroy);
                StartCoroutine(WaitToDestroy(_visualEffect.GetFloat("Lifetime B")));
            }
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
