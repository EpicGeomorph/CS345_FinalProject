using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Plays a looping background ambience for the whole time a scene (mission) is active.
// No setup needed: it spawns itself automatically when a listed scene loads.
// The clip lives at Assets/Resources/Audio/NYC_Ambience.ogg.
public class AmbientAudio : MonoBehaviour
{
    // Scene name -> clip path inside a Resources folder (no extension).
    // Add more scenes here to give them ambience too, e.g. { "Level_1", "Audio/NYC_Ambience" }.
    static readonly (string scene, string clip)[] SceneAmbience =
    {
        ("Tutorial", "Audio/NYC_Ambience"),
        ("Level_1",  "Audio/NYC_Ambience"),
        ("Level_2",  "Audio/NYC_Ambience_Level2"),
    };

    const float Volume = 0.1485f; // 0.45 * 0.33
    const float FadeInSeconds = 2f;

    static AmbientAudio instance;
    AudioSource source;
    string currentClip;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Bootstrap()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mode != LoadSceneMode.Single) return;

        string clipPath = null;
        foreach (var entry in SceneAmbience)
            if (entry.scene == scene.name) { clipPath = entry.clip; break; }

        if (clipPath == null)
        {
            // Mission is over / this scene has no ambience: stop it.
            if (instance != null) Destroy(instance.gameObject);
            return;
        }

        if (instance == null)
        {
            var go = new GameObject("AmbientAudio");
            DontDestroyOnLoad(go);
            instance = go.AddComponent<AmbientAudio>();
        }
        instance.Play(clipPath);
    }

    void Play(string clipPath)
    {
        if (currentClip == clipPath && source != null && source.isPlaying) return; // keep looping seamlessly

        var clip = Resources.Load<AudioClip>(clipPath);
        if (clip == null) { Debug.LogWarning($"AmbientAudio: couldn't find Resources/{clipPath}"); return; }

        if (source == null) source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = true;
        source.playOnAwake = false;
        source.spatialBlend = 0f; // 2D, same volume everywhere
        source.priority = 200;
        source.volume = 0f;
        source.Play();
        currentClip = clipPath;

        StopAllCoroutines();
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        for (float t = 0; t < FadeInSeconds; t += Time.unscaledDeltaTime)
        {
            source.volume = Mathf.Lerp(0f, Volume, t / FadeInSeconds);
            yield return null;
        }
        source.volume = Volume;
    }
}
