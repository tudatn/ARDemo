using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WelcomeCanvas : MonoBehaviour
{
    // load AR scene
    public void LoadARScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
