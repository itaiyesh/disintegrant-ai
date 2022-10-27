using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// GameObject player
// Dictionary<string, object> oldEffectedAttributes
// Dictionary<string, object> newEffectedAttributes
public class CharacterAttributeChangeEvent: UnityEvent<GameObject, Dictionary<string, object>, Dictionary<string, object>>{}