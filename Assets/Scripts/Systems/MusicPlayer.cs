using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] tracks;

    private int currentIndex;

    void Awake()
    {
        audioSource.volume = 50;
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        PlayTrack(currentIndex);
    }

    public void PlayTrack(int index)
    {
        if (tracks.Length == 0) return;

        currentIndex = Mathf.Clamp(index, 0, tracks.Length - 1);

        audioSource.clip = tracks[currentIndex];
        audioSource.Play();

        StopAllCoroutines();
        StartCoroutine(WaitForTrackEnd());
    }

    private IEnumerator WaitForTrackEnd()
    {
        yield return new WaitForSeconds(audioSource.clip.length);

        NextTrack();
    }

    public void NextTrack()
    {
        currentIndex = (currentIndex + 1) % tracks.Length;
        PlayTrack(currentIndex);
    }

    public void PreviousTrack()
    {
        currentIndex--;

        if (currentIndex < 0)
            currentIndex = tracks.Length - 1;

        PlayTrack(currentIndex);
    }

    public void SetVolume(int value)
    {
        audioSource.volume = Mathf.Clamp01(value / 100f);
    }
}