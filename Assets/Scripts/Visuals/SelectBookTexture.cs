// Author: Devon Wayman
using UnityEngine;

/// <summary>
/// Selects random diffuse texture for the opening menu book
/// </summary>
namespace Visuals {
    public class SelectBookTexture : MonoBehaviour {
        
        // Renderer attatched to book object on main menu
        private Renderer bookRenderer = null; 

        // Array of starting book textures
        private Texture[] bookTextures = null; 
        
        private void Awake() {
            bookTextures = Resources.LoadAll<Texture2D>("Textures/StartBookEntries");

            bookRenderer = GetComponent<Renderer>();

            int selection = Random.Range (0, bookTextures.Length);
            bookRenderer.material.mainTexture = bookTextures[selection];
        }
    }
}