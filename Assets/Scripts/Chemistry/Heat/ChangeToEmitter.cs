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

        void Awake()
        {
            _chemistryReceiver.OnReceiveHeat += StayTrigger;
        }

        private void StayTrigger(object sender, ChemistryReceiver.OnReceiveHeatArgs e)
        {
            if (e._status == ChemistryEngine.IChemistryReceiver.Status.STAY)
            {
                if (_heatReceiver._burnPercent >= _pointToChange)
                {
                    _emitter.SetActive(true);
                    _receiver.SetActive(_receiveRemeins);
                    Destroy(this);
                }
            }
        }

        void OnDestroy()
        {
            _chemistryReceiver.OnReceiveHeat -= StayTrigger;
        }
    }
}
