using Events;
using Menu;
using UnityEngine;
using UnityEngine.Events;

public class AudioEventManager : MonoBehaviour
{
    public EventSound3D eventSound3DPrefab;

    public AudioClip[] gameMenuAudios;
    public AudioClip[] footstepAudios;
    public AudioClip menuButtonHighlightedAudio;
    public AudioClip menuButtonClickedAudio;

    public AudioClip weaponSwapAudio;

    private UnityAction<bool> menuBackgroundAudioEventListener;

    private UnityAction<MenuButtonEventListener.ButtonEvent> menuButtonEventListener;

    private UnityAction<GameObject, GameObject, GameObject> weaponSwapEventListener;

	private UnityAction<GameObject, GameObject, AudioClip, Vector3> weaponFiredEventListener;

    private UnityAction<int, Vector3> footstepEventListener;

    void Awake()
    {
        menuBackgroundAudioEventListener = new UnityAction<bool>(MenuBackgroundAudioEventHandler);
        menuButtonEventListener = new UnityAction<MenuButtonEventListener.ButtonEvent>(MenuButtonAudioEventHandler);
        weaponSwapEventListener = new UnityAction<GameObject, GameObject, GameObject>(WeaponSwapEventHandler);
        weaponFiredEventListener = new UnityAction<GameObject, GameObject, AudioClip, Vector3>(WeaponFiredEventHandler);
        footstepEventListener = new UnityAction<int, Vector3>(footstepEventHandler);

    }


    // Use this for initialization
    void Start()
    {
    }


    void OnEnable()
    {
        EventManager.StartListening<GameMenuBackgroundAudioEvent, bool>(menuBackgroundAudioEventListener);
        EventManager.StartListening<GameMenuButtonAudioEvent, MenuButtonEventListener.ButtonEvent>(
            menuButtonEventListener);
	    EventManager.StartListening<WeaponSwapEvent, GameObject, GameObject, GameObject>(weaponSwapEventListener);
	    EventManager.StartListening<WeaponFiredEvent, GameObject, GameObject, AudioClip, Vector3>(weaponFiredEventListener);
        EventManager.StartListening<FootstepSoundEvent, int, Vector3>(footstepEventListener);

    }

    void OnDisable()
    {
        EventManager.StopListening<GameMenuBackgroundAudioEvent, bool>(menuBackgroundAudioEventListener);
        EventManager.StopListening<GameMenuButtonAudioEvent, MenuButtonEventListener.ButtonEvent>(
            menuButtonEventListener);
        EventManager.StopListening<WeaponSwapEvent, GameObject, GameObject, GameObject>(weaponSwapEventListener);
        EventManager.StopListening<WeaponFiredEvent, GameObject, GameObject, AudioClip, Vector3>(weaponFiredEventListener);
        EventManager.StopListening<FootstepSoundEvent, int, Vector3>(footstepEventListener);

    }
    private void footstepEventHandler(int type, Vector3 position)
    {
        var sound = Instantiate(eventSound3DPrefab, position, Quaternion.identity, null);
        //TODO: Cancel sound on death!
        // sound.gameObject.AddComponent<MinionAudioCancelOnDeath>();
        sound.audioSrc.spatialBlend = 1f;
        sound.audioSrc.rolloffMode = AudioRolloffMode.Linear;
        
        sound.audioSrc.clip = footstepAudios[type < footstepAudios.Length ? type : 0];
        sound.audioSrc.volume = 0.5f;
        sound.audioSrc.volume *= Random.Range(0.7f, 1f);
        sound.audioSrc.minDistance = 1f;
        sound.audioSrc.maxDistance = 5f;
        sound.audioSrc.Play();

    }
	private void WeaponFiredEventHandler(GameObject player, GameObject weapon, AudioClip audioClip, Vector3 position)
    {
        var sound = Instantiate(eventSound3DPrefab, position, Quaternion.identity, null);
        if (audioClip)
        {
            // enable 3d spatial blend
            sound.audioSrc.spatialBlend = 1f;
            sound.audioSrc.rolloffMode = AudioRolloffMode.Linear;
            sound.audioSrc.minDistance = 10f;
            sound.audioSrc.maxDistance = 40f;
            
            sound.audioSrc.clip = audioClip;
            sound.audioSrc.Play();
        }
    }

	private void WeaponSwapEventHandler(GameObject player, GameObject oldWeapon, GameObject newWeapon)
    {
        //Workout till we figure out why spatial sound doesnt work:
        // AudioSource.PlayClipAtPoint(weaponSwapAudio, position);
	    var sound = Instantiate(eventSound3DPrefab, newWeapon.transform.position, Quaternion.identity, null);

        if (weaponSwapAudio)
        {
            // enable 3d spatial blend
            sound.audioSrc.spatialBlend = 1f;
            sound.audioSrc.rolloffMode = AudioRolloffMode.Linear;
            sound.audioSrc.minDistance = 1f;
            sound.audioSrc.maxDistance = 1f;
            
            sound.audioSrc.clip = weaponSwapAudio;
            sound.audioSrc.Play();
        }
    }

