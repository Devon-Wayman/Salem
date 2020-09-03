// Author: Devon Wayman 2020
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// If player looks away from the main content source, pause all animations and audio. 
/// Play a quick audio clip to get them to look towards the main content. Once they do, resume
/// animations and audio
/// </summary>
namespace Salem.Core {
    [RequireComponent(typeof(AudioSource))]
    public class AttentionGrabber : MonoBehaviour {

        // Enable/disable this feature
        private bool isEnabled = true;

        // All audio sources in scene with exception of the one attatched to this game object
        private List<AudioSource> sceneAudioSources = new List<AudioSource>();

        // All animators in the scene
        private List<Animator> sceneAnimators = new List<Animator>();

        // Awareness check coroutine variables
        private Coroutine checkAwarness = null;
        private Coroutine timeScaleMaster = null;
        private WaitForSecondsRealtime awarenessDelay = new WaitForSecondsRealtime(3);

        // Player's camera
        public Transform playerCamera = null;

        // Interest region variables
        [Range(0, 180)]
        public int interestAngle = 0;
        public float minInterestAngle, maxInterestAngle; // Final min and max results for interest region
        public bool wasLookingAway = false;

        // Audio components
        public AudioSource grabberAudioSource = null;
        public AudioClip[] grabberAudioClips;

        private void Awake() {
            foreach (AudioSource source in FindObjectsOfType<AudioSource>()) {
                if (source.gameObject.name != this.gameObject.name)
                    sceneAudioSources.Add(source);
            }
            foreach (Animator  animator in FindObjectsOfType<Animator>()) {
                sceneAnimators.Add(animator);
            }

            if (grabberAudioSource == null)
                grabberAudioSource = GetComponent<AudioSource>();
            
            playerCamera = Camera.main.gameObject.transform; // Grab the main camera transform
        }

        private void Start() {
            // If this feature is enabled on start, begin the awareness check coroutine
            if (isEnabled) {
                // Set min and max view angle respectivley
                minInterestAngle = interestAngle * -1;
                maxInterestAngle = interestAngle;

                // If checkAwarness is null (coroutine is already running), stop it
                if (checkAwarness != null) 
                    StopCoroutine(checkAwarness); 
                   
                // Start the awareness check coroutine
                checkAwarness = StartCoroutine(PlayerAwarnessCheck());
            }
        }

        private IEnumerator PlayerAwarnessCheck() {

            Debug.Log("Beginning to run awareness checks...");

            while (true) {
                // Wait three seconds before running the check
                yield return awarenessDelay;

                // If player is looking in POI...
                if (WithinPOIBounds()) {
                    // If player was previously not looking into POI, resume all animations and audio
                    if (wasLookingAway) {
                        Debug.Log("Player has returned gaze to POI. Speeding up time and resuming animations/audio");

                        // Check if time scale master is already running
                        if (timeScaleMaster != null)
                            StopCoroutine(timeScaleMaster);

                        // Speed up time
                        timeScaleMaster = StartCoroutine(AdjustTimeScale(1f, 1f));         
                    }
                    Debug.Log("Player is in POI region");
                } 
                // If not looking within the subject range
                else if (!WithinPOIBounds()) {
                    Debug.Log("Player is not viewing main content. Playing attention grabber audio");

                    // If player was previously not looking away, slow down time and pause all animations/audio
                    if (!wasLookingAway) {
                        // Check if time scale master is already running
                        if (timeScaleMaster != null)
                            StopCoroutine(timeScaleMaster);

                        // slow down time
                        timeScaleMaster = StartCoroutine(AdjustTimeScale(0f, 1f));
                        PauseAll();
                    }    
                }
            }
        }

        // Increase/decrease timescale as needed
        private IEnumerator AdjustTimeScale(float requestedTimeScale, float transitionSpeed) {

            float startTimeScale = Time.timeScale;

            // Increase timescale
            if (startTimeScale < requestedTimeScale) {
                Debug.Log("Increasing time scale");
                for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / transitionSpeed) {
                    Time.timeScale = Mathf.Lerp(startTimeScale, requestedTimeScale, t);
                    yield return null;
                }

                // Ensure timescale makes it at 1
                Time.timeScale = requestedTimeScale;

                // Audio and animations shouldnt be called until time scale is at 1 to prevent
                // audio and animation sync issues
                ResumeAll();
            }

            // Decrease timescale
            if (startTimeScale > requestedTimeScale) {
                Debug.Log("Decreasing time scale");

                for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / transitionSpeed) {
                    Time.timeScale = Mathf.Lerp(startTimeScale, requestedTimeScale, t);
                    yield return null;
                }
                // Ensure timescale is at 0
                Time.timeScale = requestedTimeScale;
            }
        }

        // Check if camera is facing within the POI bounds
        private bool WithinPOIBounds() {
            if (Vector3.Angle(playerCamera.forward, Vector3.forward) <= interestAngle) {
                return true;
            } else {
                return false;
            }
        }
        // Resume all audio and animations
        private void ResumeAll() {
            wasLookingAway = false;

            foreach (Animator animator in sceneAnimators) {
                animator.speed = 1;
            }
            foreach (AudioSource audSource in sceneAudioSources) {
                audSource.Play();
            }

            // Stop playing any audio clips from this object
            grabberAudioSource.Stop();
        }
        // Pause all audio and animations
        private void PauseAll() {
            wasLookingAway = true;
            foreach (Animator animator in sceneAnimators) {
                animator.speed = 0;
            }
            foreach (AudioSource audSource in sceneAudioSources) {
                audSource.Pause();
            }

            // Grab a random attention grabber audio clip
            grabberAudioSource.clip = ChooseGrabberAudio();
            grabberAudioSource.Play();
        }
        private AudioClip ChooseGrabberAudio() {
            return grabberAudioClips[UnityEngine.Random.Range(0, grabberAudioClips.Length)];
        }
    }
}
