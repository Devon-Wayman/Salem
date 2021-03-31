// Author: Devon Wayman - November 2020
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Salem.SceneManagement {
    public class SceneLoadSystem : MonoBehaviour {

        private AsyncOperation requestedScene;
        public static SceneLoadSystem Instance;
        public static SceneLoadSystem Current {
            get {
                if (!Instance) Instance = FindObjectOfType<SceneLoadSystem>();
                return Instance;
            }
        }
        private WaitForSeconds loadDelay = new WaitForSeconds(1);

        private void Awake() {
            SceneManager.sceneLoaded += OnSceneLoaded; // subscribe event to fire off when new scene is loaded
            DontDestroyOnLoad(this.gameObject); // used to keep the player persistent between scene changes
        }


        // Remove after testing
        private void Update() {
            if (Input.GetKeyDown(KeyCode.Space)) {
                LoadNextScene();
            }
        }

        private void LoadNextScene() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }


        /// <summary>
        /// Grabs info from SceneParameter array and applies any settings needed
        /// </summary>
        private void OnSceneLoaded(Scene newScene, LoadSceneMode arg1) {
            Debug.Log($"New scene loaded: {newScene.buildIndex}");
            SceneSetup.Current.UpdateParameters(newScene.buildIndex);
        }


        // Originally called to allow black fade in/out for smoother transition but need to figure something
        // else out for the new system
        public IEnumerator BeginSceneLoad() {
            yield return loadDelay;

            while (!requestedScene.isDone) {
                if (requestedScene.progress >= 0.9f) {
                    requestedScene.allowSceneActivation = true;
                }
                yield return null;
            }
            yield return loadDelay;
        }
    }
}