// Copyright Devon Wayman 2020
using System.Collections.Generic;
using Salem.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

namespace Salem.Core.Player {
    public class ControllerInput : MonoBehaviour {

        [SerializeField] private List<InputDevice> leftHandDevices = new List<InputDevice> (); // Left hand VR items
        [SerializeField] private List<InputDevice> rightHandDevices = new List<InputDevice> (); // Right hand VR items
        public bool menuPressedLastFrame = false;

        private string currentSceneName; // Get current scene name

        private void Awake () {
            currentSceneName = SceneManager.GetActiveScene ().name;
        }
        private void Start () {
            InputDevices.GetDevicesAtXRNode (XRNode.LeftHand, leftHandDevices);
            InputDevices.GetDevicesAtXRNode (XRNode.RightHand, rightHandDevices);
        }

        private void Update () {

            // This will only be used outside the MainMenu scene, so if we are in that scene, return
            if (currentSceneName == "MainMenu") return;

            if (leftHandDevices.Count <= 0 && leftHandDevices.Count <= 0) return;

            bool menuPressed;

            if (leftHandDevices[0].TryGetFeatureValue (CommonUsages.menuButton, out menuPressed) && menuPressed && !menuPressedLastFrame) {
                menuPressedLastFrame = true;

                if (currentSceneName != "MainMenu" && menuPressed) {
                    if (Time.timeScale != 1)
                        Time.timeScale = 1;
                    Debug.Log ("Going home!");
                    GlowTransition.Instance.CallForFadeOut ("StoryScene");

                } else if (currentSceneName == "MainMenu" && menuPressed) {
                    
                }
            } else if (leftHandDevices[0].TryGetFeatureValue (CommonUsages.menuButton, out menuPressed) && !menuPressed && menuPressedLastFrame) {
                menuPressedLastFrame = false; // Reset menu pressed last frame back to false once menu button is no longer pressed
            }
        }

    }
}