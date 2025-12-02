using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class FishData
{
    public string fishName;
    public Sprite fishSprite;
    [Range(0f, 100f)]
    public float spawnChance;
}

public class FishingController : MonoBehaviour
{
    public GameObject hook;
    public GameObject fish;
    public Transform catchPoint;
    public GameObject fishingLinePrefab;
    public float fishingLineSpeed = 5f;
    public AudioSource musicrod;
    public AudioSource musichook;
    public ParticleSystem effecthook;
    public Text percentageText;
    public Text timerText;
    public Text resultText;
    public FishData[] fishDataArray;
    public float percentageDecreaseRate = 0.1f;
    public Text biteWaitText;
    public Animator animator;
    private bool fishingStarted = false;


    // ЗАМЕНА: вместо FirstPersonController используем ваши скрипты
    public MouseLook mouseLook;
    public PlayerMovement playerMovement;

    private bool isFishing = false;
    private GameObject fishingLineInstance;
    private LineRenderer lineRenderer;
    private float percentage;
    private float remainingTime;
    private bool isHoldingInRange;
    private float holdTime;

    public GameObject fishMenu;
    public Text fishNameText;
    public Image fishImage;

    private void Start()
    {
        effecthook.Pause();
        ResetGame();
        HideUI();
        fishMenu.SetActive(false);
        biteWaitText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isFishing)
        {
            StartCoroutine(FishCoroutine());
        }

        if (isFishing && fishingStarted)
        {
            UpdateFishingLine();
            UpdatePercentageAndTime();
        }

        if (Input.GetKey(KeyCode.F))
        {
            effecthook.Play();
        }

        if (Input.GetKeyDown(KeyCode.R) && isFishing)
        {
            AddPercentage(3f);
        }
    }

    private IEnumerator FishCoroutine()
    {
        musichook.Play();
        musicrod.Play();

        animator.SetBool("Zabros", true);

        // ЗАМЕНА: блокируем управление через ваши скрипты
        if (mouseLook != null)
            mouseLook.enabled = false;
        if (playerMovement != null)
            playerMovement.enabled = false;

        isFishing = true;
        ResetGameUI();

        biteWaitText.gameObject.SetActive(true);
        biteWaitText.text = "Ожидайте поклевки";

        float waitTime = Random.Range(4f, 8f);
        yield return new WaitForSeconds(waitTime);

        biteWaitText.text = "Рыба клюнула!";
        yield return new WaitForSeconds(2f);
        biteWaitText.gameObject.SetActive(false);

        ShowUI();
        ResetGameUI();

        hook.SetActive(false);
        fish.SetActive(true);

        fishingLineInstance = Instantiate(fishingLinePrefab, hook.transform.position, Quaternion.identity);
        lineRenderer = fishingLineInstance.GetComponent<LineRenderer>();

        lineRenderer.SetPosition(0, hook.transform.position);
        lineRenderer.SetPosition(1, catchPoint.position);

        fishingStarted = true; // ← МИНИ-ИГРА МОЖЕТ НАЧАТЬСЯ

        remainingTime = 15f;
        percentage = 50f;

        while (isFishing)
        {
            yield return null;
            percentage -= percentageDecreaseRate * Time.deltaTime;

            if (percentage <= 0)
            {
                EndFishing(false);
            }
        }

        fish.SetActive(false);
        hook.SetActive(true);
        Destroy(fishingLineInstance);
    }

    private void UpdateFishingLine()
    {
        if (lineRenderer == null)
            return;   // ← предотвращает ошибку


        if (Vector3.Distance(lineRenderer.GetPosition(1), catchPoint.position) > 0.1f)
        {
            Vector3 direction = (catchPoint.position - lineRenderer.GetPosition(1)).normalized;
            lineRenderer.SetPosition(1, lineRenderer.GetPosition(1) + direction * fishingLineSpeed * Time.deltaTime);
        }
    }

    private void UpdatePercentageAndTime()
    {
        remainingTime -= Time.deltaTime;
        percentageText.text = Mathf.Clamp(percentage, 0, 100).ToString("F1") + "%";
        timerText.text = Mathf.Clamp(remainingTime, 0, 100).ToString("F1") + "s";

        if (percentage >= 50 && percentage <= 80)
        {
            isHoldingInRange = true;
            holdTime += Time.deltaTime;
        }
        else
        {
            isHoldingInRange = false;
            holdTime = 0;
        }

        if (isHoldingInRange && holdTime >= 6f)
        {
            CatchFish();
        }

        if (remainingTime <= 0)
        {
            if (percentage >= 50 && percentage <= 80)
            {
                CatchFish();
            }
            else
            {
                EndFishing(false);
            }
        }
    }

    private void CatchFish()
    {
        FishData caughtFish = GetRandomFish();
        fishNameText.text = caughtFish.fishName;
        fishImage.sprite = caughtFish.fishSprite;
        fishMenu.SetActive(true);
        EndFishing(true);
    }

    private void EndFishing(bool success)
    {
        isFishing = false;
        animator.SetBool("Zabros", false);

        // ЗАМЕНА: разблокируем управление
        if (mouseLook != null)
            mouseLook.enabled = true;
        if (playerMovement != null)
            playerMovement.enabled = true;

        if (success)
        {
            StartCoroutine(DisplayResult("Рыба поймана!"));
        }
        else
        {
            StartCoroutine(DisplayResult("Рыба отцепилась!"));
        }
    }

    private IEnumerator DisplayResult(string message)
    {
        resultText.text = message;
        yield return new WaitForSeconds(2f);
        ResetGame();
        HideUI();
    }

    private void ResetGame()
    {
        fishingStarted = false;
        percentage = 50f;
        remainingTime = 15f;
        percentageText.text = "0%";
        timerText.text = "0s";
        resultText.text = "";
        fishMenu.SetActive(false);
        isHoldingInRange = false;
        holdTime = 0;
    }

    private void ResetGameUI()
    {
        percentageText.text = "0%";
        timerText.text = "0s";
        resultText.text = "";
    }

    private void ShowUI()
    {
        percentageText.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);
        resultText.gameObject.SetActive(true);
    }

    private void HideUI()
    {
        percentageText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        resultText.gameObject.SetActive(false);
    }

    private void AddPercentage(float amount)
    {
        percentage = Mathf.Clamp(percentage + amount, 0f, 100f);
    }

    private FishData GetRandomFish()
    {
        float totalChance = 0f;
        foreach (var fishData in fishDataArray)
        {
            totalChance += fishData.spawnChance;
        }

        float randomValue = UnityEngine.Random.Range(0f, totalChance);

        foreach (var fishData in fishDataArray)
        {
            if (randomValue < fishData.spawnChance)
            {
                return fishData;
            }
            randomValue -= fishData.spawnChance;
        }

        return fishDataArray[0];
    }
}