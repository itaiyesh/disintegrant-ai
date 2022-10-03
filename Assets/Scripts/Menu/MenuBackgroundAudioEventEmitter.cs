using Events;
using UnityEngine;

namespace Menu
{
    public class MenuBackgroundAudioEventEmitter : MonoBehaviour
    {
        private void Start()
        {
            // emit event to play audio
            EventManager.TriggerEvent<GameMenuBackgroundAudioEvent, bool>(true);
        }
    }
}