    private void MenuBackgroundAudioEventHandler(bool enableBackgroundAudio)
    {
        var sound = Instantiate(eventSound3DPrefab);
        if (enableBackgroundAudio)
        {
            var index = 0; // randomly pick one now.
            if (index >= gameMenuAudios.Length) return;

            sound.audioSrc.clip = gameMenuAudios[index];
            sound.audioSrc.loop = true;
            sound.audioSrc.Play();
        }
        else
        {
            sound.audioSrc.Pause();
        }
    }

    private void MenuButtonAudioEventHandler(MenuButtonEventListener.ButtonEvent buttonEvent)
    {
        var sound = Instantiate(eventSound3DPrefab);
        AudioClip audioClip;
        switch (buttonEvent)
        {
            case MenuButtonEventListener.ButtonEvent.ButtonClick:
                audioClip = menuButtonClickedAudio;
                break;
            default:
                audioClip = menuButtonHighlightedAudio;
                break;
        }

        sound.audioSrc.clip = audioClip;
        sound.audioSrc.Play();
    }

    // void boxCollisionEventHandler(Vector3 worldPos, float impactForce)
    // {
    //     //AudioSource.PlayClipAtPoint(this.boxAudio, worldPos);
    //
    //     const float halfSpeedRange = 0.2f;
    //
    //     EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);
    //
    //     snd.audioSrc.clip = this.boxAudio[Random.Range(0,boxAudio.Length)];
    //
    //     snd.audioSrc.pitch = Random.Range(1f-halfSpeedRange, 1f+halfSpeedRange);
    //
    //     snd.audioSrc.minDistance = Mathf.Lerp(1f, 8f, impactForce /200f);
    //     snd.audioSrc.maxDistance = 100f;
    //
    //     snd.audioSrc.Play();
    // }

    // void playerLandsEventHandler(Vector3 worldPos, float collisionMagnitude)
    // {
    //     //AudioSource.PlayClipAtPoint(this.explosionAudio, worldPos, 1f);
    //
    //     if (eventSound3DPrefab)
    //     {
    //         if (collisionMagnitude > 300f)
    //         {
    //
    //             EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);
    //
    //             snd.audioSrc.clip = this.playerLandsAudio;
    //
    //             snd.audioSrc.minDistance = 5f;
    //             snd.audioSrc.maxDistance = 100f;
    //
    //             snd.audioSrc.Play();
    //
    //             if (collisionMagnitude > 500f)
    //             {
    //
    //                 EventSound3D snd2 = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);
    //
    //                 snd2.audioSrc.clip = this.gruntAudio;
    //
    //                 snd2.audioSrc.minDistance = 5f;
    //                 snd2.audioSrc.maxDistance = 100f;
    //
    //                 snd2.audioSrc.Play();
    //             }
    //         }
    //
    //
    //     }
    // }

    // void minionLandsEventHandler(Vector3 worldPos, float collisionMagnitude)
    // {
    //     //AudioSource.PlayClipAtPoint(this.explosionAudio, worldPos, 1f);
    //
    //     if (eventSound3DPrefab)
    //     {
    //         if (collisionMagnitude > 300f)
    //         {
    //
    //             EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);
    //
    //             snd.audioSrc.clip = this.punchAudio;
    //
    //             snd.audioSrc.minDistance = 5f;
    //             snd.audioSrc.maxDistance = 100f;
    //
    //             snd.audioSrc.Play();
    //
    //             if (collisionMagnitude > 500f)
    //             {
    //
    //                 EventSound3D snd2 = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);
    //
    //                 snd2.audioSrc.clip = this.minionOuchAudio;
    //
    //                 snd2.audioSrc.minDistance = 5f;
    //                 snd2.audioSrc.maxDistance = 100f;
    //
    //                 snd2.audioSrc.Play();
    //             }
    //         }
    //
    //
    //     }
    // }

    //void minionJabberEventHandler(Vector3 worldPos)
    //{
    //    //AudioSource.PlayClipAtPoint(this.explosionAudio, worldPos, 1f);

    //    if (eventSound3DPrefab)
    //    {

    //        EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);

    //        snd.gameObject.AddComponent<MinionAudioCancelOnDeath>();

    //        snd.audioSrc.clip = this.minionJabberAudio[Random.Range(0, this.minionJabberAudio.Length)];

    //        snd.audioSrc.minDistance = 5f;
    //        snd.audioSrc.maxDistance = 100f;

