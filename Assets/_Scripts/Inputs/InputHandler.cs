using System;
using _Scripts.RopeMechanic;
using _Scripts.UI;
using UnityEngine;

namespace _Scripts.Inputs
{
    public class InputHandler : MonoBehaviour
    {
        public static bool IsActive = true, MudPuSelected, BananaPuSelected;

        private Camera _mainCam;
        private Camera MainCam
        {
            get
            {
                if(_mainCam == null) _mainCam = Camera.main;
                return _mainCam;
            }
        }

        private bool _isDragging;

        [SerializeField] private LayerMask dragDropLayer;

        private DragObject _pickedDragObject;
        private DragSlot _pickedDragSlot;

        private Vector3 _pickPos;

        private void Start()
        {
            IsActive = true;    // For Play Mode Options
        }

        private void Update()
        {
            if (!IsActive)
            {
                if (_isDragging)
                {
                    TryReDrop();
                }
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = MainCam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 150f, dragDropLayer))
                {
                    var dragObject = DragDrop.Instance.GetNearestDragObject(hit.point, 1.5f);
                    if (dragObject != null && !dragObject.RopePuller.IsActive)
                    {
                        _pickedDragObject = dragObject;
                        _pickedDragSlot = _pickedDragObject.DroppedSlot;
                        _pickPos = _pickedDragObject.transform.position;
                        dragObject.Pick();
                        _isDragging = true;
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (MudPuSelected)
                {
                    MudPuSelected = false;

                    Ray ray = MainCam.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit, 150f, dragDropLayer))
                    {
                        var dragObject = DragDrop.Instance.GetNearestDragObject(hit.point, 1f);
                        if (dragObject != null && dragObject.DroppedSlot != null)
                        {
                            dragObject.RopePuller.Weaken();
                            MainCanvas.Instance.MudPowerUpUsed();
                        }
                    }
                }

                if (BananaPuSelected)
                {
                    BananaPuSelected = false;

                    Ray ray = MainCam.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit, 150f, dragDropLayer))
                    {
                        var dragObject = DragDrop.Instance.GetNearestDragObject(hit.point, 1f);
                        if (dragObject != null && dragObject.DroppedSlot != null)
                        {
                            dragObject.RopePuller.DisablePuller();
                            MainCanvas.Instance.BananaPowerUpUsed();
                        }
                    }
                }

                _isDragging = false;    // not sure about the position

                if (_pickedDragObject != null)
                {
                    Ray ray = MainCam.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit, 150f, dragDropLayer))
                    {
                        var dragSlot = DragDrop.Instance.GetNearestDragSlot(hit.point, 1.5f);
                        if (dragSlot != null)
                        {
                            if (dragSlot.IsEmpty && dragSlot.RopeDragSlot && dragSlot.IsLeftist == _pickedDragObject.Leftist)
                            {
                                dragSlot.PlaceDragObject(_pickedDragObject, true);
                                _pickedDragObject.Drop(dragSlot);
                                _pickedDragObject.transform.SetParent(Rope.Instance.transform);
                            }
                            else
                            {
                                TryReDrop();
                            }
                        }
                        else
                        {
                            TryReDrop();
                        }
                    }
                    else
                    {
                        TryReDrop();
                    }
                }

                _pickedDragObject = null;
            }

            if (_isDragging)
            {
                if (_pickedDragObject != null)
                {
                    Ray ray = MainCam.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit hit, 150f, dragDropLayer))
                    {
                        _pickedDragObject.SetPos(hit.point);
                    }
                }
            }
        }

        private void TryReDrop()
        {
            if (_pickedDragSlot != null)
            {
                _pickedDragSlot.PlaceDragObject(_pickedDragObject);
                _pickedDragObject.Drop(_pickedDragSlot);
            }
            else _pickedDragObject.SetPos(_pickPos);
        }
    }
}