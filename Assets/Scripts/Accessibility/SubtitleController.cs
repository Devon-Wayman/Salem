// Author: Devon Wayman - December 2020
using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace Salem.Accessibility {
    public class SubtitleController : MonoBehaviour {

        public Canvas subitleCanvas = null;
        [SerializeField] private LocalizedStringTable[] tables;
        public LocalizedString subtitlesLocalized;
        public static SubtitleController Instance;
        public Text subtitleText = null;

        public bool subtitlesEnabled = true;

        public static SubtitleController Current {
            get {
                if (!Instance) Instance = FindObjectOfType<SubtitleController>();
                return Instance;
            }
        }


        private void Awake() {
            subtitlesLocalized.StringChanged += OnSubtitleChange;
        }
        private void OnSubtitleChange(string value) {
            subtitleText.text = value;
        }

        public void SetLocalisationTable(string tableName) {
            subtitlesLocalized.TableReference = tableName;
        }
        public void UpdateSubitles(string tableEntryRef, int duration) {
            StartCoroutine(ShowSubtitle(tableEntryRef, duration));
        }
        private IEnumerator ShowSubtitle(string tableEntryRef, int displayDuration) {
            yield return new WaitForSeconds(displayDuration);
        }
    }
}
