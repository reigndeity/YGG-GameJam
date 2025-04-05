using UnityEngine;
using UnityEngine.SceneManagement;

public class WebGLSplash : MonoBehaviour
{
    public string nextSceneName;

    void Start()
    {
        Invoke("OnWebGlEnd", 3);
    }

    public void OnWebGlEnd()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
