using UnityEngine;
using UnityEngine.UI;
// just a small change to see how commits work ## and a second one
namespace EmiGd.GUI.Core
{
    [RequireComponent(typeof(Button))]
    public class AttachElementButton: MonoBehaviour
    {
        [SerializeField]
        protected Transform     _container;
        [SerializeField]
        protected GameObject    _prefab;

        public bool StayAtEnd;

        public virtual void OnClick()
        {
            var obj = Instantiate(_prefab);
            _container.AppendChild(obj);
            if (StayAtEnd) {
                transform.SetAsLastSibling();
            }
        }
    }
}
