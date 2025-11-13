using System.Collections;
using UnityEngine;

namespace YawVR
{
    public class Blinking : MonoBehaviour
    {
        [Header("Blinking Settings")]
        [SerializeField] private Color color;
        [SerializeField] private float blinkDelay = 1f;
        
        private Coroutine _coroutine;
        private WaitForSeconds _delay;

        private void Awake()
        {
            _delay = new WaitForSeconds(blinkDelay);
        }

        public void StartBlinking()
        {

            StopBlinking();
            _coroutine = StartCoroutine(BlinkingCoroutine());
        }
        
        public void StopBlinking()
        {
            if (_coroutine != null) StopCoroutine(_coroutine);
        }
        
        private IEnumerator BlinkingCoroutine()
        {
            bool b = false;
            while (true)
            {
                Color toSet = b ? color : Color.black;

                b = !b;

                YawController.Instance().SendLED(toSet);
                yield return _delay;
            }
        }
    }
}