using UnityEngine;
using UnityEngine.EventSystems;

namespace TowerDefence.Game.Controls
{
    public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [Header("UI References")]
        public RectTransform background;
        public RectTransform handle;

        private Vector2 _pointerDownPos;
        private float _radius;

        public Vector2 Direction { get; private set; }

        void Start()
        {
            _radius = background.sizeDelta.x * 0.5f;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                background, 
                eventData.position, 
                eventData.pressEventCamera,
                out _pointerDownPos
            );

            OnDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                background,
                eventData.position,
                eventData.pressEventCamera,
                out localPos
            );

            Vector2 delta = localPos - _pointerDownPos;
            delta = Vector2.ClampMagnitude(delta, _radius);

            handle.anchoredPosition = delta;
            Direction = delta / _radius;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Direction = Vector2.zero;
            handle.anchoredPosition = Vector2.zero;
        }
    }
}
