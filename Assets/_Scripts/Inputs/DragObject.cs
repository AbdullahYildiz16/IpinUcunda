using System;
using _Scripts.RopeMechanic;
using UnityEngine;

namespace _Scripts.Inputs
{
    public class DragObject : MonoBehaviour
    {
        public bool Leftist;
        [HideInInspector]public DragSlot DroppedSlot;
        private RopePuller _ropePuller;
        public RopePuller RopePuller
        {
            get
            {
                if (_ropePuller == null) _ropePuller = GetComponent<RopePuller>();
                return _ropePuller;
            }
        }

        private void OnEnable()
        {
            DragDrop.Instance.AddDragObject(this);
        }

        private void OnDisable()
        {
            DragDrop.Instance.RemoveDragObject(this);
        }

        public void Pick()
        {
            if (DroppedSlot != null)
            {
                DroppedSlot.RemoveDragObject(this);
                DroppedSlot.IsEmpty = true;
            }
            DroppedSlot = null;
        }

        public void Drop(DragSlot dragSlot)
        {
            DroppedSlot = dragSlot;
        }

        public void SetPos(Vector3 pos)
        {
            transform.position = pos;
        }
    }
}