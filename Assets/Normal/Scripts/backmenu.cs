using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backmenu : MonoBehaviour
{
     public void PlayNowButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
}
