using System;
using System.Collections;
using System.Collections.Generic;
using ChemistryEngine;
using UnityEngine;

namespace HeatEngine
{
    public class CoolDown : AbstractProperty
    {
        [Header("Propertie-Parameter")]
        [SerializeField] float _timeBetweenCooldown = 1f;
        [SerializeField, Range(0f, 1f)] float _rateToCooldown = 0.01f;
        private Coroutine _cooldown;

        private void Awake()
        {
            _chemistryReceiver._onReceiveHeat += EnterTrigger;
            _chemistryReceiver._onReceiveHeat += ExitTrigger;
        }

        private void EnterTrigger(object sender, ChemistryReceiver.OnReceiveHeatArgs e)
        {
            if (e._status == IChemistryReceiver.Status.ENTER)
                if (_cooldown != null) StopCoroutine(_cooldown);
        }

        private void ExitTrigger(object sender, ChemistryReceiver.OnReceiveHeatArgs e)
        {
            if (e._status == IChemistryReceiver.Status.EXIT && _heatReceiver._activeTriggers == 0)
            {
                StopAllCoroutines();
                _cooldown = StartCoroutine(CooldownRoutine());
            }
        }

        IEnumerator CooldownRoutine()
        {
            while (_heatReceiver._burnPercent > 0f)
            {
                _heatReceiver._burnPercent -= _rateToCooldown;
                _heatReceiver.OnBurnPercentChangeTrigger();
                yield return new WaitForSeconds(_timeBetweenCooldown);
            }
            _heatReceiver._burnPercent = 0f;
        }
    }
}
