using System;
using UnityEngine;

namespace Arcanoid
{
    public class PlatformBehavior : MonoBehaviour
    {
        public event EventHandler<Sides> Collide;

        private void OnCollisionEnter(Collision collision)
        {
            WallBehavior isItWall;
            if (collision.gameObject.TryGetComponent<WallBehavior>(out isItWall))
            {
                Collide?.Invoke(this, isItWall.Side);
            }
            
        }
    } 
}
