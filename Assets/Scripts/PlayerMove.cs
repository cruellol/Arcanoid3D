using System.Collections.Generic;
using UnityEngine;

namespace Arcanoid
{
    public enum Players { Player1, Player2 }
    public class PlayerMove : MonoBehaviour
    {
        GameInputs inputs;
        [SerializeField]
        private float _acceleration = .05f;
        [SerializeField]
        private Players _player = Players.Player1;
        [SerializeField]
        private GameObject _platform = null;
        private PlatformBehavior _platformBehavior;
        private float _speedY = 0;
        private float _speedZ = 0;
        [SerializeField]
        private float slowing = 0.985f;
        [SerializeField]
        private GameObject _ball = null;
        private BallBehavior _ballsMove;
        private void OnEnable()
        {
            inputs = new GameInputs();
            inputs.Enable();
            inputs.NewMap.Release.performed += Release_performed;
#if UNITY_EDITOR
            inputs.NewMap.ReleaseEditor.performed += Release_performed;
#endif
        }
        private void Start()
        {
            if (_player == Players.Player1)
            {
                if (_ball != null)
                {
                    _ballsMove = _ball.GetComponent<BallBehavior>();
                }
                else
                {
                    DebugToFile.Log("Add ball link!");
                }
            }
            if (_platform != null)
            {
                _platformBehavior = _platform.GetComponent<PlatformBehavior>();
            }
            else
            {
                DebugToFile.Log("Add _platform link!");
            }            
            _platformBehavior.Collide += _platformBehavior_Collide;
        }

        private List<Sides> BlockedSides = new List<Sides>();

        private void _platformBehavior_Collide(object sender, Sides sideToBlock)
        {
            _speedY = 0;
            _speedZ = 0;
            BlockedSides.Add(sideToBlock);
        }


        private void Release_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            if (_player == Players.Player1)
            {
                DebugToFile.Log("Ball released");
                _ball.transform.parent = null;
                _ballsMove.SetStick(false);
            }
        }


        private void Update()
        {
            if (_player == Players.Player1)
            {
                if (inputs.NewMap.Movement1.IsPressed())
                {
                    var move = inputs.NewMap.Movement1.ReadValue<Vector2>();
                    if (move.y > 0)
                    {
                        if (BlockedSides.Contains(Sides.Up))
                        {
                            move.y = 0;
                        }
                        if (BlockedSides.Contains(Sides.Down))
                        {
                            BlockedSides.Remove(Sides.Down);
                        }
                    }
                    if (move.y < 0)
                    {
                        if (BlockedSides.Contains(Sides.Down))
                        {
                            move.y = 0;
                        }
                        if (BlockedSides.Contains(Sides.Up))
                        {
                            BlockedSides.Remove(Sides.Up);
                        }
                    }
                    if (move.x > 0)
                    {
                        if (BlockedSides.Contains(Sides.Right))
                        {
                            move.x = 0;
                        }
                        if (BlockedSides.Contains(Sides.Left))
                        {
                            BlockedSides.Remove(Sides.Left);
                        }
                    }
                    if (move.x < 0)
                    {
                        if (BlockedSides.Contains(Sides.Left))
                        {
                            move.x = 0;
                        }
                        if (BlockedSides.Contains(Sides.Right))
                        {
                            BlockedSides.Remove(Sides.Right);
                        }
                    }
                    _speedY = move.y * _acceleration;
                    _speedZ = move.x * _acceleration;
                }
            }
            else
            {
                if (inputs.NewMap.Movement2.IsPressed())
                {
                    var move = inputs.NewMap.Movement2.ReadValue<Vector2>();
                    if (move.y > 0)
                    {
                        if (BlockedSides.Contains(Sides.Up))
                        {
                            move.y = 0;
                        }
                        if (BlockedSides.Contains(Sides.Down))
                        {
                            BlockedSides.Remove(Sides.Down);
                        }
                    }
                    if (move.y < 0)
                    {
                        if (BlockedSides.Contains(Sides.Down))
                        {
                            move.y = 0;
                        }
                        if (BlockedSides.Contains(Sides.Up))
                        {
                            BlockedSides.Remove(Sides.Up);
                        }
                    }
                    if (move.x < 0)
                    {
                        if (BlockedSides.Contains(Sides.Right))
                        {
                            move.x = 0;
                        }
                        if (BlockedSides.Contains(Sides.Left))
                        {
                            BlockedSides.Remove(Sides.Left);
                        }
                    }
                    if (move.x > 0)
                    {
                        if (BlockedSides.Contains(Sides.Left))
                        {
                            move.x = 0;
                        }
                        if (BlockedSides.Contains(Sides.Right))
                        {
                            BlockedSides.Remove(Sides.Right);
                        }
                    }
                    _speedY = move.y * _acceleration;
                    _speedZ = -move.x * _acceleration;
                }

            }
            _speedY *= slowing;
            _speedZ *= slowing;
            var pos = transform.position;
            pos.y += _speedY * Time.deltaTime;
            pos.z += _speedZ * Time.deltaTime;
            transform.position = pos;
        }

        //private void OnCollisionEnter(Collision collision)
        //{
        //    _speedY = 0;
        //    _speedZ = 0;
        //}




        private void OnDisable()
        {
            inputs.Disable();
        }
    }
}
