using System;
using UnityEngine;

namespace SpaceShooter.Map
{
    public class MapLevelView : MonoBehaviour
    {
        public event Action<MapLevelView> OnClick;
        
        [SerializeField] private Collider clickCatcher;
        [SerializeField] private MeshRenderer planetRenderer;
        [SerializeField] private int level;
        [SerializeField] private Material openMat;
        [SerializeField] private Material closeMat;
        

        public Collider ClickCatcher => clickCatcher;
        public int Level => level;

        private void Start()
        {
            clickCatcher.GetComponent<ClickCatcher>().OnClick += () =>
            {
                OnClick?.Invoke(this);
            };
        }

        public void SetOpen(bool open)
        {
            planetRenderer.sharedMaterial = open ? openMat : closeMat;
            clickCatcher.enabled = open;
        }
    }
}