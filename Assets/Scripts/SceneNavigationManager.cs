using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigationManager : MonoBehaviour
{
    public void OnSerenityButtonClicked()
    {
        SceneManager.LoadScene("LevelPrototype");
    }
    public void OnFearButtonClicked()
    {
        SceneManager.LoadScene("FearPrototype");
    }
}
