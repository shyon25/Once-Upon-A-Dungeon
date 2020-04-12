using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectSecond : MonoBehaviour
{
    public int whatcard1 = -1;
    public int whatcard2 = -1;
    public int whatcard3 = -1;
    public void selected2()
    {
        whatcard1 = GameObject.Find("Deck").GetComponent<whoandwhere>().first;
        whatcard2 = GameObject.Find("Deck").GetComponent<whoandwhere>().second;
        whatcard3 = GameObject.Find("Deck").GetComponent<whoandwhere>().third;

        GameObject.Find("Deck").GetComponent<deck>().Intodeck(whatcard2);

        GameObject.Find("Deck").GetComponent<deck>().additionalDecklist.Remove(whatcard1);
        GameObject.Find("Deck").GetComponent<deck>().additionalDecklist.Remove(whatcard2);
        GameObject.Find("Deck").GetComponent<deck>().additionalDecklist.Remove(whatcard3);
        GameObject.Find("Deck").GetComponent<deck>().additionalDecklist.Add(whatcard1);
        GameObject.Find("Deck").GetComponent<deck>().additionalDecklist.Add(whatcard3);

        GameObject.Find("Deck").GetComponent<whoandwhere>().selectingTime = false;

        GameObject.Find("TrashOne").GetComponent<updownTrashlist>().updateyourlist();

        GameObject.Find("SelectOne").transform.SetAsFirstSibling();
    }
}
