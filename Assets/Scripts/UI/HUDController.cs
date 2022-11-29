using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

using UnityEngine.Events;
using System.Linq;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    private UnityAction<GameObject, Dictionary<string, object>, Dictionary<string, object>> characterAttributeEventListener;
    private UnityAction<GameObject, GameObject> weaponAddEventListener;
    private UnityAction<GameObject, GameObject> weaponRemoveEventListener;
    private UnityAction<GameObject, GameObject, AudioClip, Vector3> weaponFiredEventListener;
    private UnityAction<GameObject, GameObject, GameObject> weaponSwapEventListener;
    private GameManager gameManager;

    public GameObject player;
    private Text playerCount;
    private Text healthText;
    private Text ammoText;
    private SkinnedMeshRenderer silhouetteRenderer;
    private Material healthMaterial;

    private float yMin;
    private float yMax;

    private Dictionary<WeaponType, Animator> weaponAnimators = new Dictionary<WeaponType, Animator>();

    private Dictionary<WeaponType, GameObject> weaponCrosshairs = new Dictionary<WeaponType, GameObject>();

    void Awake()
    {
        characterAttributeEventListener = new UnityAction<GameObject, Dictionary<string, object>, Dictionary<string, object>>(characterAttributeEventHandler);
        weaponAddEventListener = new UnityAction<GameObject, GameObject>(weaponAddEventHandler);
        weaponRemoveEventListener = new UnityAction<GameObject, GameObject>(weaponRemoveEventHandler);
        weaponFiredEventListener = new UnityAction<GameObject, GameObject, AudioClip, Vector3>(weaponFiredEventHandler);
        weaponSwapEventListener = new UnityAction<GameObject, GameObject, GameObject>(weaponSwapEventHandler);

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("HUDWeaponContainer"))

        {
            WeaponType weaponType = obj.GetComponent<WeaponTag>().WeaponType;
            weaponAnimators[weaponType] = obj.GetComponent<Animator>();
        }

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("HUDWeaponCrosshair"))

        {
            WeaponType weaponType = obj.GetComponent<WeaponTag>().WeaponType;
            weaponCrosshairs[weaponType] = obj;
            weaponCrosshairs[weaponType].SetActive(false);
        }

        ammoText = GameObject.Find("AmmoText").GetComponent<Text>();
        playerCount = GameObject.Find("PlayerCountText").GetComponent<Text>();
        playerCount.text = $"{gameManager.numBots + 1}/{gameManager.numBots + 1} players";
        healthText = GameObject.Find("HealthText").GetComponent<Text>();
        silhouetteRenderer = GameObject.Find("PlayerSilhouette").GetComponentInChildren<SkinnedMeshRenderer>();
        healthMaterial = silhouetteRenderer.material;
        yMin = silhouetteRenderer.localBounds.center.y - silhouetteRenderer.localBounds.extents.y;
        yMax = silhouetteRenderer.localBounds.center.y + silhouetteRenderer.localBounds.extents.y;
        Vector2[] uvs = new Vector2[silhouetteRenderer.sharedMesh.vertices.Length];
        for (int i = 0; i < silhouetteRenderer.sharedMesh.vertices.Length; i++)
        {

            Vector3 vertex = silhouetteRenderer.sharedMesh.vertices[i];
            float scaledY = (1 / (yMax - yMin)) * (vertex.y - yMin);
            uvs[i] = new Vector2(scaledY, scaledY);
        }
        silhouetteRenderer.sharedMesh.uv2 = uvs;
        int initialHealth = 100; //TODO: Derive it from somewhere consistent
        healthMaterial.SetFloat("_Height", initialHealth / 100);
        healthText.text = $"{initialHealth}/100";
        OnEnable(); // we need to be ready to listen to weapon add events.
    }

    void OnEnable()
    {
        EventManager.StartListening<CharacterAttributeChangeEvent, GameObject, Dictionary<string, object>, Dictionary<string, object>>(characterAttributeEventListener);
        EventManager.StartListening<WeaponAddEvent, GameObject, GameObject>(weaponAddEventListener);
        EventManager.StartListening<WeaponRemoveEvent, GameObject, GameObject>(weaponRemoveEventListener);
        EventManager.StartListening<WeaponFiredEvent, GameObject, GameObject, AudioClip, Vector3>(weaponFiredEventListener);
        EventManager.StartListening<WeaponSwapEvent, GameObject, GameObject, GameObject>(weaponSwapEventListener);
    }

    void OnDisable()
    {
        EventManager.StopListening<CharacterAttributeChangeEvent, GameObject, Dictionary<string, object>, Dictionary<string, object>>(characterAttributeEventListener);
        EventManager.StopListening<WeaponAddEvent, GameObject, GameObject>(weaponAddEventListener);
        EventManager.StopListening<WeaponRemoveEvent, GameObject, GameObject>(weaponRemoveEventListener);
        EventManager.StopListening<WeaponFiredEvent, GameObject, GameObject, AudioClip, Vector3>(weaponFiredEventListener);
        EventManager.StopListening<WeaponSwapEvent, GameObject, GameObject, GameObject>(weaponSwapEventListener);
    }

    void characterAttributeEventHandler(
        GameObject triggeringPlayer,
        Dictionary<string, object> oldAttributes,
        Dictionary<string, object> newAttributes
    )
    {
        // Filter out any AI players
        if (triggeringPlayer != player)
            return;

        // Update health bar
        if (newAttributes.ContainsKey("Health"))
        {
            float healthValue = (float)newAttributes["Health"];
            int healthInt = (int)Math.Ceiling(healthValue);
            healthMaterial.SetFloat("_Height", healthValue / 100);
            healthText.text = $"{healthInt}/100";
        }

    }

    void weaponAddEventHandler(
        GameObject triggeringPlayer,
        GameObject weapon
    )
    {
        // Filter out any AI players
        if (triggeringPlayer != player)
            return;

        var weaponType = weapon.GetComponent<Weapon>().WeaponType;
        if (weaponAnimators.ContainsKey(weaponType)) weaponAnimators[weaponType].SetBool("active", true);
    }

    void weaponRemoveEventHandler(
        GameObject triggeringPlayer,
        GameObject weapon
    )
    {
        // Filter out any AI players
        if (triggeringPlayer != player)
            return;

        var weaponType = weapon.GetComponent<Weapon>().WeaponType;
        if (weaponAnimators.ContainsKey(weaponType)) weaponAnimators[weaponType].SetBool("active", false);
    }

    // Get active weapon and update ammo count
    void weaponFiredEventHandler(
        GameObject triggeringPlayer,
        GameObject weapon,
        AudioClip clip,
        Vector3 position
    )
    {
        // Filter out any AI players
        if (triggeringPlayer != player)
            return;

        string ammo = $"{weapon.GetComponent<Weapon>().Ammo}";
        if (weapon.GetComponent<Weapon>().Ammo == 1.0f / 0.0f) // if ammo is infinite, change to infinity symbol
        {
            ammo = "INF";
        }
        ammoText.text = $"{ammo}";

    }

    void weaponSwapEventHandler(
        GameObject triggeringPlayer,
        GameObject oldWeapon,
        GameObject newWeapon
    )
    {
        // Filter out any AI players
        if (triggeringPlayer != player)
            return;

        WeaponType weaponType;
        if (oldWeapon != null)
        {
            weaponType = oldWeapon.GetComponent<Weapon>().WeaponType;
            if (weaponAnimators.ContainsKey(weaponType)) weaponAnimators[weaponType].SetBool("selected", false);
            weaponCrosshairs[weaponType].SetActive(false);
        }
        var weapon = newWeapon.GetComponent<Weapon>();
        weaponType = weapon.WeaponType;
        if (weaponAnimators.ContainsKey(weaponType)) weaponAnimators[weaponType].SetBool("selected", true);
        weaponCrosshairs[weaponType].SetActive(true);

        string ammo = $"{weapon.GetComponent<Weapon>().Ammo}";
        if (weapon.GetComponent<Weapon>().Ammo == 1.0f / 0.0f) // if ammo is infinite, change to infinity symbol
        {
            ammo = "INF";
        }
        ammoText.text = $"{ammo}";

    }

    private void LateUpdate()
    {
        // Update player count
        playerCount.text = $"{gameManager.bots.Count + 1}/{gameManager.numBots + 1} players";
    }


}
