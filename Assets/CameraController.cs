﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(!target) {
            Debug.LogError("Camera has no target!");
            return;
        }

        gameObject.transform.LookAt(target.transform);
	}
}
