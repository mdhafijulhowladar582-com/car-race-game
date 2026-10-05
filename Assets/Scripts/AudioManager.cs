using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private float masterVolume = 0.7f;
    [SerializeField] private float engineVolume = 0.18f;

    private AudioSource engineSource;
    private AudioClip engineClip;
    private AudioClip coinClip;
    private AudioClip damageClip;
    private AudioClip crashClip;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        engineSource = gameObject.AddComponent<AudioSource>();
        engineSource.loop = true;
        engineSource.playOnAwake = false;
        engineSource.spatialBlend = 0f;
        engineSource.volume = engineVolume * masterVolume;

        engineClip = CreateEngineClip();
        coinClip = CreateToneClip(880f, 0.12f, 0.25f);
        damageClip = CreateToneClip(180f, 0.18f, 0.35f);
        crashClip = CreateCrashClip();
    }

    public static AudioManager GetOrCreate()
    {
        if (Instance != null)
            return Instance;

        GameObject audioObject = new GameObject("AudioManager");
        return audioObject.AddComponent<AudioManager>();
    }

    public void UpdateEngine(float speed01)
    {
        if (engineSource == null)
            return;

        speed01 = Mathf.Clamp01(speed01);

        if (speed01 <= 0.01f)
        {
            if (engineSource.isPlaying)
                engineSource.Stop();

            return;
        }

        if (!engineSource.isPlaying)
        {
            engineSource.clip = engineClip;
            engineSource.Play();
        }

        engineSource.pitch = Mathf.Lerp(0.75f, 1.55f, speed01);
        engineSource.volume = Mathf.Lerp(0.04f, engineVolume, speed01) * masterVolume;
    }

    public void StopEngine()
    {
        if (engineSource != null && engineSource.isPlaying)
            engineSource.Stop();
    }

    public void PlayCoin()
    {
        PlayOneShot(coinClip, 0.75f);
    }

    public void PlayDamage()
    {
        PlayOneShot(damageClip, 0.8f);
    }

    public void PlayCrash()
    {
        PlayOneShot(crashClip, 0.9f);
    }

    private void PlayOneShot(AudioClip clip, float volume)
    {
        if (clip == null)
            return;

        AudioSource.PlayClipAtPoint(clip, Vector3.zero, volume * masterVolume);
    }

    private AudioClip CreateEngineClip()
    {
        const int sampleRate = 22050;
        const float length = 2f;
        int samples = Mathf.RoundToInt(sampleRate * length);
        AudioClip clip = AudioClip.Create("ProceduralEngine", samples, 1, sampleRate, false);
        float[] data = new float[samples];

        for (int i = 0; i < samples; i++)
        {
            float t = i / (float)sampleRate;
            float sample = Mathf.Sin(2f * Mathf.PI * 70f * t) * 0.55f;
            sample += Mathf.Sin(2f * Mathf.PI * 140f * t) * 0.22f;
            sample += Mathf.Sin(2f * Mathf.PI * 210f * t) * 0.1f;
            data[i] = sample * 0.45f;
        }

        clip.SetData(data, 0);
        return clip;
    }

    private AudioClip CreateToneClip(float frequency, float length, float volume)
    {
        const int sampleRate = 22050;
        int samples = Mathf.RoundToInt(sampleRate * length);
        AudioClip clip = AudioClip.Create("ProceduralTone", samples, 1, sampleRate, false);
        float[] data = new float[samples];

        for (int i = 0; i < samples; i++)
        {
            float t = i / (float)sampleRate;
            float envelope = 1f - (i / (float)samples);
            data[i] = Mathf.Sin(2f * Mathf.PI * frequency * t) * envelope * volume;
        }

        clip.SetData(data, 0);
        return clip;
    }

    private AudioClip CreateCrashClip()
    {
        const int sampleRate = 22050;
        const float length = 0.3f;
        int samples = Mathf.RoundToInt(sampleRate * length);
        AudioClip clip = AudioClip.Create("ProceduralCrash", samples, 1, sampleRate, false);
        float[] data = new float[samples];

        Random.State state = Random.state;
        Random.InitState(9127);

        for (int i = 0; i < samples; i++)
        {
            float t = i / (float)sampleRate;
            float envelope = 1f - (i / (float)samples);
            float noise = Random.Range(-1f, 1f);
            float low = Mathf.Sin(2f * Mathf.PI * 90f * t);
            data[i] = (noise * 0.65f + low * 0.35f) * envelope * 0.45f;
        }

        Random.state = state;
        clip.SetData(data, 0);
        return clip;
    }
}
