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
            sound.audioSrc.volume = 0.5f;
            sound.audioSrc.minDistance = 5f;
            sound.audioSrc.maxDistance = 25f;
            
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
}