using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToTheField : MonoBehaviour
{
    public void comeback()
    {
        SceneManager.LoadScene(2);
        GameObject.Find("Face").GetComponent<whoandwhere>().upFloor();
    }
}
