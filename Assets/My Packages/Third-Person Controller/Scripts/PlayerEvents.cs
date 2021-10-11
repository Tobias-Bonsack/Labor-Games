using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThirdPersonController
{
    public class PlayerEvents : MonoBehaviour
    {
        #region events
        public event Action<int> _onChangeKamera;
        #endregion

        #region triggers
        public void TriggerOnChangeCamera(int camera) => _onChangeKamera?.Invoke(camera);
        #endregion

    }
}
