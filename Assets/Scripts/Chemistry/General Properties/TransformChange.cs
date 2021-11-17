using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public class TransformChange : AbstractProperty
    {
        [SerializeField] Transform _toChange;

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
            Debug.Log("Position: " + (_originPosition + _addPosition * _elementReceiver._elementPercent));
            Debug.Log("Rotation: " + (_originRotation + _addRotation * _elementReceiver._elementPercent));
            if (_changePosition) _toChange.position = _originPosition + _addPosition * _elementReceiver._elementPercent;
            if (_changeRotation) _toChange.rotation = Quaternion.Euler(_originRotation + _addRotation * _elementReceiver._elementPercent);
            if (_changeScale) _toChange.localScale = _originScale + _addScale * _elementReceiver._elementPercent;
        }
    }
}
