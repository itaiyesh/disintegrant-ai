using System;
using UnityEngine;

namespace UI.Minimap
{
    public class Minimap : MonoBehaviour
    {
        private Transform _player;
        
        private void Start()
        {
            _player = GameObject.FindWithTag("Player").transform;
        }

        private void LateUpdate()
        {
            Vector3 newPosition = _player.position;
            newPosition.y = transform.position.y;
            transform.position = newPosition;
        }
    }
}