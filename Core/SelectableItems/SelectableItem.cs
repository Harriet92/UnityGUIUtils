using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EmiGd.GUI.Core
{
    public class SelectableItem : MonoBehaviour, IPointerClickHandler
    {
        public bool IsActive
        {
            get
            {
                return _isActive;
            }
            private set
            {
                _isActive = value;
                OnActiveChanged();
            }
        }

        private SelectablesContainter container;
        private bool    _isActive;
        private int     _id;

        protected virtual void OnEnable()
        {
            container = gameObject.FindInParents<SelectablesContainter>();
            if (container != null) {
                container.ActiveElementChanged += Container_ActiveElementChanged;
                _id = container.Register();
            }
        }

        private void Container_ActiveElementChanged(int id)
        {
            IsActive = _id == id && !IsActive;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (container != null) {
                container.ChangeActive(_id);
            }
        }

        protected virtual void OnActiveChanged() { }
    }
}
