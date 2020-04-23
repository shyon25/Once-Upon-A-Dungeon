using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class hand : MonoBehaviour
    , IPointerClickHandler
    , IPointerExitHandler
{
    public enum state { mini, big };
    public state handState = state.mini;

    GameObject miniHand;
    GameObject bigHand;
    GameObject mainHand;
    void Start()
    {
        miniHand = GameObject.Find("MiniHandZone");
        bigHand = GameObject.Find("BigHandZone");
        mainHand = GameObject.Find("hand");
        makeYouMini();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        makeYouBig();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        makeYouMini();
    }
    public void makeYouBig()
    {
        float width = bigHand.GetComponent<RectTransform>().rect.width;
        float height = bigHand.GetComponent<RectTransform>().rect.height;
        mainHand.GetComponent<RectTransform>().position = bigHand.GetComponent<RectTransform>().position;
        mainHand.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        handState = state.big;
    }
    public void makeYouMini()
    {
        float width = miniHand.GetComponent<RectTransform>().rect.width;
        float height = miniHand.GetComponent<RectTransform>().rect.height;
        mainHand.GetComponent<RectTransform>().position = miniHand.GetComponent<RectTransform>().position;
        mainHand.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        handState = state.mini;
    }
}
