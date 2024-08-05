using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class MainMenuButtonLogic : MonoBehaviour, IPointerEnterHandler
{
    private GameObject arrow;
    private AudioSource selectFX, clickFX;
    private void Start()
    {
        arrow = GameObject.Find("Selection Arrow");
        selectFX = GameObject.Find("Selection SFX").GetComponent<AudioSource>();
        clickFX = GameObject.Find("Click SFX").GetComponent<AudioSource>();
    }
    void Update()
    {
            
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (EventSystem.current.currentSelectedGameObject!=this.gameObject&&gameObject.GetComponent<Button>().interactable)
        {
        selectFX.Play();
        EventSystem.current.SetSelectedGameObject(this.gameObject);
        arrow.transform.position = new Vector2(arrow.transform.position.x, this.transform.position.y - 5);
        }
        //throw new System.NotImplementedException();
    }
    public void playOnClick()
    {
        clickFX.Play();
    }
}
