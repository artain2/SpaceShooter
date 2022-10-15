using System;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class WinWindow : MonoBehaviour
    {
        [SerializeField] private Button okButton;

        private Action _okClickAction;

        private void Start()
        {
            okButton.onClick.AddListener(AtOkClick);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetOkAction(Action action)
        {
            _okClickAction = action;
        }

        private void AtOkClick()
        {
            _okClickAction?.Invoke();
        }
    }
}