// Author: Devon Wayman
using UnityEngine;

/// <summary>
/// Selects random diffuse texture for the opening menu book
/// </summary>
namespace Salem.Core.Interaction {
    public class SelectBookTexture : MonoBehaviour {
        
        // Renderer attatched to book object on main menu
        private Renderer bookRenderer = null; 

        // Array of starting book textures
        private Texture[] bookTextures = null; 
        
        private void Awake() {
            // Get all book page textures from Resources folder
            bookTextures = Resources.LoadAll<Texture2D>("Textures/StartBookEntries");

            // Set reference to books renderer
            bookRenderer = GetComponent<Renderer>();

            // Load book textures from resources and select one at random
            int selection = Random.Range (0, bookTextures.Length);
            bookRenderer.material.mainTexture = bookTextures[selection];
        }
    }
}