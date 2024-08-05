using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.IO;
using UnityEngine.SceneManagement;


/*Contains savegame logic
 */
public class PauseMenuHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public MapHandler mapHandler;
    public StoreHandler storeHandler;
    public PlayerHandler playerHandler;
    public MainSceneHandler mainSceneHandler;
    public GameObject overwriteWindow, saveSuccessMessage, menuMenu, saveMenu, saveSuccess, saveNameError, quitConfirmMenu, loadGameMenu;
    private string saveName;
    string json, filePath;


    void Start()
    {
        mapHandler = GameObject.Find("Map Handler").GetComponent<MapHandler>();
        storeHandler = GameObject.Find("Store Handler").GetComponent<StoreHandler>();
        playerHandler = GameObject.Find("Player").GetComponent<PlayerHandler>();
    }

    private void Awake()
    {
        Time.timeScale = 0;
    }
    private void OnDestroy()
    {
        Time.timeScale = 1;
    }
    public void loadGame()
    {
        loadGameMenu.SetActive(true);
        menuMenu.SetActive(false);
    }
    public void saveGame()
    {
        saveMenu.SetActive(true);
        menuMenu.SetActive(false);
    }

    public void closeSaveMenu()
    {
        saveMenu.SetActive(false);
        overwriteWindow.SetActive(false);
        menuMenu.SetActive(true);
        saveNameError.SetActive(false);
        saveName = null;
    }

    public void editSaveName(string s)
    {
        saveName = s;
    }
    public void saveToJson()
    {
        if (saveName == null)
        {
            saveNameError.SetActive(true);
        }
        else
        {
            SaveGame newSaveGame = new SaveGame(mapHandler.tileMap, saveName, DateTime.Now, mapHandler.mapSizeX, mapHandler.mapSizeY, mapHandler.buildMap, storeHandler.money, playerHandler.positionX, playerHandler.positionY, mainSceneHandler.hotbarID, mainSceneHandler.harvesterList);
            json = JsonUtility.ToJson(newSaveGame);
            filePath = Path.Combine(Application.persistentDataPath, "SaveData");
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            filePath = Path.Combine(filePath, saveName + ".json");
            if (File.Exists(filePath))
            {
                overwriteWindow.SetActive(true);
                overwriteWindow.GetComponentInChildren<TextMeshProUGUI>().text = "Warning: " + saveName + " already exists as a save! Overwrite data?";
                saveMenu.SetActive(false);
            }
            else
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(json);
                    }
                }
                print(filePath);
                saveSuccessMessage.SetActive(true);
                mainSceneHandler.recentlySaved = true;
                closeSaveMenu();
            }
        }
        
    }
    public void overwriteConfirm()
    {
        File.Delete(filePath);
        using (FileStream stream = new FileStream(filePath, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(json);
            }
        }
        print(filePath);
        saveSuccessMessage.SetActive(true);
        mainSceneHandler.recentlySaved = true;
        closeSaveMenu();
    }

    public void overwriteDeny()
    {
        overwriteWindow.SetActive(false);
        //overwriteWindow.GetComponentInChildren<TextMeshProUGUI>().text = "Warning: " + saveName + " already exists as a save! Overwrite data?";
        saveMenu.SetActive(true);
    }

    public void quit()
    {
        if (mainSceneHandler.recentlySaved)
        {
            Destroy(mapHandler.gameObject);
            SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
        }
        else
        {
            menuMenu.SetActive(false);
            quitConfirmMenu.SetActive(true);
        }
    }
    public void quitConfirm()
    {
        Destroy(mapHandler.gameObject);
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);    }
    public void quitDeny()
    {
        quitConfirmMenu.SetActive(false);
        menuMenu.SetActive(true);
    }
}
