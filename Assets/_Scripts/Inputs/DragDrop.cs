using System.Collections.Generic;
using _Scripts.Utilities;
using UnityEngine;

namespace _Scripts.Inputs
{
    public class DragDrop : MonoBehaviour
    {
        #region Singleton

        public static DragDrop Instance;

        private void Awake()
        {
            Instance = this;
        }

        #endregion

        private List<DragObject> _dragObjects = new List<DragObject>();
        private List<DragSlot> _dragSlots = new List<DragSlot>();

        public DragObject GetNearestDragObject(Vector3 point, float maxDis)
        {
            if (_dragObjects.Count < 1) return null;
            var nearestDragObj = _dragObjects.GetLowestFromList(dragObj => Vector3.Distance(dragObj.transform.position, point));
            return Vector3.Distance(nearestDragObj.transform.position, point) < maxDis ? nearestDragObj : null;
        }

        public DragSlot GetNearestDragSlot(Vector3 point, float maxDis)
        {
            if (_dragSlots.Count < 1) return null;
            var nearestDragSlot = _dragSlots.GetLowestFromList(dragSlot => Vector3.Distance(dragSlot.transform.position, point));
            return Vector3.Distance(nearestDragSlot.transform.position, point) < maxDis ? nearestDragSlot : null;
        }

        #region ListMethods

        public void AddDragObject(DragObject dragObject)
        {
            _dragObjects.Add(dragObject);
        }

        public void RemoveDragObject(DragObject dragObject)
        {
            if (_dragObjects.Contains(dragObject)) _dragObjects.Remove(dragObject);
        }

        public void AddDragSlot(DragSlot dragSlot)
        {
            _dragSlots.Add(dragSlot);
        }

        public void RemoveDragSlot(DragSlot dragSlot)
        {
            if (_dragSlots.Contains(dragSlot)) _dragSlots.Remove(dragSlot);
        }

        #endregion
    }
}