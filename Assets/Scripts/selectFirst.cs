using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectFirst : MonoBehaviour
{
    public int whatcard1 = -1;
    public int whatcard2 = -1;
    public int whatcard3 = -1;
    public void selected1()
    {
        whatcard1 = GameObject.Find("Face").GetComponent<whoandwhere>().first;
        whatcard2 = GameObject.Find("Face").GetComponent<whoandwhere>().second;
        whatcard3 = GameObject.Find("Face").GetComponent<whoandwhere>().third;

        GameObject.Find("Deck").GetComponent<deck>().Intodeck(whatcard1);

        GameObject.Find("Deck").GetComponent<deck>().additionalDecklist.Remove(whatcard1);
        GameObject.Find("Deck").GetComponent<deck>().additionalDecklist.Remove(whatcard2);
        GameObject.Find("Deck").GetComponent<deck>().additionalDecklist.Remove(whatcard3);
        GameObject.Find("Deck").GetComponent<deck>().additionalDecklist.Add(whatcard2);
        GameObject.Find("Deck").GetComponent<deck>().additionalDecklist.Add(whatcard3);

        GameObject.Find("Face").GetComponent<whoandwhere>().selectingTime = false;
        GameObject.Find("SelectOne").transform.SetAsFirstSibling();
    }
}
