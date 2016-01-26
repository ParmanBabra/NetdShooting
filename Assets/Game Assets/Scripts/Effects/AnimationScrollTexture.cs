using UnityEngine;
using System.Collections;

[AddComponentMenu("Effects/Animation Scroll Texture")]
public class AnimationScrollTexture : MonoBehaviour
{
    public float Speed = 0.25f;
    private Renderer rend;
    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var offset = Time.time * (-Speed);
        rend.material.mainTextureOffset = new Vector2(0, offset);
    }
}
