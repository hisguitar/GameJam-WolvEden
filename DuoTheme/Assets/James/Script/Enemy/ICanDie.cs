public interface IDeadable
{
    public bool IsDead { get; }
    public void OnDie(){}
}