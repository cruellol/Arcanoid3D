using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Arcanoid
{
    public class AnimateText : MonoBehaviour
    {
        [SerializeField]
        private float _secondsUpdate = 0.15f;
        private Text _currentText;
        private string _initialString;
        private int _charCount;
        private int _index;
        private int _indexOfCurrentAffectedSymbol
        {
            get
            {
                return _index;
            }
            set
            {
                _index = value % _charCount;
            }
        }

        private Coroutine _weirdTextCoroutine;

        private void Awake()
        {
            _currentText = GetComponent<Text>();
            _initialString = _currentText.text;
            _charCount = _initialString.Length;
            _indexOfCurrentAffectedSymbol = UnityEngine.Random.Range(0, _charCount - 1);
        }

        private void OnEnable()
        {
            _weirdTextCoroutine = StartCoroutine(MakeTextWeird());
        }

        private void OnDisable()
        {
            if (_weirdTextCoroutine != null)
            {
                StopCoroutine(_weirdTextCoroutine);
            }

        }

        Dictionary<int, string> ColorScheme = new Dictionary<int, string>();

        public void GenerateColorScheme()
        {
            ColorScheme.Clear();
            for(int i=0;i< _charCount; i++)
            {
                ColorScheme.Add(i, ColorUtility.ToHtmlStringRGB(UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.8f, 1f)));
            }
        }

        IEnumerator MakeTextWeird()
        {
            if (_currentText != null)
            {
                GenerateColorScheme();
                string texttoshow = _initialString;
                while (true)
                {
                    texttoshow = _initialString.Substring(0, _indexOfCurrentAffectedSymbol);                    
                    texttoshow += "<color=#"+ ColorScheme [_indexOfCurrentAffectedSymbol] + ">"+ _initialString.Substring(_indexOfCurrentAffectedSymbol,1) +"</color>";
                    texttoshow += _initialString.Substring(_indexOfCurrentAffectedSymbol+1, _charCount - _indexOfCurrentAffectedSymbol-1);
                    _currentText.text = texttoshow;                    
                    yield return new WaitForSecondsRealtime(_secondsUpdate);
                    _indexOfCurrentAffectedSymbol++;
                }
            }
        }
    }
}
