using UnityEngine;

namespace ProjectDawn.Demo
{
    public class CreateEffectOnTrigger : MonoBehaviour
    {
        public string TriggerName;
        public Animator Animator;
        public GameObject[] Prefabs;
        int m_PrefabIndex;

        void Update()
        {
            if (Prefabs.Length == 0 || Animator == null)
                return;

            if (Animator.GetBool("doSwing"))
            {
                Instantiate(Prefabs[m_PrefabIndex]);
                Animator.SetBool("doSwing", true);
                m_PrefabIndex = (m_PrefabIndex + 1) % Prefabs.Length;
            }
        }
    }
}
