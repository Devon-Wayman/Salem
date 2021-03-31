// Author: Devon Wayman - May 2021
using UnityEngine;

/// <summary>
/// Used to do basic object manipulations (rotate, translate, scale) and apply different modifiers for different interactions
/// </summary>
namespace Salem {
    public class BasicManipulation : MonoBehaviour {

        [System.Serializable]
        public enum TransformType {
            Translate,
            Rotate
        }
        [System.Serializable]
        public enum InterpolationMode {
            Continuous,
            PingPong
        }
        public TransformType transformType;
        public InterpolationMode interpolationMode;


        [SerializeField] private float rotationSpeed;

        private void Update() {

            switch (transformType) {
                case TransformType.Rotate:
                    if (interpolationMode == InterpolationMode.Continuous) {
                        transform.Rotate(Vector3.up * (rotationSpeed * Time.deltaTime));
                    }
                    break;
            }
        }
    }
}
