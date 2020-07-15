// Copyright Devon Wayman 2020
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls transitions between stop motion animation like objects
/// All child objects are disabled at start. When an object is called to appear, its
/// renderer slowly fades it in. When the function is called again, the current visible section will fade
/// out or simply disable, before fading in or showing the next object
/// </summary>
namespace Salem.Core.Animation {
    public class FrameController : MonoBehaviour {

        // Array of parent game objects in order to fade between
        [SerializeField] private List<GameObject> animStages = new List<GameObject>();
        
        // Integer of animation state
        [SerializeField] private int currentAnimFrame = 0;

        // Next scene that needs to be loaded in once current additive scene is done
        public string nextSceneName;

        // Name of the loaded in additive scene
        public string thisSceneName;

        private void Awake() {
            // Add each transforms gameobejct to the animStages array
            for (int i = 0; i < gameObject.transform.childCount; i++) {
                animStages.Add(gameObject.transform.GetChild(i).gameObject);
            }

            // Disable all items in the list except the first
            for (int i = 1; i < animStages.Count; i++) {
                animStages[i].SetActive(false);
            }
        }

        private void Update()
        {
            if (thisSceneName != null)
                return;

            thisSceneName = SceneManager.GetActiveScene().name;   
        }

        // Called from external source to disable currently enabled item, then enable the next
        public void GoToNextFrame(){

            // Disable current frame
            animStages[currentAnimFrame].SetActive(false);

            // Increase requested frame by 1 (should go to 0 on first call)
            currentAnimFrame++;

            // If end of animations has been reached, load next scene async and unload the current scene
            if (currentAnimFrame == animStages.Count){
                SceneManager.LoadScene(nextSceneName, LoadSceneMode.Additive);
                SceneManager.UnloadSceneAsync(thisSceneName);
                return;
            }

            // Activate newly requested object
            animStages[currentAnimFrame].SetActive(true);
        }
    }
}