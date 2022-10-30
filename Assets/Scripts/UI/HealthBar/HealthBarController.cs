using UnityEngine;

namespace UI
{
    /**
     * Controller for the health bar.
     *
     * It enables the health bar for AI enemy players only.
     */
    public class HealthBarController : MonoBehaviour
    {
        private void Awake()
        {
            var enemyAI = GetComponentInParent<EnemyAI>();
            gameObject.SetActive(enemyAI);
        }
    }
}
