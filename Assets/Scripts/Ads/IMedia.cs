using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IMedia {

    void Initialize();
    void Show(Action complete, Action fail);
    void Destroy();

}
