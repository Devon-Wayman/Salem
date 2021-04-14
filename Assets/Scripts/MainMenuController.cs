// Author: Devon Wayman - March 2021
using System.Collections;
using Salem.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace Salem {
    public class MainMenuController : MonoBehaviour {

        [Header("Opening Text")]
        public CanvasGroup homeCanvasGroup = null;
        public Text homeText = null;

        [Header("Audio Control")]
        [SerializeField] private AudioSource themeMusic;

        private OVRScreenFade sceneFader = null;

        private WaitForSeconds textChangeDelay = new WaitForSeconds(1);
        private WaitForSeconds textDisplayDuration = new WaitForSeconds(5);

        private bool canPressStart = false;
        private int fontStartSize;
        private int textIndex;
        private string[] openingTextArray = new string[] { "Split Box Studios\npresents", "1692" };

        private void Awake() {
            sceneFader = FindObjectOfType<OVRScreenFade>(); //FindObjectOfType<OVRScreenFade>();
        }
        private void Start() {
            fontStartSize = homeText.fontSize;
            StartCoroutine(BeginHomeIntro());
        }
        private void Update() {
            if (!canPressStart) return; // do not continue if start button detection is disabled


#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Space)) {
                Debug.Log("Loading into next scene");

                if (sceneFader != null) {
                    sceneFader.FadeOut();
                    StartCoroutine(FadeOutAudio(3, 0)); // Slowly lowers the master volume of the application for a smoother transition
                } else {
                    Debug.LogWarning("Scene fade controller is null. No fade animation will play");
                }
                SceneLoadSystem.Current.LoadNextScene();
                canPressStart = false;
            }
#endif
            if (OVRInput.Get(OVRInput.Button.One, OVRInput.Controller.RTouch)) {
                Debug.Log("Loading into next scene");

                if (sceneFader != null) {
                    sceneFader.FadeOut();
                    StartCoroutine(FadeOutAudio(3, 0)); // Slowly lowers the master volume of the application for a smoother transition
                } else {
                    Debug.LogWarning("Scene fade controller is null. No fade animation will play");
                }

                canPressStart = false;
                SceneLoadSystem.Current.LoadNextScene();
            }
        }

        private IEnumerator FadeOutAudio(float duration, float targetVolume) {
            float currentTime = 0;
            float start = themeMusic.volume;

            while (currentTime < duration) {
                currentTime += Time.deltaTime;
                themeMusic.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
                yield return null;
            }
            yield break;
        }


        private IEnumerator BeginHomeIntro() {
            while (textIndex < openingTextArray.Length) {
                if (textIndex == 1) {
                    homeText.fontSize = fontStartSize + 4;
                }

                homeText.text = openingTextArray[textIndex];

                yield return textChangeDelay;

                while (homeCanvasGroup.alpha < 0.8f) {
                    homeCanvasGroup.alpha += 0.8f * Time.deltaTime;
                    yield return null;
                }

                homeCanvasGroup.alpha = 0.8f;
                yield return textDisplayDuration;

                while (homeCanvasGroup.alpha > 0f) {
                    homeCanvasGroup.alpha -= 0.8f * Time.deltaTime;
                    yield return null;
                }

                homeCanvasGroup.alpha = 0;
                textIndex++;
            }

            homeText.fontSize = fontStartSize;
            yield return textChangeDelay;
            homeCanvasGroup.alpha = 0;
            homeText.text = "Press the A button on the right\ncontroller to begin...";

            while (homeCanvasGroup.alpha < 0.8f) {
                homeCanvasGroup.alpha += 0.8f * Time.deltaTime;
                yield return null;
            }

            homeCanvasGroup.alpha = 0.8f;
            canPressStart = true; // allow detection of the Start button being pressed
            Debug.Log("Can now load into the next scene");
        }
    }
}
