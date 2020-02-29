using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class showyourCardSeriously : MonoBehaviour
{
    public List<Text> information = new List<Text>();

    public Camera mainCamera;
    

    private void Update()
    {
        
    }


    void showthisCard()
    {
        for(int i = 0; i < 5; i++) // cost, name, atk, hp, effect
            information.Add(GameObject.Find("WhatCard").transform.GetChild(i).GetComponent<Text>());
        information[0].text = "비용";
        information[1].text = "이름";
        information[2].text = "위력";
        information[3].text = "체력";
        information[4].text = "효과 이것저것";
    }

    void coverthisCard()
    {
        for (int i = 0; i < 5; i++) // cost, name, atk, hp, effect
        {
            information[0].text = "";
            information.RemoveAt(0);
        }
    }
}
