using UnityEngine;

public enum WeaponId { Lightning, Plasma, Laser }

[CreateAssetMenu(menuName = "Audio/Weapon SFX Set")]
public class WeaponSfxSet : ScriptableObject
{
    public AudioClip[] fireClips; // Fire variants
    public AudioClip[] impactClips; // Impact variants
    [Range(0f, 1f)] public float volume = 1f;
    public Vector2 pitchJitter = new Vector2(0.97f, 1.03f);
}

public class Sfx : MonoBehaviour
{
    public static Sfx I;
        
    [Header("Sets")] 
    public WeaponSfxSet lightning;
    public WeaponSfxSet plasma;
    public WeaponSfxSet laser;

    AudioSource oneShot;

    void Awake()
    {
        I = this;
        oneShot = gameObject.AddComponent<AudioSource>();
        oneShot.playOnAwake = false;
        oneShot.loop = false;
        oneShot.spatialBlend = 0f;
        oneShot.dopplerLevel = 0f;
        oneShot.volume = 1f;
    }

    void Play(AudioClip clip, WeaponSfxSet set)
    {
        if (!clip || set == null) return;
        oneShot.pitch = Random.Range(set.pitchJitter.x, set.pitchJitter.y);
        oneShot.PlayOneShot(clip, set.volume);
    }

    WeaponSfxSet GetSet(WeaponId id) => id switch
    {
        WeaponId.Lightning => lightning,
        WeaponId.Plasma => plasma,
        WeaponId.Laser => laser,
        _ => null
    };
    
    static AudioClip Pick(AudioClip[] arr)  => 
            (arr != null && arr.Length > 0) ? arr[Random.Range(0, arr.Length)] : null;
    
    public void Fire(WeaponId id)
    {
        var set = GetSet(id);
        var clip = Pick(set?.impactClips);
        Play(clip, set);
    }
    
    
}
