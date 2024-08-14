using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Crafter : MonoBehaviour
{
    KeyValuePair<int, int> inputInventory, outputInventory;
    private int selectedRecipe, craftTimer, speedLvl = 1;
    public bool craftTime;

    // Start is called before the first frame update
    void Start()
    {
        craftTimer = 0;
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
        if (craftTimer >= (720 - (120 * speedLvl)))
            craftTime = true;
    }
    public abstract void craft();
}
