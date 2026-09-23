using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCanvasHandler : MonoBehaviour
{
    [Header("Ball Spawning")]
    [SerializeField] private RectTransform spawnButton;
    [SerializeField] private Camera arCamera;
    [SerializeField] private GameObject ballPrefab;

    [SerializeField] private float distanceFromCamera = 0.5f;

    [Header("Game State")]
    [SerializeField] private GameObject reloadButton;
    [SerializeField] private SpawnHandler spawnHandler;

    private GameObject _currentBall;


    void OnEnable()
    {
        CaptureHandler.OnMissedCreature += DisplaySpawnButton;
        CreatureHandler.OnCreatureHatched += DisplaySpawnButton;
        CaptureHandler.OnCreatureCaught += DisplayCaught;
    }


    void OnDisable()
    {
        CaptureHandler.OnMissedCreature -= DisplaySpawnButton;
        CreatureHandler.OnCreatureHatched -= DisplaySpawnButton;
        CaptureHandler.OnCreatureCaught -= DisplayCaught;
    }

    void Start()
    {
        spawnButton.gameObject.SetActive(false);
        reloadButton.SetActive(false);
    }


    public void SpawnBall()
    {
        // Get the center of the spawn button in screen coordinates.
        Vector2 screenPosition =
            RectTransformUtility.WorldToScreenPoint(
                null,
                spawnButton.position
            );


        // Convert the screen position into a point
        // in front of the AR camera.
        Vector3 worldPosition =
            arCamera.ScreenToWorldPoint(
                new Vector3(
                    screenPosition.x,
                    screenPosition.y,
                    distanceFromCamera
                )
            );


        // Spawn the ball as a child of the camera.
        _currentBall =
            Instantiate(
                ballPrefab,
                worldPosition,
                Quaternion.identity,
                arCamera.transform
            );


        // Keep the ball stationary until
        // the player begins dragging it.
        Rigidbody rb = _currentBall.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = true;
        }


        // Hide the spawn button.
        spawnButton.gameObject.SetActive(false);
    }


    public void ReloadGame()
    {
        Destroy(_currentBall);

        reloadButton.SetActive(false);
        spawnHandler.ResetEncounter();

        // SceneManager.LoadScene("MainMenu");
    }


    private void DisplaySpawnButton()
    {
        spawnButton.gameObject.SetActive(true);
    }


    private void DisplayGameReloadButton()
    {
        reloadButton.gameObject.SetActive(true);
    }


    private void DisplayCaught(CreatureDataSO data)
    {
        DisplayGameReloadButton();

        Debug.Log($"Captured {data.CreatureID}");
    }
}
