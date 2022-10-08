using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    /**
     * An event to be used when a weapon is fired. It sends the audio clip of the sound and the position.
     */
    public class WeaponFiredEvent : UnityEvent<AudioClip, Vector3>
    {
    }
}