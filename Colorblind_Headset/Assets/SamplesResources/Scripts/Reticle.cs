/*============================================================================== 
Copyright (c) 2015-2018 PTC Inc. All Rights Reserved.

Copyright (c) 2015 Qualcomm Connected Experiences, Inc. All Rights Reserved. 

Vuforia is a trademark of PTC Inc., registered in the United States and other 
countries.   
==============================================================================*/
using UnityEngine;
using Vuforia;

public class Reticle : MonoBehaviour
{
    #region PRIVATE_METHODS
    const float Scale = 0.012f; // relative to viewport width
    Transform reticle;
    BackgroundPlaneBehaviour backgroundPlaneBehaviour;
    #endregion


    #region MONOBEHAVIOUR_METHODS
    void Start()
    {
        reticle = transform;
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
    }

    void OnDestroy()
    {
        VuforiaARController.Instance.UnregisterVuforiaStartedCallback(OnVuforiaStarted);
    }

    void Update()
    {
        Camera cam = DigitalEyewearARController.Instance.PrimaryCamera ?? Camera.main;

        if (cam.projectionMatrix.m00 > 0 || cam.projectionMatrix.m00 < 0)
        {
            // We adjust the reticle distance from camera
            if (VideoBackgroundManager.Instance.VideoBackgroundEnabled)
            {
                // When the frustum skewing is applied (e.g. in AR view),
                // we shift the Reticle at the background depth,
                // so that the reticle appears in focus in stereo view
                if (backgroundPlaneBehaviour)
                {
                    float bgDepth = backgroundPlaneBehaviour.transform.localPosition.z;

                    reticle.localPosition = (bgDepth > cam.nearClipPlane) ?
                        Vector3.forward * bgDepth :
                        Vector3.forward * (cam.nearClipPlane + 0.5f);
                }
            }
            else
            {
                // if the frustum is not skewed, then we apply a default depth (which works nicely in VR view)
                reticle.localPosition = Vector3.forward * (cam.nearClipPlane + 0.5f);
            }

            // We scale the reticle to be a small % of viewport width
            float localDepth = reticle.localPosition.z;
            float tanHalfFovX = 1.0f / cam.projectionMatrix[0, 0];
            float tanHalfFovY = 1.0f / cam.projectionMatrix[1, 1];
            float maxTanFov = Mathf.Max(tanHalfFovX, tanHalfFovY);
            float viewWidth = 2 * maxTanFov * localDepth;
            reticle.localScale = new Vector3(Scale * viewWidth, Scale * viewWidth, 1);
        }
    }
    #endregion // MONOBEHAVIOUR_METHODS

    #region VUFORIA_CALLBACK_METHODS
    void OnVuforiaStarted()
    {
        backgroundPlaneBehaviour = FindObjectOfType<BackgroundPlaneBehaviour>();
    }
    #endregion // VUFORIA_CALLBACK_METHODS
}
