using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCollectable : MonoBehaviour
{
    public bool disableTrigger = false;

    public abstract bool TryCollect(Collider c);
    
}
