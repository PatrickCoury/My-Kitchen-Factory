using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class NewGameMenuHandler : MonoBehaviour
{
    public MapHandler mapHandler;
    public GameObject mainMenu;
    public Slider waterSlider,smoothnessSlider, cowSeedSlider, chickenSeedSlider, pigSeedSlider, fishSeedSlider, cowAmtSlider, chickenAmtSlider, pigAmtSlider, fishAmtSlider;
    public TextMeshProUGUI waterText,smoothnessText,cowSeed,chickenSeed,pigSeed,fishSeed,cowAmt,chickenAmt,pigAmt,fishAmt;

    private void Update()
    {
        updateWaterText();
        for(int i = 1; i <= 4; i++)
        {
            updateNodeText(i);
            updateAmountText(i);
        }
        updateSmoothnessText();
    }
    public void xButton()
    {
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }
    public void setDefault()
    {
        mapHandler.waterSeeds = 3;
        waterSlider.value = 3;
        mapHandler.grassSeeds = 7;
        mapHandler.cowSeeds = 2;
        cowSeedSlider.value = 2;
        mapHandler.chickenSeeds = 2;
        chickenSeedSlider.value = 2;
        mapHandler.pigSeeds = 2;
        pigSeedSlider.value = 2;
        mapHandler.fishSeeds = 2;
        fishSeedSlider.value = 2;
        mapHandler.cowAmount = 3;
        cowAmtSlider.value = 3;
        mapHandler.chickenAmount = 3;
        chickenAmtSlider.value = 3;
        mapHandler.pigAmount = 3;
        pigAmtSlider.value = 3;
        mapHandler.fishAmount = 3;
        fishAmtSlider.value = 3;
        mapHandler.smoothness = 2;
        smoothnessSlider.value = 2;
    }
    public void setSmoothness()
    {
        mapHandler.smoothness = (int)smoothnessSlider.value;
    }
    private void updateSmoothnessText()
    {
        switch (mapHandler.smoothness)
        {
            case 1:
                smoothnessText.text = "Chunky";
                break;
            case 2:
                smoothnessText.text = "Smooth-ish";
                break;
            case 3:
                smoothnessText.text = "Very Smooth";
                break;
            default:
                smoothnessText.text = "Uh oh!";
                break;
        }
    }
    public void setWaterRatio()
    {
        mapHandler.waterSeeds = (int)waterSlider.value;
        mapHandler.grassSeeds = 10-(int)waterSlider.value;
    }
    private void updateWaterText()
    {
        switch (mapHandler.waterSeeds)
        {
            case 1:
                waterText.text = "Not very much...";
                break;
            case 2:
                waterText.text = "A little bit.";
                break;
            case 3:
                waterText.text = "A fair amount";
                break;
            case 4:
                waterText.text = "Quite a bit!";
                break;
            case 5:
                waterText.text = "Too much!!!";
                break;
            default:
                waterText.text = "Uh oh!";
                break;
        }
    }
    public void setNode(int i)
    {
        switch (i)
        {
            case 1:
                mapHandler.cowSeeds = (int)cowSeedSlider.value;
                break;
            case 2:
                mapHandler.chickenSeeds = (int)chickenSeedSlider.value;
                break;
            case 3:
                mapHandler.pigSeeds = (int)pigSeedSlider.value;
                break;
            case 4:
                mapHandler.fishSeeds = (int)fishSeedSlider.value;
                break;

        }
    }
    private void updateNodeText(int i)
    {
        TextMeshProUGUI correctText = cowSeed;
        int correctAmount = 0;
        switch (i)
        {
            case 1:
                correctText = cowSeed;
                correctAmount = mapHandler.cowSeeds;
                break;
            case 2:
                correctText = chickenSeed;
                correctAmount = mapHandler.chickenSeeds;
                break;
            case 3:
                correctText = pigSeed;
                correctAmount = mapHandler.pigSeeds;
                break;
            case 4:
                correctText = fishSeed;
                correctAmount = mapHandler.fishSeeds;
                break;

        }
        switch (correctAmount)
        {
            case 1:
                correctText.text = "Just the one...";
                break;
            case 2:
                correctText.text = "A couple.";
                break;
            case 3:
                correctText.text = "Quite a few!";
                break;
            case 4:
                correctText.text = "Too many!!!";
                break;
            default:
                correctText.text = "Uh oh!";
                break;

        }
    }
    public void setAmount(int i)
    {
        switch (i)
        {
            case 1:
                mapHandler.cowAmount = (int)cowAmtSlider.value;
                break;
            case 2:
                mapHandler.chickenAmount = (int)chickenAmtSlider.value;
                break;
            case 3:
                mapHandler.pigAmount = (int)pigAmtSlider.value;
                break;
            case 4:
                mapHandler.fishAmount = (int)fishAmtSlider.value;
                break;

        }
    }
    private void updateAmountText(int i)
    {
        TextMeshProUGUI correctText = cowAmt;
        int correctAmount = 0;
        switch (i)
        {
            case 1:
                correctText = cowAmt;
                correctAmount = mapHandler.cowAmount;
                break;
            case 2:
                correctText = chickenAmt;
                correctAmount = mapHandler.chickenAmount;
                break;
            case 3:
                correctText = pigAmt;
                correctAmount = mapHandler.pigAmount;
                break;
            case 4:
                correctText = fishAmt;
                correctAmount = mapHandler.fishAmount;
                break;

        }
        switch (correctAmount)
        {
            case 1:
                correctText.text = "Not very many...";
                break;
            case 2:
                correctText.text = "A couple.";
                break;
            case 3:
                correctText.text = "A fair amount";
                break;
            case 4:
                correctText.text = "Quite a few!";
                break;
            case 5:
                correctText.text = "Too many!!!";
                break;
            default:
                correctText.text = "Uh oh!";
                break;
        }
    }
    public void generateMap()
    {
        mapHandler.newGame = true;
        SceneManager.LoadScene("Main Scene",LoadSceneMode.Single);
    }
}
