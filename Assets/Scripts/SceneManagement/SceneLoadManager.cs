// Author Devon Wayman
// Date November 26 2020
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;


// TODO:
// Need to use same scene management as in Christmas Land; keep player in persistent scene
// and load main menu as additive. Once the story mode scene is loaded everything else will be additive.
// This will make executing timelines setup in individual scenes easier and prevent errors such as 
// the camera returning null in this class from occuring.


namespace SceneManagement {
    public class SceneLoadManager : MonoBehaviour {

        public static SceneLoadManager Instance;

        private AsyncOperation requestedScene;
        private WaitForSeconds loadDelay = new WaitForSeconds(2);
        public SceneIndexes currentlyLoadedScene;
        public SceneIndexes nextScene;

        [Header("Player Objects")]
        public Camera playerCamera;
        public Animator fadeTransition;

        public static SceneLoadManager Current {
            get {
                if (!Instance) Instance = FindObjectOfType<SceneLoadManager> ();
                //check for null again
                //dummy not in scene
                //might wanna make one
                return Instance;
            }
        }
        private void Awake() {  
            if (Current != null && Current != this) {
                DestroyImmediate(this);
            } else {
                name = "SceneLoadManager";
                SceneManager.sceneLoaded += OnSceneLoaded;
                SceneManager.sceneUnloaded += OnSceneUnloaded;
            }

            LoadMainMenu();
        }
        private void LoadMainMenu(){
            SceneManager.LoadScene((int)SceneIndexes.MAIN_MENU, LoadSceneMode.Additive);
            currentlyLoadedScene = SceneIndexes.MAIN_MENU;
            fadeTransition.Play("FadeIn");
        }


        #region Callable events
        public void LoadStoryScene() {
            LoadSceneIndex((int)SceneIndexes.STORY_SCENE);
        }
        #endregion


        #region Event Subscribers
        private void OnSceneUnloaded(Scene arg0) {
            Resources.UnloadUnusedAssets();
            Debug.Log($"Unloaded {arg0.name}'s unused assets");
        }
        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) {
            switch((SceneIndexes)arg0.buildIndex) {
                case SceneIndexes.STORY_SCENE:
                    playerCamera.farClipPlane = 100;
                    playerCamera.GetComponent<UniversalAdditionalCameraData>().renderPostProcessing = false;
                    break;
            }
        }
        #endregion

        #region Main Loading Behavior
        // Pass in requested scene index from SceneIndexes. Also set previously loaded scene to
        // passed in index so we can use it to unload that scene later
        public void LoadSceneIndex(int requestedIndex) {
            switch (requestedIndex) {
                case ((int)SceneIndexes.STORY_SCENE):
                    requestedScene = SceneManager.LoadSceneAsync((int)SceneIndexes.STORY_SCENE, LoadSceneMode.Additive);
                    requestedScene.allowSceneActivation = false;
                    nextScene = SceneIndexes.STORY_SCENE;
                    break;
            }
            fadeTransition.Play("FadeOut");
        }

        // Called by event pointer in SnowStormIn animation clip
        public IEnumerator BeginSceneLoad() {
            Debug.Log($"Unloading {currentlyLoadedScene.ToString()}");
            SceneManager.UnloadSceneAsync((int)currentlyLoadedScene);

            yield return loadDelay;

            while (!requestedScene.isDone) {
                if (requestedScene.progress >= 0.9f) {
                    requestedScene.allowSceneActivation = true;
                }
                yield return null;
            }
            Debug.Log($"{nextScene.ToString()} ready. Enabling activation now!");
            yield return loadDelay;
            fadeTransition.Play("FadeIn");
            currentlyLoadedScene = nextScene;
        }
        #endregion
    }
}