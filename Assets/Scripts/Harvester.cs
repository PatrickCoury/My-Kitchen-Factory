using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;


//Contains the information for an attached harvester block
public class Harvester : MonoBehaviour
{

    private int rangeX, rangeY, rangeLvl = 3, speedLvl = 1, qtyLvl, costLvl, invLvl = 3;
    public bool showRange;
    public GameObject tilePrefab;
    public KeyValuePair<int, int>[] range;
    private GameObject[] rangeInstance;
    private int harvestTimer;
    private bool harvestTime;
    private MainSceneHandler mainSceneHandler;
    private MapHandler mapHandler;
    private Dictionary<int,bool> idsInRangeHarvested;
    private List<KeyValuePair<int, int>> inventory;
    private GameObject menuPrefab, menu;
    // Start is called before the first frame update
    void Start()
    {
        mainSceneHandler = GameObject.Find("MainSceneHandler").GetComponent<MainSceneHandler>();
        lvlUp("range", rangeLvl);
        //lvlUp("inventory", invLvl);
        mapHandler = GameObject.Find("Map Handler").GetComponent<MapHandler>();
        tilePrefab = Resources.Load("Square") as GameObject;
        range = new KeyValuePair<int, int>[rangeX * rangeY];
        rangeInstance = new GameObject[rangeX * rangeY];
        showRange = false;
        setRange();
        harvestTimer = 0;
        harvestTime = false;
        for(int i = 0; i<mainSceneHandler.harvesterList.Count;i++)
        {
            if(mainSceneHandler.harvesterList[i]==null)
                mainSceneHandler.harvesterList.RemoveAt(i);
        }
        setIDsInRange();
        menuPrefab = Resources.Load("HarvesterMenuPrefab") as GameObject;
        inventory = new List<KeyValuePair<int, int>>();
    }

    // Update is called once per frame
    void Update()
    {
        if (showRange)
        {
            displayRange();
        }
        if (harvestTime)
        {
            harvest();
        }
    }

    private void FixedUpdate()
    {
        timeTick();
    }

    private void OnDestroy()
    {
        if (showRange)
        {
            foreach (GameObject rangeTile in rangeInstance)
                Destroy(rangeTile);
        }
        mainSceneHandler.harvesterList.Remove(gameObject);
        updateRanges();
        if(menu!=null)
            closeMenu();
    }

