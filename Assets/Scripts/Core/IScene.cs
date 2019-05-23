using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LOT;
using LOT.Core;

public interface IScene
{
    IEnumerator Initialize (GeneralOptions options);
}

