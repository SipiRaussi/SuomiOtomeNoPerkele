using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    AudioClip[] mainTheme = new AudioClip[2];
    AudioClip[] bossBattle = new AudioClip[2];
    AudioClip gameOver, title, victory;

    AudioClip eagle, explosion, hit, hit2, sqeeck, tsirp;

    AudioSource audioSource;
    float currentTime;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        LoadMusic();
        LoadSFX();
        PlayMusic(0);
    }

    private void LoadMusic()
    {
        AssetBundle music = AssetBundle.LoadFromFile("AssetBundles/audio.music");
        // Main theme
        mainTheme[0] = music.LoadAsset<AudioClip>("FinlandVania - MainTheme - Intro");
        mainTheme[1] = music.LoadAsset<AudioClip>("FinlandVania - MainTheme");
        // Boss battle
        bossBattle[0] = music.LoadAsset<AudioClip>("Bossi - Intro");
        bossBattle[1] = music.LoadAsset<AudioClip>("Bossi - Main");
        // Game Over
        gameOver = music.LoadAsset<AudioClip>("GameOver");
        // Victory
        victory = music.LoadAsset<AudioClip>("Victory");
        // Title
        title = music.LoadAsset<AudioClip>("Title");
    }

    private void LoadSFX()
    {
        AssetBundle sfx = AssetBundle.LoadFromFile("AssetBundles/sfx");

        eagle = sfx.LoadAsset<AudioClip>("Eagle");
        explosion = sfx.LoadAsset<AudioClip>("Explosio");
        hit = sfx.LoadAsset<AudioClip>("Hit");
        hit2 = sfx.LoadAsset<AudioClip>("Hit2");
        sqeeck = sfx.LoadAsset<AudioClip>("Sqeeck!");
        tsirp = sfx.LoadAsset<AudioClip>("Tsirp");
    }

    private void LateUpdate()
    {
        if (audioSource.clip == mainTheme[0] && Time.time >= currentTime + mainTheme[0].length)
        {
            audioSource.clip = mainTheme[1];
            audioSource.Play();
            audioSource.loop = true;
        }

        if (audioSource.clip == bossBattle[0] && Time.time >= currentTime + bossBattle[0].length)
        {
            audioSource.clip = bossBattle[1];
            audioSource.Play();
            audioSource.loop = true;
        }


    }

    public void PlayMusic(int index)
    {
        switch (index)
        {
            case 0: audioSource.clip = mainTheme[0]; break;
            case 1: audioSource.clip = mainTheme[1]; break;
            case 2: audioSource.clip = bossBattle[0]; break;
            case 3: audioSource.clip = bossBattle[1]; break;
            default: audioSource.Stop(); return;
        }
        
        audioSource.Play();
        currentTime = Time.time;
    }
}
