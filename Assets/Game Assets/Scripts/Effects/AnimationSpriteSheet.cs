using UnityEngine;
using System.Collections;

namespace NetdShooting.GameEffects
{
    [AddComponentMenu("Effects/Animation Sprite Sheet")]
    public class AnimationSpriteSheet : MonoBehaviour
    {
        public int uvX = 4;
        public int uvY = 2;
        public float fps = 24.0f;

        private Renderer rend;

        void Start()
        {
            rend = GetComponent<Renderer>();
            rend.enabled = true;
        }
        // Update is called once per frame
        void Update()
        {
            int index = Mathf.FloorToInt(Time.time * fps);

            index = index % (uvX * uvY);

            var size = new Vector2(1.0f / uvX, 1.0f / uvY);

            var uIndex = index % uvX;
            var vIndex = index / uvX;
            var offset = new Vector2(uIndex * size.x, 1.0f - size.y - vIndex * size.y);


            rend.material.SetTextureOffset("_MainTex", offset);
            rend.material.SetTextureScale("_MainTex", size);
        }
    }
}