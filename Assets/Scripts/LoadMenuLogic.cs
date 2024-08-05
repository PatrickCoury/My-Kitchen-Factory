using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;


//Specifically for loading a game, either from the Main Menu or Main Scene
public class LoadMenuLogic : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject mainMenu, errorMessage, lastSavedButton, otherSavesPrefab, otherSavesScroll;
    string filepath;
    SaveGame[] allSaves;
    SaveGame loadingSave;
    MapHandler mapHandler;
    StoreHandler storeHandler;

    private void Start()
    {
        mapHandler = GameObject.Find("Map Handler").GetComponent<MapHandler>();
    }
    void Awake()
    {
        filepath = Path.Combine(Application.persistentDataPath, "SaveData");
        var info = new DirectoryInfo(filepath);
        var fileInfo = info.GetFiles().OrderByDescending(f => f.LastWriteTime).ToList();

        allSaves = new SaveGame[fileInfo.Capacity];
        int count = 0;
        foreach (FileInfo file in fileInfo)
        {
            using (StreamReader sr = file.OpenText())
            {
                allSaves[count] = JsonUtility.FromJson<SaveGame>(sr.ReadToEnd());
                count++;
            }
        }
        lastSavedButton.GetComponentInChildren<TextMeshProUGUI>().text = "  " + allSaves[0].saveName + " | " + allSaves[0].saveDate;
        GameObject[] otherSaves = new GameObject[fileInfo.Capacity];
        for(int i = 1; i < fileInfo.Capacity;i++)
        {
            otherSaves[i] = Instantiate(otherSavesPrefab, GameObject.Find("Other Saves Content").transform);
            //otherSaves[i].transform.position = new Vector2(otherSaves[i].transform.position.x, 175 - (45 * i));
            otherSaves[i].GetComponentInChildren<TextMeshProUGUI>().text = "  " + allSaves[i].saveName + " | " + allSaves[i].saveDate;
            int iCopy = i;
            otherSaves[i].GetComponent<Button>().onClick.AddListener(() => selectLoadingSave(iCopy));
        }
    }

    public void selectLoadingSave(int i)
    {
        loadingSave = allSaves[i];
    }

    //final execution of the load, hands it over to the Map Handler... stupidly.
    public void loadGame()
    {
        mapHandler.loadGame = true;
        mapHandler.loadingSave = loadingSave;
        SceneManager.LoadScene("Main Scene", LoadSceneMode.Single);
    }
    public void xButton()
    {
        gameObject.SetActive(false);
        mainMenu.SetActive(true);
    }
}
