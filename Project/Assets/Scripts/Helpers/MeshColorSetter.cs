using System;
using UnityEngine;

namespace TowerDefence.Helpers
{
    public class MeshColorSetter : MonoBehaviour
    {
        [Serializable]
        public class MeshColorizationInfo
        {
            [SerializeField] private Renderer renderer;
            [SerializeField] private string colorPropertyName = "_BaseColor";

            private int _colorPropertyId;
            private MaterialPropertyBlock _propertyBlock;

            public MeshColorizationInfo()
            {
                colorPropertyName = "_BaseColor";
            }

            public void Initizalize()
            {
                _colorPropertyId = Shader.PropertyToID(colorPropertyName);
                _propertyBlock = new MaterialPropertyBlock();
                renderer.GetPropertyBlock(_propertyBlock);
            }

            public void SetColor(in Color color)
            {
                _propertyBlock.SetColor(_colorPropertyId, color);
                renderer.SetPropertyBlock(_propertyBlock);
            }
        }

        [SerializeField] private MeshColorizationInfo[] colorizationInfos;

        private bool _isInitialized;

        public void SetColor(Color color)
        {
            if (!_isInitialized) Initialize();

            foreach (var meshColorizationInfo in colorizationInfos)
                meshColorizationInfo.SetColor(in color);
        }

        private void Initialize()
        {
            foreach (var meshColorizationInfo in colorizationInfos)
                meshColorizationInfo.Initizalize();
        }
    }
}
