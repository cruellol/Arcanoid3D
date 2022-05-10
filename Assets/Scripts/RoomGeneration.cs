using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arcanoid
{
    [ExecuteInEditMode]
    public class RoomGeneration : MonoBehaviour
    {
        [SerializeField, Tooltip("Generates room base on GetEdges cube.")]
        private bool Generate;
        [SerializeField]
        private Material _planeMaterial = null;
        private List<GameObject> _walls = new List<GameObject>();
        private List<GameObject> _boxes = new List<GameObject>();
        private GameObject wallsOwner = null;
        private GameObject boxesOwner = null;
        [SerializeField]
        private GameObject _boxPrefab = null;
        [SerializeField]
        private int _boxesToGenerate = 30;
        [SerializeField]
        private Transform _getEdges = null;
        private Transform _edgesTransform;
        public Transform EdgesTransform
        {
            get
            {
                return _edgesTransform;
            }
            set
            {
                _edgesTransform = value;
            }
        }

        private void Start()
        {
            _edgesTransform = _getEdges;
        }

        void Update()
        {
            if (Generate)
            {
                GenerateRoom();
            }                
            Generate = false;
        }

        internal int GetMaxBoxCount()
        {
            return _boxesToGenerate;
        }

        private System.Random Rand = new System.Random();
        public void GenerateRoom()
        {
            DestroyLast();
            MakeWalls();
            if (_edgesTransform != null)
            {
                foreach (object child in transform)
                {
                    var childTransform = child as Transform;
                    if (childTransform.gameObject.name == "Player1")
                    {
                        childTransform.localPosition = new Vector3(_edgesTransform.localScale.x / 2 - 1, 0, 0);
                    }
                    if (childTransform.gameObject.name == "Player2")
                    {
                        childTransform.localPosition = new Vector3(-_edgesTransform.localScale.x / 2 + 1, 0, 0);
                    }
                    if (childTransform.gameObject.name == "Player1Edge")
                    {
                        childTransform.localPosition = new Vector3(_edgesTransform.localScale.x / 2, 0, 0);
                        childTransform.localScale = new Vector3(1, _edgesTransform.localScale.y, _edgesTransform.localScale.z);
                    }
                    if (childTransform.gameObject.name == "Player2Edge")
                    {
                        childTransform.localPosition = new Vector3(-_edgesTransform.localScale.x / 2, 0, 0);
                        childTransform.localScale = new Vector3(1, _edgesTransform.localScale.y, _edgesTransform.localScale.z);
                    }
                    if (childTransform.gameObject.name == "Ball")
                    {
                        childTransform.localPosition = new Vector3(-_edgesTransform.localScale.x / 2, 0, 0);
                        childTransform.localScale = new Vector3(1, _edgesTransform.localScale.y, _edgesTransform.localScale.z);
                    }
                }

                boxesOwner = new GameObject() { name = "Boxes" };
                boxesOwner.transform.parent = transform;
                boxesOwner.transform.localPosition = Vector3.zero;
                for (int i = 0; i < _boxesToGenerate; i++)
                {
                    var newbox = GameObject.Instantiate(_boxPrefab);
                    newbox.transform.parent = boxesOwner.transform;
                    _boxes.Add(newbox);
                    float x = UnityEngine.Random.Range(-_edgesTransform.localScale.x * 0.2f, _edgesTransform.localScale.x * 0.2f);
                    float y = UnityEngine.Random.Range(-_edgesTransform.localScale.y * 0.4f, _edgesTransform.localScale.y * 0.4f);
                    float z = UnityEngine.Random.Range(-_edgesTransform.localScale.z * 0.4f, _edgesTransform.localScale.z * 0.4f);
                    newbox.transform.localPosition = new Vector3(x, y, z);
                    var mesh = newbox.GetComponent<MeshRenderer>();
                    var light = newbox.GetComponent<Light>();
                    Color newcolor = UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                    var tempMaterial = new Material(mesh.sharedMaterial);
                    tempMaterial.SetColor("_EmissionColor", newcolor);
                    mesh.sharedMaterial = tempMaterial;
                    light.color = newcolor;
                    newbox.transform.localRotation = UnityEngine.Random.rotationUniform;
                }
            }
        }

        private void DestroyLast()
        {
            DestroyImmediate(boxesOwner);
            _boxes.Clear();
            DestroyImmediate(wallsOwner);
            _walls.Clear();
            List<GameObject> ToDelete = new List<GameObject>();
            foreach (object child in transform)
            {
                var childTransform = child as Transform;
                if (childTransform.gameObject.name == "Walls")
                {
                    ToDelete.Add(childTransform.gameObject);
                }
                if (childTransform.gameObject.name == "Boxes")
                {
                    ToDelete.Add(childTransform.gameObject);
                }
            }
            foreach (var delete in ToDelete)
            {
                DestroyImmediate(delete);
            }
        }

        private void MakeWalls()
        {
            wallsOwner = new GameObject() { name = "Walls" };
            wallsOwner.transform.parent = transform;
            wallsOwner.transform.localPosition = Vector3.zero;
            foreach (Sides side in (Sides[])Enum.GetValues(typeof(Sides)))
            {
                var newwall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                WallBehavior wallbehavior = (WallBehavior)newwall.AddComponent(typeof(WallBehavior));
                wallbehavior.Side = side;
                newwall.name = side.ToString();
                newwall.transform.parent = wallsOwner.transform;
                if (_edgesTransform == null)
                {
                    foreach (object child in transform)
                    {
                        var childTransform = child as Transform;
                        if (childTransform.gameObject.name == "GetEdges")
                        {
                            _edgesTransform = childTransform;
                        }
                    }
                }
                var scale = _edgesTransform.localScale;
                var myposition = Vector3.zero;
                switch (side)
                {
                    case Sides.Up:
                        {
                            myposition.y = _edgesTransform.localScale.y / 2;
                            scale.y = 0.1f;
                            break;
                        }
                    case Sides.Down:
                        {
                            myposition.y = -_edgesTransform.localScale.y / 2;
                            scale.y = 0.1f;
                            break;
                        }
                    case Sides.Left:
                        {
                            myposition.z = -_edgesTransform.localScale.z / 2;
                            scale.z = 0.1f;
                            break;
                        }
                    case Sides.Right:
                    default:
                        {
                            myposition.z = _edgesTransform.localScale.z / 2;
                            scale.z = 0.1f;
                            break;
                        }
                }
                newwall.transform.localPosition = myposition;
                newwall.transform.localScale = scale;
                var renderer = newwall.GetComponent<MeshRenderer>();
                renderer.material = _planeMaterial;
                _walls.Add(newwall);
            }
        }
    }
    public enum Sides { Up, Down, Left, Right }
}