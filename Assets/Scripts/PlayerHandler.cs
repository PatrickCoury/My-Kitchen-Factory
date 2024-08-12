using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public int positionX, positionY;
    public float slowness;
    public float actualX, actualY;
    public int invLvl = 1;
    private KeyValuePair<int, int>[] inventory;
    private GameObject inventoryInstance;
    private MainSceneHandler mainSceneHandler;
    public GameObject inventoryPrefab;
    bool diag;
    int oneFrameBefore;
    void Start()
    {
        gameObject.transform.position = new Vector3(positionX, positionY, -2);
        actualX = (float)positionX;
        actualY = (float)positionY;
        inventory = new KeyValuePair<int, int>[6+(6*invLvl)];
        mainSceneHandler = GameObject.Find("MainSceneHandler").GetComponent<MainSceneHandler>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ((Vector2)gameObject.transform.position != new Vector2(positionX, positionY))
        {
            if (diag)
                gameObject.transform.position = (gameObject.transform.position + (new Vector3((.75f)*(positionX - gameObject.transform.position.x) / slowness, (.75f) * (positionY - gameObject.transform.position.y) / slowness)));
            else
                gameObject.transform.position = (gameObject.transform.position + (new Vector3((positionX - gameObject.transform.position.x) / slowness, (positionY - gameObject.transform.position.y) / slowness)));

            actualX = gameObject.transform.position.x;
            actualY = gameObject.transform.position.y;
        }
        
    }
    public void move(string direction)
    {
        
        switch (direction.ToLower())
            {
                case "up":
                if (Mathf.Abs((float)positionY - actualY) < 0.5f)
                {
                    if (Mathf.Abs((float)positionX - actualX) > 0.25f)
                        diag = true;
                    else
                        diag = false;
                    positionY += 1;
                }
                break;
                case "right":
                if (Mathf.Abs((float)positionX - actualX) < 0.5f)
                {
                    if (Mathf.Abs((float)positionY - actualY) > 0.25f)
                        diag = true;
                    else
                        diag = false;
                    positionX += 1;
                }
                    break;
                case "down":
                if (Mathf.Abs((float)positionY - actualY) < 0.5f)
                {
                    if (Mathf.Abs((float)positionX - actualX) > 0.25f)
                        diag = true;
                    else
                        diag = false;
                    positionY -= 1;
                }
                    break;
                case "left":
                if (Mathf.Abs((float)positionX - actualX) < 0.5f)
                {
                    if (Mathf.Abs((float)positionY - actualY) > 0.25f)
                        diag = true;
                    else
                        diag = false;
                    positionX -= 1;
                }
                    break;
                default:
                    break;
            }
        }

    public void openInventory()
    {
        if (inventoryInstance!=null)
            Destroy(inventoryInstance);
        else
        {
            inventoryInstance = Instantiate(inventoryPrefab, GameObject.Find("Main UI").transform);
            inventoryInstance.transform.Find("X Button").GetComponent<Button>().onClick.AddListener(openInventory);
            updateInventoryContents();
        }
    }

    public void updateInventoryContents()
    {
        Transform[] slots = new Transform[inventoryInstance.transform.Find("Slots").childCount];
        for(int i = 0; i < slots.Length; i++)
        {
            slots[i] = inventoryInstance.transform.Find("Slots").GetChild(i);
            if (i >= inventory.Length)
            {
                slots[i].GetComponent<Button>().interactable = false;
                slots[i].GetChild(0).gameObject.SetActive(true);
                slots[i].GetChild(0).GetComponentInChildren<Image>().color = new Color32(255, 0, 0, 155);
                slots[i].GetChild(0).GetChild(0).gameObject.SetActive(false);
            }
            else if (inventory[i].Value == 0)
            {
                slots[i].GetComponent<Button>().interactable = true;
                slots[i].GetChild(0).gameObject.SetActive(false);
                int iCopy = i;
                slots[i].GetComponent<Button>().onClick.RemoveAllListeners();
                slots[i].GetComponent<Button>().onClick.AddListener(() => slotLogic(iCopy));
            }
            else
            {
                ItemID tempItem = new ItemID(inventory[i].Key);
                slots[i].GetChild(0).gameObject.SetActive(true);
                slots[i].GetChild(0).GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                slots[i].GetChild(0).GetComponent<Image>().sprite = tempItem.getSprite();
                slots[i].GetChild(0).GetChild(0).gameObject.SetActive(true);
                slots[i].GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = inventory[i].Value.ToString();
                slots[i].GetComponent<Button>().onClick.RemoveAllListeners();
                int iCopy = i;
                slots[i].GetComponent<Button>().onClick.AddListener(() => slotLogic(iCopy));
            }
        }
    }
    private void slotLogic(int i)
    {
        Transform slot = inventoryInstance.transform.Find("Slots").GetChild(i);
        inventory[i] = mainSceneHandler.sendToHands(inventory[i]);
        updateInventoryContents();
    }
}
