using System;
using System.Collections;
using System.Collections.Generic;
using ChemistryEngine;
using UnityEngine;

namespace HeatEngine
{
    public class ChangeToEmitter : AbstractProperty
    {
        [Header("Parameter")]
        [SerializeField] GameObject _emitter;
        [SerializeField] GameObject _receiver;
        [SerializeField, Range(0f, 1f)] float _pointToChange;
        [SerializeField] bool _receiveRemeins;
        [SerializeField, Range(0f, 2f)] float _multiplierForSusceptibility = 1f;

        private bool _isTriggert = false;

        void Awake()
        {
            _chemistryReceiver._onReceiveHeat += StayTrigger;
            _heatReceiver._onBurnPercentChange += OnBurnPercentChange;
        }

        private void StayTrigger(object sender, ChemistryReceiver.OnReceiveElementArgs e)
        {
            if (e._status == ChemistryEngine.IChemistryReceiver.Status.STAY)
            {
                OnBurnPercentChange(null, null);
            }
        }

        private void OnBurnPercentChange(object sender, EventArgs e)
        {
            if (!_isTriggert && _heatReceiver._burnPercent >= _pointToChange)
            {
                _isTriggert = true;
                _emitter.SetActive(true);
                _heatReceiver.MultiplieSusceptibility(_multiplierForSusceptibility);
                _receiver.SetActive(_receiveRemeins);
            }
            else if (_isTriggert && _heatReceiver._burnPercent <= _pointToChange)
            {
                _isTriggert = false;
                _emitter.SetActive(false);
                _heatReceiver.MultiplieSusceptibility(1f / _multiplierForSusceptibility);
                _receiver.SetActive(!_receiveRemeins);
            }
        }
    }
}
