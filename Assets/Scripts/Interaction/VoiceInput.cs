// Author Devon Wayman
// Date 12/14/2020
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

/// <summary>
/// Used to detect spoken words/pharases from the player
/// Mainly used in the jury chapter but can me modified for other purposes
/// </summary>
public class VoiceInput : MonoBehaviour {

    private KeywordRecognizer recognizer;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();
    public bool registerVoiceInput = false;
    public bool enableSpeechDetectionOnStart;

    private void Start() {
        actions.Add("yes", ExecuteNotGuilty);
        actions.Add("yes i am", ExecuteNotGuilty);
        actions.Add("no", ExecuteGuilty);
        actions.Add("no i am not", ExecuteGuilty);
        actions.Add("nah", ExecuteGuilty);

        recognizer = new KeywordRecognizer(actions.Keys.ToArray());
        recognizer.OnPhraseRecognized += OnPlayerInputRecognized;

        if (!enableSpeechDetectionOnStart) return; 
            
        recognizer.Start();
        registerVoiceInput = true;
    }

    public void ToggleSpeechDetection() {
        registerVoiceInput = !registerVoiceInput;

        if (registerVoiceInput && !recognizer.IsRunning) {
            recognizer.Start();
        } else if (!registerVoiceInput && recognizer.IsRunning) {
            recognizer.Stop();
        }
    }

    // Exectues when a predetermined phrase is detected
    private void OnPlayerInputRecognized(PhraseRecognizedEventArgs userSpeech) {
        if (!registerVoiceInput) return;
        
        Debug.Log($"Detected phrase {userSpeech.text}");

        // ExectuteGuilty or NotGuilty if confidence is medium or high
        if (userSpeech.confidence <= ConfidenceLevel.Medium) {
            actions[userSpeech.text].Invoke();
        } else {
            Debug.LogWarning($"Speech confidence too low to execute either main function. Confidence: {userSpeech.confidence}");

            // Run some sore of "Sorry speak up i cant hear you" response animation
        }
    }

    // Called when player says they are not a witch (guilty in
    // the eyes of the court)
    private void ExecuteGuilty() {
        Debug.Log("Executing guilty");
    }

    // Executes when the player says they are guilty of witchcraft (innocent
    // and saved by the court for "telling the truth")
    private void ExecuteNotGuilty() {
        Debug.Log("Executing not guilty");
    }
}
