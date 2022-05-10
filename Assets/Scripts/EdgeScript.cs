using System;
using UnityEngine;

namespace Arcanoid
{
    public class EdgeScript : MonoBehaviour
    {
        public event EventHandler<EventArgs> EdgeTouch;
        private void OnTriggerEnter(Collider other)
        {
            EdgeTouch?.Invoke(this, new EventArgs());
        }
    } 
}
