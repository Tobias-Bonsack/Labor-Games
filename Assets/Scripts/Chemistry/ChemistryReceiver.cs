using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    [RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public class ChemistryReceiver : MonoBehaviour, IChemistryReceiver
    {
        #region events
        public event EventHandler<OnReceiveElementArgs> _onReceiveHeat;
        public event EventHandler<OnReceiveElementArgs> _onReceiveFrost;
        #endregion

        #region event args
        public class OnReceiveElementArgs : EventArgs
        {
            public IChemistryReceiver.Status _status;
            public float _radiance;
        }
        #endregion

        #region trigger
        public void OnReceiveHeatTrigger(OnReceiveElementArgs args) => _onReceiveHeat?.Invoke(this, args);
        public void OnReceiveFrostTrigger(OnReceiveElementArgs args) => _onReceiveFrost?.Invoke(this, args);
        #endregion

        #region parameter
        [SerializeField] bool _burnItself = false, _frostItself = false;
        [SerializeField] ChemistryEmitter _ownEmitter;
        #endregion

        void OnTriggerEnter(Collider other)
        {
            TriggerEvents(IChemistryReceiver.Status.ENTER, other);
        }
        void OnTriggerStay(Collider other)
        {
            TriggerEvents(IChemistryReceiver.Status.STAY, other);
        }
        void OnTriggerExit(Collider other)
        {
            TriggerEvents(IChemistryReceiver.Status.EXIT, other);
        }
        private void TriggerEvents(IChemistryReceiver.Status status, Collider other)
        {
            if (other.gameObject.TryGetComponent<ChemistryEmitter>(out ChemistryEmitter chemistryEmitter))
            {
                for (int i = 0; i < chemistryEmitter._types.Count; i++)
                {
                    IChemistry.ChemistryTypes type = chemistryEmitter._types[i];
                    switch (type)
                    {
                        case IChemistry.ChemistryTypes.HEAT:
                            if (IsStrangerEmitter(chemistryEmitter, _burnItself))
                            {
                                OnReceiveElementArgs onReceiveHeatArgs = new OnReceiveElementArgs { _status = status, _radiance = chemistryEmitter._radiance[i] };
                                OnReceiveHeatTrigger(onReceiveHeatArgs);
                            }
                            break;
                        case IChemistry.ChemistryTypes.COLD:
                            if (IsStrangerEmitter(chemistryEmitter, _frostItself))
                            {
                                OnReceiveElementArgs onReceiveFrostArgs = new OnReceiveElementArgs { _status = status, _radiance = chemistryEmitter._radiance[i] };
                                OnReceiveFrostTrigger(onReceiveFrostArgs);
                            }
                            break;
                        default:
                            Debug.LogWarning("Unknown type");
                            break;
                    }
                }
            }

        }

        private bool IsStrangerEmitter(ChemistryEmitter chemistryEmitter, bool typeItself)
        {
            return typeItself || _ownEmitter == null || _ownEmitter != chemistryEmitter;
        }

    }
}
