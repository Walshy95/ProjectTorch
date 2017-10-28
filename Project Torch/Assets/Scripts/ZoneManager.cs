﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
/// <summary>
/// Handles zones/zone transition
/// </summary>
public class ZoneManager : MonoBehaviour {
    #region Enums
    //All possible zones for the player to be in
    public enum ZoneNames
    {
        Battlefield,
        SullenVillage,
        ThrivingVillage,
        CastleOfMan,
        FortressOfDark
    }
    //Used to keep track of how to manipulate the black screen obj
    protected enum TransitionPhase
    {
        NONE,
        FadingOut,
        FadingIn
    }
    #endregion

    #region Public Fields
    //Reference to the black screen object for scene transitions
    public GameObject blackScreen;
    //Array of all possible zones in the game
    public GameObject[] zones;
    #endregion

    #region Private Fields
    //Reference to the camera's post process changer
    protected PostProcessChange profileChanger;
    //The material of the black screen, used for fading
    protected Material screenFade;
    //The current phase of transitioning
    protected TransitionPhase phase;
    //The player
    protected GameObject player;
    //Camera clamp
    protected float cameraMax;
    //Stuff to transition the player
    protected Zone nextZone;
    //Sorted dictionary of zones for easy lookup
    protected Dictionary<ZoneNames, Zone> zonesSorted;
    //The current zone the player is in
    protected Zone currentZone;
    #endregion

    #region Unity Defaults
    void Start () {
        profileChanger = Camera.main.GetComponent<PostProcessChange>();
        screenFade = blackScreen.GetComponent<Renderer>().material;
        player = GameObject.Find("Player");
        //Make black screen transparent at first
        blackScreen.SetActive(true);
        Color fadeColor = screenFade.color;
        fadeColor.a = 0f;
        screenFade.color = fadeColor;
        //Get all zone scripts on gameobjects
        //zones = GameObject.FindObjectsOfType(typeof(Zone)) as Zone[];
        //zones = GameObject.FindObjectsOfType(typeof(Zone)) as GameObject[];
        // ^^NOTE: This doesn't work because FindObjectsOfType doesn't get inactive objects
        //Sort by zone name for easy access/lookup
        zonesSorted = new Dictionary<ZoneNames, Zone>();
        Zone z;
        for (int i = 0; i < zones.Length; ++i)
        {
            z = zones[i].GetComponent<Zone>();
            zonesSorted.Add(z.zone, z);
        }
        //Set current zone
        currentZone = zonesSorted[ZoneNames.Battlefield];
        //Assign camera clamp values
        UpdateCameraClamp();
    }
	
	void Update () {
        //Set darkness to true if in the thriving village or fortress of dark
        profileChanger.darkness = (currentZone.zone == ZoneNames.ThrivingVillage || currentZone.zone == ZoneNames.FortressOfDark);
        //Keep the camera within the bounds
        ClampCamera();
        //Check to see if within range of a zone end
        switch(phase)
        {
            case TransitionPhase.NONE:
                //Check if player is past the endpoint
                if (player.transform.position.x >= currentZone.endPoint.transform.position.x)
                {
                    //ChangeZone(currentZone.endPoint.GetComponent<ZoneEndPoint>().nextZone); //Start transition to next zone
                    ChangeZone(GetNextZone());
                }
                break;
            case TransitionPhase.FadingIn:
                FadeIn();
                break;
            case TransitionPhase.FadingOut:
                FadeOut();
                break;
        }
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Executes fade out to a different zone
    /// </summary>
    /// <param name="_nextZone">The next zone to move to</param>
    /// <param name="newXCoordinate">Where to move the player to</param>
    protected void ChangeZone(Zone _nextZone)
    {
        phase = TransitionPhase.FadingOut;
        nextZone = _nextZone;
        //newPlayerXCoordinate = newXCoordinate;
    }
    /// <summary>
    /// Locks the camera within a bounds, based on the zone
    /// </summary>
    protected void ClampCamera()
    {
        if (Camera.main.transform.position.x < 0f)
            Camera.main.transform.position = new Vector3(0f, Camera.main.transform.position.y, Camera.main.transform.position.z);
        else if (Camera.main.transform.position.x > cameraMax)
            Camera.main.transform.position = new Vector3(cameraMax, Camera.main.transform.position.y, Camera.main.transform.position.z);
    }
    /// <summary>
    /// Updates where the camera should be clamped to
    /// </summary>
    protected void UpdateCameraClamp()
    {
        cameraMax = currentZone.endPoint.transform.position.x;
    }
    /// <summary>
    /// Activates/deactivates the zone game objects
    /// </summary>
    /// <param name="zone">Zone (script) to manipulate</param>
    /// <param name="active">Whether it should be active or inactive</param>
    protected void SetZoneActive(Zone zone, bool active)
    {
        zone.gameObject.SetActive(active);
    }
    /// <summary>
    /// Fades out into a black screen and then transitions to a new level
    /// </summary>
    protected void FadeOut()
    {
        if (screenFade.color.a < 1)
        {
            Color fadeColor = screenFade.color;
            fadeColor.a += Time.deltaTime;
            screenFade.color = fadeColor;
        }
        else
        {
            //If we get here, the screen is completely black, so...
            //Deactivate this zone
            SetZoneActive(currentZone, false);
            //Activate the next one
            SetZoneActive(nextZone, true);
            //Update current zone
            currentZone = nextZone;
            //Update the camera min/max
            UpdateCameraClamp();
            //Update player's position
            player.transform.position = new Vector3(0f, player.transform.position.y, player.transform.position.z);
            //Move to fade in phase
            phase = TransitionPhase.FadingIn;
        }
    }
    /// <summary>
    /// Fades the black screen back in
    /// </summary>
    protected void FadeIn()
    {
        if (screenFade.color.a > 0)
        {
            Color fadeColor = screenFade.color;
            fadeColor.a -= Time.deltaTime;
            screenFade.color = fadeColor;
        }
        else phase = TransitionPhase.NONE;
    }
    /// <summary>
    /// Determines the next zone to move the player to and returns it
    /// TODO: Figure out this algorithm based on flags
    /// </summary>
    /// <returns>Next zone the player should go to</returns>
    protected Zone GetNextZone()
    {
        return zonesSorted[ZoneNames.SullenVillage];
    }
#endregion
}