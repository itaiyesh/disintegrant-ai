using Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Menu
{
    public class MenuButtonEventListener : MonoBehaviour, IPointerEnterHandler
    {
        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => { SendButtonEvent(ButtonEvent.ButtonClick); });
            }
            else
            {
                Debug.LogError("Button is null. Please add a button component to this prefab.");
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // send a button sound.
            SendButtonEvent(ButtonEvent.ButtonHighlight);
        }

        private static void SendButtonEvent(ButtonEvent buttonEvent)
        {
            EventManager.TriggerEvent<GameMenuButtonAudioEvent, ButtonEvent>(buttonEvent);
        }

        public enum ButtonEvent
        {
            ButtonClick,
            ButtonHighlight
        }
    }
}