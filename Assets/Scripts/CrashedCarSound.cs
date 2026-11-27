using UnityEngine;
using UnityEngine.SceneManagement;

// Quiet steam hiss, ticking metal and radiator gurgle coming from a crashed car.
// Gets louder as the player walks closer and pans left/right depending on which side the car is on.
//
// Attaches itself automatically to any sprite using the CrashedCar sprite, so nothing needs
// to be added to the scene. You can also add this component to any object by hand.
public class CrashedCarSound : MonoBehaviour
{
    [SerializeField] private AudioClip clip;                    // leave empty to use Resources/Audio/CrashedCarSteam
    [SerializeField, Range(0f, 1f)] private float maxVolume = 0.25f;
    [Tooltip("Within this distance (world units) the sound is at full volume.")]
    [SerializeField] private float fullVolumeDistance = 2f;
    [Tooltip("Beyond this distance the sound is silent.")]
    [SerializeField] private float silentDistance = 10f;

    private AudioSource source;
    private Transform listener;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Bootstrap()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
        AttachToCrashedCars();
    }

    static void OnSceneLoaded(Scene scene, LoadSceneMode mode) => AttachToCrashedCars();

    static void AttachToCrashedCars()
    {
        foreach (var sr in FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None))
        {
            if (sr.sprite != null && sr.sprite.name.StartsWith("CrashedCar") &&
                sr.GetComponent<CrashedCarSound>() == null)
            {
                sr.gameObject.AddComponent<CrashedCarSound>();
            }
        }
    }

    void Start()
    {
        if (clip == null) clip = Resources.Load<AudioClip>("Audio/CrashedCarSteam");
        if (clip == null) { Debug.LogWarning("CrashedCarSound: missing Resources/Audio/CrashedCarSteam"); return; }

        source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.loop = true;
        source.playOnAwake = false;
        source.spatialBlend = 0f;   // distance handled below in 2D, so the camera's z offset doesn't matter
        source.volume = 0f;
        source.time = Random.Range(0f, clip.length); // don't always start at the same point
        source.Play();
    }

    void Update()
    {
        if (source == null) return;

        if (listener == null)
        {
            var player = FindFirstObjectByType<PlayerHandler>();
            if (player != null) listener = player.transform;
            else if (Camera.main != null) listener = Camera.main.transform;
            else return;
        }

        Vector2 offset = (Vector2)(transform.position - listener.position);
        float distance = offset.magnitude;
        float t = Mathf.InverseLerp(silentDistance, fullVolumeDistance, distance);
        source.volume = maxVolume * t * t;                    // smooth falloff
        source.panStereo = Mathf.Clamp(offset.x / silentDistance, -0.7f, 0.7f);
    }
}
