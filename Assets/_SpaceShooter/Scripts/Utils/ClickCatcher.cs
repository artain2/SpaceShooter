using System;
using UnityEngine;

namespace SpaceShooter
{
    public class ClickCatcher : MonoBehaviour
    {
        public event Action OnClick;
        
        private void OnMouseUpAsButton()
        {
            OnClick?.Invoke();
        }
    }
}