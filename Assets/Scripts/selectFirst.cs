using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectFirst : MonoBehaviour
{
    public void selected1()
    {
        GameObject.Find("Face").GetComponent<whoandwhere>().whatiselect = 1;
        GameObject.Find("Face").GetComponent<whoandwhere>().selectingTime = false;
    }
}
