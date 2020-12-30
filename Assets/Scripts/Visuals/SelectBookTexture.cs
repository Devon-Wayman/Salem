// Author: Devon Wayman
using UnityEngine;


namespace Salem.Visuals {
    public class SelectBookTexture : MonoBehaviour {
        
        private Renderer bookRenderer = null; 
        private Texture[] bookTextures = null; 
        
        private void Awake() {
            bookTextures = Resources.LoadAll<Texture2D>("Textures/StartBookEntries");

            bookRenderer = GetComponent<Renderer>();

            int selection = Random.Range (0, bookTextures.Length);
            bookRenderer.material.mainTexture = bookTextures[selection];
        }
    }
}