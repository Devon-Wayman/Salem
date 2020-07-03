using System.Collections;
// Copyright Devon Wayman 2020
using System;
using UnityEngine;
using UnityEngine.XR;
using Salem.Audio;

namespace Salem.Core.Player {
    public class HeadsetState : MonoBehaviour {
        
        // Determine if the player had the headset on during
        // the last frame update
        private bool isPresentLastFrame;

        public InputDevice headDevice;


        private void Update() {
            bool headsetOn = XRDevice.userPresence == UserPresenceState.Present;

            if (isPresentLastFrame && !headsetOn) {
                // HEADSET IS OFF
                Debug.Log("User has removed headset");
                PauseApplication();

            } else if (!isPresentLastFrame && headsetOn) {
                // HEADSET HAS BEEN PUT BACK ON
                Debug.Log("User has returned to headset. Resuming in 3 seconds...");
                StartCoroutine(ResumeApplication());
            }

            isPresentLastFrame = headsetOn;
        }

        private void PauseApplication(){
            Time.timeScale = 0;
            AudioManager.Instance.PauseAllAudio();
        }

        private IEnumerator ResumeApplication() {
            yield return new WaitForSeconds(3);

            // Resume audio and movement
            Time.timeScale = 1;
            AudioManager.Instance.PlayAllAudio();
        }
    }
}