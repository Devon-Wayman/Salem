// Copyright Devon Wayman 2020
using System.Collections;
using UnityEngine;

/// <summary>
/// Various audio controls that can be called by UnityEvents per object
/// </summary>
namespace Salem.Audio {
    public class AudioController : MonoBehaviour {

        // Variables for object's AudioSource component
        [SerializeField] private AudioSource audioSource = null;
        [SerializeField] private float startVolume;

        

        // Amount of time to fade in/out audio
        [HideInInspector][SerializeField] private float fadeTime = 2f;

        private void Awake () {
            audioSource = GetComponent<AudioSource> ();
            startVolume = audioSource.volume;
        }

        public void FadeOutAudio () {
            StartCoroutine (FadeAudio (audioSource, startVolume, 0f));
        }
        private IEnumerator FadeAudio (AudioSource source, float originalVolume, float requestedVolume) {

            // If requested volume is higher than start volume, fade volume in 
            if (requestedVolume > originalVolume) {
                while (audioSource.volume < requestedVolume) {
                    audioSource.volume += startVolume * Time.deltaTime / fadeTime;
                    yield return null;
                }
            } 
            // If requested volume is lower than start volume, fade volume out
            else if (requestedVolume < originalVolume) {
                while (audioSource.volume > requestedVolume) {
                    audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
                    yield return null;
                }
            }
            yield return null;
        }
    }
}