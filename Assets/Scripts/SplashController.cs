using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashController : MonoBehaviour {

	public GameObject helpObject;
    void Start() {
        helpObject.SetActive(false);
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

    public void OnClickBegin() {
    	SceneManager.LoadScene("ARScene");
    }
}
