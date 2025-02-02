using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningAttack : IAction
{
    private Blackboard blackboard;
    private GameObject targetPrefab;
    private float targetTime;
    private float timeElapsed;

    private float fadeDuration = 0.3f;
    private bool isFading = false; 

    public LightningAttack(Blackboard blackBoard)
    {
        this.blackboard = blackBoard;
        targetTime = 0.5f;
    }

    public void OnEnter()
    {
        timeElapsed = 0f;
        Fire();
    }

    public NodeState Execute()
    {
        timeElapsed += Time.deltaTime;

        // 0.2초가 지나면 투명화 시작
        if (!isFading && timeElapsed >= targetTime - fadeDuration)
        {
            isFading = true;
        }

        // 투명화 진행
        if (isFading)
        {
            float alpha = Mathf.Lerp(1f, 0f, (timeElapsed - (targetTime - fadeDuration)) / fadeDuration);
            SetAlpha(alpha);
        }

        if (timeElapsed >= targetTime)
        {
            return NodeState.Success;
        }

        return NodeState.Running;
    }

    public void ExecuteInFixedUpdate() { }

    public void OnExit()
    {
        Object.Destroy(targetPrefab);
    }

    public void OnAnimationEnterEvent() { }
    public void OnAnimationTransitionEvent() { }
    public void OnAnimationExitEvent() { }

    private void Fire()
    {
        var transform = blackboard.GetData<ChasingEnemy>("owner").transform;
        var target = blackboard.GetData<ChasingEnemy>("owner").lightningAttackPrefab;
        targetPrefab = Object.Instantiate(target, transform.position, target.transform.rotation);
    }

    private void SetAlpha(float alpha)
    {
        if (targetPrefab == null) return;

        MeshRenderer meshRenderer = targetPrefab.GetComponentInChildren<MeshRenderer>();
        if (meshRenderer == null) return;

        Material material = meshRenderer.material;
        material.SetFloat("_Fllipbook_Opacity", Mathf.Clamp01(alpha));
    }
}
