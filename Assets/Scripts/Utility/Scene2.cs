using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene2 : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.LoadScene("Intro_Interior", LoadSceneMode.Single);
    }

}
