using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public abstract class AbstractReceiver : MonoBehaviour
    {
        [Header("Universal Properties")]
        [SerializeField] protected IChemistry.ChemistryTypes _type;
        [SerializeField] protected ChemistryReceiver _chemistryReceiver;
        [SerializeField, Tooltip("0f = Full Resistance, 1f = Zero Resistance"), Range(0f, 1f)] protected float _susceptibility = 1f;
        private float _elementPercent = 0f;
        public float ElementPercent
        {
            get
            {
                return _elementPercent;
            }
            set
            {
                _elementPercent = value;
                _onElementPercentChange?.Invoke(this, EventArgs.Empty);
            }
        }
        private int _activeTriggers = 0;
        public int ActiveTriggers
        {
            get
            {
                return _activeTriggers;
            }
            set
            {
                _activeTriggers = value;
                _onActiveTriggerChange?.Invoke(this, EventArgs.Empty);
            }
        }

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
                case IChemistry.ChemistryTypes.ELECTRICITY:
                    _chemistryReceiver._onReceiveElectricity += EnterTrigger;
                    _chemistryReceiver._onReceiveElectricity += StayTrigger;
                    _chemistryReceiver._onReceiveElectricity += ExitTrigger;
                    break;
                default:
                    break;
            }
        }
        protected void EnterTrigger(object sender, ChemistryReceiver.OnReceiveElementArgs e)
        {
            if (e._status == IChemistryReceiver.Status.ENTER)
            {
                ActiveTriggers = ActiveTriggers + 1;
                Debug.Log("Active triggers: " + ActiveTriggers + " Object: " + _chemistryReceiver.transform.parent.gameObject.name);
                ExtendEnterTrigger(e);
            }
        }
        protected void StayTrigger(object sender, ChemistryReceiver.OnReceiveElementArgs e)
        {
            if (e._status == IChemistryReceiver.Status.STAY)
            {
                float newElementPercent = ElementPercent + _susceptibility * e._radiance * Time.fixedDeltaTime;
                ElementPercent = Mathf.Clamp(newElementPercent, 0f, 1f);
                ExtendStayTrigger(e);
            }
        }
        protected void ExitTrigger(object sender, ChemistryReceiver.OnReceiveElementArgs e)
        {
            if (e._status == IChemistryReceiver.Status.EXIT)
            {
                ActiveTriggers = ActiveTriggers - 1;
                Debug.Log("Active triggers: " + ActiveTriggers + " Object: " + _chemistryReceiver.transform.parent.gameObject.name);
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
    }
}
