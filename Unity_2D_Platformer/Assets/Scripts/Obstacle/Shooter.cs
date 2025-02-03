using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private GameObject rangedPrefabs;
    [SerializeField] private float targetCoolDownTime;
    [SerializeField] private float targetDistance;
    [SerializeField] private float targetVelocity;
    [SerializeField] private float startDelay;

    private float currentCoolDownTime;
    // Start is called before the first frame update
    void Start()
    {
        currentCoolDownTime -= startDelay;
    }

    // Update is called once per frame
    void Update()
    {
        currentCoolDownTime += Time.deltaTime;

        if(currentCoolDownTime >= targetCoolDownTime)
        {
            var weapon = ObjectPoolManager.Instance.GetPoolableObject(rangedPrefabs, this.transform.position, this.transform.rotation).GetComponent<Projectile>();
            weapon.SetTargetDistanceAndVelocity(this.transform.position, targetDistance, targetVelocity);
            currentCoolDownTime = 0f;
        }
    }

    #region Unity Editor
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        Vector3 startPosition = transform.position;
        Vector3 endPosition = transform.position + transform.right * targetDistance;

        Gizmos.DrawLine(startPosition, endPosition);
    }
#endif
    #endregion
}