    private void setRange()
    {
        bool alreadyInRange = false;
        int count;
        int rangeTemp;
        if (gameObject.transform.rotation.eulerAngles.z >=0&&gameObject.transform.rotation.eulerAngles.z <= 1)
        {//facing up
            count = 0;
            for (int x = (int)gameObject.transform.position.x - (rangeX / 2); x <= gameObject.transform.position.x + (rangeX / 2); x++)
            {
                for (int y = (int)gameObject.transform.position.y + 1; y <= gameObject.transform.position.y + rangeY; y++)
                {
                    for (int i = 0; i < mainSceneHandler.harvesterList.Count; i++)
                    {
                        if (mainSceneHandler.harvesterList[i] != null)
                        {
                            foreach (KeyValuePair<int, int> rangeCheck in mainSceneHandler.harvesterList[i].GetComponent<Harvester>().range)
                            {
                                if (mainSceneHandler.harvesterList[i]!=gameObject&&rangeCheck.Key == x && rangeCheck.Value == y)
                                    alreadyInRange = true;
                            }
                        }
                    }
                    if (!alreadyInRange)
                        range[count] = new KeyValuePair<int, int>(x, y);
                    else
                        range[count] = new KeyValuePair<int, int>(-1, -1);
                    alreadyInRange = false;
                    count++;
                }
            }
        }
        else if (gameObject.transform.rotation.eulerAngles.z > 1 && gameObject.transform.rotation.eulerAngles.z < 179)
        {//facing left
            count = 0;
            rangeTemp = rangeX;
            rangeX = rangeY;
            rangeY = rangeTemp;
            for (int x = (int)gameObject.transform.position.x - rangeX; x < gameObject.transform.position.x ; x++)
            {
                for (int y = (int)gameObject.transform.position.y - (rangeY / 2); y <= gameObject.transform.position.y + (rangeY / 2); y++)
                {
                    for(int i = 0; i < mainSceneHandler.harvesterList.Count; i++)
                    {
                        if (mainSceneHandler.harvesterList[i] != null)
                        {
                            foreach (KeyValuePair<int, int> rangeCheck in mainSceneHandler.harvesterList[i].GetComponent<Harvester>().range)
                            {
                                if (mainSceneHandler.harvesterList[i] != gameObject && rangeCheck.Key == x && rangeCheck.Value == y)
                                    alreadyInRange = true;
                            }
                        }
                    }
                    if (!alreadyInRange)
                        range[count] = new KeyValuePair<int, int>(x, y);
                    else
                        range[count] = new KeyValuePair<int, int>(-1, -1);
                    alreadyInRange = false;
                    count++;
                }
            }
            rangeTemp = rangeX;
            rangeX = rangeY;
            rangeY = rangeTemp;
        }
        else if (Mathf.Abs(gameObject.transform.rotation.eulerAngles.z) == 180)
        {//facing down
            count = 0;
            for (int x = (int)gameObject.transform.position.x - (rangeX / 2); x <= gameObject.transform.position.x + (rangeX / 2); x++)
            {
                for (int y = (int)gameObject.transform.position.y - 1; y >= gameObject.transform.position.y - rangeY; y--)
                {
                    for (int i = 0; i < mainSceneHandler.harvesterList.Count; i++)
                    {
                        if (mainSceneHandler.harvesterList[i] != null)
                        {
                            foreach (KeyValuePair<int, int> rangeCheck in mainSceneHandler.harvesterList[i].GetComponent<Harvester>().range)
                            {
                                if (mainSceneHandler.harvesterList[i] != gameObject && rangeCheck.Key == x && rangeCheck.Value == y)
                                    alreadyInRange = true;
                            }
                        }
                    }
                    if (!alreadyInRange)
                        range[count] = new KeyValuePair<int, int>(x, y);
                    else
                        range[count] = new KeyValuePair<int, int>(-1, -1);
                    alreadyInRange = false;
                    count++;
                }
            }
        }
        else if (gameObject.transform.rotation.eulerAngles.z >= 270)
        {//facing right
            count = 0;
        rangeTemp = rangeX;
        rangeX = rangeY;
        rangeY = rangeTemp;
        for (int x = (int)gameObject.transform.position.x + 1; x <= gameObject.transform.position.x + rangeX; x++)
        {
            for (int y = (int)gameObject.transform.position.y - (rangeY / 2); y <= gameObject.transform.position.y + (rangeY / 2); y++)
            {
                    for (int i = 0; i < mainSceneHandler.harvesterList.Count; i++)
                    {
                        if (mainSceneHandler.harvesterList[i] != null)
                        {
                            foreach (KeyValuePair<int, int> rangeCheck in mainSceneHandler.harvesterList[i].GetComponent<Harvester>().range)
                            {
                                if (mainSceneHandler.harvesterList[i] != gameObject && rangeCheck.Key == x && rangeCheck.Value == y)
                                    alreadyInRange = true;
                            }
                        }
                    }
                    if (!alreadyInRange)
                        range[count] = new KeyValuePair<int, int>(x, y);
                    else
                        range[count] = new KeyValuePair<int, int>(-1, -1);
                    alreadyInRange = false;
                count++;
            }
        }
        rangeTemp = rangeX;
        rangeX = rangeY;
        rangeY = rangeTemp;
        }
    }

    public void displayRange()
    {
        for(int i = 0; i<rangeInstance.Length; i++)
        {
            if (range[i].Key >= 0)
            {
                if (rangeInstance[i] != null)
                {
                    if (rangeInstance[i].transform.position == new Vector3(range[i].Key, range[i].Value, -1))
                        continue;
                    else
                        Destroy(rangeInstance[i]);
                }
                rangeInstance[i] = Instantiate(tilePrefab);
                rangeInstance[i].transform.position = new Vector3(range[i].Key, range[i].Value, -1);
                rangeInstance[i].GetComponent<SpriteRenderer>().color = new Color32(255, 0, 0, 155);
                rangeInstance[i].GetComponent<BoxCollider2D>().enabled = false;
                rangeInstance[i].GetComponent<TileCursorLogic>().enabled = false;

            }
        }
    }

