/*==============================================================================
Copyright (c) 2013-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Vuforia
{
    /// <summary>
    /// This class encapsulates functionality to detect various surface events
    /// (size, orientation changed) and delegate this to native.
    /// These are used by Unity Extension code and should usually not be called by app code.
    /// </summary>
    class IOSUnityPlayer : IUnityPlayer
    {
        private ScreenOrientation mScreenOrientation = ScreenOrientation.Unknown;

        /// <summary>
        /// Loads native plugin libraries on platforms where this is explicitly required.
        /// </summary>
        public void LoadNativeLibraries()
        {
#if ((UNITY_IPHONE || UNITY_IOS) && !UNITY_EDITOR)
            VuforiaWrapper.SetImplementation(new VuforiaNativeIosWrapper());
#endif
        }

        /// <summary>
        /// Initialized platform specific settings
        /// </summary>
        public void InitializePlatform()
        {
#if ((UNITY_IPHONE || UNITY_IOS) && !UNITY_EDITOR)
            setPlatFormNative();
#endif
        }

        /// <summary>
        /// Initializes Vuforia
        /// </summary>
        public VuforiaUnity.InitError InitializeVuforia(string licenseKey)
        {
#if ((UNITY_IPHONE || UNITY_IOS) && !UNITY_EDITOR)
            VuforiaRenderer.RendererAPI rendererAPI = VuforiaRenderer.Instance.GetRendererAPI();
            int errorCode = initQCARiOS((int)rendererAPI, (int)Screen.orientation, licenseKey);
#else
            int errorCode = 0;
#endif
            if (errorCode >= 0)
                InitializeSurface();
            return (VuforiaUnity.InitError)errorCode;
        }

        /// <summary>
        /// Called on start each time a new scene is loaded
        /// </summary>
        public void StartScene()
        { }

        /// <summary>
        /// Called from Update, checks for various life cycle events that need to be forwarded
        /// to Vuforia, e.g. orientation changes
        /// </summary>
        public void Update()
        {
            if (SurfaceUtilities.HasSurfaceBeenRecreated())
            {
                InitializeSurface();
            }
            else
            {
                // if Unity reports that the orientation has changed, set it correctly in native
                if (Screen.orientation != mScreenOrientation)
                    SetUnityScreenOrientation();
            }

        }

        public void Dispose()
        {
        }

        /// <summary>
        /// Pauses Vuforia
        /// </summary>
        public void OnPause()
        {
            VuforiaUnity.OnPause();
        }

        /// <summary>
        /// Resumes Vuforia
        /// </summary>
        public void OnResume()
        {
            VuforiaUnity.OnResume();
        }

        /// <summary>
        /// Deinitializes Vuforia
        /// </summary>
        public void OnDestroy()
        {
            VuforiaUnity.Deinit();
        }


        private void InitializeSurface()
        {
            SurfaceUtilities.OnSurfaceCreated();

            SetUnityScreenOrientation();
        }

        private void SetUnityScreenOrientation()
        {
            mScreenOrientation = Screen.orientation;
            SurfaceUtilities.SetSurfaceOrientation(mScreenOrientation);
            // set the native orientation (only required on iOS and WSA)
#if ((UNITY_IPHONE || UNITY_IOS) && !UNITY_EDITOR)
            setSurfaceOrientationiOS((int) mScreenOrientation);
#endif
        }

#if ((UNITY_IPHONE || UNITY_IOS) && !UNITY_EDITOR)
        [DllImport("__Internal")]
        private static extern void setPlatFormNative();

        [DllImport("__Internal")]
        private static extern int initQCARiOS(int rendererAPI, int screenOrientation, string licenseKey);

        [DllImport("__Internal")]
        private static extern void setSurfaceOrientationiOS(int screenOrientation);
#endif

    }
}
