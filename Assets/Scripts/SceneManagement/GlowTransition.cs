// Author: Devon Wayman
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Salem.SceneManagement {
    public class GlowTransition : MonoBehaviour {

        public static GlowTransition Instance { get; set; }

        private Animator fadeAnimator = null;
        private string sceneToLoad = String.Empty;

        private void Awake () {
            if (Instance == null)
                Instance = this;

            // Get Animation component in inspector
            fadeAnimator = GetComponent<Animator> ();
        }

        // Call for scene to fade to another (called via instance of this object)
        public void CallForFadeOut (string requestedScene) {
            sceneToLoad = requestedScene;
            fadeAnimator.SetTrigger ("FadeOut");
        }

        #region Fade to white and load scene async before switching 
        // Method called to async load a passed in scene (plays when fade out animation finishes)
        public void LoadScene () {
            StartCoroutine (LoadSceneAsync ());
        }

        private IEnumerator LoadSceneAsync () {
            // Make an asyncScene item to manage the async loading
            var asyncScene = SceneManager.LoadSceneAsync (sceneToLoad);

            // This value stops the scene from displaying when it's finished loading
            asyncScene.allowSceneActivation = false;

            while (!asyncScene.isDone) {
                // Activate scene when 90% is loaded; last 10% can't be multi-threaded
                if (asyncScene.progress >= 0.9f) {
                    asyncScene.allowSceneActivation = true;
                }
                // Break out of IEnumerator
                yield return null;
            }
        }
        #endregion
    }
}