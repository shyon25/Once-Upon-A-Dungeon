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
            GameObject.Find("Face").GetComponent<whoandwhere>().selectingTime = false;
            GameObject.Find("TrashOne").transform.SetAsFirstSibling();
        }
    }
    bool canidoit(int name)
    {
        return GameObject.Find("TrashOne").GetComponent<updownTrashlist>().how_many_down * slot + name <= GameObject.Find("Deck").GetComponent<deck>().decklist.Count;
    }

    public void doCancel()
    {
        GameObject.Find("Face").GetComponent<whoandwhere>().selectingTime = false;
        GameObject.Find("TrashOne").transform.SetAsFirstSibling();
    }
}
