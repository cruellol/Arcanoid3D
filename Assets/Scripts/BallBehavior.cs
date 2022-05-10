using System;
using UnityEngine;

namespace Arcanoid
{
    public class BallBehavior : MonoBehaviour
    {
        [SerializeField]
        private float _speed=10;
        private float _currentspeed;
        [SerializeField]
        private float _speedOnKill = 0.3f;

        [SerializeField]
        private bool _sticked = true;

        private void Start()
        {
            _currentspeed = _speed;
        }

        // Update is called once per frame
        void Update()
        {
            if (!_sticked)
            {
                gameObject.transform.localPosition += gameObject.transform.forward * _speed * Time.deltaTime;
            }            
        }
        public void ResetSpeed()
        {
            _currentspeed = _speed;
        }

        public event EventHandler<BoxScript> BoxToKill;
        private void OnCollisionEnter(Collision collision)
        {
            transform.rotation=Quaternion.LookRotation(Vector3.Reflect(gameObject.transform.forward, collision.GetContact(0).normal));

            BoxScript isItBox;
            if (collision.gameObject.TryGetComponent<BoxScript>(out isItBox))
            {
                BoxToKill?.Invoke(this, isItBox);
                _speed += _speedOnKill;
            }         
        }

        internal void SetStick(bool isSticked)
        {
            _sticked = isSticked;
        }
    } 
}
