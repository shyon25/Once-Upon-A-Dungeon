using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectToAbandon : MonoBehaviour
{
    public int myname;
    public int trashcard;
    public int slot;

    void Start()
    {
        slot = GameObject.Find("TrashOne").GetComponent<updownTrashlist>().slot;
    }
    public void doAbandon()
    {
        myname = int.Parse(this.name.Substring(8, 1));
        if (canidoit(myname))
        {
            int howmany = GameObject.Find("TrashOne").GetComponent<updownTrashlist>().how_many_down * slot;
            trashcard = GameObject.Find("Deck").GetComponent<deck>().decklist[howmany + myname - 1];
            GameObject.Find("Deck").GetComponent<deck>().Getoutfromdeck(trashcard);
            GameObject.Find("Deck").GetComponent<whoandwhere>().selectingTime = false;
            GameObject.Find("TrashOne").transform.SetAsFirstSibling();
            GameObject.Find("TrashOne").transform.GetChild(11).gameObject.SetActive(true);
            GameObject.Find("EmptyField").GetComponent<fieldcards>().starry = false;
        }
        if(GameObject.Find("EmptyField").GetComponent<fieldcards>().secondChance == true)
            secondChance();
    }
    bool canidoit(int name)
    {
        return GameObject.Find("TrashOne").GetComponent<updownTrashlist>().how_many_down * slot + name <= GameObject.Find("Deck").GetComponent<deck>().decklist.Count;
    }

    public void doCancel()
    {
        GameObject.Find("Deck").GetComponent<whoandwhere>().selectingTime = false;
        GameObject.Find("TrashOne").transform.SetAsFirstSibling();
    }
    void secondChance()
    {
        GameObject.Find("TrashOne").transform.GetChild(11).gameObject.SetActive(false);
        GameObject.Find("EmptyField").GetComponent<fieldcards>().secondChance = false;
        int playerHere = GameObject.Find("EmptyField").GetComponent<fieldcards>().playerHere;
        GameObject.Find("EmptyField").transform.GetChild(playerHere).GetComponent<eachFieldcards>().abandonCard();
    }
}
