using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public abstract class AbstractReceiver : MonoBehaviour
    {
        [Header("Universal Properties")]
        [SerializeField] protected ChemistryReceiver _chemistryReceiver;
        [SerializeField, Tooltip("0f = Full Resistance, 1f = Zero Resistance"), Range(0f, 1f)] protected float _susceptibility;
        [HideInNormalInspector] public float _elementPercent = 0f;
        [HideInNormalInspector] public int _activeTriggers = 0;

        public void MultiplieSusceptibility(float multiplier)
        {
            _susceptibility *= multiplier;
            _susceptibility = Mathf.Clamp(_susceptibility, 0f, 1f);
        }

        protected void SetAbstractEvents(IChemistry.ChemistryTypes type)
        {
            switch (type)
            {
                case IChemistry.ChemistryTypes.HEAT:
                    _chemistryReceiver._onReceiveHeat += EnterTrigger;
                    _chemistryReceiver._onReceiveHeat += StayTrigger;
                    _chemistryReceiver._onReceiveHeat += ExitTrigger;
                    break;
                case IChemistry.ChemistryTypes.COLD:
                    _chemistryReceiver._onReceiveFrost += EnterTrigger;
                    _chemistryReceiver._onReceiveFrost += StayTrigger;
                    _chemistryReceiver._onReceiveFrost += ExitTrigger;
                    break;
                default:
                    break;
            }
        }
        protected void EnterTrigger(object sender, ChemistryReceiver.OnReceiveElementArgs e)
        {
            if (e._status == IChemistryReceiver.Status.ENTER)
            {
                _activeTriggers++;
                Debug.Log(_activeTriggers);
                OnActiveTriggerChangeTrigger();
                ExtendEnterTrigger(e);
            }
        }
        protected void StayTrigger(object sender, ChemistryReceiver.OnReceiveElementArgs e)
        {
            if (e._status == IChemistryReceiver.Status.STAY)
            {
                _elementPercent += _susceptibility * e._radiance * Time.fixedDeltaTime;
                _elementPercent = Mathf.Clamp(_elementPercent, 0f, 1f);
                OnElementPercentChangeTrigger();
                ExtendStayTrigger(e);
            }
        }
        protected void ExitTrigger(object sender, ChemistryReceiver.OnReceiveElementArgs e)
        {
            if (e._status == IChemistryReceiver.Status.EXIT)
            {
                _activeTriggers--;
                Debug.Log(_activeTriggers);
                OnActiveTriggerChangeTrigger();
                ExtendExitTrigger(e);
            }
        }

        protected abstract void ExtendEnterTrigger(ChemistryReceiver.OnReceiveElementArgs e);
        protected abstract void ExtendStayTrigger(ChemistryReceiver.OnReceiveElementArgs e);
        protected abstract void ExtendExitTrigger(ChemistryReceiver.OnReceiveElementArgs e);

        #region events
        public event EventHandler<EventArgs> _onElementPercentChange;
        public event EventHandler<EventArgs> _onActiveTriggerChange;
        #endregion

        #region trigger methods
        public void OnElementPercentChangeTrigger() => _onElementPercentChange?.Invoke(this, EventArgs.Empty);
        public void OnActiveTriggerChangeTrigger() => _onActiveTriggerChange?.Invoke(this, EventArgs.Empty);
        #endregion
    }
}
