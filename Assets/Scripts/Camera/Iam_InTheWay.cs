using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Iam_InTheWay : MonoBehaviour
{
    [SerializeField] public GameObject solidBody;
    [SerializeField] private GameObject transperantBody;


    private void Start()
    {
        // Debug.Log("GAME OBJECT: " + gameObject.name);
    }

    // Start is called before the first frame update
    private void Awake()
    {
        ShowSolid();
    }

    // Update is called once per frame
    public void ShowTransperant()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        //solidBody.GetComponent<MeshRenderer>().enabled = false;
      //transperantBody.SetActive(true);
    }

    public void ShowSolid()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        //solidBody.GetComponent<MeshRenderer>().enabled = true;
        //transperantBody.SetActive(false);
    }
}
