using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene3 : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.LoadScene("Tutorial", LoadSceneMode.Single);
    }

}
