using UnityEngine;

public interface IPickup
{
    public void getGunStats(gunStats gun);

    public void jumpPowerUp(int amount);
}
