// Author: Devon Wayman - March 2021
using System.Collections;
using Salem.SceneManagement;
using UnityEngine;

namespace Salem {
    public class MainMenuController : MonoBehaviour {

        [Header("Opening Text")]
        public CanvasGroup homeCanvasGroup = null;
        public TMPro.TMP_Text homeText = null;
        private OVRScreenFade sceneFader = null;
        private WaitForSeconds textChangeDelay = new WaitForSeconds(1);
        private WaitForSeconds textDisplayDuration = new WaitForSeconds(5);

        private bool canPressStart = false;
        private int textIndex;
        private string[] openingTextArray = new string[] { "Split Box Studios\npresents", "1692" };

        private void Awake() {
            sceneFader = FindObjectOfType<OVRScreenFade>(); //FindObjectOfType<OVRScreenFade>();
        }

        private void Start() {
            StartCoroutine(BeginHomeIntro());
        }

        private void Update() {
            if (!canPressStart) return;

#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Space)) {
                LoadPersistenScene();
            }
#endif
            if (OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch)) {
                LoadPersistenScene();
            }
        }

        private void LoadPersistenScene() {
            Debug.Log("Loading into persistent scene for story mode");

            if (sceneFader != null) {
                sceneFader.FadeOut();
            } else {
                Debug.LogWarning("Scene fade controller is null. No fade animation will play");
            }

            SceneLoadSystem.Current.LoadNextScene();
            canPressStart = false;
        }

        private IEnumerator BeginHomeIntro() {
            while (textIndex < openingTextArray.Length) {
                yield return textChangeDelay;
                homeText.text = openingTextArray[textIndex];
                LeanTween.alphaCanvas(homeCanvasGroup, 0.8f, 0.5f);
                yield return textDisplayDuration;
                LeanTween.alphaCanvas(homeCanvasGroup, 0, 0.5f);
                textIndex++;
            }

            yield return textChangeDelay;
            homeText.text = "Press the A button on the right\ncontroller to begin...";
            LeanTween.alphaCanvas(homeCanvasGroup, 0.8f, 0.5f);
            canPressStart = true; // allow detection of the Start button being pressed
            Debug.Log("Can now load into the next scene");
        }
    }
}
