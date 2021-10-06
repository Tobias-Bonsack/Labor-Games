using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public class HeatReceiver : MonoBehaviour, IChemistryReceiver
    {
        [SerializeField] GameObject _heatVFX;
        [SerializeField] float _addWaitTime;
        private Coroutine _routine;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void receiv(IChemistry.ChemistryTypes[] chemistryType, float time)
        {
            if (_routine != null) StopCoroutine(_routine);

            Debug.Log("Receive: " + chemistryType);

            _routine = StartCoroutine(HeatRoutine(time));

        }

        IEnumerator HeatRoutine(float time)
        {
            _heatVFX.SetActive(true);
            yield return new WaitForSeconds(time + _addWaitTime);
            _heatVFX.SetActive(false);
        }
    }
}
