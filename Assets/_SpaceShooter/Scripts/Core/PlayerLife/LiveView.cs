using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class LiveView : MonoBehaviour
    {
        [SerializeField] private Text amountLabel;

        public void SetAmount(string text)
        {
            amountLabel.text = text;
        }
    }
}