using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingEnemy : MonoBehaviour
{
    [SerializeField] public GameObject attackReadyPrefab;
    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] public GameObject lightningAttackReadyPrefab;
    [SerializeField] public GameObject lightningAttackPrefab;

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
                        .AddAction(new Wait(3f), btBuilder.actionManager)
                        .AddRandomAttackSelector()
                            .AddAttackSequence()
                                .AddAction(new ReadyToAttack(btBuilder.blackboard, 1.4f), btBuilder.actionManager)
                                .AddAction(new Wait(1f), btBuilder.actionManager)
                                .AddAction(new LightningAttack(btBuilder.blackboard), btBuilder.actionManager)
                            .EndComposite()
                            .AddAttackSequence()
                                .AddAction(new ReadyToAttack(btBuilder.blackboard, 3f), btBuilder.actionManager)
                                .AddAction(new FireBullet(btBuilder.blackboard), btBuilder.actionManager)
                            .EndComposite()
                        .EndComposite()
                    .EndComposite()
                .EndComposite()

            .EndComposite()
            .Build();
    }


}
