using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAddCard : MonoBehaviour
{
    public void testAdd()
    {
        GameObject.Find("Deck").GetComponent<deck>().Intodeck(20);
        GameObject.Find("Deck").GetComponent<deck>().Intodeck(21);
    }
    public void testOut()
    {
        GameObject.Find("Deck").GetComponent<deck>().Getoutfromdeck(20);
    }
    public void testOut2()
    {
        GameObject.Find("Deck").GetComponent<deck>().Getoutfromdeck(21);
    }
    public void testIncrease()
    {
        GameObject.Find("Face").GetComponent<whoandwhere>().upFloor();
        GameObject.Find("Face").GetComponent<whoandwhere>().heal(1);
    }
    public void testDecrease()
    {
        GameObject.Find("Face").GetComponent<whoandwhere>().downFloor();
        GameObject.Find("Face").GetComponent<whoandwhere>().damage(1);
    }
}
