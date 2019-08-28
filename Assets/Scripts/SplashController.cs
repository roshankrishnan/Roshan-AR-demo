using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashController : MonoBehaviour {

	public Button startButton;
	public GameObject helpObject;

    void Start() {
        helpObject.SetActive(false);
        startButton.onClick.AddListener(() => OnStartClicked());
    }

    // Update is called once per frame
    void Update() {
    	if (Input.touchCount > 0) {
    		DisplayHelp();
    	}
    }

    void DisplayHelp() {
    	helpObject.SetActive(true);
    }

    public void OnStartClicked() {
    	SceneManager.UnloadSceneAsync("SplashScene");
    	SceneManager.LoadScene("ARScene");
    }
 
}
