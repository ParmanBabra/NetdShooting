using UnityEngine;
using System.Collections;


namespace NetdShooting.GameEffects
{
    [AddComponentMenu("Effects/Transparencyable")]
    public class Transparencyable : MonoBehaviour
    {
        private bool _started = false;
        private float _currentTime = 0;
        public float Time;
        private Renderer[] renderers;

        public void Start()
        {
            renderers = GetComponentsInChildren<Renderer>();
        }

        public void Update()
        {
            if (!_started)
                return;

            var deltaTime = UnityEngine.Time.deltaTime;
            _currentTime = Mathf.Min(0, _currentTime - deltaTime);

            //foreach (var renderer in renderers)
            //{
            //    Particle
            //    renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, _currentTime / Time);
            //}

            
        }

        public void StartTransparency()
        {
            _started = true;
            _currentTime = Time;
        }
    }
}
