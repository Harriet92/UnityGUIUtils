using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;

namespace EmiGd.GUI.Core
{
    public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        #region Inspector
        [SerializeField]
        public GameObject DraggingIconPlaceholder;
        #endregion

        #region Protected
        protected ItemsContainer _itemsContainer;
        #endregion

        #region Private 
        private Transform _draggingIconTransform;
        private GameObject _draggingIcon;
        #endregion

        protected virtual void OnEnable()
        {
            CreateDraggingIcon();
            if (_draggingIcon != null)
                _draggingIcon.Hide();
            _itemsContainer = gameObject.FindInParents<ItemsContainer>();
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            _draggingIcon.Show();
            SetDraggedPosition(eventData);
        }

        public virtual void OnDrag(PointerEventData data)
        {
            SetDraggedPosition(data);
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            if (_itemsContainer != null) {
                _itemsContainer.AttachItem(gameObject);
            }
            _draggingIcon.Hide();
        }

        protected virtual GameObject GetDraggingIcon()
        {
            Assert.IsNotNull(DraggingIconPlaceholder);
            return Instantiate(DraggingIconPlaceholder);
        }

        private void SetDraggedPosition(PointerEventData data)
        {
            GetCachedIconTransform().position = data.pointerCurrentRaycast.screenPosition;
        }

        private void CreateDraggingIcon()
        {
            var canvas = gameObject.FindInParents<Canvas>();
            if(canvas == null) {
                return;
            }
            _draggingIcon = GetDraggingIcon();
            _draggingIcon.transform.SetParent(canvas.transform, false);
            _draggingIcon.transform.SetAsLastSibling();
        }

        private Transform GetCachedIconTransform()
        {
            if (_draggingIconTransform == null && _draggingIcon != null) {
                _draggingIconTransform = _draggingIcon.transform;
            }
            return _draggingIconTransform;
        }
    }
}
