using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public abstract class Crafter : MonoBehaviour
{
    public List<KeyValuePair<int, int>> inputInventory;
    public KeyValuePair<int, int> outputInventory = new KeyValuePair<int, int>(0,0), fuelInventory = new KeyValuePair<int, int>(0, 0);
    public List<CraftingRecipes> craftingRecipes;
    public CraftingRecipes selectedRecipe;
    public GameObject menuPrefab, recipeButtonPrefab, menu;
    public int craftTimer, invLvl = 2, speedLvl = 1, maxCraftTime = 720, requiredFuel = 0;
    public bool craftTime;
    MainSceneHandler mainSceneHandler;

    // Start is called before the first frame update
    public void Start()
    {
        inputInventory = new List<KeyValuePair<int, int>>();
        craftTimer = 0;
        menuPrefab = Resources.Load("CrafterMenuPrefab") as GameObject;
        recipeButtonPrefab = Resources.Load("RecipeSelectionButton") as GameObject;
        mainSceneHandler = GameObject.Find("MainSceneHandler").GetComponent<MainSceneHandler>();
    }

    // Update is called once per frame
    public void Update()
    {
        if (craftTime)
        {
            craft();
        }

    }

    private void FixedUpdate()
    {
        timeTick();
    }
    private void OnDestroy()
    {
        closeMenu();
    }

    private void timeTick()
    {
        if (!craftTime)
        {
            craftTimer++;
            if (menu != null)
                menu.transform.Find("Processing Screen").GetChild(0).GetComponent<Slider>().value = craftTimer;
        }
        else
            return;
        if (craftTimer >= (maxCraftTime - (120 * speedLvl)))
            craftTime = true;
    }
    public void craft()
    {
        if (selectedRecipe == null)
            return;
        int count = 0;
        List<int> alreadyDone = new List<int>();
            foreach (int input in selectedRecipe.getInputs())
            {
                foreach (KeyValuePair<int, int> inputSlot in inputInventory)
                {
                    if (inputSlot.Key == input && !alreadyDone.Contains(inputSlot.Key))
                    {
                        count++;
                        alreadyDone.Add(inputSlot.Key);
                    }
                }
            }
        alreadyDone = new List<int>();
        ItemID tempItem = new ItemID(selectedRecipe.getOutput());
        if (selectedRecipe.getInputs().Count == count && (selectedRecipe.getOutput() == outputInventory.Key || outputInventory.Key == 0) && outputInventory.Value < tempItem.getStackSize())
        {
            if (requiredFuel == 0 || (fuelInventory.Key == requiredFuel && fuelInventory.Value > 0))
            {
                if (requiredFuel != 0)
                    fuelInventory = new KeyValuePair<int, int>(fuelInventory.Key, fuelInventory.Value - 1);
                outputInventory = new KeyValuePair<int, int>(selectedRecipe.getOutput(), outputInventory.Value + 1);
                craftTimer = 0;
                craftTime = false;
                foreach (int input in selectedRecipe.getInputs())
                {
                    for (int i = 0; i < inputInventory.Count; i++)
                    {
                        if (inputInventory[i].Key == input && !alreadyDone.Contains(inputInventory[i].Key))
                        {
                            inputInventory[i] = new KeyValuePair<int, int>(inputInventory[i].Key, inputInventory[i].Value - 1);
                            alreadyDone.Add(inputInventory[i].Key);
                            if (inputInventory[i].Value <= 0)
                                inputInventory.RemoveAt(i);
                        }
                    }
                }
            }
        }
        if(menu!=null)
            updateInvDisplay();
    }


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
    public void openMenu()
    {
        
        if (menu == null)
        {
            
            menu = Instantiate(menuPrefab, GameObject.Find("Main UI").transform);
            updateDisplayName();
            menu.transform.Find("X Button").GetComponent<Button>().onClick.AddListener(() => closeMenu());
            if (selectedRecipe != null)
            {
                Transform processScreen = menu.transform.Find("Processing Screen");
                processScreen.transform.Find("Select Recipe Button").GetComponent<Button>().onClick.AddListener(openRecipeMenu);
                processScreen.GetComponentInChildren<Slider>().maxValue = maxCraftTime - (120 * speedLvl);
                processScreen.gameObject.SetActive(true);
                menu.transform.Find("Recipe Selection Screen").gameObject.SetActive(false);
                for (int i = 1; i <= 4; i++)
                {
                    Transform temp = processScreen.transform.Find("Input Button " + i);
                    if (i > invLvl)
                    {
                        temp.GetComponent<Button>().interactable = false;
                        temp.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                    }
                }
                
            }
            else
                openRecipeMenu();
        }
        updateInvDisplay();
    }
    public void closeMenu()
    {
        Destroy(menu);
    }
    public void updateInvDisplay()
    {
        
        ItemID tempItem;
        Transform display;
        Transform processScreen = menu.transform.Find("Processing Screen");
        for (int i = 1; i <= 4; i++)
        {
            processScreen.transform.Find("Input Button " + i).GetChild(0).gameObject.SetActive(false);
            processScreen.transform.Find("Input Button " + i).GetComponent<Button>().onClick.RemoveAllListeners();
            int iCopy = i - 1;
            processScreen.transform.Find("Input Button " + i).GetComponent<Button>().onClick.RemoveAllListeners();
            processScreen.transform.Find("Input Button " + i).GetComponent<Button>().onClick.AddListener(() => inputSlotLogic(iCopy));
        }
            for (int i = 1; i <= inputInventory.Count; i++)
            {
                if (inputInventory[i - 1].Value > 0)
                {
                    tempItem = new ItemID(inputInventory[i - 1].Key);
                    display = processScreen.transform.Find("Input Button " + i).GetChild(0);
                    display.GetComponent<Image>().sprite = tempItem.getSprite();
                    display.GetChild(0).GetComponent<TextMeshProUGUI>().text = inputInventory[i - 1].Value.ToString();
                    display.gameObject.SetActive(true);
                    
                }
            }
        processScreen.transform.Find("Output Button").GetComponent<Button>().onClick.RemoveAllListeners();
        processScreen.transform.Find("Output Button").GetComponent<Button>().onClick.AddListener(outputSlotLogic);
        display = processScreen.transform.Find("Output Button").GetChild(0);
        if (outputInventory.Value != 0)
        {
            tempItem = new ItemID(outputInventory.Key);
            display.GetComponent<Image>().sprite = tempItem.getSprite();
            display.GetChild(0).GetComponent<TextMeshProUGUI>().text = outputInventory.Value.ToString();
            display.gameObject.SetActive(true);
           
        }
        else
            display.gameObject.SetActive(false);
        if (requiredFuel == 0)
            processScreen.transform.Find("Fuel Button").gameObject.SetActive(false);
        else
        {
            processScreen.transform.Find("Fuel Button").gameObject.SetActive(true);
            processScreen.transform.Find("Fuel Button").GetComponent<Button>().onClick.RemoveAllListeners();
            processScreen.transform.Find("Fuel Button").GetComponent<Button>().onClick.AddListener(fuelSlotLogic);

        }
        if (fuelInventory.Value != 0)
        {
            tempItem = new ItemID(outputInventory.Key);
            display = processScreen.transform.Find("Fuel Button").GetChild(0);
            display.GetComponent<Image>().sprite = tempItem.getSprite();
            display.GetChild(0).GetComponent<TextMeshProUGUI>().text = fuelInventory.Value.ToString();
            display.gameObject.SetActive(true);
        }

    }

    public void inputSlotLogic(int i)
    {
        KeyValuePair<int, int> temp;
        if (inputInventory.Count <= i)
            temp = new KeyValuePair<int, int>(0, 0);
        else
        {
            temp = inputInventory[i];
            inputInventory.RemoveAt(i);
        }
        inputInventory.Add(mainSceneHandler.sendToHands(temp));
        for (i = 0; i < inputInventory.Count; i++)
            if (inputInventory[i].Value == 0)
                inputInventory.RemoveAt(i);
        updateInvDisplay();
    }

    public void fuelSlotLogic()
    {
        fuelInventory = mainSceneHandler.sendToHands(fuelInventory);
        updateInvDisplay();
    }

    public void outputSlotLogic()
    {
        if (!mainSceneHandler.handsFull)
            outputInventory = mainSceneHandler.sendToHands(outputInventory);
        updateInvDisplay();
    }

    public void selectRecipe(CraftingRecipes recipe)
    {
        this.selectedRecipe = recipe;
        //int i = menu.transform.Find("Recipe Selection Screen").GetChild(0).Find("Viewport").childCount;
        for(int i = 0; i< menu.transform.Find("Recipe Selection Screen").GetChild(0).Find("Viewport").childCount; i++)
        {
            Destroy(menu.transform.Find("Recipe Selection Screen").GetChild(0).Find("Viewport").GetChild(i).gameObject);
        }
        menu.transform.Find("Recipe Selection Screen").gameObject.SetActive(false);
        menu.transform.Find("Processing Screen").gameObject.SetActive(true);
        updateInvDisplay();
    }

    public void openRecipeMenu()
    {
        Transform recipeScreen = menu.transform.Find("Recipe Selection Screen");
        recipeScreen.gameObject.SetActive(true);
        menu.transform.Find("Processing Screen").gameObject.SetActive(false);
        int xCount = 0;
        int yCount = 1;
        foreach (CraftingRecipes recipe in craftingRecipes)
        {
            GameObject tempObject = Instantiate(recipeButtonPrefab, menu.transform.Find("Recipe Selection Screen").GetChild(0).Find("Viewport"));
            if (xCount < 5)
            {
                xCount++;
            }
            else
            {
                xCount = 1;
                yCount++;
            }
            tempObject.transform.localPosition = new Vector2(-285, 150);
            for (int i = 1; i <= xCount; i++)
                tempObject.transform.localPosition = new Vector2(tempObject.transform.localPosition.x + 95, tempObject.transform.localPosition.y);
            for (int i = 1; i <= yCount; i++)
                tempObject.transform.localPosition = new Vector2(tempObject.transform.localPosition.x, tempObject.transform.localPosition.y - 90);
            ItemID tempItem = new ItemID(recipe.getOutput());
            tempObject.GetComponent<Image>().sprite = tempItem.getSprite();
            CraftingRecipes recipeClone = recipe;
            tempObject.GetComponent<Button>().onClick.AddListener(() => selectRecipe(recipeClone));
        }
    }

    public abstract void updateDisplayName();
}
