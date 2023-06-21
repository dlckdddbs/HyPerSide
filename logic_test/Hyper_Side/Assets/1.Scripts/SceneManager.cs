using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public void SceneChange()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("");
    }
}
