// Author: Devon Wayman - March 2021
using System;
using UnityEngine;

namespace Salem {
    [CreateAssetMenu(fileName = "Scene Parameter", menuName = "Salem/Scene Setup", order = 0)]
    public class SceneParameters : ScriptableObject {

        [System.Serializable]
        public struct SceneStartSettings {
            public float camNearClip;
            public float camFarClip;
            public bool postProcessing;
            public Vector3 playerStartPosition;
        }

        public SceneStartSettings[] settings;
    }
}
