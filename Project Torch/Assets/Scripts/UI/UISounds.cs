﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UISounds : MonoBehaviour,ISubmitHandler, ISelectHandler, IPointerEnterHandler, IPointerClickHandler{
    public void OnPointerClick(PointerEventData eventData) {
        AkSoundEngine.PostEvent("ButtonPress", gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        AkSoundEngine.PostEvent("Scroll", gameObject);
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void ISelectHandler.OnSelect(BaseEventData eventData) {
        AkSoundEngine.PostEvent("Scroll", gameObject);
    }

    public void OnSubmit(BaseEventData eventData) {
        AkSoundEngine.PostEvent("ButtonPress", gameObject);
    }
}
