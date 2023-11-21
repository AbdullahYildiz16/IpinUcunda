using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Scripts.Utilities
{
    public static class Helper
    {
        public static T GetHighestFromList<T>(this List<T> list, Func<T, float> predicate)
        {
            if (list.Count <= 0) throw new Exception("List cannot be null");

            var highest = predicate(list[0]);
            var highestIdx = 0;
            for (var i = 0; i < list.Count; i++)
            {
                var value = predicate(list[i]);

                if (!(value > highest)) continue;
                highestIdx = i;
                highest = value;
            }

            return list[highestIdx];
        }

        public static T GetLowestFromList<T>(this List<T> list, Func<T, float> predicate)
        {
            if (list.Count <= 0) throw new Exception("List cannot be null");

            var lowest = predicate(list[0]);
            var lowestIdx = 0;
            for (var i = 0; i < list.Count; i++)
            {
                var value = predicate(list[i]);

                if (!(value < lowest)) continue;
                lowestIdx = i;
                lowest = value;
            }

            return list[lowestIdx];
        }

        public static IEnumerator Move(this Transform transform, Vector3 pos, float duration)
        {
            var startPos = transform.position;
            var elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                yield return null;
                elapsedTime += Time.deltaTime;
                if (elapsedTime > duration) elapsedTime = duration;
                transform.position = Vector3.Lerp(startPos, pos, elapsedTime / duration);
            }
        }

        public static T GetRandom<T>(this List<T> list)
        {
            if(list.Count < 1) Debug.Log("Need at least 1");
            if (list.Count == 1) return list[0];
            return list[Random.Range(0, list.Count)];
        }
    }
}