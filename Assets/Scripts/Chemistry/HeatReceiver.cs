using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChemistryEngine
{
    public class HeatReceiver : MonoBehaviour, IChemistryReceiver
    {

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void receiv(IChemistry.ChemistryTypes[] chemistryType)
        {
            Debug.Log("Receive: " + chemistryType);
        }
    }
}
