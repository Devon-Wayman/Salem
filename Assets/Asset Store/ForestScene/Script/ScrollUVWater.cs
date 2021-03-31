using UnityEngine;

public class ScrollUVWater : MonoBehaviour {

    public Vector2 AnimatRate = new Vector2(0.0f, 0.0f);
    private Vector2 UVOffs = Vector2.zero;
    private Material waterMaterial = null;

    private void Awake() {
        waterMaterial = GetComponent<Renderer>().materials[0];
    }

    void Update() {
        UVOffs += (AnimatRate * Time.deltaTime);

        // Fixed by Devon: Originally was running GetComponent on EVERY update call!
        // Stored reference on awake above and simply change the UV offset on each frame
        waterMaterial.SetTextureOffset("_MainTex", UVOffs);
    }
}
