using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


/*Contains logic for all of the Canvas objects on the Main Scene
 *Hands over load game information from the Map Handler to other Handlers
 *Contains user input logic
 */
public class MainSceneHandler : MonoBehaviour
{
    private MapHandler mapHandler;
    private StoreHandler storeHandler;
    private PlayerHandler playerHandler;
    private CameraHandler cameraHandler;
    public Canvas pauseMenu, mainUI;
    public GameObject saveMenu;
    public bool recentlySaved = false;
    private SaveGame loadingSave;
    public AudioSource noBuild;
    private GameObject[] hotbarButtons;
    public int[] hotbarID;
    private int hotbarSelectedButton = -1;
    public GameObject hoveredTile, hands;
    GameObject cursor;
    Sprite defaultCursor;
    public float cursorRotation = 0;
    public List<GameObject> harvesterList;
    private bool paused = false;
    private KeyValuePair<int, int> itemInHands;
    public bool handsFull = false;
    // Start is called before the first frame update
    void Start()
    {
        cursor = GameObject.Find("Cursor");
        defaultCursor = cursor.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        mapHandler = GameObject.Find("Map Handler").GetComponent<MapHandler>();
        mapHandler.setMainSceneHandler(this);
        storeHandler = GameObject.Find("Store Handler").GetComponent<StoreHandler>();
        playerHandler = GameObject.Find("Player").GetComponent<PlayerHandler>();
        cameraHandler = GameObject.Find("Main Camera").GetComponent<CameraHandler>();
        hotbarButtons = new GameObject[10];
        hotbarID = new int[hotbarButtons.Length];
        for (int i = 0; i<hotbarButtons.Length; i++)
        {
            hotbarButtons[i] = GameObject.Find("Hotbar Button " + i);
            hotbarID[i] = -1;
        }
        if (mapHandler.newGame)
        {
            mapHandler.newGame = false;
            mapHandler.generateMap();
            storeHandler.money = 300;
            playerHandler.positionX = mapHandler.getClosest(0,mapHandler.mapSizeX / 2, mapHandler.mapSizeY / 2).Key;
            playerHandler.positionY = mapHandler.getClosest(0, mapHandler.mapSizeX / 2, mapHandler.mapSizeY / 2).Value;
            hotbarID[1] = 20;
            hotbarID[2] = 30;
        }
        else if (mapHandler.loadGame)
        {
            mapHandler.loadGame = false;
            loadingSave = mapHandler.loadingSave;
            mapHandler.loadMap();
            storeHandler.load(loadingSave.money);
            playerHandler.positionX = loadingSave.playerPositionX;
            playerHandler.positionY = loadingSave.playerPositionY;
            for (int i = 0; i < hotbarButtons.Length; i++)
            {
                hotbarID[i] = loadingSave.hotBarID[i];
            }
            for(int i = 0; i < loadingSave.buildMap.Length; i++)
            {
                if (loadingSave.buildMap[i] >= 0)
                {
                    build(loadingSave.buildMap[i], loadingSave.buildMapRotation[i],loadingSave.buildMapX[i],loadingSave.buildMapY[i]);
                }
            }
            
        }
        cameraHandler.maxZoomIn = 3;
        cameraHandler.maxZoomOut = mapHandler.mapSizeY / 4;
        cameraHandler.setZoom(mapHandler.mapSizeY / 8);

        updateHotBar();
        hoveredTile = mapHandler.instanceGrid[0, 0];
    }

    // Update is called once per frame
    void Update()
    {
        //money display
        foreach(Transform child in mainUI.transform)
        {
            switch (child.name)
            {
                case "Money":
                    child.GetComponent<TextMeshProUGUI>().text = "$ = " + storeHandler.money;
                    break;
            }
        }

        //sets cursor back if nothing is selected in hotbar
        if (hotbarSelectedButton < 0)
            cursor.transform.GetChild(0).rotation = Quaternion.Euler(0,0,180);

       
        playerInputs();
        setCursorSprite();
    }

    //sets the pause menu or additonal logic.
    private void pause()
    {
        if (saveMenu.activeSelf)
            pauseMenu.GetComponent<PauseMenuHandler>().closeSaveMenu();
        else if (pauseMenu.gameObject.activeSelf)
        {
            pauseMenu.GetComponent<PauseMenuHandler>().saveSuccess.SetActive(false);
            recentlySaved = false;
            pauseMenu.gameObject.SetActive(false);
            mainUI.gameObject.SetActive(true);
            paused = false;
            Time.timeScale = 1;
            
        }
        else
        {
            pauseMenu.gameObject.SetActive(true);
            mainUI.gameObject.SetActive(false);
            paused = true;
        }
    }

