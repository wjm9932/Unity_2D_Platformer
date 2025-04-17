using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingEnemy : MonoBehaviour
{
    [SerializeField] public GameObject attackReadyPrefab;
    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] public GameObject lightningAttackReadyPrefab;
    [SerializeField] public GameObject lightningAttackPrefab;
    private BehaviorTree bt;
    public Chasing chasing { get; private set; }

    private void Awake()
    {
        chasing = GetComponent<Chasing>();
    }

    void Start()
    {
        BuildBT();
    }

    void Update()
    {
        bt.root.Evaluate();
    }

    private void BuildBT()
    {
        Blackboard blackboard = new Blackboard();
        blackboard.SetData<ChasingEnemy>("owner", this);

        bt = new BehaviorTreeBuilder(blackboard)
            .AddSelector()
                .AddAttackSequence()
                    .AddCondition(() => chasing.player != null && chasing.player.isDead == false)
                    .AddAttackSequence()
                        .AddAction(new Wait(5f))
                        .AddRandomAttackSelector()
                            .AddAttackSequence()
                                .AddAction(new ReadyToAttack(blackboard, 1.4f))
                                .AddAction(new Wait(1f))
                                .AddAction(new LightningAttack(blackboard))
                            .EndComposite()
                            .AddAttackSequence()
                                .AddAction(new ReadyToAttack(blackboard, 3f))
                                .AddAction(new FireBullet(blackboard))
                            .EndComposite()
                        .EndComposite()
                    .EndComposite()
                .EndComposite()
            .EndComposite()
            .Build();
    }
}
