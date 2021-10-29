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
        [HideInNormalInspector] public float _burnPercent = 0f;
        [HideInNormalInspector] public int _activeTriggers = 0;

        void Awake()
        {
            _chemistryReceiver._onReceiveHeat += EnterTrigger;
            _chemistryReceiver._onReceiveHeat += ExitTrigger;
            _chemistryReceiver._onReceiveHeat += TriggerStay;
        }

        private void EnterTrigger(object sender, ChemistryReceiver.OnReceiveElementArgs e)
        {
            if (e._status == IChemistryReceiver.Status.ENTER) _activeTriggers++;
        }
        private void TriggerStay(object sender, ChemistryReceiver.OnReceiveElementArgs e)
        {
            if (e._status == IChemistryReceiver.Status.STAY)
            {
                _burnPercent += _heatSusceptibility * e._radiance * Time.fixedDeltaTime;
                _burnPercent = Mathf.Clamp(_burnPercent, 0f, 1f);
            }
        }
        private void ExitTrigger(object sender, ChemistryReceiver.OnReceiveElementArgs e)
        {
            if (e._status == IChemistryReceiver.Status.EXIT) _activeTriggers--;
        }


        public void MultiplieSusceptibility(float multiplier)
        {
            _heatSusceptibility *= multiplier;
            _heatSusceptibility = Mathf.Clamp(_heatSusceptibility, 0f, 1f);
        }

        #region events
        public event EventHandler<EventArgs> _onBurnPercentChange;
        #endregion

        #region triggermethods
        public void OnBurnPercentChangeTrigger() => _onBurnPercentChange?.Invoke(this, EventArgs.Empty);
        #endregion
    }
}
