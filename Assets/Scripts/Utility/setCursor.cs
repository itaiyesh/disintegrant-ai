using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setCursor : MonoBehaviour
{
    public Texture2D crosshair;

    // Start is called before the first frame update
    void Start()
    {
        // code below from: https://wintermutedigital.com/post/2020-01-29-the-ultimate-guide-to-custom-cursors-in-unity/
        //set the cursor origin to its centre. (default is upper left corner)
        Vector2 cursorOffset = new Vector2(crosshair.width / 2, crosshair.height / 2);

        //Sets the cursor to the Crosshair sprite with given offset 
        //and automatic switching to hardware default if necessary
        Cursor.SetCursor(crosshair, cursorOffset, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
