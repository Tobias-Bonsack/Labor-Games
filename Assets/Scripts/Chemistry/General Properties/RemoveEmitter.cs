using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public class RemoveEmitter : AbstractProperty
    {
        [Header("Propertie-Parameter")]
        [SerializeField] ChemistryEmitter _emitter;
        [SerializeField, Range(0f, 1f)] float _percentToRemove;
        [SerializeField] bool _destroyAfterRemove = false;
        [SerializeField] GameObject _toDestroy;
        [SerializeField, Range(0f, 1f)] float _afterRemovePercentToDestroy;
        [SerializeField] GameObject _reducer;
        bool _isElementRemoved = false;
        private void Awake()
        {
            _elementReceiver._onElementPercentChange += ElementPercentChange;
        }

        private void ElementPercentChange(object sender, EventArgs e)
        {
            if (!_isElementRemoved && _elementReceiver.ElementPercent >= _percentToRemove)
            {
                _emitter.RemoveType(_type);
                _isElementRemoved = true;
                _reducer.SetActive(true);
            }

            if (_destroyAfterRemove && _isElementRemoved && _elementReceiver.ElementPercent <= _afterRemovePercentToDestroy)
            {
                Destroy(_toDestroy);
            }
        }
    }
}
