
public interface IState
{
    public void Enter();
    public void Update();
    public void FixedUpdate();
    public void LateUpdate();
    public void Exit();
    public void OnAnimationEnterEvent();
    public void OnAnimationExitEvent();
    public void OnAnimationTransitionEvent();
    public void OnAnimatorIK();

}
