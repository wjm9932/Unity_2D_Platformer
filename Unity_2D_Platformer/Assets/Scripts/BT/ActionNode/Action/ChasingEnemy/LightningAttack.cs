using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class LightningAttack : IAction
{
    private Blackboard blackboard;
    private GameObject targetPrefab;
    private GameObject effectPrefab;

    private float fadeDuration = 0.3f;
    private float totalDuration = 0.4f;

    private NodeState currentState;

    public LightningAttack(Blackboard blackBoard)
    {
        this.blackboard = blackBoard;
    }

    public void OnEnter()
    {
        currentState = NodeState.Running;
        blackboard.GetData<ChasingEnemy>("owner").StartCoroutine(AttackSequence());
    }

    public NodeState Execute()
    {
        return currentState;
    }

    public void ExecuteInFixedUpdate() { }

    public void OnExit()
    {
        Object.Destroy(targetPrefab);
    }

    public void OnAnimationEnterEvent() { }
    public void OnAnimationTransitionEvent() { }
    public void OnAnimationExitEvent() { }

    private IEnumerator AttackSequence()
    {
        var owner = blackboard.GetData<ChasingEnemy>("owner");
        var spawnPos = GetSpawnPosition(owner);

        // Ready Effect 持失
        effectPrefab = Object.Instantiate(owner.lightningAttackReadyPrefab, spawnPos, owner.lightningAttackReadyPrefab.transform.rotation);
        yield return FadeOut(effectPrefab, 0.5f);

        Object.Destroy(effectPrefab);

        // Lightning Effect 持失
        effectPrefab = Object.Instantiate(owner.lightningAttackPrefab, spawnPos, owner.lightningAttackPrefab.transform.rotation);
        SoundManager.Instance.PlaySoundEffect(SoundManager.InGameSoundEffectType.ENEMY_CHASING_LIGHNING, 0.2f);

        yield return new WaitForSeconds(totalDuration - fadeDuration);

        // Lightning Fade Out
        effectPrefab.GetComponentInChildren<Collider2D>().enabled = false;
        yield return FadeOut(effectPrefab, fadeDuration);

        Object.Destroy(effectPrefab);

        currentState = NodeState.Success;
    }

    private IEnumerator FadeOut(GameObject obj, float duration)
    {
        if (obj == null) yield break;
        var meshRenderer = obj.GetComponentInChildren<MeshRenderer>();
        if (meshRenderer == null) yield break;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            meshRenderer.material.SetFloat("_Fllipbook_Opacity", alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }
        meshRenderer.material.SetFloat("_Fllipbook_Opacity", 0f);
    }

    private Vector2 GetSpawnPosition(ChasingEnemy owner)
    {
        return new Vector2(owner.transform.position.x, owner.chasing.player.transform.position.y);
    }
}
