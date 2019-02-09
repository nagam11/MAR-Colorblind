/*===============================================================================
Copyright (c) 2015-2018 PTC Inc. All Rights Reserved.

Copyright (c) 2015 Qualcomm Connected Experiences, Inc. All Rights Reserved.

Vuforia is a trademark of PTC Inc., registered in the United States and other
countries.
===============================================================================*/
using UnityEngine;
using Vuforia;

public class BlackMaskBehaviour : MonoBehaviour
{
    #region PRIVATE_MEMBER_VARIABLES
    float fadeFactor;
    Camera m_Camera;
    Renderer m_Renderer;
    #endregion //PRIVATE_MEMBER_VARIABLES

    #region MONOBEHAVIOUR_METHODS
    void Start()
    {
        m_Renderer = GetComponent<Renderer>();

        SetFadeFactor(0);

        VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
    }

    void Update()
    {
        if (m_Camera != null)
        {
            float fovX = 2.0f * Mathf.Atan(1.0f / m_Camera.projectionMatrix[0, 0]);
            float fovY = 2.0f * Mathf.Atan(1.0f / m_Camera.projectionMatrix[1, 1]);

            // Set black mask position at near clip plane
            float near = m_Camera.nearClipPlane;
            transform.localPosition = 1.05f * Vector3.forward * near;
            transform.localScale = new Vector3(
                16.0f * near * Mathf.Tan(fovX / 2),
                16.0f * near * Mathf.Tan(fovY / 2),
                1);
        }

        // Update black mask transparency
        // black mask becomes fully opaque (black) at half transition (0.5)
        // then, beyond 0.5, the black mask plane gradually becomes transparent again (until 1.0).
        m_Renderer.material.SetFloat("_Alpha", fadeFactor);
        m_Renderer.enabled = (fadeFactor > 0.02f && fadeFactor < 0.98f);
    }
    #endregion // MONOBEHAVIOUR_METHODS


    #region PUBLIC_METHODS
    public void SetFadeFactor(float ff)
    {
        fadeFactor = Mathf.Clamp01(ff);
    }
    #endregion // PUBLIC_METHODS


    #region VUFORIA_CALLBACKS
    void OnVuforiaStarted()
    {
        m_Camera = DigitalEyewearARController.Instance.PrimaryCamera ?? Camera.main;
    }
    #endregion // VUFORIA_CALLBACKS
}
