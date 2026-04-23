using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    //Sources
    public AudioSource musicSource;
    public AudioSource staticSource;

    //Clips
    public AudioClip staticClip;
    public AudioClip[] tracks;

    //Canvas
    public Slider volumeSlider;
    public TextMeshProUGUI trackNameText;

    private int currentTrackIndex = -1;
    private bool isSwitching = false;

    void Start()
    {
        if (volumeSlider != null)
        {
            musicSource.volume = volumeSlider.value;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        UpdateTrackUI();
    }

    void Update()
    {
        if (isSwitching) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            NextTrack();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            PreviousTrack();
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

        if (staticClip != null && staticSource != null)
        {
            staticSource.clip = staticClip;
            staticSource.Play();

            yield return new WaitForSeconds(staticClip.length);
        }

        if (currentTrackIndex == -1)
        {
            musicSource.clip = null;
        }
        else
        {
            musicSource.clip = tracks[currentTrackIndex];
            musicSource.Play();
        }

        UpdateTrackUI();
        isSwitching = false;
    }

    void SetVolume(float value)
    {
        musicSource.volume = value;
    }

    void UpdateTrackUI()
    {
        if (trackNameText == null) return;

        if (currentTrackIndex == -1)
        {
            trackNameText.text = "Radio Off";
        }
        else
        {
            trackNameText.text = "Now Playing: " + tracks[currentTrackIndex].name;
        }
    }
}
