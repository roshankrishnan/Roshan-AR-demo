using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems; 
using System;

public class FurnitureController : MonoBehaviour {

	private ARRaycastManager arRaycastManager;
	private static List<ARRaycastHit> hits;
	public GameObject furniturePrefab;
    private GameObject furnitureSpawn;
    public GameObject configurationsObject;
    public GameObject arReminder;
    private bool isSpawned = false;

    private float holdTime = 0f;
    private Vector2 startPos;
    private float startTime;

    void Awake() {
    	arRaycastManager = GetComponent<ARRaycastManager>();
    	hits = new List<ARRaycastHit>();
        configurationsObject.SetActive(false);
    }

    void Update() {
        DetectSwipe();
    	if (Input.touchCount > 0) {
            holdTime += Input.GetTouch(0).deltaTime;
            if (isSpawned 
                && holdTime > Constants.TOUCH_DELAY
                && Input.GetTouch(0).phase == TouchPhase.Ended) {
                TryDeleteObject();
                holdTime = 0f;
            } else if (!isSpawned) {
                SpawnObject();
            }
    	}
    }
    void SpawnObject() {
        Vector2 touchPos = Input.GetTouch(0).position;
        if (arRaycastManager.Raycast(touchPos, hits, TrackableType.Planes)) {
            var pose = hits[0].pose;
            furnitureSpawn = Instantiate(furniturePrefab, pose.position, pose.rotation);
            isSpawned = true;
            furnitureSpawn.transform.position = pose.position;
            Debug.Log("Object spawned");
        }
    }

    void TryDeleteObject() {
        Ray touchRay = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        RaycastHit hit;
        if (Physics.Raycast(touchRay, out hit)) {
            if (hit.collider.CompareTag("Furniture")) {
                Destroy(hit.transform.gameObject);
                isSpawned = false;
                furnitureSpawn = null;
                Debug.Log("Object deleted");
            }
        }
    }

    void DetectSwipe() {
        if(Input.touches.Length > 0) {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began) {
                startPos = new Vector2(t.position.x/(float)Screen.width, t.position.y/(float)Screen.width);
                startTime = Time.time;
            }
            if (t.phase == TouchPhase.Ended) {
                if (Time.time - startTime > Constants.MAX_SWIPE_TIME) {
                    return;
                } 

                Vector2 endPos = new Vector2(t.position.x/(float)Screen.width, t.position.y/(float)Screen.width);
                Vector2 swipe = new Vector2(endPos.x - startPos.x, endPos.y - startPos.y);

                if (swipe.magnitude < Constants.MIN_SWIPE_DISTANCE) {
                    return;
                }

                if (Mathf.Abs (swipe.x) > Mathf.Abs (swipe.y) && swipe.x < 0
                    && furnitureSpawn != null) { 
                    configurationsObject.SetActive(true);
                }
            }
        }
    }

    public void OnClickKing() {
        furnitureSpawn.transform.localScale = new Vector3(Constants.KING_BED_WIDTH,
            furnitureSpawn.transform.localScale.y, Constants.LARGE_BED_LENGTH);
        configurationsObject.SetActive(false);
    }

     public void OnClickQueen() {
        furnitureSpawn.transform.localScale = new Vector3(Constants.QUEEN_BED_WIDTH,
            furnitureSpawn.transform.localScale.y, Constants.LARGE_BED_LENGTH);
        configurationsObject.SetActive(false);
    }

     public void OnClickDouble() {
        furnitureSpawn.transform.localScale = new Vector3(Constants.DOUBLE_BED_WIDTH,
            furnitureSpawn.transform.localScale.y, Constants.SMALL_BED_LENGTH);
        configurationsObject.SetActive(false);
    }

     public void OnClickTwin() {
        furnitureSpawn.transform.localScale = new Vector3(Constants.TWIN_BED_WIDTH,
            furnitureSpawn.transform.localScale.y, Constants.SMALL_BED_LENGTH);
        configurationsObject.SetActive(false);
    }


}