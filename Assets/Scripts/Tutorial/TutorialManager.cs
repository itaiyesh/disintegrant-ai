using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TutorialManager : MonoBehaviour
{

    public GameObject player;
    public GameObject TutorialPanel;
    public GameObject HealthPanel;
    public GameObject PlayerCountPanel;
    public GameObject WeaponsPanel;
    public GameObject AmmoPanel;
    public GameObject CrossHairPanel;
    public GameObject Door1;

    public GameObject HealthCollectable;
    public GameObject WeaponCollectable;

    public GameObject Door2;
    public GameObject enemy;
    public GameObject Door3;
    public GameObject LiftDoors;
    public Text TextElement;
    private string saved_text;
    public float writingDelay = 0.10f;

    void Awake()
    {
	    StartCoroutine(Room1());
    }

    IEnumerator Room1()
    {
        HealthPanel.SetActive(false);
        PlayerCountPanel.SetActive(false);
        AmmoPanel.SetActive(false);
        CrossHairPanel.SetActive(false);
        WeaponsPanel.SetActive(false);

        player.GetComponent<CharacterAttributes>().characterAttributes.Health = 85.0f;

        yield return StartCoroutine(WriteText("Welcome soldier!\nDuring this training session you will learn the basics of navigation, survival techniques, and weapons training."));
        yield return StartCoroutine(Wait(1.5f));

        // Enable look around
        player.GetComponent<CharacterInputController>().enabled = true;

        yield return StartCoroutine(WriteText("Use your mouse to look around your environment. Go ahead, try it now soldier!"));
        yield return StartCoroutine(Wait(1.5f));

        yield return StartCoroutine(WriteText("Great! Now it's time to learn how to navigate. Use the AWSD keys to move left, forward, backward and right respectively. Try them out now!"));

        // Enable movement
        player.GetComponent<CharacterInputController>().turnInputFilter = 5;
        player.GetComponent<CharacterInputController>().forwardInputFilter = 5;
        yield return StartCoroutine(WaitForMovement());

        yield return StartCoroutine(WriteText("Good job soldier! Now let's go over a few features of your heads-up display."));
        yield return StartCoroutine(Wait(2f));

        HealthPanel.SetActive(true); // Display health panel
        yield return StartCoroutine(WriteText("On your top left you can see your health."));
        yield return StartCoroutine(Wait(2f));

        PlayerCountPanel.SetActive(true); // Display player count panel
        yield return StartCoroutine(WriteText("On your top right you can see the number of remaining enemies."));
        yield return StartCoroutine(Wait(2.0f));

        WeaponsPanel.SetActive(true); // Display weapons panel
        yield return StartCoroutine(Wait(1f));
        player.GetComponent<WeaponController>().enabled = true; // Enable weapons
        yield return StartCoroutine(WriteText("On your right you can view your equipped weapons."));
        yield return StartCoroutine(Wait(2f));

        AmmoPanel.SetActive(true); // Display ammo panel
        yield return StartCoroutine(WriteText("And on your bottom right you can view your remaining ammo."));
        yield return StartCoroutine(Wait(2f));

        yield return StartCoroutine(WriteText("Keep up the good work soldier! Please proceed to the next room."));
        yield return StartCoroutine(Wait(1.0f));
        Door1.SetActive(false);
    }

    IEnumerator Room2()
    {
        yield return StartCoroutine(WriteText("There are two types of collectables that can help keep you alive in the fight to come."));
        yield return StartCoroutine(Wait(1.5f));

        yield return StartCoroutine(WriteText("The first type are health packs. Go ahead and pick up the health pack in front of you."));
        HealthCollectable.SetActive(true);
        yield return StartCoroutine(Wait(1.0f));
    }

    IEnumerator Room2_Weapon()
    {
        yield return StartCoroutine(WriteText("Good job! You can see that your health was restored."));
        yield return StartCoroutine(Wait(1.5f));

        yield return StartCoroutine(WriteText("The second type of collectable are weapons. You can pick up new weapons for an edge, or gain ammo for your existing weapons."));
        yield return StartCoroutine(Wait(2.0f));
        yield return StartCoroutine(WriteText("Go ahead and pick up the weapon in front of you."));
        WeaponCollectable.SetActive(true);
        yield return StartCoroutine(Wait(1.0f));
    }

    IEnumerator Room2_End()
    {
        yield return StartCoroutine(WriteText("Good job! To switch between your weapons, use the scrollwheel or the equivalent, or the \"C\" key. Try this now."));
        yield return StartCoroutine(WaitForScrollwheel());

        yield return StartCoroutine(WriteText("Great! You're all set to begin combat training. Please proceed to the next room."));
        Door2.SetActive(false);
        yield return StartCoroutine(Wait(1.0f));
    }

    IEnumerator Room3()
    {
        yield return StartCoroutine(WriteText("To shoot your weapon, click the left mouse button. You can hold the left mouse for rapid fire weapons such as the machine gun."));
        yield return StartCoroutine(Wait(2f));
        CrossHairPanel.SetActive(true);
        yield return StartCoroutine(WriteText("Notice the indicator in the center of the screen. This can help you aim better soldier!"));
        yield return StartCoroutine(Wait(2f));
        enemy.SetActive(true);
        yield return StartCoroutine(WriteText("Now kill the enemy player in front of you."));
        yield return StartCoroutine(Wait(3f));
        yield return StartCoroutine(WriteText("Once you're ready to continue, proceed to the final room."));
        Door3.SetActive(false);
        yield return StartCoroutine(Wait(1.0f));
    }

    IEnumerator Room4()
    {
        yield return StartCoroutine(WriteText("Congratulations! You have completed the basic training regimine! Now go eliminate all the agents of the Orient Empire!"));
        yield return StartCoroutine(Wait(2f));
        yield return StartCoroutine(WriteText("For glory! For survival! Go through the portal and make our people proud!"));
        LiftDoors.SetActive(false);
        yield return StartCoroutine(Wait(1.0f));
    }

    IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    private IEnumerator WaitForScrollwheel()
    {
        bool done = false;

        while (!done)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetAxis("Mouse ScrollWheel") < 0f || Input.GetKeyUp("c"))
            {
                done = true;
            }

            yield return null;
        }
    }

    private IEnumerator WaitForMovement()
    {
        bool left_done = false;
        bool forward_done = false;
        bool backward_done = false;
        bool right_done = false;

        while (!left_done || !forward_done || !backward_done || !right_done)
        {
            if (player.GetComponent<CharacterInputController>().Turn > 0.5)
            {
                right_done = true;
            }

            if (player.GetComponent<CharacterInputController>().Turn < -0.5)
            {
                left_done = true;
            }

            if (player.GetComponent<CharacterInputController>().Forward > 0.5)
            {
                forward_done = true;
            }

            if (player.GetComponent<CharacterInputController>().Forward < -0.5)
            {
                backward_done = true;
            }

            yield return null;
        }
    }

    // Type writer effect
    IEnumerator WriteText(string text, bool append = false)
    {
        if (!append)
        {
            TextElement.text = "";
        }

        yield return new WaitForSecondsRealtime(writingDelay);

        foreach (char c in text)
        {
            TextElement.text += c;
            yield return new WaitForSecondsRealtime(writingDelay);
        }
    }

}
