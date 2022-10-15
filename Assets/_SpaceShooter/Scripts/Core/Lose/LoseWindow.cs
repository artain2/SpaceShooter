using System;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter.Lose
{
    public class LoseWindow : MonoBehaviour
    {
        [SerializeField] private Button restartButton;
        [SerializeField] private Button exitButton;

        private Action _restartAction;
        private Action _exitAction;

        private void Start()
        {
            restartButton.onClick.AddListener(() => _restartAction?.Invoke());
            exitButton.onClick.AddListener(() => _exitAction?.Invoke());
        }


        public void SetRestartAction(Action action)
        {
            _restartAction = action;
        }

        public void SetExitAction(Action action)
        {
            _exitAction = action;
        }

        public void Show()
        {
            transform.SetAsLastSibling();
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}