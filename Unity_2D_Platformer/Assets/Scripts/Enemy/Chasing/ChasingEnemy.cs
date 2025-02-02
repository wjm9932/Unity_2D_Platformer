using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingEnemy : MonoBehaviour
{
    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] public GameObject lightningAttackPrefab;
    [SerializeField] public GameObject lightningAttackReadyPrefab;

    private BehaviorTreeBuilder btBuilder;
    private CompositeNode root;
    public Chasing chasing { get; private set; }

    private void Awake()
    {
        btBuilder = GetComponent<BehaviorTreeBuilder>();
        chasing = GetComponent<Chasing>();
    }

    void Start()
    {
        BuildBT();
    }

    void Update()
    {
        root.Evaluate();
    }

    private void BuildBT()
    {
        btBuilder.blackboard.SetData<ChasingEnemy>("owner", this);

        root = btBuilder
            .AddSelector()
                .AddAttackSequence()
                    .AddCondition(() => chasing.player != null && chasing.player.isDead == false)
                    .AddAttackSequence()
                        .AddAction(new Wait(2f), btBuilder.actionManager)
                        .AddRandomAttackSelector()
                            .AddAttackSequence()
                                .AddAction(new ReadyToAttack(btBuilder.blackboard, 3.5f), btBuilder.actionManager)
                                .AddAction(new LightningAttack(btBuilder.blackboard), btBuilder.actionManager)
                            .EndComposite()
                            .AddAttackSequence()
                                .AddAction(new ReadyToAttack(btBuilder.blackboard, 2f), btBuilder.actionManager)
                                .AddAction(new FireBullet(btBuilder.blackboard), btBuilder.actionManager)
                            .EndComposite()
                        .EndComposite()
                    .EndComposite()
                .EndComposite()

            .EndComposite()
            .Build();
    }


}
