// Author Devon Wayman
// Date 12/14/2020
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Sets up event subscribers to load next required level once
/// playable director timeline stops playing
/// </summary>
public class SceneDirectorController : MonoBehaviour {

    [SerializeField] private PlayableDirector director = null;


    private void Awake() {
        if (director != null) {
            director.played += OnPlayableDirectorPlaying;
            director.stopped += OnPlayableDirectorStopped;
            return;
        }

        if (director == null)
            gameObject.GetComponent<PlayableDirector>();

        if(director == null) {
            Debug.LogWarning($"Error finding director object on {gameObject.name}", gameObject);
            return;
        }
        director.played += OnPlayableDirectorPlaying;
        director.stopped += OnPlayableDirectorStopped;
    }
    private void Start() { 
        director.Play();
    }

    private void OnPlayableDirectorStopped(PlayableDirector obj) {
        Debug.Log("Reached end of timeline", gameObject);
    }

    private void OnPlayableDirectorPlaying(PlayableDirector obj) {
        Debug.Log($"Playable director on {gameObject.name} has begun", gameObject);
    } 
}
