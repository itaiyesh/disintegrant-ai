using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    public enum AudioState
    {
        Low,
        Normal,
        Muted
    }

    public class AudioMixerController : MonoBehaviour
    {
        public AudioMixer audioMixer;
        public AudioState audioState = AudioState.Normal;

        public float normalVolume = 0f;
        public float lowVolume = -8f;
        public float mutedVolume = -80f;

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.V))
            {
                var nextState = GetNextAudioState(audioState);
                if (audioMixer == null) return;

                switch (nextState)
                {
                    case AudioState.Low:
                        SetVolume(lowVolume);
                        break;
                    case AudioState.Muted:
                        SetVolume(mutedVolume);
                        break;
                    case AudioState.Normal:
                    default:
                        SetVolume(normalVolume);
                        break;
                }

                audioState = nextState;
            }
        }

        private void SetVolume(float volume)
        {
            audioMixer.SetFloat("MasterVolume", volume);
        }

        private static AudioState GetNextAudioState(AudioState currentAudioState)
        {
            switch (currentAudioState)
            {
                case AudioState.Low:
                    return AudioState.Muted;
                case AudioState.Normal:
                    return AudioState.Low;
                default:
                    return AudioState.Normal;
            }
        }
    }
}