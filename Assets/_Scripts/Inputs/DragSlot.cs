using _Scripts.RopeMechanic;
using UnityEngine;

namespace _Scripts.Inputs
{
    public class DragSlot : MonoBehaviour
    {
        public bool IsLeftist, RopeDragSlot;

        public bool IsEmpty
        {
            get => _placedDragObject == null;
            set => _placedDragObject = null;
        }

        private DragObject _placedDragObject;

        private void OnEnable()
        {
            IsLeftist = transform.position.x < 0;
            DragDrop.Instance.AddDragSlot(this);
        }

        private void OnDisable()
        {
            DragDrop.Instance.RemoveDragSlot(this);
        }

        public void PlaceDragObject(DragObject dragObject, bool sendEvent = false)
        {
            if(RopeDragSlot) Rope.Instance.AddPullerToRope(dragObject.RopePuller, IsLeftist);
            // dragObject.OnPlaced();
            _placedDragObject = dragObject;
            dragObject.SetPos(transform.position);
            if(sendEvent)RopePullerPool.OnDragObjectNeeded?.Invoke(IsLeftist);
        }

        public void RemoveDragObject(DragObject dragObject)
        {
            Rope.Instance.RemovePullerFromRope(dragObject.RopePuller, IsLeftist);
            // dragObject.OnRemove();
            _placedDragObject = null;
        }
    }
}