// Author: Devon Wayman - December 2020
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceInput : MonoBehaviour {
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();
    private KeywordRecognizer recognizer;
    public bool enableSpeechOnStart;

    private void Start() {
        actions.Add("yes", ExecuteNotGuilty);
        actions.Add("yes i am", ExecuteNotGuilty);
        actions.Add("no", ExecuteGuilty);
        actions.Add("no i am not", ExecuteGuilty);
        actions.Add("nah", ExecuteGuilty);
        actions.Add("nope", ExecuteGuilty);

        recognizer = new KeywordRecognizer(actions.Keys.ToArray());
        recognizer.OnPhraseRecognized += OnPlayerInputRecognized;

        if (!enableSpeechOnStart) return;

        if (recognizer != null && enableSpeechOnStart) {
            recognizer.Start();
        }
    }

    public void ToggleSpeechDetection() {
        if (!recognizer.IsRunning) {
            recognizer.Start();
        } else if (recognizer.IsRunning) {
            recognizer.Stop();
        }
    }

    private void OnPlayerInputRecognized(PhraseRecognizedEventArgs userSpeech) {
        Debug.Log($"User said: {userSpeech.text}");

        if (userSpeech.confidence <= ConfidenceLevel.Medium) {
            actions[userSpeech.text].Invoke();
        } else {
            Debug.LogWarning($"Confidence too low to execute either function.");
        }
    }

    private void ExecuteGuilty() {
        Debug.Log("User says guilty");
    }
    private void ExecuteNotGuilty() {
        Debug.Log("User says not guilty");
    }
}
