using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class showyourCharacterSeriously : MonoBehaviour
    , IPointerEnterHandler
    , IPointerExitHandler
{
    public List<Text> information = new List<Text>();
    public RawImage thisimage;
    public int numberOfComponent = 5;

    void Start()
    {
        for (int i = 0; i < numberOfComponent; i++) // cost, name, atk, hp, effect
        {
            information.Add(GameObject.Find("WhatCard").transform.GetChild(i).GetComponent<Text>());
            information[i].text = "";
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        showthisCard();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        coverthisCard();
    }

    void showthisCard()
    {
        thisimage.transform.SetAsLastSibling();
        thisimage.color = new Color(thisimage.color.r, thisimage.color.g, thisimage.color.b, 1);
        for (int i = 0; i < numberOfComponent; i++) // cost, name, atk, hp, effect
            information.Add(GameObject.Find("WhatCard").transform.GetChild(i).GetComponent<Text>());
        information[0].text = "";
        information[1].text = this.GetComponent<whoandwhere>().Carddata.name.ToString();
        information[2].text = "위력 " + this.GetComponent<whoandwhere>().Carddata.atk.ToString();
        information[3].text = "체력 " + this.GetComponent<whoandwhere>().Carddata.hp.ToString();
        information[4].text = this.GetComponent<whoandwhere>().Carddata.effect.ToString();
    }

    void coverthisCard()
    {
        thisimage.transform.SetAsFirstSibling();
        thisimage.color = new Color(thisimage.color.r, thisimage.color.g, thisimage.color.b, 0);
        for (int i = 0; i < numberOfComponent; i++) // cost, name, atk, hp, effect
        {
            information[0].text = "";
            information.RemoveAt(0);
        }
    }
}
