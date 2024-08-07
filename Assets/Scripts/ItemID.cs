using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemID
{
    private int ID, stackSize;
    private string name;
    private Sprite sprite;

    public ItemID(int ID)
    {
        this.ID = ID;
        stackSize = 50;
        this.sprite = Resources.LoadAll<Sprite>("TileID")[ID];
        switch (ID)
        {
            case 100:
                this.name = "Cow Unit";
                break;
            case 101:
                this.name = "Prime Beef";
                break;
            case 102:
                this.name = "Beef Sirloin";
                break;
            case 103:
                this.name = "Milk";
                break;
            case 200:
                this.name = "Chicken Unit";
                break;
            case 201:
                this.name = "Chicken Breast";
                break;
            case 202:
                this.name = "Chicken Thigh";
                break;
            case 203:
                this.name = "Eggs";
                break;
            case 300:
                this.name = "Pig Unit";
                break;
            case 301:
                this.name = "Pork Shoulder Butt";
                break;
            case 302:
                this.name = "Pork Belly";
                break;
            case 303:
                this.name = "Pork Loin";
                break;
            case 501:
                this.name = "Salmon";
                break;
            case 502:
                this.name = "Tuna";
                break;
            case 503:
                this.name = "Shrimp";
                break;
            case 104:
                this.name = "Potato";
                break;
            case 105:
                this.name = "Rice";
                break;
            case 106:
                this.name = "Wheat";
                break;
            case 107:
                this.name = "Tomato";
                break;
            case 108:
                this.name = "Lettuce";
                break;
            case 109:
                this.name = "Onion";
                break;
            case 110:
                this.name = "Soy Bean";
                break;
            case 111:
                this.name = "Fruit";
                break;

        }
    }

    public int getStackSize()
    {
        return stackSize;
    }

    public Sprite getSprite()
    {
        return sprite;
    }
}
