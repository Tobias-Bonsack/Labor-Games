using System;
using System.Collections;
using System.Collections.Generic;
using ChemistryEngine;
using UnityEngine;

namespace HeatEngine
{
    public class Destructible : AbstractProperty
    {
        [Header("Parameter")]
        [SerializeField] GameObject _objectToDestroy;
        [SerializeField, Range(0f, 1f)] float _pointToDestroy;

        private void Awake()
        {
            _chemistryReceiver._onReceiveHeat += StayTrigger;
        }

        private void StayTrigger(object sender, ChemistryReceiver.OnReceiveHeatArgs e)
        {
            if (e._status == IChemistryReceiver.Status.STAY && _heatReceiver._burnPercent >= _pointToDestroy)
            {
                Destroy(_objectToDestroy);
            }
        }

        void OnDestroy()
        {
            _chemistryReceiver._onReceiveHeat -= StayTrigger;
        }
    }
}