    //        snd.audioSrc.Play();
    //    }
    //}

    // void explosionEventHandler(Vector3 worldPos)
    // {
    //     //AudioSource.PlayClipAtPoint(this.explosionAudio, worldPos, 1f);
    //
    //     if (eventSound3DPrefab)
    //     {
    //         
    //         EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);
    //
    //         snd.audioSrc.clip = this.explosionAudio;
    //
    //         snd.audioSrc.minDistance = 50f;
    //         snd.audioSrc.maxDistance = 500f;
    //
    //         snd.audioSrc.Play();
    //     }
    // }

    // void bombBounceEventHandler(Vector3 worldPos)
    // {
    //     //AudioSource.PlayClipAtPoint(this.explosionAudio, worldPos, 1f);
    //
    //     if (eventSound3DPrefab)
    //     {
    //
    //         EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);
    //
    //         snd.audioSrc.clip = this.bombBounceAudio;
    //
    //         snd.audioSrc.minDistance = 10f;
    //         snd.audioSrc.maxDistance = 500f;
    //
    //         snd.audioSrc.Play();
    //     }
    // }

    // void jumpEventHandler(Vector3 worldPos)
    // {
    //     //AudioSource.PlayClipAtPoint(this.explosionAudio, worldPos, 1f);
    //
    //     if (eventSound3DPrefab)
    //     {
    //
    //         EventSound3D snd = Instantiate(eventSound3DPrefab, worldPos, Quaternion.identity, null);
    //
    //         snd.audioSrc.clip = this.jumpAudio;
    //
    //         snd.audioSrc.minDistance = 5f;
    //         snd.audioSrc.maxDistance = 100f;
    //
    //         snd.audioSrc.Play();
    //     }
    // }

    // void deathEventHandler(GameObject go)
    // {
    //     //AudioSource.PlayClipAtPoint(this.explosionAudio, worldPos, 1f);
    //
    //     if (eventSound3DPrefab)
    //     {
    //
    //         EventSound3D snd = Instantiate(eventSound3DPrefab, go.transform);
    //
    //         snd.audioSrc.clip = this.deathAudio;
    //
    //         snd.audioSrc.minDistance = 5f;
    //         snd.audioSrc.maxDistance = 100f;
    //
    //         snd.audioSrc.Play();
    //     }
    // }

    //void minionDeathEventHandler(Vector3 pos, MinionScript ms)
    //{

    //    if (minionDeathAudio)
    //    {

    //        EventSound3D snd = Instantiate(eventSound3DPrefab, pos, Quaternion.identity, null);

    //        snd.audioSrc.clip = this.minionDeathAudio;

    //        snd.audioSrc.minDistance = 5f;
    //        snd.audioSrc.maxDistance = 100f;

    //        snd.audioSrc.Play();
    //    }

    //}


    //void minionOuchEventHandler(Vector3 pos)
    //{


    //    if (punchAudio)
    //    {

    //        EventSound3D snd = Instantiate(eventSound3DPrefab, pos, Quaternion.identity, null);

    //        snd.audioSrc.clip = this.punchAudio;

    //        snd.audioSrc.minDistance = 5f;
    //        snd.audioSrc.maxDistance = 100f;

    //        snd.audioSrc.Play();
    //    }


    //    if (minionOuchAudio)
    //    {

    //        EventSound3D snd2 = Instantiate(eventSound3DPrefab, pos, Quaternion.identity, null);

    //        snd2.audioSrc.clip = this.minionOuchAudio;

    //        snd2.audioSrc.minDistance = 5f;
    //        snd2.audioSrc.maxDistance = 100f;

    //        snd2.audioSrc.Play();
    //    }
    //}


    //void minionSpawnEventHandler(MinionScript minion) {

    //    if (minionSpawnAudio)
    //    {

    //        EventSound3D snd = Instantiate(eventSound3DPrefab, minion.transform.position, Quaternion.identity, null);

    //        snd.audioSrc.clip = this.minionSpawnAudio;

    //        snd.audioSrc.minDistance = 5f;
    //        snd.audioSrc.maxDistance = 100f;

    //        snd.audioSrc.Play();
    //    }


    //}


    //void minionFootstepEventHandler(Vector3 pos) {

    //    if (minionSpawnAudio)
    //    {

    //        EventSound3D snd = Instantiate(eventSound3DPrefab, pos, Quaternion.identity, null);

    //        snd.audioSrc.clip = this.minionFootstepAudio[Random.Range(0, minionFootstepAudio.Length)];

    //        snd.audioSrc.minDistance = 5f;
    //        snd.audioSrc.maxDistance = 100f;

    //        snd.audioSrc.Play();
    //    }


    //}
}