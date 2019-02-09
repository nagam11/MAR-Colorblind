/*===============================================================================
Copyright (c) 2015-2018 PTC Inc. All Rights Reserved.

Copyright (c) 2015 Qualcomm Connected Experiences, Inc. All Rights Reserved.

Vuforia is a trademark of PTC Inc., registered in the United States and other
countries.
===============================================================================*/

using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using Vuforia;

public class TransitionManager : MonoBehaviour
{
    #region PRIVATE_MEMBER_VARIABLES
    float transitionCursor;
    float currentTime;
    bool playing;
    bool backward;
    BlackMaskBehaviour blackMaskBehaviour;
    MixedRealityController.Mode currentMode = MixedRealityController.Mode.HANDHELD_AR;
    [SerializeField] GameObject[] AROnlyObjects;
    [SerializeField] GameObject[] VROnlyObjects;
    [Range(0.1f, 5.0f)]
    [SerializeField] float transitionDuration = 1.5f; // seconds
    [SerializeField] Animator astronaut, drone;
    #endregion // PRIVATE_MEMBER_VARIABLES


    #region PUBLIC_MEMBER_VARIABLES
    static public bool isFullScreenMode = true;
    public float TransitionDuration { get { return this.transitionDuration; } }
    public bool InAR { get { return this.transitionCursor <= 0.66f; } }
    #endregion PUBLIC_MEMBER_VARIABLES


    #region MONOBEHAVIOUR_METHODS
    void Awake()
    {
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(ScheduleSetupMixedRealityMode);
    }

    void OnDestroy()
    {
        VuforiaARController.Instance.UnregisterVuforiaStartedCallback(ScheduleSetupMixedRealityMode);
    }

    void Start()
    {
        // At start we assume we are in AR
        this.transitionCursor = 0;

        this.blackMaskBehaviour = FindObjectOfType<BlackMaskBehaviour>();
        SetBlackMaskVisible(false, 0);

        this.currentTime = Time.realtimeSinceStartup;
    }

    void Update()
    {
        // We need to check if the video background is curently enabled
        // because Vuforia may restart the video background when the App is resumed
        // even if the app was paused in VR mode

        MixedRealityController.Mode mixedRealityMode = GetMixedRealityMode();

        if ((this.currentMode != mixedRealityMode) ||
            (this.InAR != VideoBackgroundManager.Instance.VideoBackgroundEnabled))
        {
            // mixed reality mode to switch to
            this.currentMode = mixedRealityMode;

            // When we transition to VR, we deactivate the Datasets
            // before setting the mixed reality mode.
            // so to reduce CPU usage, as tracking is not needed in this phase
            // (with AutoStopCameraIfNotRequired ON by default, camera/tracker
            //  will be turned off for performance optimization).

            if (this.currentMode == MixedRealityController.Mode.HANDHELD_VR ||
                this.currentMode == MixedRealityController.Mode.VIEWER_VR)
            {
                Debug.Log("Switching to VR: deactivating datasets");
                ActivateDataSets(false);
            }

            // As we are moving back to AR, we re-activate the Datasets,
            // before setting the mixed reality mode.
            // this will ensure that the Tracker and Camera are restarted,
            // in case they were previously stopped when moving to VR
            // before activating the AR mode
            if (this.currentMode == MixedRealityController.Mode.HANDHELD_AR ||
                this.currentMode == MixedRealityController.Mode.VIEWER_AR)
            {
                Debug.Log("Switching to AR: activating datasets");
                ActivateDataSets(true);
            }

            MixedRealityController.Instance.SetMode(this.currentMode);
            UpdateVisibleObjects();
        }

        float time = Time.realtimeSinceStartup;
        float deltaTime = Mathf.Clamp01(time - this.currentTime);
        this.currentTime = time;

        if (playing)
        {
            float fadeFactor = 0;
            if (this.transitionCursor < 0.33f)
            {
                // fade to full black in first part of transition
                fadeFactor = Mathf.SmoothStep(0, 1, this.transitionCursor / 0.33f);
            }
            else if (this.transitionCursor < 0.66f)
            {
                // between 33% and 66% we stay in full black
                fadeFactor = 1;
            }
            else // > 0.66
            {
                // between 66% and 100% we fade out
                fadeFactor = Mathf.SmoothStep(1, 0, (this.transitionCursor - 0.66f) / 0.33f);
            }
            SetBlackMaskVisible(true, fadeFactor);

            float delta = (backward ? -1 : 1) * deltaTime / this.transitionDuration;
            this.transitionCursor += delta;

            if (this.transitionCursor <= 0 || this.transitionCursor >= 1)
            {
                // Done: stop animated transition
                this.transitionCursor = Mathf.Clamp01(this.transitionCursor);
                this.playing = false;
                SetBlackMaskVisible(false, 0);
            }
        }
    }
    #endregion // MONOBEHAVIOUR_METHODS


