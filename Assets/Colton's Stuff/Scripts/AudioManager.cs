using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource staticSource;
    public AudioSource honkSource;

    [Header("Music")]
    public AudioClip staticClip;
    public AudioClip[] tracks;

    [Header("Honk")]
    public AudioClip honkClip;

    [Header("Sliders")]
    public Slider musicVolumeSlider;
    public Slider staticVolumeSlider;
    public Slider honkVolumeSlider;

    [Header("Toggles")]
    public Toggle musicToggle;
    public Toggle staticToggle;
    public Toggle honkToggle;

    [Header("Text")]
    public TextMeshProUGUI trackNameText;
    public TextMeshProUGUI honkCounterText;

    private int currentTrackIndex = -1;
    private bool isSwitching = false;
    private int honkCount = 0;

    public int numOfHonks => honkCount;

    void Start()
    {
        if (musicVolumeSlider != null)
        {
            musicSource.volume = musicVolumeSlider.value;
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (staticVolumeSlider != null)
        {
            staticSource.volume = staticVolumeSlider.value;
            staticVolumeSlider.onValueChanged.AddListener(SetStaticVolume);
        }

        if (honkVolumeSlider != null)
        {
            honkSource.volume = honkVolumeSlider.value;
            honkVolumeSlider.onValueChanged.AddListener(SetHonkVolume);
        }

        if (musicToggle != null)
        {
            musicToggle.isOn = true;
            musicToggle.onValueChanged.AddListener(SetMusicState);
        }

        if (staticToggle != null)
        {
            staticToggle.isOn = true;
            staticToggle.onValueChanged.AddListener(SetStaticState);
        }

        if (honkToggle != null)
        {
            honkToggle.isOn = true;
        }

        UpdateTrackUI();
        UpdateHonkUI();
    }

    void Update()
    {
        if (!isSwitching)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                NextTrack();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                PreviousTrack();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Honk();
        }
    }

    void NextTrack()
    {
        currentTrackIndex++;

        if (currentTrackIndex >= tracks.Length)
        {
            currentTrackIndex = -1;
        }

        StartCoroutine(SwitchTrack());
    }

    void PreviousTrack()
    {
        currentTrackIndex--;

        if (currentTrackIndex < -1)
        {
            currentTrackIndex = tracks.Length - 1;
        }

        StartCoroutine(SwitchTrack());
    }

    IEnumerator SwitchTrack()
    {
        isSwitching = true;

        musicSource.Stop();

        if (staticToggle == null || staticToggle.isOn)
        {
            if (staticClip != null)
            {
                staticSource.clip = staticClip;
                staticSource.Play();

                yield return new WaitForSecondsRealtime(staticClip.length);
            }
        }

        if (currentTrackIndex != -1 &&
            (musicToggle == null || musicToggle.isOn))
        {
            musicSource.clip = tracks[currentTrackIndex];
            musicSource.Play();
        }
        else
        {
            musicSource.clip = null;
        }

        UpdateTrackUI();

        isSwitching = false;
    }

    void Honk()
    {
        if (honkToggle != null && !honkToggle.isOn)
        {
            return;
        }

        if (honkClip != null)
        {
            honkSource.PlayOneShot(honkClip);

            honkCount++;

            UpdateHonkUI();
        }
    }

    void UpdateTrackUI()
    {
        if (trackNameText == null)
            return;

        if (currentTrackIndex == -1)
        {
            trackNameText.text = "Radio Off";
        }
        else
        {
            trackNameText.text =
                "Playing: " + tracks[currentTrackIndex].name;
        }
    }

    void UpdateHonkUI()
    {
        if (honkCounterText != null)
        {
            honkCounterText.text = "Honks: " + honkCount;
        }
    }

    void SetMusicVolume(float value)
    {
        musicSource.volume = value;
    }

    void SetStaticVolume(float value)
    {
        staticSource.volume = value;
    }

    void SetHonkVolume(float value)
    {
        honkSource.volume = value;
    }

    void SetMusicState(bool isOn)
    {
        if (!isOn)
        {
            musicSource.Stop();
        }
        else
        {
            if (currentTrackIndex != -1 &&
                musicSource.clip != null)
            {
                musicSource.Play();
            }
        }
    }

    void SetStaticState(bool isOn)
    {
        if (!isOn)
        {
            staticSource.Stop();
        }
    }
}
