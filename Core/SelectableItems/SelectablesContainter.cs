using System;
using UnityEngine;

namespace EmiGd.GUI.Core
{
    public class SelectablesContainter: MonoBehaviour
    {
        public event Action<int> ActiveElementChanged = delegate { };
        private int elemCounter = 0;
        public void ChangeActive(int id)
        {
            ActiveElementChanged(id);
        }

        public int Register()
        {
            return elemCounter++;
        }
    }
}
