using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Inputs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.RopeMechanic
{
    public class RopePullerPool : MonoBehaviour
    {
        public static Action<bool> OnDragObjectNeeded;

        [SerializeField] private List<RopePuller> leftRopePullerPrefabs = new List<RopePuller>();
        [SerializeField] private List<RopePuller> rightRopePullerPrefabs = new List<RopePuller>();

        [SerializeField] private List<DragSlot> leftDragSlots = new List<DragSlot>();
        [SerializeField] private List<DragSlot> rightDragSlots = new List<DragSlot>();

        private void OnEnable()
        {
            OnDragObjectNeeded += AddRandomRopePuller;
        }

        private void OnDisable()
        {
            OnDragObjectNeeded -= AddRandomRopePuller;
        }

        private void Start()
        {
            foreach (var leftDragSlot in leftDragSlots)
            {
                AddRandomRopePuller(true);
            }
            foreach (var rightDragSlot in rightDragSlots)
            {
                AddRandomRopePuller(false);
            }
        }

        // private void Update()
        // {
        //     if (Input.GetKeyUp(KeyCode.D))
        //     {
        //         AddRandomRopePuller(true);
        //     }
        //     if (Input.GetKeyUp(KeyCode.F))
        //     {
        //         AddRandomRopePuller(false);
        //     }
        // }

        private void AddRandomRopePuller(bool toLeft)
        {
            if (toLeft)
            {
                if (leftDragSlots.Any(ds => ds.IsEmpty))
                {
                    var emptyDs = leftDragSlots.Find(ds => ds.IsEmpty);
                    var puller = Instantiate(leftRopePullerPrefabs[Random.Range(0, leftRopePullerPrefabs.Count)]);
                    puller.transform.position = emptyDs.transform.position;
                    ParticleManager.Instance.PlayPoofParticle(emptyDs.transform.position + Vector3.up);
                    puller.transform.rotation = Quaternion.LookRotation(Vector3.back, Vector3.up);
                    var dO = puller.GetComponent<DragObject>();
                    emptyDs.PlaceDragObject(dO);
                    dO.Drop(emptyDs);
                }
            }
            else
            {
                if (rightDragSlots.Any(ds => ds.IsEmpty))
                {
                    var emptyDs = rightDragSlots.Find(ds => ds.IsEmpty);
                    var puller = Instantiate(rightRopePullerPrefabs[Random.Range(0, rightRopePullerPrefabs.Count)]);
                    puller.transform.position = emptyDs.transform.position;
                    ParticleManager.Instance.PlayPoofParticle(emptyDs.transform.position + Vector3.up);
                    puller.transform.rotation = Quaternion.LookRotation(Vector3.back, Vector3.up);
                    var dO = puller.GetComponent<DragObject>();
                    emptyDs.PlaceDragObject(dO);
                    dO.Drop(emptyDs);
                }
            }
        }
    }
}