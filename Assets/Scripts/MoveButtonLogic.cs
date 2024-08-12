using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//using UnityEngine.UI;

public class MoveButtonLogic : MonoBehaviour, IDragHandler
{
    public void OnDrag(PointerEventData eventData)
    {

        //if (this==EventSystem.current.currentSelectedGameObject)
        transform.parent.position = new Vector3(eventData.position.x, eventData.position.y, -2);
        transform.parent.SetAsLastSibling();
        //transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(eventData.position.x, eventData.position.y);
    }
}
