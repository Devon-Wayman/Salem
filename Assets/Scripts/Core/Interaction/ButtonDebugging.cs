// Copyright Devon Wayman 2020
using Salem.Core.Animation;
using UnityEngine;

/// <summary>
/// Use only for debugging. Do not include in release
/// </summary>
namespace Salem.Core.Interaction {
    public class ButtonDebugging : MonoBehaviour {

        private static FrameController frameController = null;

        private void Awake() {
            frameController = FindObjectOfType<FrameController>();
        }


        void Update () {
            if (Input.GetKeyDown (KeyCode.Space)){
                frameController.GoToNextFrame();
            }
        }
    }
}