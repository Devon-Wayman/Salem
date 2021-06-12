// Author: Devon Wayman - March 2021
using UnityEngine;
using UnityEngine.Events;

namespace Salem {
    public class CurveMovement : MonoBehaviour {

        [Header("Animation Curve Parameters")]
        public AnimWrapMode animWrapMode;
        [System.Serializable]
        public enum AnimWrapMode {
            PingPong,
            Clamp,
            Loop,
            Once,
            Default
        }

        public AnimationCurve animCurve;
        public AxisOfInfluence axisOfInfluence;

        [System.Serializable]
        public enum AxisOfInfluence {
            X,
            Y,
            Z
        }

        [Header("Fire Ending Event")]
        [Tooltip("Only works if wrap mode is Default or Once")] public bool fireEventAtEnd;
        private bool canFireEvent; // becomes true if fireEventAtEnd is true on start. Set to false once events are fired
        public UnityEvent endingEvents;

        private float lastTime;
        [SerializeField] private Vector3 startPos;

        void Start() {
            animCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
            SetMovementMode();
            startPos = this.gameObject.transform.position;
        }

        private void SetMovementMode() {
            switch (animWrapMode) {
                case AnimWrapMode.Clamp:
                    animCurve.preWrapMode = WrapMode.Clamp;
                    animCurve.postWrapMode = WrapMode.Clamp;
                    break;
                case AnimWrapMode.Default:
                    animCurve.preWrapMode = WrapMode.Default;
                    animCurve.postWrapMode = WrapMode.Default;
                    break;
                case AnimWrapMode.Loop:
                    animCurve.preWrapMode = WrapMode.Loop;
                    animCurve.postWrapMode = WrapMode.Loop;
                    break;
                case AnimWrapMode.Once:
                    animCurve.preWrapMode = WrapMode.Once;
                    animCurve.postWrapMode = WrapMode.Once;
                    break;
                case AnimWrapMode.PingPong:
                    animCurve.preWrapMode = WrapMode.PingPong;
                    animCurve.postWrapMode = WrapMode.PingPong;
                    break;
            }
        }

        void Update() {
            // if (fireEventAtEnd && canFireEvent) {
            //     lastTime = Time.deltaTime;

            //     if (Time.deltaTime - lastTime >= 1) {
            //         endingEvents.Invoke();
            //         lastTime = Time.deltaTime;
            //         canFireEvent = false;
            //     }
            // }

            if (axisOfInfluence == AxisOfInfluence.X) {
                transform.position = new Vector3(animCurve.Evaluate(Time.time), startPos.y, startPos.z);
            } else if (axisOfInfluence == AxisOfInfluence.Y) {
                transform.position = new Vector3(startPos.x, animCurve.Evaluate(Time.time), startPos.z);
            } else if (axisOfInfluence == AxisOfInfluence.Z) {
                transform.position = new Vector3(startPos.x, startPos.y, animCurve.Evaluate(Time.time));
            }
        }
    }
}
