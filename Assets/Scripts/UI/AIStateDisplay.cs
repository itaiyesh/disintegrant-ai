using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace UI
{
    /**
     * Display just above the player health bar that shows the AI state.
     */
    [RequireComponent(typeof(IPanel))]
    public class AIStateDisplay : MonoBehaviour, StateChangeListener
    {
        private EnemyAI enemyAI;
        private Text text;

        private void Start()
        {
            text = GetComponentInChildren<Text>();
            enemyAI = GetComponentInParent<EnemyAI>();

            if (enemyAI)
            {
                text.text = "";
                enemyAI.fsm.SetStateChangeListener(this);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        public void OnStateChange(State oldState, State newState)
        {
            text.text = newState.ToString();
        }
    }
}