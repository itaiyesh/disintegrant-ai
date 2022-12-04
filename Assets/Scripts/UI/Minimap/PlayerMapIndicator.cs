using System;
using UnityEngine;

namespace UI.Minimap
{
    public class PlayerMapIndicator : MonoBehaviour
    {
        private Boolean _isAIAgent;
        private SpriteRenderer _spriteRenderer;
        
        private void Awake()
        {
            _isAIAgent = gameObject.CompareTag("AI");
            var indicator = gameObject.transform.Find("MapIndicator");
            _spriteRenderer = indicator.gameObject.GetComponent<SpriteRenderer>();
            _spriteRenderer.color = _isAIAgent ? Color.red : Color.green;
        }
    }
}