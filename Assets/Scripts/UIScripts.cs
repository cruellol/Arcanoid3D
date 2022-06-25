using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Arcanoid
{
    public class UIScripts : MonoBehaviour
    {
        [SerializeField]
        private InputAction PauseAction;

        [SerializeField]
        private GameObject _pausePanel;

        [SerializeField]
        private GameObject _introPanel;

        [SerializeField]
        private GameObject _settingsPanel;

        [SerializeField]
        private bool _isOnIntro = false;

        [SerializeField]
        private float _secondsToShowPanel = 0.3f;

        private void Start()
        {
            PauseAction.Enable();
            PauseAction.performed += PauseAction_performed;
            if (_introPanel != null)
            {
                _introPanel.SetActive(false);
                StartCoroutine(ShowPanelAnimation(_introPanel));
            }
            if (_pausePanel != null)
            {
                _pausePanel.SetActive(false);
            }
            if (_settingsPanel != null)
            {
                _settingsPanel.SetActive(false);
            }
        }

        public void StartGame()
        {
            SceneManager.LoadScene(sceneName: "GameScene");
        }

        private void PauseAction_performed(InputAction.CallbackContext obj)
        {
            DebugToFile.Log("Pausing game");
            if (!_pausePanel.activeSelf)
            {
                StartCoroutine(ShowPanelAnimation(_pausePanel));
                Time.timeScale = 0f;
            }
        }

        public void Resume()
        {
            StartCoroutine(HidePanelAnimation(_pausePanel));
            Time.timeScale = 1f;
            DebugToFile.Log("Resume game");
        }

        public void OpenSettings()
        {
            if (_isOnIntro)
            {
                StartCoroutine(ChangePanelsAnimation(_introPanel, _settingsPanel));
            }
            else
            {
                StartCoroutine(ChangePanelsAnimation(_pausePanel, _settingsPanel));
            }
        }
        public void CloseSettings()
        {
            if (_isOnIntro)
            {
                StartCoroutine(ChangePanelsAnimation(_settingsPanel, _introPanel));
            }
            else
            {
                StartCoroutine(ChangePanelsAnimation(_settingsPanel, _pausePanel));
            }
        }

        IEnumerator ChangePanelsAnimation(GameObject from, GameObject to)
        {
            yield return HidePanelAnimation(from);
            yield return ShowPanelAnimation(to);
        }
        IEnumerator HidePanelAnimation(GameObject tohide)
        {
            int countChangeScaleAnimation = 100;
            float SecondsToAnimate = (_secondsToShowPanel / 2) / countChangeScaleAnimation;
            float moveweight = 1f / countChangeScaleAnimation;
            for (int i = countChangeScaleAnimation; i > 0; i -= 1)
            {
                tohide.transform.localScale = new Vector3(moveweight * i, moveweight * i, 1);
                yield return new WaitForSecondsRealtime(SecondsToAnimate);
            }
            tohide.transform.localScale = Vector3.zero;
            tohide.SetActive(false);
        }
        IEnumerator ShowPanelAnimation(GameObject toshow)
        {
            int countChangeScaleAnimation = 100;
            float SecondsToAnimate = (_secondsToShowPanel / 2) / countChangeScaleAnimation;
            float moveweight = 1f / countChangeScaleAnimation;
            toshow.SetActive(true);
            for (int i = 0; i < countChangeScaleAnimation; i += 1)
            {
                toshow.transform.localScale = new Vector3(moveweight * i, moveweight * i, 1);
                yield return new WaitForSecondsRealtime(SecondsToAnimate);
            }
            toshow.transform.localScale = Vector3.one;
        }


        public void CloseGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif     
        }
    }
}
