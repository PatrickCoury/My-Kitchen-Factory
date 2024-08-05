using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class TitleCanvasHandler : MonoBehaviour
{
    public GameObject mainMenu, newGameMenu, loadGameMenu;
    string filePath;

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "SaveData");
        if (Directory.Exists(filePath))
        {

        }
        else
        {
            GameObject.Find("Continue").GetComponent<Button>().interactable = false;
            GameObject.Find("Continue").GetComponentInChildren<TextMeshProUGUI>().color = new Color32(46, 46, 46, 255);
        }
    }
    void Update()
    {
        
    }
    public void loadGame()
    {
        mainMenu.SetActive(false);
        loadGameMenu.SetActive(true);
    }
    public void newGame()
    {
        mainMenu.SetActive(false);
        newGameMenu.SetActive(true);
    }
    public void quit()
    {
        Application.Quit();
    }
}
