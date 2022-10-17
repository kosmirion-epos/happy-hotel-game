using UnityEngine;

public abstract class AmmoWeapon : TestWeapon
{
    [SerializeField] protected ScopedValue<int> maxBullets;
    protected int bullets;
    [SerializeField] protected ScopedValue<bool> isInfiniteAmmo;

    public void RemoveBullet() => bullets = Mathf.Max(0, --bullets);

    public void AddBullet() => bullets = Mathf.Min(maxBullets.Value, ++bullets);

    public bool RemoveBulletIfNotEmpty()
    {
        if (bullets <= 0)
            return false;

        --bullets;
        return true;
    }

    public bool AddBulletIfNotFull()
    {
        if (bullets >= maxBullets.Value)
            return false;

        ++bullets;
        return true;
    }

    protected void OnEnable()
    {
        bullets = Mathf.Max(0, maxBullets.Value);
        UpdateBulletDisplay();
    }

    protected abstract void UpdateBulletDisplay();

    protected virtual void OnBulletsDepleted() { }

    public void TryShoot()
    {
        if (bullets > 0 || isInfiniteAmmo.Value)
        {
            Shoot();
            if(!isInfiniteAmmo.Value) RemoveBullet();
            UpdateBulletDisplay();

            if (bullets == 0)
                OnBulletsDepleted();
        }
    }
}