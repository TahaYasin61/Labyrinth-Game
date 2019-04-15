using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class main : MonoBehaviour
{

    public void AnaMenu()
    {

        SceneManager.LoadScene("Start");
        Time.timeScale = 1f;

    }
}

