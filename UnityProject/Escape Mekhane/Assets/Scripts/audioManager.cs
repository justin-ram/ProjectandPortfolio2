using UnityEngine;

public class audioManager : MonoBehaviour
{
    public static audioManager instance;

    public AudioSource audPlayer;

    private void Awake()
    {
        instance = this;
    }
}
