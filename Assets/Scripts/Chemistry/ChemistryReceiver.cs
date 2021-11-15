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
        public event EventHandler<OnReceiveElementArgs> _onReceiveElectricity;
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
        public void OnReceiveElectricityTrigger(OnReceiveElementArgs args) => _onReceiveElectricity?.Invoke(this, args);
        #endregion

        #region parameter
        [SerializeField] bool _burnItself = false, _frostItself = false, _shockItself = false;
        [SerializeField] ChemistryEmitter _ownEmitter;
        #endregion

        private HashSet<IChemistryEmitter> _activeEmitter = new HashSet<IChemistryEmitter>();

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<IChemistryEmitter>(out IChemistryEmitter emitter)) _activeEmitter.Add(emitter);
            TriggerEvents(IChemistryReceiver.Status.ENTER, other);
        }
        void OnTriggerStay(Collider other)
        {
            TriggerEvents(IChemistryReceiver.Status.STAY, other);

        }
        void OnTriggerExit(Collider other)
        {
            if (other.gameObject.TryGetComponent<IChemistryEmitter>(out IChemistryEmitter emitter)) _activeEmitter.Remove(emitter);
            TriggerEvents(IChemistryReceiver.Status.EXIT, other);
        }
        private void TriggerEvents(IChemistryReceiver.Status status, Collider other)
        {
            if (other.gameObject.TryGetComponent<ChemistryEmitter>(out ChemistryEmitter chemistryEmitter))
            {
                for (int i = 0; i < chemistryEmitter.Types.Count; i++)
                {
                    IChemistry.ChemistryTypes type = chemistryEmitter.Types[i];
                    float radiance = chemistryEmitter.Radiance[i];
                    TriggerElementEvents(status, chemistryEmitter, type, radiance);
                }
            }
        }

        private void TriggerElementEvents(IChemistryReceiver.Status status, ChemistryEmitter chemistryEmitter, IChemistry.ChemistryTypes type, float radiance)
        {
            Debug.Log("Receiver " + status + ": " + _activeEmitter.Count + " Object: " + transform.parent.gameObject.name);

            switch (type)
            {
                case IChemistry.ChemistryTypes.HEAT:
                    if (IsStrangerEmitter(chemistryEmitter, _burnItself))
                    {
                        OnReceiveElementArgs onReceiveHeatArgs = new OnReceiveElementArgs { _status = status, _radiance = radiance };
                        OnReceiveHeatTrigger(onReceiveHeatArgs);
                    }
                    break;
                case IChemistry.ChemistryTypes.COLD:
                    if (IsStrangerEmitter(chemistryEmitter, _frostItself))
                    {
                        OnReceiveElementArgs onReceiveFrostArgs = new OnReceiveElementArgs { _status = status, _radiance = radiance };
                        OnReceiveFrostTrigger(onReceiveFrostArgs);
                    }
                    break;
                case IChemistry.ChemistryTypes.ELECTRICITY:
                    if (IsStrangerEmitter(chemistryEmitter, _shockItself))
                    {
                        OnReceiveElementArgs onReceiveFrostArgs = new OnReceiveElementArgs { _status = status, _radiance = radiance };
                        OnReceiveElectricityTrigger(onReceiveFrostArgs);
                    }
                    break;
                default:
                    Debug.LogWarning("Unknown type");
                    break;
            }
        }

        private bool IsStrangerEmitter(ChemistryEmitter chemistryEmitter, bool typeItself)
        {
            return typeItself || _ownEmitter == null || _ownEmitter != chemistryEmitter;
        }

        public void NewEmitType(ChemistryEmitter emitter, IChemistry.ChemistryTypes type)
        {
            _activeEmitter.Add(emitter);
            TriggerElementEvents(IChemistryReceiver.Status.STAY, emitter, type, emitter.Radiance[emitter.Types.IndexOf(type)]);
        }
        public void RemoveEmitType(ChemistryEmitter emitter, IChemistry.ChemistryTypes type)
        {
            _activeEmitter.Remove(emitter);
            TriggerElementEvents(IChemistryReceiver.Status.EXIT, emitter, type, 0f);
        }

        void OnDestroy()
        {
            Debug.Log("OnDestroy Chemistry-Receiver");
            foreach (IChemistryEmitter emitter in _activeEmitter)
            {
                emitter.RemoveReceiver(this);
            }
        }

    }
}