    //tries to spend the amount of money on the block and returns if you have enough
    public bool spend(TileID tile)
    {
        if (storeHandler.money - tile.getCost() >= 0)
        {
            storeHandler.money -= tile.getCost();
            return true;
        }
        else
            return false;
    }

    //gives money for tile passed to it
    public void sell(TileID tile)
    {
        storeHandler.money += tile.getCost();
    }

    //contains all logic for if a player can move on to a tile
    //TODO: add logic for rotation of bridge
    public bool validPlayerMove(int x, int y)
    {
        if (x >= 0 && y >= 0)
            if (x < mapHandler.mapSizeX && y < mapHandler.mapSizeY)//within bounds
            {
                if (mapHandler.buildMap[x, y] == null)//not a building
                {
                    if (mapHandler.tileMap[x, y].isGrass)//is grass
                        return true;
                }
                else if (mapHandler.buildMap[x, y].getID() == 20)//if building, is bridge
                    return true;
            }
        return false;
    }

    //run after making change to the hotbar, updates all sprites and objects inside of it
    private void updateHotBar()
    {
        for(int i = 0; i < hotbarButtons.Length; i++)
        {
            if (hotbarID[i] == -1)
                hotbarButtons[i].transform.GetChild(0).gameObject.SetActive(false);
            else
            {
                TileID tempTile = new TileID(hotbarID[i], 0, 0, 0);
                hotbarButtons[i].transform.GetChild(0).gameObject.SetActive(true);
                hotbarButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = tempTile.getSprite();
            }
        }
    }

    //sets a tileID to the provided button
    public void setHotbarID(int buttNum, int ID)
    {
        hotbarID[buttNum] = ID;
        updateHotBar();
    }

    //sets the hotbar button passed to it to selected in the EventSystem
    private void selectHotbarButton(int buttNum)
    {
        if (hotbarButtons[buttNum] == EventSystem.current.currentSelectedGameObject)
        {
            EventSystem.current.SetSelectedGameObject(null);
            hotbarSelectedButton = -1;
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(hotbarButtons[buttNum]);
            hotbarSelectedButton = buttNum;
            cursor.transform.GetChild(0).Rotate(0, 0, 180);
        }
    }

