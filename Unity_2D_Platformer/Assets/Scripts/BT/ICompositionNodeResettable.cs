using System;
using System.Collections.Generic;
using UnityEngine;

public interface ICompositionNodeResettable
{
    void SetResetAction(Action resetAction);
}
