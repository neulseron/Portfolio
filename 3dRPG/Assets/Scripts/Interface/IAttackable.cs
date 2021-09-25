public interface IAttackable
{
    AttackBehaviour CurrentAttackBehaviour
    {
        get;
    }

    void OnExcuteAttack(int attackIndex);
}
