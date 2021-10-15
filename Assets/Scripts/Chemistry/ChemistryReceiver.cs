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
        public event EventHandler<OnReceiveHeatArgs> _onReceiveHeat;
        #endregion

        #region event args
        public class OnReceiveHeatArgs : EventArgs
        {
            public IChemistryReceiver.Status _status;
            public float _radiance;
        }
        #endregion

        #region trigger
        public void OnReceiveHeatTrigger(OnReceiveHeatArgs args) => _onReceiveHeat?.Invoke(this, args);
        #endregion

        #region parameter
        [SerializeField] bool _burnItself = false;
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
            ChemistryEmitter chemistryEmitter = other.gameObject.GetComponent<ChemistryEmitter>();
            if (IsStrangerEmitter(chemistryEmitter))
            {
                for (int i = 0; i < chemistryEmitter._types.Length; i++)
                {
                    IChemistry.ChemistryTypes type = chemistryEmitter._types[i];
                    switch (type)
                    {
                        case IChemistry.ChemistryTypes.HEAT:
                            OnReceiveHeatArgs onReceiveHeatArgs = new OnReceiveHeatArgs { _status = status, _radiance = chemistryEmitter._radiance[i] };
                            OnReceiveHeatTrigger(onReceiveHeatArgs);
                            break;
                        default:
                            Debug.LogWarning("Unknown type");
                            break;
                    }
                }
            }
        }

        private bool IsStrangerEmitter(ChemistryEmitter chemistryEmitter)
        {
            return _burnItself || _ownEmitter == null || _ownEmitter != chemistryEmitter;
        }

    }
}
