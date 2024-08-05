using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Handles camera position, zoom, camera wrapping
public class CameraHandler : MonoBehaviour
{
    public int maxZoomIn, maxZoomOut, zoomSpeed = 1;
    public float positionX, positionY;
    public int zoom;
    private float minX, minY, maxX, maxY;

    // Update is called once per frame
    void FixedUpdate()
    {
        gameObject.GetComponent<Camera>().orthographicSize = zoom;

        if ((Vector2)gameObject.transform.position != new Vector2(positionX, positionY))
        {
            float dX = (positionX - gameObject.transform.position.x) / 10;
            float dY = (positionY - gameObject.transform.position.y) / 10;
            gameObject.transform.position += new Vector3(dX, dY, 0);
        }
    }

    public void move(float x, float y)
    {
        positionX = x;
        positionY = y;
        if (positionX > maxX)
            positionX = maxX;
        else if (positionX < minX)
            positionX = minX;
        if (positionY > maxY)
            positionY = maxY;
        else if (positionY < minY)
            positionY = minY;
    }
    public void setZoom(int zoom)
    {
        this.zoom = zoom*zoomSpeed;
        if (zoom > maxZoomOut)
            this.zoom = maxZoomOut;
        if (zoom < maxZoomIn)
            this.zoom = maxZoomIn;
        

        //TODO: Figure out why math is so weird, probably the unity scaling?
        minX = 1.3f;
        maxX = GameObject.Find("Map Handler").GetComponent<MapHandler>().mapSizeX - 2.3f;
        for (int i = 2; i <= this.zoom; i++)
        {
            if (i % 2 == 0)
            {
                minX += 1.8f;
                maxX -= 1.8f;
            }
            else
            {
                minX += 1.75f;
                maxX -= 1.75f;
            }
        }
        minY = this.zoom - 0.5f;
        maxY = GameObject.Find("Map Handler").GetComponent<MapHandler>().mapSizeY - 0.5f - this.zoom;
        
    }
    public int getZoom()
    {
        return zoom;
    }
}
