using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public int money;
    SaveGame loadingSave;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void load(int money)
    {
        this.money = money;
    }
}
