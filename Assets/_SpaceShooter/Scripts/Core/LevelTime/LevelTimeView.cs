using System;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class LevelTimeView : MonoBehaviour
    {
        [SerializeField] private Text valueLabel;
        
        public void SetTime(TimeSpan span)
        {
            valueLabel.text = ToTimeString(span);
        }

        private string ToTimeString(TimeSpan span)
        {
            return new DateTime(span.Ticks).ToString("mm:ss");
        }
    }
}