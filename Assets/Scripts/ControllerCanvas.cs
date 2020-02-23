using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControllerCanvas : MonoBehaviour
{
    // exit AR Scene
    public void ExitScene()
    {
        SceneManager.LoadScene("Demo");
    }
}
