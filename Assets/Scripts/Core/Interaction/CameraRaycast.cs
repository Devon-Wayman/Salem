// Author: Devon Wayman
using System;
using Salem.Audio;
using Salem.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Salem.Core.Interaction {
    public class CameraRaycast : MonoBehaviour {

        // Cache a ray to send out to save memory
        private Ray ray;

        // Cache hit object to use when raycasting
        private RaycastHit hit;

        // Determine if raycast should be on or off
        [SerializeField] private bool canRaycast = true;

        // Distance raycast should shoot (default is 1)
        [SerializeField] private float rayDistance = 1f;

        // Current scene name
        private string currentSceneName = String.Empty;

        private void Awake() {
            // Set current scene name to scene we've loaded into
            currentSceneName = SceneManager.GetActiveScene().name;

            // Disable this object if not in the main menu
            if (currentSceneName != "MainMenu")
                this.enabled = false;
        }

        void FixedUpdate() {
            // If we shouldnt be raycasting, return
            if (!canRaycast)
                return;

            ray = new Ray(transform.position, transform.forward);
            Vector3 forward = transform.TransformDirection(Vector3.forward) * rayDistance;
            Debug.DrawRay(transform.position, forward, Color.green);

            if (Physics.Raycast(ray, out hit, rayDistance)) {
                if (hit.transform.name == "Book") {
                    GlowTransition.Instance.CallForFadeOut("StoryScene");
                    AudioManager.Instance.FadeOutAudio();
                    canRaycast = false; // Disable the raycaster
                }
            }
        }

        // Draw the ray in editor when it is selected
        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.green;
            Vector3 direction = transform.TransformDirection(Vector3.forward) * rayDistance;
            Gizmos.DrawRay(transform.position, direction);
        }
    }
}