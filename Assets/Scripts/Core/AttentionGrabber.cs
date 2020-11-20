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
        public bool isEnabled = true;

        // Audio sources in scene except the one on this gameobject
        private List<AudioSource> sceneAudioSources = new List<AudioSource>();

        // Animators in the scene
        private List<Animator> sceneAnimators = new List<Animator>();

        // Awareness check coroutine variables
        private Coroutine checkAwarness = null;
        private Coroutine timeScaleMaster = null;
        private WaitForSecondsRealtime awarenessDelay = new WaitForSecondsRealtime(3);

        // Player's camera transform
        private Transform playerCamera = null;

        // Interest region variables
        [Header("User POI")]
        [Range(0, 180)]
        public int interestAngle = 0;
        // Bool to determine if player was previously looking away from POI
        public bool wasLookingAway = false;

        // Audio components
        [Header("Audio Clips")]
        public AudioClip[] lookedAwayAudio;
        public AudioClip[] returnedAttentionClips;
        private AudioSource grabberAudioSource = null;

        [Header("Visual Cues")]
        public GameObject visualCueManager = null;

        private void Awake() {
            // Get audio sources and animators in the scene
            RetrieveAnimatorsAndAudioSources();

            if (grabberAudioSource == null)
                grabberAudioSource = GetComponent<AudioSource>();

            // Grab the main camera transform
            playerCamera = Camera.main.gameObject.transform;

            // Make sure visual look cues arent activated
            if (visualCueManager.activeSelf)
                visualCueManager.SetActive(false);
        }

        private void Start() {
            // If this feature is enabled on start, begin the awareness check coroutine
            if (isEnabled) {
                // If checkAwarness is null (coroutine is already running), stop it
                if (checkAwarness != null) 
                    StopCoroutine(checkAwarness); 
                   
                // Start the awareness check coroutine
                checkAwarness = StartCoroutine(PlayerAwarnessCheck());
            }
        }

        public void SetIncomingScene(int newPOIRange) {
            // Clear all lists of previously held objects
            sceneAnimators.Clear();
            sceneAudioSources.Clear();

            // Get all newly imported animators and audio sources in the scene
            RetrieveAnimatorsAndAudioSources();

            // Set interestAngle to passed in value if its >= 1
            if (newPOIRange >= 1) {
                // Ensure that the max value of 180 cannot be surpassed
                if (newPOIRange > 180)
                    newPOIRange = 180;

                interestAngle = newPOIRange;
            }
            // If 0 is passed in, disable the POI check behavior
            else if (newPOIRange == 0) {
                
                isEnabled = false;
            }
        }

        private void RetrieveAnimatorsAndAudioSources() {
            foreach (AudioSource source in FindObjectsOfType<AudioSource>()) {
                if (source.gameObject.name != this.gameObject.name)
                    sceneAudioSources.Add(source);
            }
            foreach (Animator animator in FindObjectsOfType<Animator>()) {
                sceneAnimators.Add(animator);
            }
        }

        // Check if player is looking within POI boundary every 3 seconds
        private IEnumerator PlayerAwarnessCheck() {
            Debug.Log("Beginning awareness checks...");

            while (true) {
                // Wait three seconds before running the check
                yield return awarenessDelay;

                // If player is looking in POI...
                if (WithinPOIBounds()) {
                    // If player was previously not looking into POI, resume all animations and audio
                    if (wasLookingAway) {
                        Debug.Log("Player has returned gaze to POI. Resuming animations and audio");

                        // Check if time scale master is already running
                        if (timeScaleMaster != null)
                            StopCoroutine(timeScaleMaster);

                        // If visual cues are enabled, disable them
                        if (visualCueManager.activeSelf)
                            visualCueManager.SetActive(false);

                        ResumeAll();
                    }
                } 
                // If not looking within the subject range
                else if (!WithinPOIBounds()) {
                    Debug.Log("Player is not viewing main content. Playing attention grabber audio");

                    if (!visualCueManager.activeSelf) 
                        visualCueManager.SetActive(true);

                    // If player was previously not looking away, slow down time and pause all animations/audio
                    if (!wasLookingAway) {
                        // Check if time scale master is already running
                        if (timeScaleMaster != null)
                            StopCoroutine(timeScaleMaster);
                        
                        PauseAll();
                    }    
                }
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
            // Play a "thank you" audio clip
            if (grabberAudioSource.isPlaying)
                grabberAudioSource.Stop();
            grabberAudioSource.clip = ChooseGrabberAudio(1);
            grabberAudioSource.Play();
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
            // Play a "Look at the material!" audio clip
            if (grabberAudioSource.isPlaying)
                grabberAudioSource.Stop();

            grabberAudioSource.clip = ChooseGrabberAudio(0);
            grabberAudioSource.Play();
        }
        // Grabs a random clip for either resume or pause needs. 0 = player looked/looking away audio. 1 = player returned attention
        private AudioClip ChooseGrabberAudio(int requestId) {
            if (requestId == 0)
                return lookedAwayAudio[Random.Range(0, lookedAwayAudio.Length)];
            else {
                // If ID is anything other than 0, play a thank you
                return returnedAttentionClips[Random.Range(0, returnedAttentionClips.Length)];
            }
        }
    }
}
