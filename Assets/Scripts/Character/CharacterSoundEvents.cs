using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundEvents : MonoBehaviour
{

    public void Footstep(int type) 
    {
        EventManager.TriggerEvent<Events.FootstepSoundEvent, int, Vector3>(type, gameObject.transform.position);
    }
}
