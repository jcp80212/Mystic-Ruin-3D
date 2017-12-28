using UnityEngine;

namespace RPG.Character
{
    [CreateAssetMenu(menuName = ("RPG/Skill/Single Target Attack"))]
    public class SingleTargetAttackConfig : SkillConfig
    {

        public override SkillBehaviour GetBehaviourComponent(GameObject objectToAttachTo)
        {
            return objectToAttachTo.AddComponent<SingleTargetAttackBehaviour>();
        }
    }
}