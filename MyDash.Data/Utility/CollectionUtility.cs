using System;
using System.Collections.Generic;

namespace MyDash.Data.Utility;

internal static class CollectionUtility
{
    public static void SortedMerge<T>(this IList<T> destList, IList<T> newList) where T : IComparable<T>
    {
        int newIndex = 0, destIndex = 0;
        while (newIndex < newList.Count)
        {
            if (destIndex == destList.Count)
            {
                destList.Add(newList[newIndex++]);
                destIndex++;
            }
            else
            {
                switch (newList[newIndex].CompareTo(destList[destIndex]))
                {
                    case < 0:
                        destList.Insert(destIndex++, newList[newIndex++]);
                        break;

                    case 0:
                    default:
                        // Use the old object
                        destIndex++;
                        newIndex++;
                        break;

                    case > 0:
                        destList.RemoveAt(destIndex);
                        break;
                }
            }
        }

        while (destIndex < destList.Count)
        {
            destList.RemoveAt(destList.Count - 1);
        }
    }
}
