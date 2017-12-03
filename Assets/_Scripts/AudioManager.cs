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
        PlayMusic();
    }

    private void LoadMusic()
    {
        AssetBundle music = AssetBundle.LoadFromFile("AssetBundles/audio.music");
        // Main theme
        mainTheme[0] = music.LoadAsset<AudioClip>("FinlandVania - MainTheme - Intro");
        mainTheme[1] = music.LoadAsset<AudioClip>("FinlandVania - MainTheme");
        // Boss battle
        bossBattle[0] = music.LoadAsset<AudioClip>("Boss - Intro");
        bossBattle[1] = music.LoadAsset<AudioClip>("Boss - Main");
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
    }

    private void PlayMusic()
    {
        audioSource.clip = mainTheme[0];
        audioSource.Play();
        currentTime = Time.time;
    }
}
