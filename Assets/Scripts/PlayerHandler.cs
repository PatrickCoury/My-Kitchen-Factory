using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public int positionX, positionY;
    public float slowness;
    public float actualX, actualY;
    bool diag;
    void Start()
    {
        gameObject.transform.position = new Vector3(positionX, positionY, -2);
        actualX = (float)positionX;
        actualY = (float)positionY;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ((Vector2)gameObject.transform.position != new Vector2(positionX, positionY))
        {
            if (diag)
                gameObject.transform.position = (gameObject.transform.position + (new Vector3((.75f)*(positionX - gameObject.transform.position.x) / slowness, (.75f) * (positionY - gameObject.transform.position.y) / slowness)));
            else
                gameObject.transform.position = (gameObject.transform.position + (new Vector3((positionX - gameObject.transform.position.x) / slowness, (positionY - gameObject.transform.position.y) / slowness)));

            actualX = gameObject.transform.position.x;
            actualY = gameObject.transform.position.y;
        }
        
    }
    public void move(string direction)
    {
        
        switch (direction.ToLower())
            {
                case "up":
                if (Mathf.Abs((float)positionY - actualY) < 0.5f)
                {
                    if (Mathf.Abs((float)positionX - actualX) > 0.25f)
                        diag = true;
                    else
                        diag = false;
                    positionY += 1;
                }
                break;
                case "right":
                if (Mathf.Abs((float)positionX - actualX) < 0.5f)
                {
                    if (Mathf.Abs((float)positionY - actualY) > 0.25f)
                        diag = true;
                    else
                        diag = false;
                    positionX += 1;
                }
                    break;
                case "down":
                if (Mathf.Abs((float)positionY - actualY) < 0.5f)
                {
                    if (Mathf.Abs((float)positionX - actualX) > 0.25f)
                        diag = true;
                    else
                        diag = false;
                    positionY -= 1;
                }
                    break;
                case "left":
                if (Mathf.Abs((float)positionX - actualX) < 0.5f)
                {
                    if (Mathf.Abs((float)positionY - actualY) > 0.25f)
                        diag = true;
                    else
                        diag = false;
                    positionX -= 1;
                }
                    break;
                default:
                    break;
            }
        }
}
