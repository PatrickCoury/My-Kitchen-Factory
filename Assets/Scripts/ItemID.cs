using System;
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
        try
        {
            this.sprite = Resources.LoadAll<Sprite>("TileID")[ID];
        }
        catch (Exception e)
        {
            this.sprite = Resources.LoadAll<Sprite>("TileID")[0];
        }
            
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
            case 112:
                this.name = "Steak";
                break;
            case 113:
                this.name = "Roast Beef";
                break;
            case 114:
                this.name = "Cheese";
                break;
            case 211:
                this.name = "Roasted Chicken Breast";
                break;
            case 212:
                this.name = "Chicken Wing";
                break;
            case 213:
                this.name = "Fried Egg";
                break;
            case 311:
                this.name = "Ground Pork";
                break;
            case 314:
                this.name = "Sausage";
                break;
            case 312:
                this.name = "Pork Chops";
                break;
            case 313:
                this.name = "Raw Bacon";
                break;
            case 315:
                this.name = "Cooked Bacon";
                break;
            case 511:
                this.name = "Cooked Salmon";
                break;
            case 512:
                this.name = "Tuna Sashimi";
                break;
            case 513:
                this.name = "Cooked Shrimp";
                break;
            case 514:
                this.name = "Shrimp Cocktail";
                break;
            case 124:
                this.name = "Raw Fries";
                break;
            case 134:
                this.name = "French Fries";
                break;
            case 115:
                this.name = "Cooked Rice";
                break;
            case 120:
                this.name = "Soy Bean Pulp";
                break;
            case 130:
                this.name = "Tofu";
                break;
            case 140:
                this.name = "Fried Tofu";
                break;
            case 121:
                this.name = "Fruit Pulp";
                break;
            case 131:
                this.name = "Jam";
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

    public int getID()
    {
        return ID;
    }
}
