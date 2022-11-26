using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WallKiller : MonoBehaviour
{
    private Transform camera;
    // adapted from https://www.youtube.com/watch?v=xMFx9HfRknU
    public GameObject player;
    [SerializeField] private List<Iam_InTheWay> currentlyInTheWay;
    [SerializeField] private List<Iam_InTheWay> alreadyTransparent;

    private void Update()
    {
        getAllObjectsInTheWay();
        MakeObjectsTransperant();
        MakeObjectsSolid();
        
    }


    private void Awake()
    {
        currentlyInTheWay = new List<Iam_InTheWay>();
        alreadyTransparent = new List<Iam_InTheWay>();

        camera = this.gameObject.transform;
    }
    private void getAllObjectsInTheWay()
    {
        currentlyInTheWay.Clear();
        float cameraPlayerDistance = Vector3.Magnitude(camera.position - player.transform.position);

        Ray ray1_Forward = new Ray(camera.position, player.transform.position - camera.position);
        Ray ray1_Backward = new Ray(player.transform.position, camera.position - player.transform.position);

        var hits1_Forward = Physics.RaycastAll(ray1_Forward, cameraPlayerDistance);
        var hits1_Backward = Physics.RaycastAll(ray1_Backward, cameraPlayerDistance);

        foreach (var hit in hits1_Forward)
        {
            if (hit.collider.gameObject.TryGetComponent(out Iam_InTheWay inTheWay))
            {
                if (!currentlyInTheWay.Contains(inTheWay))
                {
                    currentlyInTheWay.Add(inTheWay);
                }
            }
        }

        foreach (var hit in hits1_Backward)
        {
            if (hit.collider.gameObject.TryGetComponent(out Iam_InTheWay inTheWay))
            {
                if (!currentlyInTheWay.Contains(inTheWay))
                {
                    currentlyInTheWay.Add(inTheWay);
                }
            }
        }
    }

    private void MakeObjectsTransperant()
    {
        for (int i = 0; i < currentlyInTheWay.Count; i++)
        {
            Iam_InTheWay inTheWay = currentlyInTheWay[i];
            
            if (!alreadyTransparent.Contains(inTheWay))
            {
                inTheWay.ShowTransperant();
                alreadyTransparent.Add(inTheWay);
            }
        }
    }

    private void MakeObjectsSolid()
    {
        for (int i = alreadyTransparent.Count - 1; i >= 0; i--)
        {
            Iam_InTheWay wasInTheWay = alreadyTransparent[i];

            
            if (!currentlyInTheWay.Contains(wasInTheWay))
            {
                wasInTheWay.ShowSolid();
                alreadyTransparent.Remove(wasInTheWay);
            }

        }
    }
}