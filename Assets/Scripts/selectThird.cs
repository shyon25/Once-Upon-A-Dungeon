using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectThird : MonoBehaviour
{
    public void selected3()
    {
        GameObject.Find("Face").GetComponent<whoandwhere>().whatiselect = 3;
        GameObject.Find("Face").GetComponent<whoandwhere>().selectingTime = false;
    }
}
