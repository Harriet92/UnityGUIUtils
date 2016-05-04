using UnityEngine;
using UnityEngine.UI;

namespace EmiGd.GUI.Core
{
    [RequireComponent(typeof(Image))]
    public class ColorSelectableItem : SelectableItem
    {
        public Color ActiveColor;
        public Color InaActiveColor;

        private Image _image;

        protected override void OnEnable()
        {
            base.OnEnable();
            _image = GetComponent<Image>();
            OnActiveChanged();
        }

        protected override void OnActiveChanged()
        {
            _image.color = IsActive ? ActiveColor : InaActiveColor;
        }
    }
}
