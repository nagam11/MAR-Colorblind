/*==============================================================================
Copyright (c) 2017-2018 PTC Inc. All Rights Reserved.

Vuforia is a trademark of PTC Inc., registered in the United States and other
countries.
==============================================================================*/

using UnityEngine;

public class ARVRAboutManager : AboutManager
{
    #region PUBLIC_METHODS
    public void OnStartFullScreen(bool willRunFullScreen)
    {
        TransitionManager.isFullScreenMode = willRunFullScreen;
        LoadNextScene();
    }
    #endregion // PUBLIC_METHODS
}
