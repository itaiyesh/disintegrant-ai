using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/VoicePack", order = 1)]
public class VoicePack : ScriptableObject
{
    public AudioClip[] chase;
    public AudioClip[] heal;
    public AudioClip[] combat;
    public AudioClip[] collectWeapon;
    public AudioClip[] death;
}