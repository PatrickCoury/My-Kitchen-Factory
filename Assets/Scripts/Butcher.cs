using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butcher : Crafter
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        craftingRecipes = getValidRecipes("Butcher");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
