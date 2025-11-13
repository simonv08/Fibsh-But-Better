using System;
using System.Collections;
using UnityEngine;

namespace YawVR
{
    public class Rainbow : MonoBehaviour
    {
        [SerializeField] private float loopDelay = 0.02f;
        
        private Coroutine _coroutine;
        private WaitForSeconds _delay;

        private Color32[] _colors = new Color32[129];
        private byte[] _bytes = new byte[390];

        private double _multiplier = 10;
        private ushort _counter = 0;
        
        private void Awake()
        {
            _delay = new WaitForSeconds(loopDelay);
        }

        public void StartRainbow()
        {
            StopRainbow();
            _coroutine = StartCoroutine(RainbowCoroutine());
        }
        
        public void StopRainbow()
        {
            if (_coroutine != null) StopCoroutine(_coroutine);
        }
        
        private IEnumerator RainbowCoroutine()
        {
            while (true)
            {
                for (int i = 0; i < _colors.Length; i++)
                {
                    _colors[i].g = (byte)(Math.Sin(X(i)) * 255);
                    _colors[i].r = (byte)(-Math.Sin(X(i)) * 255);
                    _colors[i].b = (byte)(-Math.Cos(X(i)) * 255);
                }
                _counter++;

                YawController.Instance().SendLED(_colors);
                yield return _delay;
            }
        }

        private double X(int i)
        {
            return (_multiplier * (i + _counter)) / 129;
        }
    }
}
