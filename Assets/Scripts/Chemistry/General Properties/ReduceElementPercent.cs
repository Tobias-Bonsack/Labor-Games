using System;
using System.Collections;
using System.Collections.Generic;
using ChemistryEngine;
using UnityEngine;

namespace ChemistryEngine
{
    public class ReduceElementPercent : AbstractProperty
    {
        [Header("Propertie-Parameter")]
        [SerializeField] IChemistry.ChemistryTypes _weaknessType;
        [SerializeField, Range(0f, 2f)] float _timeMultiplier = 0.5f;
        [SerializeField] float _timeBetweenCooldown = 1f;
        [SerializeField, Range(0f, 1f)] float _rateToCooldown = 0.01f;
        private Coroutine _cooldown;

        private void Awake()
        {
            _elementReceiver._onActiveTriggerChange += TriggerChange;

            switch (_weaknessType)
            {
                case IChemistry.ChemistryTypes.HEAT:
                    _chemistryReceiver._onReceiveHeat += WeaknesEnterTrigger;
                    _chemistryReceiver._onReceiveHeat += WeaknesExitTrigger;
                    break;
                default:
                    break;
            }
        }

        private void TriggerChange(object sender, EventArgs e)
        {
            if (_elementReceiver._activeTriggers != 0)
            {
                if (_cooldown != null) StopCoroutine(_cooldown);
            }
            else
            {
                StopAllCoroutines();
                _cooldown = StartCoroutine(CooldownRoutine());
            }
        }

        #region weaknes mehtods
        private void WeaknesEnterTrigger(object sender, ChemistryReceiver.OnReceiveElementArgs e)
        {
            if (e._status == IChemistryReceiver.Status.ENTER) _timeBetweenCooldown *= _timeMultiplier;
        }

        private void WeaknesExitTrigger(object sender, ChemistryReceiver.OnReceiveElementArgs e)
        {
            if (e._status == IChemistryReceiver.Status.EXIT) _timeBetweenCooldown /= _timeMultiplier;
        }
        #endregion

        IEnumerator CooldownRoutine()
        {
            while (_elementReceiver._elementPercent > 0f)
            {
                _elementReceiver._elementPercent -= _rateToCooldown;
                _elementReceiver.OnElementPercentChangeTrigger();
                yield return new WaitForSeconds(_timeBetweenCooldown);
            }
            _elementReceiver._elementPercent = 0f;
        }
    }
}