    //changes cursor to selected hotbar item or default cursor
    public void setCursorSprite()
    {
        if (handsFull)
        {
            cursor.SetActive(false);
            hands.SetActive(true);
            hands.transform.position = Input.mousePosition / mainUI.scaleFactor;
            ItemID tempItem = new ItemID(itemInHands.Key);
            hands.GetComponent<Image>().sprite = tempItem.getSprite();
            hands.GetComponentInChildren<TextMeshProUGUI>().text = itemInHands.Value.ToString();
            hands.transform.SetAsLastSibling();
        }
        else
        {
            cursor.SetActive(true);
            hands.SetActive(false);
            if (hotbarSelectedButton >= 0)
            {
                TileID tempTile = new TileID(hotbarID[hotbarSelectedButton], 0, 0, 0);
                cursor.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = tempTile.getSprite();
                cursor.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 155);
                cursor.transform.GetChild(0).localPosition = new Vector3(0, 0, -19);
            }
            else
            {
                cursor.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = defaultCursor;
                cursor.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color32(196, 117, 220, 255);
                cursor.transform.GetChild(0).localPosition = new Vector3(0, .75f, -19);
            }
        }
    }

    //builds an item in world and assigns it to a list if applicable
    public void build(int ID, float r, int x, int y)
    {
        GameObject tempObject = mapHandler.buildTile(ID, r, x, y);
        if (tempObject != null)
        {
            switch (ID)
            {
                case 30:
                    harvesterList.Add(tempObject);
                    break;
            }
        }
    }

    public void playerInputs()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            playerHandler.openInventory();
        }

        if (!paused)
        {
            //movement controls
            if (Input.GetKey(KeyCode.W))
                if (validPlayerMove(playerHandler.positionX, playerHandler.positionY + 1))
                    playerHandler.move("Up");
            if (Input.GetKey(KeyCode.S))
                if (validPlayerMove(playerHandler.positionX, playerHandler.positionY - 1))
                    playerHandler.move("Down");
            if (Input.GetKey(KeyCode.A))
                if (validPlayerMove(playerHandler.positionX - 1, playerHandler.positionY))
                    playerHandler.move("Left");
            if (Input.GetKey(KeyCode.D))
                if (validPlayerMove(playerHandler.positionX + 1, playerHandler.positionY))
                    playerHandler.move("Right");

            //camera controls
            if (Input.mouseScrollDelta.y < 0)
                cameraHandler.setZoom(cameraHandler.getZoom() + 1);
            else if (Input.mouseScrollDelta.y > 0)
                cameraHandler.setZoom(cameraHandler.getZoom() - 1);
            cameraHandler.move(playerHandler.actualX, playerHandler.actualY);
            if (!handsFull)
            {
                //hotbar controls
                if (Input.GetKeyDown(KeyCode.Alpha1))
                    selectHotbarButton(1);
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                    selectHotbarButton(2);
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                    selectHotbarButton(3);
                else if (Input.GetKeyDown(KeyCode.Alpha4))
                    selectHotbarButton(4);
                else if (Input.GetKeyDown(KeyCode.Alpha5))
                    selectHotbarButton(5);
                else if (Input.GetKeyDown(KeyCode.Alpha6))
                    selectHotbarButton(6);
                else if (Input.GetKeyDown(KeyCode.Alpha7))
                    selectHotbarButton(7);
                else if (Input.GetKeyDown(KeyCode.Alpha8))
                    selectHotbarButton(8);
                else if (Input.GetKeyDown(KeyCode.Alpha9))
                    selectHotbarButton(9);
                else if (Input.GetKeyDown(KeyCode.Alpha0))
                    selectHotbarButton(0);
                //moves cursor over the hovered tile
                if (hoveredTile != null)
                    cursor.transform.position = new Vector2(hoveredTile.transform.position.x, hoveredTile.transform.position.y);

                //left click
                if (Input.GetMouseButtonDown(0))
                {
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        if (mapHandler.buildMap[(int)hoveredTile.transform.position.x, (int)hoveredTile.transform.position.y] != null)
                        {
                            openMenu(hoveredTile);
                        }
                        if (hotbarSelectedButton >= 0)
                            build(hotbarID[hotbarSelectedButton], cursorRotation, (int)hoveredTile.transform.position.x, (int)hoveredTile.transform.position.y);
                    }
                }
                //right click
                else if (Input.GetMouseButtonDown(1))
                {
                    mapHandler.sellTile((int)hoveredTile.transform.position.x, (int)hoveredTile.transform.position.y);
                }
            
            //rotates a selected block
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (hotbarSelectedButton >= 0)
                {
                    cursor.transform.GetChild(0).Rotate(0, 0, 90);
                    cursorRotation = cursor.transform.GetChild(0).rotation.eulerAngles.z;

                }
            }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        if (mapHandler.buildMap[(int)hoveredTile.transform.position.x, (int)hoveredTile.transform.position.y] != null)
                        {
                            openMenu(hoveredTile);
                        }
                    }
                }
            }
        }

    }
    public int getTileID(int x, int y)
    {
        return mapHandler.tileMap[x, y].getID();
    }

    public void openMenu(GameObject tile)
    {
        TileID buildTile = mapHandler.buildMap[(int)tile.transform.position.x, (int)tile.transform.position.y];
        //GameObject buildTileInstance = mapHandler.instanceGrid[(int)tile.transform.position.x, (int)tile.transform.position.y];
        switch (buildTile.getID())
        {
            case 30:
                tile.GetComponent<Harvester>().openMenu();
                break;
        }
    }

    public KeyValuePair<int,int> sendToHands(KeyValuePair<int,int> slotItem)
    {
        if (handsFull)
        {
            
            if (slotItem.Key == itemInHands.Key)
            {
                ItemID tempItem = new ItemID(slotItem.Key);
                if (slotItem.Value + itemInHands.Value > tempItem.getStackSize())
                {
                    itemInHands = new KeyValuePair<int, int>(itemInHands.Key, (slotItem.Value + itemInHands.Value - tempItem.getStackSize()));
                    return new KeyValuePair<int, int>(slotItem.Key, tempItem.getStackSize());
                }
                else
                {
                    handsFull = false;
                    return new KeyValuePair<int, int>(slotItem.Key, slotItem.Value + itemInHands.Value);
                }
            }
            else
            {
                KeyValuePair<int, int> temp = itemInHands;
                itemInHands = slotItem;
                if (itemInHands.Key == 0 || itemInHands.Value == 0)
                    handsFull = false;
                return temp;
            }

        }
        else
        {
            itemInHands = slotItem;
            if (itemInHands.Key == 0 || itemInHands.Value == 0)
                handsFull = false;
            else
                handsFull = true;
            return new KeyValuePair<int, int>(0, 0);
        }
    }

}
