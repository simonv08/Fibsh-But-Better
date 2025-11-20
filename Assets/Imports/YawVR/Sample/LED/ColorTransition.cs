using System.Collections;
using UnityEngine;

namespace YawVR
{
    public class ColorTransition : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float time = 1f;
        [SerializeField] private Color[] colors;
        
        private Coroutine _coroutine;
        private WaitForSeconds _delay;

        private void Awake()
        {
            _delay = new WaitForSeconds(time);
        }

        public void StopEffect()
        {
            if (_coroutine != null) StopCoroutine(_coroutine);
        }
        
        public void StartEffect()
        {
            StopEffect();

            _coroutine = StartCoroutine(EffectCoroutine());
        }

        private IEnumerator EffectCoroutine()
        {
            //float effectT = 0;
            float t = 0;
            int index = 0;
            var nextIndex = index + 1;
            while (true)
            {
                Color toSet = Color.Lerp(colors[index], colors[nextIndex], t / time);
                YawController.Instance().SendLED(toSet);
                t += Time.deltaTime;
                if (t >= time)
                {               
                    t = 0;
                    index = nextIndex;
                    nextIndex = index + 1;
                    if (nextIndex >= colors.Length) nextIndex = 0;
                }
                yield return null;
            }
        }
    }
}