using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public class ScaleChanger : AbstractProperty
    {
        [Header("Propertie-Parameter")]
        [SerializeField] GameObject _objectToChange;
        [SerializeField] Vector3 _lowestSize = Vector3.zero;
        [SerializeField] Vector3 _biggestSize = Vector3.one;
        void Awake()
        {
            _elementReceiver._onElementPercentChange += ChangeScale;

            switch (_type)
            {
                case IChemistry.ChemistryTypes.HEAT:
                    _chemistryReceiver._onReceiveHeat += StayTrigger;
                    break;
                case IChemistry.ChemistryTypes.COLD:
                    _chemistryReceiver._onReceiveFrost += StayTrigger;
                    break;
                default:
                    Debug.LogError("UnknownType");
                    break;
            }
        }

        private void StayTrigger(object sender, ChemistryReceiver.OnReceiveElementArgs e)
        {
            if (e._status == IChemistryReceiver.Status.STAY)
            {
                ChangeScale(null, null);
            }
        }

        private void ChangeScale(object sender, EventArgs e)
        {
            Vector3 addSize = (_biggestSize - _lowestSize) * _elementReceiver._elementPercent;
            _objectToChange.transform.localScale = _lowestSize + addSize;
        }
    }
}
