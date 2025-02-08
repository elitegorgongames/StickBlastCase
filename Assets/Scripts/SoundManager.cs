using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance;


    [SerializeField] private AudioClip _shineAudioClip;
    [SerializeField] private AudioSource _shineAudioSource;

    [SerializeField] private AudioClip _dissolveAudioClip;
    [SerializeField] private AudioSource _dissolveAudioSource;

    [SerializeField] private AudioClip _diamondReachedTargetAudioClip;
    [SerializeField] private AudioSource _diamondReachedTargetAudioSource;

    [SerializeField] private AudioClip _pickStickAudioClip;
    [SerializeField] private AudioSource _pickStickAudioSource;

    [SerializeField] private AudioClip _settleStickAudioClip;
    [SerializeField] private AudioClip _stickBackToInitialPointSound;  
    [SerializeField] private AudioSource _settleStickAudioSource;

    [SerializeField] private AudioClip _spawnStickSound;
    [SerializeField] private AudioSource _spawnStickAudioSource;

    [SerializeField] private AudioClip _uIAudioClip;
    [SerializeField] private AudioClip _loseAudioClip;
    [SerializeField] private AudioClip _successAudioClip;


    private void Awake()
    {
        Instance = this;
    }


    public void PlayShineAudioClip()
    {
        _shineAudioSource.PlayOneShot(_shineAudioClip);
    }

    public void PlayDissolveAudioClip()
    {
        _dissolveAudioSource.PlayOneShot(_dissolveAudioClip);
    }

    public void PlayDiamondReachedTargetAudioClip()
    {
        _diamondReachedTargetAudioSource.PlayOneShot(_diamondReachedTargetAudioClip);
    }

    public void PlayPickStickAudioClip()
    {
        _pickStickAudioSource.PlayOneShot(_pickStickAudioClip);
    }

    public void PlaySettleStickAudioClip()
    {
        _settleStickAudioSource.PlayOneShot(_settleStickAudioClip);
    }

    public void PlayStickBackToInitialPointClip()
    {
        _settleStickAudioSource.PlayOneShot(_stickBackToInitialPointSound);
    }

    public void PlaySpawnStickClip()
    {
        _spawnStickAudioSource.PlayOneShot(_spawnStickSound);
    }

    public void PlayUIAudioClip()
    {
        _settleStickAudioSource.PlayOneShot(_uIAudioClip);
    }

    public void PlayLoseAudioClip()
    {
        _settleStickAudioSource.PlayOneShot(_loseAudioClip);
    }

    public void PlaySuccessAudioClip()
    {
        _settleStickAudioSource.PlayOneShot(_successAudioClip);
    }
}
