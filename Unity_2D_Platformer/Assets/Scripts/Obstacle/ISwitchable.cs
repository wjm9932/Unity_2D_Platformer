using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISwitchable
{
    public bool isTurnOn { get; }
    public void Trigger();
}
