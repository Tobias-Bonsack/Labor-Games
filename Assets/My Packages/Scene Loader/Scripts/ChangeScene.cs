using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneLoader
{
    public class ChangeScene : MonoBehaviour
    {

        [SerializeField] SceneLoader.EventSystem _eventSystem;
        [SerializeField] int _sceneNumber;

        void OnTriggerEnter(Collider other)
        {
            _eventSystem.TriggerOnEndScene(_sceneNumber);
        }

    }
}
