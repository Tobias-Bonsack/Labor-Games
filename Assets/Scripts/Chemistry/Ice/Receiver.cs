using System;
using System.Collections;
using System.Collections.Generic;
using ChemistryEngine;
using UnityEngine;

namespace FrostEngine
{
    public class Receiver : MonoBehaviour
    {
        [SerializeField] ChemistryReceiver _chemistryReceiver;
        [SerializeField, Tooltip("0f = Full Resistance, 1f = Zero Resistance"), Range(0f, 1f)] protected float _frostSusceptibility;
        [HideInNormalInspector] public float _frostPercent = 0f;


        void Awake()
        {
            _chemistryReceiver._onReceiveHeat += TriggerStay;
        }

        private void TriggerStay(object sender, ChemistryReceiver.OnReceiveElementArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
