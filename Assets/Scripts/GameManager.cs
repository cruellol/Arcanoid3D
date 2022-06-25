using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arcanoid
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _ball = null;
        [SerializeField]
        private GameObject _player1 = null;
        [SerializeField]
        private GameObject _player2 = null;
        [SerializeField]
        private GameObject _edge1 = null;
        private EdgeScript _edgeScript1 = null;
        [SerializeField]
        private GameObject _edge2 = null;
        private EdgeScript _edgeScript2 = null;
        private BallBehavior _ballsMove = null;
        private RoomGeneration _roomGenerator = null;
        private List<GameObject> _boxes;
        private int _maxBoxes;
        [SerializeField]
        private float _levelShrinkCoef = 0.9f;
        [SerializeField]
        private GameObject _player1HealthPanel;
        [SerializeField]
        private GameObject _player2HealthPanel;
        [SerializeField]
        private GameObject _heartImage;

        [SerializeField]
        private int _maxHealth;
        private int _player1Health;
        private int _player2Health;

        // Start is called before the first frame update
        void Start()
        {
            _roomGenerator = GetComponent<RoomGeneration>();
            _boxes = GetBoxes();
            _maxBoxes = _roomGenerator.GetMaxBoxCount();
            _ballsMove = _ball.GetComponent<BallBehavior>();
            _ballsMove.BoxToKill += _ballsMove_BoxToKill;
            _edgeScript1 = _edge1.GetComponent<EdgeScript>();
            _edgeScript1.EdgeTouch += _edgeScript_EdgeTouch;
            _edgeScript2 = _edge2.GetComponent<EdgeScript>();
            _edgeScript2.EdgeTouch += _edgeScript_EdgeTouch;

            DebugToFile.Log(DateTime.Now + ": Game starts");
            if (_boxes.Count < _maxBoxes)
            {
                GenerateNew();
            }
            else
            {
                _player1Health = _maxHealth;
                _player2Health = _maxHealth;
                CheckHealthAndFill();
            }

        }

        private void OnApplicationQuit()
        {
            DebugToFile.Log(DateTime.Now + ": Exit Game");
        }



        public void GenerateHarder()
        {
            Vector3 scale = _roomGenerator.EdgesTransform.localScale;
            scale.x = scale.x * _levelShrinkCoef;
            _roomGenerator.EdgesTransform.localScale = scale;
            GenerateNew();
        }
        public void GenerateNew()
        {
            DebugToFile.Log("Generating new level");
            _player1Health = _maxHealth;
            _player2Health = _maxHealth;
            CheckHealthAndFill();
            _roomGenerator.GenerateRoom();
            _boxes = GetBoxes();
            ResetBall();
        }

        private void CheckHealthAndFill()
        {
            if (_player1Health > 0 && _player2Health > 0)
            {
                ResetHealthForPlayer(_player1HealthPanel, _player1Health);
                ResetHealthForPlayer(_player2HealthPanel, _player2Health);
            }
            else
            {
                CloseGame();
            }
        }

        private static void CloseGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif            
        }

        private void ResetHealthForPlayer(GameObject healthPanel, int healthCount)
        {
            foreach (Transform child in healthPanel.transform)
            {
                Destroy(child.gameObject);
            }
            for (int i = 0; i < healthCount; i++)
            {
                var newheart = GameObject.Instantiate(_heartImage);
                newheart.transform.SetParent(healthPanel.transform);
                newheart.transform.localScale = new Vector3(1, 1, 1);
                newheart.transform.localPosition = Vector3.zero;
                newheart.transform.localRotation = Quaternion.Euler(Vector3.zero);
            }
        }

        private List<GameObject> GetBoxes()
        {
            List<GameObject> toreturn = new List<GameObject>();
            foreach (object child in transform)
            {
                var childTransform = child as Transform;
                if (childTransform.gameObject.name == "Boxes")
                {
                    foreach (object box in childTransform)
                    {
                        var boxtransform = box as Transform;
                        toreturn.Add(boxtransform.gameObject);
                    }
                }
            }
            return toreturn;
        }

        private void _ballsMove_BoxToKill(object sender, BoxScript box)
        {
            _boxes.Remove(box.gameObject);
            Destroy(box.gameObject);
            if (_boxes.Count == 0)
            {
                GenerateNew();
            }
        }

        private void _edgeScript_EdgeTouch(object sender, System.EventArgs e)
        {
            if (sender as EdgeScript == _edgeScript1)
            {

                DebugToFile.Log("Player 1 got hit");
                _player1Health--;
            }
            else
            {
                DebugToFile.Log("Player 2 got hit");
                _player2Health--;
            }
            CheckHealthAndFill();
            ResetBall();
        }
        private void ResetBall()
        {
            _ball.transform.parent = _player1.transform;
            _ball.transform.localPosition = new Vector3(-1.5f, 0, 0);
            _ball.transform.localRotation = Quaternion.Euler(0, -90, 0);
            _ballsMove.SetStick(true);
            _ballsMove.ResetSpeed();
            var pos1 = _player1.transform.localPosition;
            pos1.y = 0;
            pos1.z = 0;
            _player1.transform.localPosition = pos1;
            var pos2 = _player2.transform.localPosition;
            pos2.y = 0;
            pos2.z = 0;
            _player2.transform.localPosition = pos2;
        }
    }
}
