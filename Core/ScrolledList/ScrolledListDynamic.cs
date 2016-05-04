using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EmiGd.GUI.Core
{
    public class ScrollListDynamic<TData> : ScrolledList<TData, ScrolledListElement<TData>>
    {
        protected List<TData> _elements;
        protected float _childWidth;
        protected float _childHeigth;
        protected int _first = 0;
        protected int _last = 0;
        protected int _count = 10;
        protected float _height = 0f;

        public override void Init()
        {
            if (!IsInitialized) {
                base.Init();
                Debug.Assert(_containerTransform.anchorMax.y == _containerTransform.anchorMin.y, "ScrollListPooled: Vertical stretching must be turned off! " + GetType());
                _elements = new List<TData>();
                var childTrans = _listElementCache.GetComponent<RectTransform>();
                _childWidth = childTrans.rect.width;
                _childHeigth = childTrans.rect.height;
                if (_transformCache.childCount > 0 && _childHeigth > 0) {
                    RectTransform rt = _transformCache.GetChild(0).GetComponent<RectTransform>();
                    _height = rt.rect.height;
                    _count = Mathf.CeilToInt(_height / _childHeigth);
                }
                GetComponent<ScrollRect>().onValueChanged.AddListener((v) => UpdateChildren());
            }
        }

        public void UpdateChildren()
        {
            UpdateChildren(_containerTransform.anchoredPosition.y);
        }

        public void UpdateChildren(float scrolledY)
        {
            int newFirst = Mathf.Clamp(Mathf.CeilToInt(scrolledY / _childHeigth) - 1, 0, _elements.Count - 1);
            int newLast = Mathf.Clamp(Mathf.CeilToInt(scrolledY / _childHeigth) + _count, 0, _elements.Count);
            if (newFirst > _last || newLast < _first) {
                RemoveAllChildren();
                for (int i = _first; i < _last; i++) {
                    AddChild(i, -1);
                }
            }
            else {
                if (_first != newFirst) {
                    for (int i = _first - 1; i >= newFirst; i--) {
                        AddChild(i, 0);
                    }
                    for (int i = _first; i < newFirst; i++) {
                        RemoveChildFirst();
                    }
                }
                if (_last != newLast) {
                    for (int i = _last; i < newLast; i++) {
                        AddChild(i, -1);
                    }
                    for (int i = newLast; i < _last; i++) {
                        RemoveChildLast();
                    }
                }
            }
            _first = newFirst;
            _last = newLast;
        }

        protected override void AddElement(TData element)
        {
            _elements.Add(element);
            UpdateContainerSize();
        }

        protected void UpdateContainerSize()
        {
            float height = _elements.Count * _childHeigth;
            RectTransform rt = _container.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, height);
            UpdateChildren();
        }

        public override void ClearPanel()
        {
            RemoveAllChildren();
            _elements.Clear();
        }

        protected void AddChild(int dataIndex, int childIndex = -1)
        {
            Debug.Assert(dataIndex >= 0 && dataIndex < _elements.Count);
            var _prefabTransform = _listElementCache.transform;
            if (dataIndex >= 0 && dataIndex < _elements.Count) {
                var newElement = SetUpChild(_elements[dataIndex]);
                if (childIndex == -1) {
                    _children.Add(newElement);
                }
                else {
                    _children.Insert(childIndex, newElement);
                }
                Vector3 position = _prefabTransform.localPosition;
                position.y = -dataIndex * _childHeigth;
                newElement.GetComponent<RectTransform>().localPosition = position;
            }
        }

        protected void RemoveChildFirst()
        {
            RemoveChildAt(0);
        }

        protected void RemoveChildLast()
        {
            RemoveChildAt(_children.Count - 1);
        }

        protected void RemoveChildAt(int i)
        {
            if (_children.Count > 0) {
                ObjectPoolManager.DestroyPooled(_children[i]);
                _children.RemoveAt(i);
            }
        }

        protected void RemoveAllChildren()
        {
            _children.ForEach(ObjectPoolManager.DestroyPooled);
            _children.Clear();
            _first = 0;
            _last = 0;
        }        

        public void ScrollTo(int id, float align = 0f)
        {
            Debug.Assert(IsInitialized);
            Vector2 pos = _containerTransform.anchoredPosition;
            pos.y = Mathf.Max(0f, Mathf.Lerp((id - 1) * _childHeigth, id * _childHeigth - _height, align));
            _containerTransform.anchoredPosition = pos;
            UpdateChildren();
        }
    }
}
