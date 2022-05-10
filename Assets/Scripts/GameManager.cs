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
        private GameObject _edge1=null;
        private EdgeScript _edgeScript1 = null;
        [SerializeField]
        private GameObject _edge2 = null;
        private EdgeScript _edgeScript2 = null;
        private BallBehavior _ballsMove = null;
        private RoomGeneration _roomGenerator = null;
        private List<GameObject> _boxes;
        private int _maxBoxes;
        [SerializeField]
        private float _levelShrinkCoef=0.9f;
        // Start is called before the first frame update
        void Start()
        {
            _roomGenerator = GetComponent<RoomGeneration>();
            _boxes=GetBoxes();
            _maxBoxes = _roomGenerator.GetMaxBoxCount();
            _ballsMove = _ball.GetComponent<BallBehavior>();
            _ballsMove.BoxToKill += _ballsMove_BoxToKill;
            _edgeScript1 = _edge1.GetComponent<EdgeScript>();
            _edgeScript1.EdgeTouch += _edgeScript1_EdgeTouch;
            _edgeScript2 = _edge2.GetComponent<EdgeScript>();
            _edgeScript2.EdgeTouch += _edgeScript1_EdgeTouch;

            if (_boxes.Count < _maxBoxes)
            {
                GenerateNew();
            }
            
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
            _roomGenerator.GenerateRoom();
            _boxes = GetBoxes(); 
            ResetBall();
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

        private void _edgeScript1_EdgeTouch(object sender, System.EventArgs e)
        {
            ResetBall();
        }
        private void ResetBall()
        {
            _ball.transform.parent = _player1.transform;
            _ball.transform.localPosition = new Vector3(-1.5f, 0, 0);
            _ball.transform.localRotation = Quaternion.Euler(0, -90, 0);
            _ballsMove.SetStick(true);
            _ballsMove.ResetSpeed();
        }
    } 
}
