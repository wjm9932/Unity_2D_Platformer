using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimationEventHandler 
{
    public void RegisterEvents();
    public void RemoveEvents();
    public void OnAnimationEnter();
    public void OnAnimationTransition();
    public void OnAnimationExit();
    public void OnAnimatorIK();
    
}
