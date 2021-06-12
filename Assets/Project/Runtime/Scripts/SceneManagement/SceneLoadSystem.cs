// Author: Devon Wayman - November 2020
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Salem.SceneManagement {
    public class SceneLoadSystem : MonoBehaviour {

        private AsyncOperation asyncLoader;
        public static SceneLoadSystem Instance;
        public static SceneLoadSystem Current {
            get {
                if (!Instance) Instance = FindObjectOfType<SceneLoadSystem>();
                return Instance;
            }
        }
        private WaitForSeconds transitionDelay = new WaitForSeconds(4);

        private int requestedSceneIndex;
        private int totalBuiltScenes = 0;

        private void Awake() {
            SceneManager.sceneLoaded += OnSceneLoaded; // subscribe event to fire off when new scene is loaded
            totalBuiltScenes = SceneManager.sceneCountInBuildSettings;
            Debug.Log($"Found {totalBuiltScenes} scenes in project");
            DontDestroyOnLoad(this.gameObject); // used to keep the player persistent between scene changes
        }

        public void LoadNextScene() {
            StartCoroutine(BeginSceneLoad());
        }


        /// <summary>
        /// Grabs info from SceneParameter array and applies any settings needed
        /// </summary>
        private void OnSceneLoaded(Scene newScene, LoadSceneMode arg1) {
            Debug.Log($"New scene loaded: {newScene.buildIndex}");
        }

        private IEnumerator BeginSceneLoad() {
            requestedSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

            // request to load main menu if the currently loaded scene + 1 does not exist
            if (requestedSceneIndex > totalBuiltScenes) {
                requestedSceneIndex = 0;
                Debug.LogWarning("Scene at initial requested index does not exist. Loading Main Menu at index 0 instead");
            }

            yield return transitionDelay;
            asyncLoader = SceneManager.LoadSceneAsync(requestedSceneIndex);
            asyncLoader.allowSceneActivation = false;

            while (!asyncLoader.isDone) {
                if (asyncLoader.progress >= 0.9f) {
                    asyncLoader.allowSceneActivation = true;
                }
                yield return null;
            }
        }
    }
}