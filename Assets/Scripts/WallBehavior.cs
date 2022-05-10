using UnityEngine;

namespace Arcanoid
{
    public class WallBehavior : MonoBehaviour
    {
        [SerializeField]
        private Sides _side;
        public Sides Side
        {
            get
            {
                return _side;
            }
            set
            {
                _side = value;
            }
        }
    }
}
