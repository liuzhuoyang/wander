using System;
using PlayerInteraction;
using UnityEngine;

namespace BattleActor.Basement.Skill
{
    public class UI_BasementSkillCaster : MonoBehaviour
    {
        [SerializeField] private ClickableUI clickable_UI;
        private Action<Vector2> onLocationChosen;
        public void Init(Action<Vector2> onLocationChosen)
        {
            this.onLocationChosen = onLocationChosen;
            clickable_UI.Init(OnChooseLocation);
        }
        public void Btn_Cancel()
        {
            Destroy(this.gameObject);
        }
        void OnChooseLocation(Vector2 scrPos)
        {
            onLocationChosen?.Invoke(Camera.main.ScreenToWorldPoint(scrPos));
            Destroy(this.gameObject);
        }
    }
}