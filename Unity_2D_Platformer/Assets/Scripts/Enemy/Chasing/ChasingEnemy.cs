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
                    .AddAction(new Wait(2f), btBuilder.actionManager)
                    .AddAction(new FireBullet(btBuilder.blackboard), btBuilder.actionManager)
                .EndComposite()
            .EndComposite()
            .Build();
    }
}
