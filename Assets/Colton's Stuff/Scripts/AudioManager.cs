using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource staticSource;

    [Header("Audio Clips")]
    public AudioClip staticClip;
    public AudioClip[] tracks;

    [Header("UI - Sliders")]
    public Slider musicVolumeSlider;
    public Slider staticVolumeSlider;

    [Header("UI - Toggles")]
    public Toggle musicToggle;
    public Toggle staticToggle;

    [Header("UI - Text")]
    public TextMeshProUGUI trackNameText;

    private int currentTrackIndex = -1;
    private bool isSwitching = false;

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

        UpdateTrackUI();
    }

    void Update()
    {
        if (isSwitching) return;

        if (Input.GetKeyDown(KeyCode.E))
            NextTrack();

        if (Input.GetKeyDown(KeyCode.R))
            PreviousTrack();
    }

    void NextTrack()
    {
        currentTrackIndex++;

        if (currentTrackIndex >= tracks.Length)
            currentTrackIndex = -1;

        StartCoroutine(SwitchTrack());
    }

    void PreviousTrack()
    {
        currentTrackIndex--;

        if (currentTrackIndex < -1)
            currentTrackIndex = tracks.Length - 1;

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
                yield return new WaitForSeconds(staticClip.length);
            }
        }

        if (currentTrackIndex != -1 && (musicToggle == null || musicToggle.isOn))
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

    void SetMusicVolume(float value)
    {
        musicSource.volume = value;
    }

    void SetStaticVolume(float value)
    {
        staticSource.volume = value;
    }

    void SetMusicState(bool isOn)
    {
        if (!isOn)
        {
            musicSource.Stop();
        }
        else if (currentTrackIndex != -1 && musicSource.clip != null)
        {
            musicSource.Play();
        }
    }

    void SetStaticState(bool isOn)
    {
        if (!isOn)
        {
            staticSource.Stop();
        }
    }

    void UpdateTrackUI()
    {
        if (trackNameText == null) return;

        if (currentTrackIndex == -1)
            trackNameText.text = "Radio Off";
        else
            trackNameText.text = "Now Playing: " + tracks[currentTrackIndex].name;
    }
}
