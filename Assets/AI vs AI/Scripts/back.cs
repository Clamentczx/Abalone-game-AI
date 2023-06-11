using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class back : MonoBehaviour
{
    public void PlayNowButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
}