    public void lvlUp(string lvlType, int lvl)
    {
        if (lvlType.ToLower() == "range")
        {
            rangeLvl = lvl;
            switch (rangeLvl)
            {
                case 1:
                    rangeX = 3;
                    rangeY = 3;
                    break;
                case 2:
                    rangeX = 5;
                    rangeY = 3;
                    break;
                case 3:
                    rangeX = 5;
                    rangeY = 4;
                    break;
                case 4:
                    rangeX = 5;
                    rangeY = 5;
                    break;
            }
        }
        if (lvlType.ToLower() == "inventory")
        {
            invLvl = lvl;
        }
    }

    private void timeTick()
    {
        if (!harvestTime)
        {
            harvestTimer++;
            if (menu != null)
                menu.GetComponentInChildren<Slider>().value = harvestTimer;
        }
        else
            return;
        if (harvestTimer >= (720 - (120 * speedLvl)))
            harvestTime = true;
    }

    private void updateRanges()
    {
        int X = (int)gameObject.transform.position.x;
        int Y = (int)gameObject.transform.position.y;
        for (int i = 0; i < range.Length; i++)
        {
            foreach (GameObject harvester in mainSceneHandler.harvesterList)
                if (harvester != null && harvester == gameObject)
                    harvester.GetComponent<Harvester>().range[i] = new KeyValuePair<int, int>(-1, -1);
        }
        foreach (GameObject harvester in mainSceneHandler.harvesterList)
        {
            if (harvester!=null&&Mathf.Abs(X - harvester.transform.position.x) < 8 || Mathf.Abs(Y - harvester.transform.position.y) < 8)
                harvester.GetComponent<Harvester>().setRange();
        }
    }

    private void harvest()
    {
        foreach(KeyValuePair<int, bool> harvestableTile in idsInRangeHarvested)
        {
            if (harvestableTile.Value)
                continue;
            else
            {
                int count = 0;
                for(int i = 0; i < range.Length; i++)
                {
                    if(range[i].Key >= 0)
                        if(mapHandler.tileMap[range[i].Key, range[i].Value]!=null&&mapHandler.tileMap[range[i].Key, range[i].Value].getID()==harvestableTile.Key|| mapHandler.tileMap[range[i].Key, range[i].Value].getID() == harvestableTile.Key + 10)
                        {
                            count++;
                        }
                }
                addToInventory(harvestableTile.Key, count);
                idsInRangeHarvested[harvestableTile.Key] = true;
                break;
            }
        }
        bool harvestLoopAND = true;
        foreach (KeyValuePair<int, bool> harvestableTile in idsInRangeHarvested)
            harvestLoopAND = harvestableTile.Value;
        if (harvestLoopAND)
            foreach (int key in idsInRangeHarvested.Keys.ToList<int>())
                idsInRangeHarvested[key] = false;
        harvestTime = false;
        harvestTimer = 0;
        if(menu!=null)
            updateInvDisplay();
    }

    private void setIDsInRange()
    {
        idsInRangeHarvested = new Dictionary<int, bool>();
        foreach (KeyValuePair<int,int> tile in range)
        {
            try
            {
                idsInRangeHarvested.Add(mapHandler.tileMap[tile.Key, tile.Value].getID(),false);
            }
            catch
            {
                continue;
            }
        }
        if (idsInRangeHarvested.ContainsKey(0))
            idsInRangeHarvested.Remove(0);
        if (idsInRangeHarvested.ContainsKey(4))
            idsInRangeHarvested.Remove(4);
        if (idsInRangeHarvested.ContainsKey(11))
            idsInRangeHarvested.Remove(11);
        if (idsInRangeHarvested.ContainsKey(12))
            idsInRangeHarvested.Remove(12);
        if (idsInRangeHarvested.ContainsKey(13))
            idsInRangeHarvested.Remove(13);
        if (idsInRangeHarvested.ContainsKey(15))
            idsInRangeHarvested.Remove(15);
    }

