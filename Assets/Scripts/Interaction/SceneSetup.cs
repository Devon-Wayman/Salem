// Author: Devon Wayman - March 2021
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Applys preset user position and other player based settings whenever loaded into a new scene
/// </summary>
namespace Salem.SceneManagement {
    public class SceneSetup : MonoBehaviour {
        private Camera playerCam = null;
        private GameObject playerRoot = null;
        private SceneParameters sceneParameters; // Place SceneParameters scriptable object in here
        public static SceneSetup Instance;
        public static SceneSetup Current {
            get {
                if (!Instance) Instance = FindObjectOfType<SceneSetup>();
                return Instance;
            }
        }

        private void Awake() {
            playerRoot = GameObject.FindGameObjectWithTag("Player");
            playerCam = Camera.main;
            sceneParameters = Resources.Load<SceneParameters>("SceneParameterMaster");
        }

        /// <summary>
        /// Call this whenever we load into a new scene
        /// </summary>
        public void UpdateParameters(int newSceneIndex) {
            var startSettings = sceneParameters.settings[newSceneIndex];
            playerCam.nearClipPlane = startSettings.camNearClip;
            playerCam.farClipPlane = startSettings.camFarClip;
            playerCam.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = startSettings.postProcessing;
            playerRoot.transform.position = startSettings.playerStartPosition;
        }
    }
}
