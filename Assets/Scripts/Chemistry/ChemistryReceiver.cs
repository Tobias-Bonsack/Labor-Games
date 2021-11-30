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
            public IChemistryEmitter.Type _emitterType;
        }
        #endregion

        #region trigger
        public void OnReceiveHeatTrigger(object sender, OnReceiveElementArgs args) => _onReceiveHeat?.Invoke(sender, args);
        public void OnReceiveFrostTrigger(object sender, OnReceiveElementArgs args) => _onReceiveFrost?.Invoke(sender, args);
        public void OnReceiveElectricityTrigger(object sender, OnReceiveElementArgs args) => _onReceiveElectricity?.Invoke(sender, args);
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
            if (other.gameObject.TryGetComponent<AbstractEmitter>(out AbstractEmitter chemistryEmitter))
            {
                for (int i = 0; i < chemistryEmitter.Types.Count; i++)
                {
                    IChemistry.ChemistryTypes type = chemistryEmitter.Types[i];
                    float radiance = chemistryEmitter.Radiance[i];
                    TriggerElementEvents(status, chemistryEmitter, type, radiance);
                }
            }
        }

        private void TriggerElementEvents(IChemistryReceiver.Status status, AbstractEmitter chemistryEmitter, IChemistry.ChemistryTypes type, float radiance)
        {
            Debug.Log("Receiver " + status + ": " + _activeEmitter.Count + " Object: " + transform.parent.gameObject.name);
            OnReceiveElementArgs onReceiveArgs = new OnReceiveElementArgs { _status = status, _radiance = radiance, _emitterType = chemistryEmitter._emitType };

            switch (type)
            {
                case IChemistry.ChemistryTypes.HEAT:
                    if (IsStrangerEmitter(chemistryEmitter, _burnItself))
                    {
                        OnReceiveHeatTrigger(chemistryEmitter, onReceiveArgs);
                    }
                    break;
                case IChemistry.ChemistryTypes.COLD:
                    if (IsStrangerEmitter(chemistryEmitter, _frostItself))
                    {
                        OnReceiveFrostTrigger(chemistryEmitter, onReceiveArgs);
                    }
                    break;
                case IChemistry.ChemistryTypes.ELECTRICITY:
                    if (IsStrangerEmitter(chemistryEmitter, _shockItself))
                    {
                        OnReceiveElectricityTrigger(chemistryEmitter, onReceiveArgs);
                    }
                    break;
                default:
                    Debug.LogWarning("Unknown type");
                    break;
            }
        }

        private bool IsStrangerEmitter(AbstractEmitter chemistryEmitter, bool typeItself)
        {
            return typeItself || _ownEmitter == null || _ownEmitter != chemistryEmitter;
        }

        public void NewEmitType(AbstractEmitter emitter, IChemistry.ChemistryTypes type)
        {
            Debug.Log("New EmitType");
            _activeEmitter.Add((IChemistryEmitter)emitter);
            TriggerElementEvents(IChemistryReceiver.Status.ENTER, emitter, type, emitter.Radiance[emitter.Types.IndexOf(type)]);
        }
        public void RemoveEmitType(AbstractEmitter emitter, IChemistry.ChemistryTypes type)
        {
            _activeEmitter.Remove((IChemistryEmitter)emitter);
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
