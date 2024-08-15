using System.Collections;
using System.Collections.Generic;

public class CraftingRecipes
{

    private List<int> inputs;
    private int output;
    private string machineType;
    private int tier;

    public CraftingRecipes(int output)
    {
        this.output = output;
        switch (output)
        {
            case 101:
            case 102:
                inputs.Add(100);
                machineType = "Butcher";
                tier = 1;
                break;
            case 103:
                inputs.Add(100);
                machineType = "Squeezer";
                tier = 1;
                break;
            case 201:
            case 202:
                inputs.Add(200);
                machineType = "Butcher";
                tier = 1;
                break;
            case 203:
                inputs.Add(200);
                machineType = "Squeezer";
                tier = 1;
                break;
            case 301:
            case 302:
            case 303:
                inputs.Add(300);
                machineType = "Butcher";
                tier = 1;
                break;
            case 304:
                inputs.Add(302);
                machineType = "Butcher";
                tier = 1;
                break;
            case 112:
                inputs.Add(101);
                machineType = "Stovetop";
                tier = 1;
                break;
            case 113:
                inputs.Add(102);
                machineType = "Oven";
                tier = 1;
                break;
            case 114:
                inputs.Add(114);
                machineType = "Fermenter";
                tier = 1;
                break;
            case 211:
                inputs.Add(201);
                machineType = "Oven";
                tier = 1;
                break;
            case 212:
                inputs.Add(202);
                machineType = "Fryer";
                tier = 1;
                break;
            case 213:
                inputs.Add(203);
                machineType = "Stovetop";
                tier = 1;
                break;
            case 311:
                inputs.Add(301);
                machineType = "Grinder";
                tier = 1;
                break;
            case 314:
                inputs.Add(311);
                machineType = "Oven";
                tier = 1;
                break;
            case 312:
                inputs.Add(302);
                machineType = "Stovetop";
                tier = 1;
                break;
            case 313:
                inputs.Add(303);
                machineType = "Butcher";
                tier = 1;
                break;
            case 315:
                inputs.Add(313);
                machineType = "Stovetop";
                tier = 1;
                break;
            case 511:
                inputs.Add(501);
                machineType = "Stovetop";
                tier = 1;
                break;
            case 512:
                inputs.Add(502);
                machineType = "Prep Surface";
                tier = 1;
                break;
            case 513:
                inputs.Add(503);
                machineType = "Steamer";
                tier = 1;
                break;
            case 514:
                inputs.Add(513);
                inputs.Add(107);
                machineType = "Prep Surface";
                tier = 1;
                break;
            case 124:
                inputs.Add(104);
                machineType = "Prep Surface";
                tier = 1;
                break;
            case 134:
                inputs.Add(124);
                machineType = "Fryer";
                tier = 1;
                break;
            case 115:
                inputs.Add(105);
                machineType = "Steamer";
                tier = 1;
                break;
            case 120:
                inputs.Add(110);
                machineType = "Grinder";
                tier = 1;
                break;
            case 130:
                inputs.Add(120);
                machineType = "Squeezer";
                tier = 1;
                break;
            case 140:
                inputs.Add(130);
                machineType = "Stovetop";
                tier = 1;
                break;
            case 121:
                inputs.Add(111);
                machineType = "Squeezer";
                tier = 1;
                break;
            case 131:
                inputs.Add(121);
                machineType = "Stovetop";
                tier = 1;
                break;
            default:
                machineType = "Gay";
                tier = 0;
                break;
        }
    }
    public string getMachineType()
    {
        return this.machineType;
    }
}
