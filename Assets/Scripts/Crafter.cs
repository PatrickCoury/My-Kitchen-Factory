using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Crafter : MonoBehaviour
{
    List<KeyValuePair<int, int>> inputInventory;
    KeyValuePair<int, int> outputInventory;
    public List<CraftingRecipes> craftingRecipes;
    public GameObject menuPrefab;
    private int selectedRecipe, craftTimer, speedLvl = 1, maxCraftTime = 720;
    public bool craftTime;

    // Start is called before the first frame update
    void Start()
    {
        craftTimer = 0;
        menuPrefab = Resources.Load("CrafterMenuPrefab") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (craftTime)
        {
            craft();
            craftTime = false;
        }
        
    }

    private void FixedUpdate()
    {
        timeTick();
    }

    private void timeTick()
    {
        if (!craftTime)
        {
            craftTimer++;
        }
        else
            return;
        if (craftTimer >= (maxCraftTime - (120 * speedLvl)))
            craftTime = true;
    }
    public abstract void craft();


    public List<CraftingRecipes> getValidRecipes(string machineType)
    {
        List<CraftingRecipes> tempList = new List<CraftingRecipes>();
        for(int i = 100; i<=10000; i++)
        {
            CraftingRecipes temp = new CraftingRecipes(i);
            if (temp.getMachineType() == machineType)
                tempList.Add(temp);
        }
        return tempList;
    }
}
