using System;
using System.Collections.Generic;
using UnityEngine;

public interface ICompositionNodeResettable
{
    public void SetResetAction(Action<int> resetAction, int parentCompositionNodeIndex);
    public Action<int> onResetCompositionNode { set; }
    public int parentCompositionNodeIndex { set; }
}
