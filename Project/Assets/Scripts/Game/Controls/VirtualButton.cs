using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TowerDefence.Game.Controls
{
    public class VirtualButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Image image;
        [SerializeField] private Color pressedColor = Color.white;

        private Color _originalColor = Color.white;

        public bool IsPressed { get; private set; }

        private void Start()
        {
            if (image == null)
            {
                image = GetComponent<Image>();
            }

            if (image != null)
            {
                _originalColor = image.color;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            IsPressed = true;
            image.color = pressedColor;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            IsPressed = false;
            image.color = _originalColor;
        }
    }
}