    #region PUBLIC_METHODS
    public void Play(bool reverse)
    {
        // dont' restart playing during a transition
        if (!this.playing)
        {
            this.playing = true;
            this.backward = reverse;
            this.transitionCursor = this.backward ? 1 : 0;
        }
    }
    #endregion // PUBLIC_METHODS


    #region PRIVATE_METHODS
    void ScheduleSetupMixedRealityMode()
    {
        StartCoroutine(WaitForFrame(SetupMixedRealityMode));
    }

    IEnumerator WaitForFrame(Action setupAction)
    {
        yield return new WaitForEndOfFrame();
        setupAction();
    }

    // on Vuforia Started
    void SetupMixedRealityMode()
    {
        this.currentMode = GetMixedRealityMode();
        MixedRealityController.Instance.SetMode(this.currentMode);

        UpdateVisibleObjects();
    }

    MixedRealityController.Mode GetMixedRealityMode()
    {
        if (this.InAR)
        {
            return isFullScreenMode ?
                MixedRealityController.Mode.HANDHELD_AR : MixedRealityController.Mode.VIEWER_AR;
        }

        return isFullScreenMode ?
            MixedRealityController.Mode.HANDHELD_VR : MixedRealityController.Mode.VIEWER_VR;
    }

    void ActivateDataSets(bool enableDataset)
    {
        ObjectTracker objectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();

        // We must stop the ObjectTracker before activating/deactivating datasets
        if (objectTracker.IsActive)
            objectTracker.Stop();

        if (!objectTracker.IsActive)
        {
            IEnumerable<DataSet> datasets = objectTracker.GetDataSets();
            foreach (DataSet dataset in datasets)
            {
                // Activate or Deactivate each DataSet
                if (enableDataset)
                    objectTracker.ActivateDataSet(dataset);
                else
                    objectTracker.DeactivateDataSet(dataset);
            }
        }

        if (!objectTracker.IsActive)
            objectTracker.Start();
    }

    void UpdateVisibleObjects()
    {
        foreach (var go in this.VROnlyObjects)
        {
            go.SetActive(!this.InAR);
        }

        // Start Astronaut and Drone animations in VR mode
        if (!this.InAR)
        {
            if (this.astronaut)
            {
                this.astronaut.SetBool("IsDrilling", !this.InAR);
            }

            if (this.drone != null)
            {
                this.drone.SetBool("IsScanning", !this.InAR);
                this.drone.SetBool("IsShowingLaser", !this.InAR);
                this.drone.SetBool("IsFacingObject", !this.InAR);
            }
        }
    }

    void SetBlackMaskVisible(bool visible, float fadeFactor)
    {
        if (this.blackMaskBehaviour && this.blackMaskBehaviour.enabled)
        {
            this.blackMaskBehaviour.GetComponent<Renderer>().enabled = visible;
            this.blackMaskBehaviour.SetFadeFactor(fadeFactor);
        }
    }

    #endregion PRIVATE_METHODS
}
