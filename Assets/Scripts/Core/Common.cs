using UnityEngine;
using System;

namespace LOT.Core.Common
{
    [Flags]
    public enum Direction
    {
        None = 0,
        Horizontal = 1,
        Vertical = 2,
        Both = 3
    }
}