    private void addToInventory(int tileID, int amt)
    {
        int itemID;
        switch (tileID)
        {
            case 1:
                itemID = 100;
                break;
            case 2:
                itemID = 200;
                break;
            case 3:
                itemID = 300;
                break;
            case 5:
                float rng = Random.Range(0f,1f);
                if (rng < 0.34)
                    itemID = 501;
                else if (rng < 0.67)
                    itemID = 502;
                else
                    itemID = 503;
                break;
            default:
                itemID = 100;
                break;
        }
        bool containedInInventory = false;
        int inventoryLocation = -1;
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].Key == itemID)
                {
                    containedInInventory = true;
                    inventoryLocation = i;
                }
            }
        if (containedInInventory)
        {
            ItemID tempItem = new ItemID(itemID);
            if (inventory[inventoryLocation].Value + amt > tempItem.getStackSize())
            {
                if (inventory.Count < invLvl)
                {
                    inventory.Add(new KeyValuePair<int, int>(itemID, (amt - (tempItem.getStackSize() - inventory[inventoryLocation].Value))));
                }
                inventory[inventoryLocation] = new KeyValuePair<int, int>(itemID, tempItem.getStackSize());


            }
            else
                inventory[inventoryLocation] = new KeyValuePair<int, int>(itemID, inventory[inventoryLocation].Value + amt);
            return;
        }
        else if (inventory.Count < invLvl)
            inventory.Add(new KeyValuePair<int, int>(itemID, amt));
    }

    public void openMenu()
    {
        if (menu == null)
        {
            menu = Instantiate(menuPrefab,GameObject.Find("Main UI").transform);
            menu.GetComponentInChildren<Slider>().maxValue = 720 - (120 * speedLvl);
            menu.transform.Find("X Button").GetComponent<Button>().onClick.AddListener(() => closeMenu());
            menu.transform.Find("Display Range").GetComponent<Button>().onClick.AddListener(() => setShowRange());
            for (int i = 1; i <= 5; i++)
            {
                Transform temp = menu.transform.Find("Inventory Button " + i);
                if (i > invLvl)
                {
                    temp.GetComponent<Button>().interactable = false;
                    temp.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                }
            }
            updateInvDisplay();
        }
    }

    public void closeMenu()
    {
        Destroy(menu);
    }

    public void updateInvDisplay()
    {
        for (int i = 1; i <= 5; i++)
        {
            menu.transform.Find("Inventory Button " + i).GetChild(0).gameObject.SetActive(false);
            menu.transform.Find("Inventory Button " + i).GetComponent<Button>().onClick.RemoveAllListeners();
        }
        for (int i = 1; i<=inventory.Count;i++)
        {
            if (inventory[i - 1].Value > 0)
            {
                ItemID tempItem = new ItemID(inventory[i - 1].Key);
                Transform display = menu.transform.Find("Inventory Button " + i).GetChild(0);
                display.GetComponent<Image>().sprite = tempItem.getSprite();
                display.GetChild(0).GetComponent<TextMeshProUGUI>().text = inventory[i - 1].Value.ToString();
                display.gameObject.SetActive(true);
                int iCopy = i - 1;
                menu.transform.Find("Inventory Button " + i).GetComponent<Button>().onClick.AddListener(() => slotLogic(iCopy));
            }
        }
    }
    private void setShowRange()
    {
        if (showRange)
            foreach (GameObject rangeObject in rangeInstance)
                Destroy(rangeObject);
        showRange = !showRange;
    }

    public void slotLogic(int i)
    {
        KeyValuePair<int, int> temp = inventory[i];
        inventory.RemoveAt(i);
        inventory.Add(mainSceneHandler.sendToHands(temp));
        for(i = 0; i<inventory.Count;i++)
            if(inventory[i].Value==0)
                inventory.RemoveAt(i);
        updateInvDisplay();
    }

}
