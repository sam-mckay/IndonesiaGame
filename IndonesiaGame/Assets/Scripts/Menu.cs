using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Menu : MonoBehaviour
{
    GameObject playButton;
    Slider loadingBar;
    bool isPlayable;
    AsyncOperation sceneLoader;
    // Use this for initialization
    void Start ()
    {
        loadingBar = this.transform.FindChild("loadingBar").GetComponent<Slider>();
        loadingBar.value = 0;
        isPlayable = false;
        sceneLoader = SceneManager.LoadSceneAsync(1);
        sceneLoader.allowSceneActivation = false;
        playButton = this.transform.FindChild("PlayButton").gameObject;
	}
	
	// Update is called once per frame
	void Update ()
    {
        loadingBar.value = sceneLoader.progress;
        if (sceneLoader.progress == 0.9f && isPlayable)
        {
            sceneLoader.allowSceneActivation = true;
        }
    }

    public void Play()
    {
        playButton.SetActive(false);
        isPlayable = true;
    }
}
