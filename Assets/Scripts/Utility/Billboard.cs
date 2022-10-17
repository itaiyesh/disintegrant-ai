using UnityEngine;

/**
 * Class to implement a "Billboard" in unity such that the object is always facing the camera.
 *
 * Useful for things like player health bars.
 */
public class Billboard : MonoBehaviour
{

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(transform.position + mainCamera.transform.forward);
    }
}
