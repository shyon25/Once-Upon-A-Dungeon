using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class showyourCardSeriously : MonoBehaviour
    ,IPointerEnterHandler
    ,IPointerExitHandler
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
        if(isThisACard())
            showthisCard();
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        coverthisCard();
    }

    void showthisCard()
    {
        thisimage.transform.SetAsLastSibling();
        thisimage.color = new Color(thisimage.color.r,thisimage.color.g,thisimage.color.b,1);
        for (int i = 0; i < numberOfComponent; i++) // cost, name, atk, hp, effect
            information.Add(GameObject.Find("WhatCard").transform.GetChild(i).GetComponent<Text>());
        information[0].text = this.GetComponent<showyourCard>().Carddata.cost.ToString();
        information[1].text = this.GetComponent<showyourCard>().Carddata.name.ToString();
        if (this.GetComponent<showyourCard>().Carddata.type != Type.SPELL)
            information[2].text = "위력 " + this.GetComponent<showyourCard>().Carddata.atk.ToString();
        if(this.GetComponent<showyourCard>().Carddata.type == Type.MERCENARY || this.GetComponent<showyourCard>().Carddata.type == Type.CHARACTER)
            information[3].text = "체력 " + this.GetComponent<showyourCard>().Carddata.hp.ToString();
        else if (this.GetComponent<showyourCard>().Carddata.type == Type.ITEM)
            information[3].text = "영양 " + this.GetComponent<showyourCard>().Carddata.hp.ToString();
        information[4].text = this.GetComponent<showyourCard>().Carddata.effect.ToString();
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

    bool isThisACard()
    {
        return this.GetComponent<showyourCard>().WhatIsMyPlace() <= GameObject.Find("Deck").GetComponent<deck>().decklist.Count - GameObject.Find("Cardlist Logo").GetComponent<showyourdecklist>().how_many_down;
    }
}
