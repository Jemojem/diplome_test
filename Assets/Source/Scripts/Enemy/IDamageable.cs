public interface IDamageable
{
    bool CanTakeDamage { get; }
    void TakeDamage(int amount, bool isPermanent = false,bool isPet = false);
}


