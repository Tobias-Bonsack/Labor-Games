using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public class TransformChange : AbstractProperty
    {
        [SerializeField] Transform _toChange;
        [SerializeField, Range(0f, 1f)] float _pointToStartChagne = 0f, _pointToEndChange = 1f;

        [Header("Position")]
        [SerializeField] bool _changePosition;
        private Vector3 _originPosition;
        [SerializeField] Vector3 _addPosition;

        [Header("Rotation")]
        [SerializeField] bool _changeRotation;
        private Vector3 _originRotation;
        [SerializeField] Vector3 _addRotation;

        [Header("Scale")]
        [SerializeField] bool _changeScale;
        private Vector3 _originScale;
        [SerializeField] Vector3 _addScale;

        void Awake()
        {
            _originPosition = _toChange.position;
            _originRotation = _toChange.rotation.eulerAngles;
            _originScale = _toChange.localScale;

            _elementReceiver._onElementPercentChange += ChangeScale;
        }

        private void ChangeScale(object sender, EventArgs e)
        {
            float range = _pointToEndChange - _pointToStartChagne;
            float changePercent = Mathf.Clamp(_elementReceiver.ElementPercent - _pointToStartChagne, 0f, range) / range;
            if (_changePosition) _toChange.position = _originPosition + _addPosition * changePercent;
            if (_changeRotation) _toChange.rotation = Quaternion.Euler(_originRotation + _addRotation * changePercent);
            if (_changeScale) _toChange.localScale = _originScale + _addScale * changePercent;
        }
    }
}
