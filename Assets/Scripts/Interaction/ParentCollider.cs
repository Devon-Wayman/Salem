// Author: Devon Wayman - December 2020
using UnityEngine;

public class ParentCollider : MonoBehaviour {

    [Tooltip("Set physics layer this item can interact with")]
    public LayerMask collisionMask;

    [Tooltip("Object to parent to on start")]
    [SerializeField] private string parentObjectName = null;

    [Header("Collider Generation")]
    [Tooltip("Set collider shape")]
    public ColliderType colliderType;
    public enum ColliderType { None, Sphere, Cube }
    public Vector3 boxColliderSize;
    public float sphereColliderRadius;


    private void Awake() {
        this.gameObject.transform.parent = GameObject.Find(parentObjectName).transform;
    }

    private void Start() {
        switch(colliderType) {
            case (ColliderType.None):
                Debug.Log($"Collider set to None on {gameObject.name}");
                break;
            case (ColliderType.Sphere):
                Debug.Log($"Collider set to Sphere on {gameObject.name}");
                gameObject.AddComponent<SphereCollider>();
                this.GetComponent<SphereCollider>().radius = sphereColliderRadius;
                break;
            case (ColliderType.Cube):
                Debug.Log($"Collider set to Cube on {gameObject.name}");
                gameObject.AddComponent<BoxCollider>();
                this.GetComponent<BoxCollider>().size = boxColliderSize;
                break;
        }
    }


    private void OnTriggerEnter(Collider other) {
        Debug.Log($"Collided with {other.name}");
        if (other.TryGetComponent(out EventOnCollide eventCollider)) {
            eventCollider.Execute();
        }
    }
}
