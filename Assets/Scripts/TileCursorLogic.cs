using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Lives inside each tile, tells the cursor where to be and the MapHandler
public class TileCursorLogic : MonoBehaviour
{
    
    MapHandler mapHandler;
    //PlayerHandler playerHandler;
    MainSceneHandler mainSceneHandler;
    
    // Start is called before the first frame update
    void Start()
    {
        
        mapHandler = GameObject.Find("Map Handler").GetComponent<MapHandler>();
        mainSceneHandler = GameObject.Find("MainSceneHandler").GetComponent<MainSceneHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isTouchingMouse())
            hovering();
    }

    public void hovering()
    {
            mainSceneHandler.hoveredTile = gameObject;
    }
    public bool isTouchingMouse()
    {
        Vector2 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return gameObject.GetComponent<Collider2D>().OverlapPoint(point);
    }

}
