using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectSecond : MonoBehaviour
{
    public void selected2()
    {
        GameObject.Find("Face").GetComponent<whoandwhere>().whatiselect = 2;
        GameObject.Find("Face").GetComponent<whoandwhere>().selectingTime = false;
    }
}
