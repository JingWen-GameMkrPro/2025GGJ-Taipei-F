using UnityEngine;

public enum SoundEffectType
{
    Walk,
    Pickup,
    Shoot,
    Block,
    Explosion,
    Stick,
    Spawn,
    Death,
    Damage1,
    Damage2,
    Join,
    Ready
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] AudioClip[] audioClips;
    
    private static SoundManager _instance;
    public static SoundManager Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundEffect(SoundEffectType soundEffectType)
    {
        _audioSource.PlayOneShot(audioClips[(int)soundEffectType]);
    }
}