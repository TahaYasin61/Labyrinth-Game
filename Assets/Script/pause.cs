using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pause : MonoBehaviour {



        public void TogglePause()
    {
        Time.timeScale = Mathf.Approximately(Time.timeScale, 0.0f) ? 1.0f : 0.0f;

        
        }


   
  



}
