
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Custom class containing information for the tiles
public class TileID 
{
    private string name;
    private int ID=0, cost;
    public bool goesOnGrass, goesOnWater, isGrass, isWater;
    private Sprite sprite;
    private KeyValuePair<int, int> coords;
    private float rotation;
   
    public TileID(int ID, int xPosition, int yPosition, float rotation)
    {
        setID(ID);
        coords = new KeyValuePair<int, int>(xPosition, yPosition);
        this.rotation = rotation;
    }

    //attaches all relevant information for a tile based on the ID passed to it
    public void setID(int ID)
    {
        goesOnGrass = false;
        goesOnWater = false;
        isGrass = false;
        isWater = false;
        cost = 0;
        this.ID = ID;
        if(ID>=0)
            this.sprite = Resources.LoadAll<Sprite>("TileID")[ID];
        switch (ID)
        {
            case 0:
                this.name = "grass";
                isGrass = true;
                break;
            case 1:
                this.name = "cows";
                goesOnGrass = true;
                isGrass = true;
                break;
            case 11:
                this.name = "cows_middle";
                goesOnGrass = true;
                isGrass = true;
                break;
            case 2:
                this.name = "chickens";
                goesOnGrass = true;
                isGrass = true;
                break;
            case 12:
                this.name = "chickens_middle";
                goesOnGrass = true;
                isGrass = true;
                break;
            case 3:
                this.name = "pigs";
                goesOnGrass = true;
                isGrass = true;
                break;
            case 13:
                this.name = "pigs_middle";
                goesOnGrass = true;
                isGrass = true;
                break;
            case 4:
                this.name = "water";
                isWater = true;
                break;
            case 5:
                this.name = "fish";
                goesOnWater = true;
                isWater = true;
                break;
            case 15:
                this.name = "fish_middle";
                goesOnWater = true;
                isWater = true;
                break;
            case 20:
                this.name = "bridge";
                goesOnWater = true;
                this.cost = 3;
                isGrass = true;
                break;
            case 30:
                this.name = "harvester";
                goesOnGrass = true;
                this.cost = 10;
                break;
            case 31:
                this.name = "Butcher";
                goesOnGrass = true;
                this.cost = 10;
                break;
            default:
                this.name = "grass";
                isGrass = true;
                break;
        }
    }
    public int getID()
    {
        return ID;
    }
    public KeyValuePair<int,int> getCoords()
    {
        return coords;
    }
    public string getName()
    {
        return name;
    }

    //sets the position, rotation, sprite, name, and additional logic to the GameObject tile passed to it
    //returns the made game object
    public void matchTile(GameObject tile, int layer)
    {
        tile.transform.position = new Vector3(coords.Key, coords.Value, layer);
        tile.transform.Rotate(new Vector3(0,0,rotation));
        tile.GetComponent<SpriteRenderer>().sprite = sprite;
        tile.name = name;
        switch (this.ID)
        {
            case 30:
                tile.AddComponent<Harvester>();
                break;
            case 31:
                tile.AddComponent<Butcher>();
                break;
        }
    }
    public int getCost()
    {
        return cost;
    }
    public Sprite getSprite()
    {
        return sprite;
    }
    public float getRotation()
    {
        return rotation;
    }
}
