﻿using Events;
using Menu;
using UnityEngine;
using UnityEngine.Events;

public class AudioEventManager : MonoBehaviour
{
    public EventSound3D eventSound3DPrefab;

    public AudioClip[] gameMenuAudios;

    public AudioClip menuButtonHighlightedAudio;
    public AudioClip menuButtonClickedAudio;

    public AudioClip weaponSwapAudio;

    // public AudioClip[] minionJabberAudio = null;
    // public AudioClip[] boxAudio = null;
    // public AudioClip playerLandsAudio;
    // public AudioClip explosionAudio;
    // public AudioClip deathAudio;
    // public AudioClip bombBounceAudio;
    // public AudioClip jumpAudio;
    // public AudioClip gruntAudio;
    // public AudioClip minionDeathAudio;
    // public AudioClip minionOuchAudio;
    // public AudioClip minionSpawnAudio;
    // public AudioClip[] minionFootstepAudio;
    // public AudioClip punchAudio;

    // private UnityAction<Vector3,float> boxCollisionEventListener;
    //
    // private UnityAction<Vector3, float> playerLandsEventListener;
    //
    // private UnityAction<Vector3, float> minionLandsEventListener;
    //
    // private UnityAction<Vector3> minionJabberEventListener;
    //
    // private UnityAction<Vector3> explosionEventListener;
    //
    // private UnityAction<Vector3> bombBounceEventListener;
    //
    // private UnityAction<Vector3> jumpEventListener;
    //
    // private UnityAction<GameObject> deathEventListener;

    //private UnityAction<Vector3,MinionScript> minionDeathEventListener;

    //private UnityAction<MinionScript> minionSpawnEventListener;

    // private UnityAction<Vector3> minionFootstepEventListener;
    //
    // private UnityAction<Vector3> minionOuchEventListener;

    private UnityAction<bool> menuBackgroundAudioEventListener;

    private UnityAction<MenuButtonEventListener.ButtonEvent> menuButtonEventListener;

    private UnityAction<Vector3> weaponSwapEventListener;

    void Awake()
    {
        menuBackgroundAudioEventListener = new UnityAction<bool>(MenuBackgroundAudioEventHandler);
        menuButtonEventListener = new UnityAction<MenuButtonEventListener.ButtonEvent>(MenuButtonAudioEventHandler);
        weaponSwapEventListener = new UnityAction<Vector3>(WeaponSwapEventHandler);

        // boxCollisionEventListener = new UnityAction<Vector3,float>(boxCollisionEventHandler);
        //
        // playerLandsEventListener = new UnityAction<Vector3, float>(playerLandsEventHandler);

        //minionLandsEventListener = new UnityAction<Vector3, float>(minionLandsEventHandler);

        // explosionEventListener = new UnityAction<Vector3>(explosionEventHandler);

        //minionJabberEventListener = new UnityAction<Vector3>(minionJabberEventHandler);

        // bombBounceEventListener = new UnityAction<Vector3>(bombBounceEventHandler);
        //
        // jumpEventListener = new UnityAction<Vector3>(jumpEventHandler);
        //
        // deathEventListener = new UnityAction<GameObject>(deathEventHandler);

        //minionDeathEventListener = new UnityAction<Vector3,MinionScript>(minionDeathEventHandler);

        //minionSpawnEventListener = new UnityAction<MinionScript>(minionSpawnEventHandler);

        //minionFootstepEventListener = new UnityAction<Vector3>(minionFootstepEventHandler);

        //minionOuchEventListener = new UnityAction<Vector3>(minionOuchEventHandler);
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
        EventManager.StartListening<WeaponSwapEvent, Vector3>(weaponSwapEventListener);
        // EventManager.StartListening<BoxCollisionEvent, Vector3,float>(boxCollisionEventListener);
        // EventManager.StartListening<PlayerLandsEvent, Vector3, float>(playerLandsEventListener);
        //EventManager.StartListening<MinionLandsEvent, Vector3, float>(minionLandsEventListener);
        //EventManager.StartListening<MinionJabberEvent, Vector3>(minionJabberEventListener);
        // EventManager.StartListening<ExplosionEvent, Vector3>(explosionEventListener);
        // EventManager.StartListening<BombBounceEvent, Vector3>(bombBounceEventListener);
        // EventManager.StartListening<JumpEvent, Vector3>(jumpEventListener);
        // EventManager.StartListening<DeathEvent, GameObject>(deathEventListener);
        //EventManager.StartListening<MinionDeathEvent, Vector3, MinionScript>(minionDeathEventListener);
        //EventManager.StartListening<MinionSpawnEvent, MinionScript>(minionSpawnEventListener);
        //EventManager.StartListening<MinionFootstepEvent, Vector3>(minionFootstepEventListener);
        //EventManager.StartListening<MinionOuchEvent, Vector3>(minionOuchEventListener);
    }

    void OnDisable()
    {
        EventManager.StopListening<GameMenuBackgroundAudioEvent, bool>(menuBackgroundAudioEventListener);
        EventManager.StopListening<GameMenuButtonAudioEvent, MenuButtonEventListener.ButtonEvent>(
            menuButtonEventListener);
        EventManager.StopListening<WeaponSwapEvent, Vector3>(weaponSwapEventListener);

        // EventManager.StopListening<BoxCollisionEvent, Vector3,float>(boxCollisionEventListener);
        // EventManager.StopListening<PlayerLandsEvent, Vector3, float>(playerLandsEventListener);
        //EventManager.StopListening<MinionLandsEvent, Vector3, float>(minionLandsEventListener);
        //EventManager.StopListening<MinionJabberEvent, Vector3>(minionJabberEventListener);
        // EventManager.StopListening<ExplosionEvent, Vector3>(explosionEventListener);
        // EventManager.StopListening<BombBounceEvent, Vector3>(bombBounceEventListener);
        // EventManager.StopListening<JumpEvent, Vector3>(jumpEventListener);
        // EventManager.StopListening<DeathEvent, GameObject>(deathEventListener);
        //EventManager.StopListening<MinionDeathEvent, Vector3, MinionScript>(minionDeathEventListener);
        //EventManager.StopListening<MinionSpawnEvent, MinionScript>(minionSpawnEventListener);
        //EventManager.StopListening<MinionFootstepEvent, Vector3>(minionFootstepEventListener);
        //EventManager.StopListening<MinionOuchEvent, Vector3>(minionOuchEventListener);
    }

    private void WeaponSwapEventHandler(Vector3 position)
    {
        var sound = Instantiate(eventSound3DPrefab, position, Quaternion.identity, null);

        if (weaponSwapAudio)
        {
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