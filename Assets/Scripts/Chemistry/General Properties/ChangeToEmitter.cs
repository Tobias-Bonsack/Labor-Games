using System;
using System.Collections;
using System.Collections.Generic;
using ChemistryEngine;
using UnityEngine;

namespace ChemistryEngine
{
    public class ChangeToEmitter : AbstractProperty
    {
        [Header("Parameter")]
        [SerializeField] ChemistryEmitter _emitter;
        [SerializeField, Range(0f, 1f)] float _pointToChange;
        [SerializeField] bool _receiveRemeins;
        [SerializeField, Range(0f, 2f)] float _multiplierForSusceptibility = 1f;
        [SerializeField, Range(0f, 1f)] float _radiance;

        private Coroutine _queue;
        private bool _isEmitter = false;
        private int _numberOfChanges = 0;

        void Awake()
        {
            _elementReceiver._onElementPercentChange += OnBurnPercentChange;
        }

        private void OnBurnPercentChange(object sender, EventArgs e)
        {
            _numberOfChanges++;
            if (_queue == null)
            {
                _queue = StartCoroutine(CheckPercentChange());
            }
        }

        private IEnumerator CheckPercentChange()
        {
            while (_numberOfChanges-- > 0)
            {
                yield return new WaitForSeconds(Time.fixedDeltaTime * 2);
                if (!_isEmitter && _elementReceiver._elementPercent >= _pointToChange)
                {
                    _isEmitter = true;
                    ChangeEmitter(true);
                    _elementReceiver.MultiplieSusceptibility(_multiplierForSusceptibility);
                    _elementReceiver.gameObject.SetActive(_receiveRemeins);
                }
                else if (_isEmitter && _elementReceiver._elementPercent <= _pointToChange)
                {
                    _isEmitter = false;
                    ChangeEmitter(false);
                    _elementReceiver.MultiplieSusceptibility(1f / _multiplierForSusceptibility);
                    _elementReceiver.gameObject.SetActive(!_receiveRemeins);
                }
            }
            _queue = null;
        }

        private void ChangeEmitter(bool isAdd)
        {
            if (isAdd)
            {
                _emitter.gameObject.SetActive(true);
                int position = _emitter._types.IndexOf(_type);
                if (position == -1)
                {
                    _emitter._types.Add(_type);
                    _emitter._radiance.Add(_radiance);
                }
                else
                {
                    _emitter._radiance[position] += _radiance;
                }
            }
            else
            {
                int position = _emitter._types.IndexOf(_type);
                if (position != -1)
                {
                    _emitter._types.Remove(_type);
                    _emitter._radiance.RemoveAt(position);
                }
                _emitter.gameObject.SetActive(_emitter._types.Count != 0);
            }
        }
    }
}
