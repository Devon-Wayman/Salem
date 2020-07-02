// Copyright Devon Wayman 2020
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Functions that execute and influence all audio sources in the scene
/// </summary>
namespace Salem.Audio {
    public class AudioManager : MonoBehaviour {
        public static AudioManager Instance { get; set; }

        [HideInInspector] [SerializeField] private List<AudioSource> audioSources = new List<AudioSource>();
        [SerializeField] private float fadeTime = 2f;

        private void Awake () {
            if (Instance == null)
                Instance = this;

            // Add all audio sources in scene to the list
            foreach (AudioSource aud in FindObjectsOfType<AudioSource>())
                audioSources.Add(aud);
        }

        public void PauseAllAudio(){
            foreach (AudioSource audio in audioSources){
                audio.Pause();
            }
        }

        public void FadeOutAudio () {
            StartCoroutine (FadeAudioOut());
        }

        private IEnumerator FadeAudioOut () {            
            foreach (AudioSource audio in audioSources) {
                while (audio.volume > 0) {
                    audio.volume -= 0.9f * Time.deltaTime / fadeTime;
                    yield return null;
                }
            } 
            yield return null;
        }
    }
}