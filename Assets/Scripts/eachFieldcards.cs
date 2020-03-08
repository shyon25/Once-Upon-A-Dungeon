using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class eachFieldcards : MonoBehaviour
    , IPointerClickHandler
    , IPointerEnterHandler
    , IPointerExitHandler
{
    public int thisfield;
    public bool isThischecked;

    public fieldData FieldData;
    void Start()
    {
        isThischecked = false;
        CoverYourField();
    }

    void Update()
    {
        if(GameObject.Find("EmptyField").GetComponent<fieldcards>().didyouchecklevel)
        {
            thisfield = GameObject.Find("EmptyField").GetComponent<fieldcards>().fields[itsnumber()];
            FieldData = GameObject.Find("EmptyField").GetComponent<fieldcards>().fieldDatas[itsnumber()];
            if(thisfield == 0)
                openYourField();
            else if(thisfield == 14 || thisfield == 15)
                thisfield = 20 + GameObject.Find("Face").GetComponent<whoandwhere>().thefloor;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(isThischecked == true)
            ShowYourField();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        CoverYourField();
    }

    void ShowYourField()
    {
        Text text = GameObject.Find("WhatField").transform.GetChild(0).GetComponent<Text>();
        text.text = FieldData.name + "\n" + FieldData.text;
    }
    void CoverYourField()
    {
        Text text = GameObject.Find("WhatField").transform.GetChild(0).GetComponent<Text>();
        text.text = "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int playerpoint = GameObject.Find("EmptyField").GetComponent<fieldcards>().playerHere;
        if(isThischikai(playerpoint, itsnumber()))
            moveHere();
        if(isThischecked == false)
            openYourField();
        ShowYourField();
    }

    void openYourField()
    {
        Texture fieldimage = GameObject.Find("EmptyField").GetComponent<fieldcards>().fieldImages[thisfield];
        RawImage thisimage = this.GetComponent<RawImage>();
        thisimage.texture = fieldimage;
        isThischecked = true;
    }

    bool isThischikai(int from, int to)
    {
        int distance = from - to;
        if(distance == 1 || distance == -1 || distance == 4 || distance == -4)
            return true;
        else
            return false;
    }

    void moveHere()
    {
        int here = 0;
        here = itsnumber();
        GameObject.Find("EmptyField").GetComponent<fieldcards>().changePos(here);
        GameObject.Find("EmptyField").GetComponent<fieldcards>().characterPosChange();
        
    }

    int itsnumber()
    {
        int here = 0;
        if(this.name.Length == 6)
            here = int.Parse(this.name.Substring(5,1));
        else if(this.name.Length == 7)
            here = int.Parse(this.name.Substring(5,2));
        return here;
    }
}
