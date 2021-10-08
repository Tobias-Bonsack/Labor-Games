using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scanner
{
    public class Interactive : MonoBehaviour
    {
        [SerializeField] Outline _outline;
        [SerializeField] float _timer;

        public void ActivateOutline()
        {
            _outline.enabled = true;
            StartCoroutine(TimerDeactivateOutline());
        }

        IEnumerator TimerDeactivateOutline()
        {
            yield return new WaitForSecondsRealtime(_timer);
            _outline.enabled = false;
        }
    }
}
