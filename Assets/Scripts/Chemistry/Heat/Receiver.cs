using System;
using System.Collections;
using System.Collections.Generic;
using ChemistryEngine;
using UnityEngine;
using UnityEngine.VFX;

namespace HeatEngine
{
    public class Receiver : MonoBehaviour
    {

        [SerializeField] ChemistryReceiver _chemistryReceiver;
        [SerializeField, Tooltip("0f = Full Resistance, 1f = Zero Resistance"), Range(0f, 1f)] protected float _heatSusceptibility;
        public float _burnPercent = 0f;

        void Awake()
        {
            _chemistryReceiver.OnReceiveHeat += TriggerStay;
        }

        private void TriggerStay(object sender, ChemistryReceiver.OnReceiveHeatArgs e)
        {
            if (e._status == IChemistryReceiver.Status.STAY)
            {
                _burnPercent += _heatSusceptibility * e._radiance * Time.fixedDeltaTime;
                _burnPercent = Mathf.Clamp(_burnPercent, 0f, 1f);
            }
        }
    }
}
