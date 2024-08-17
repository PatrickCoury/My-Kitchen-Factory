using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stovetop : Crafter
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        craftingRecipes = getValidRecipes("Stovetop");
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    public override void updateDisplayName()
    {
        menu.transform.Find("Crafter").GetComponent<TextMeshProUGUI>().text = "Stovetop";
    }
}
