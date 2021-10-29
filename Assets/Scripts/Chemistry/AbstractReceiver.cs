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

        protected void UpdateElementPercent(ChemistryReceiver.OnReceiveElementArgs e)
        {
            _elementPercent += _susceptibility * e._radiance * Time.fixedDeltaTime;
            _elementPercent = Mathf.Clamp(_elementPercent, 0f, 1f);
        }

        public void MultiplieSusceptibility(float multiplier)
        {
            _susceptibility *= multiplier;
            _susceptibility = Mathf.Clamp(_susceptibility, 0f, 1f);
        }

        #region events
        public event EventHandler<EventArgs> _onBurnPercentChange;
        #endregion

        #region trigger methods
        public void OnBurnPercentChangeTrigger() => _onBurnPercentChange?.Invoke(this, EventArgs.Empty);
        #endregion
    }
}
