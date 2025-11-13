using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class WorldMapController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform worldGroup;
    [SerializeField] private Transform dani;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject[] backgrounds;

    [Header("Island Spots Per Page")]
    [SerializeField] private Vector3[] page0IslandPositions;
    [SerializeField] private Vector3[] page1IslandPositions;
    [SerializeField] private Vector3[] page2IslandPositions;

    [Header("Movement Settings")]
    [SerializeField] private float mapLerpSpeed = 1f;
    [SerializeField] private float daniLerpSpeed = 0.8f;
    [SerializeField] private float cameraLerpSpeed = 1.5f;
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 0, -10);

    [Header("Dani Hover Motion")]
    [SerializeField] private float hoverAmplitude = 0.1f;
    [SerializeField] private float hoverFrequency = 2f;

    private Vector3[] pagePositions;
    private int currentPage = 0;
    private int currentIslandIndex = 0;
    private Vector3 targetWorldPos;
    private Vector3 targetDaniPos;
    private Vector3 targetCameraPos;

    private SpriteRenderer daniRenderer;
    private float hoverTimer;

    private void Start()
    {
        CalculatePagePositions();

        currentPage = 0;
        currentIslandIndex = 0;
        targetWorldPos = pagePositions[currentPage];
        worldGroup.position = targetWorldPos;

        Vector3[] islands = GetCurrentIslandArray();
        targetDaniPos = worldGroup.position + islands[currentIslandIndex];
        dani.position = targetDaniPos;

        daniRenderer = dani.GetComponent<SpriteRenderer>();
        hoverTimer = Random.Range(0f, Mathf.PI * 2f);

        Vector3 startCenter = GetPageCenter(currentPage);
        mainCamera.transform.position = startCenter + cameraOffset;

        UpdateBackground();

        // ✅ 월드맵 진입 시 각 에피소드 섬의 색상 자동 갱신
        foreach (var island in FindObjectsOfType<EpisodeIsland>())
            island.UpdateIslandSprite();

    }

    private void Update()
    {
        HandleInput();
        SmoothMove();
    }

    private void HandleInput()
    {
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            MoveRight();

        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            MoveLeft();

        // ✅ 스페이스바로 씬 이동
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            LoadCurrentIslandScene();
    }

    // ✅ 현재 섬의 이름과 페이지에 맞춰 씬 로드
    private void LoadCurrentIslandScene()
    {
        // 씬 이름 규칙: 폴더 이름과 동일하게 (예: "BL", "GR", "RD" 등)
        string[] sceneNamesByPage0 = { "TUTO", "GR", "YL", "BL" };
        string[] sceneNamesByPage1 = { "OR", "RD", "PR", "SK" };
        string[] sceneNamesByPage2 = { "BOSE", "RAINBOW" };

        string sceneName = null;

        switch (currentPage)
        {
            case 0:
                if (currentIslandIndex < sceneNamesByPage0.Length)
                    sceneName = sceneNamesByPage0[currentIslandIndex];
                break;
            case 1:
                if (currentIslandIndex < sceneNamesByPage1.Length)
                    sceneName = sceneNamesByPage1[currentIslandIndex];
                break;
            case 2:
                if (currentIslandIndex < sceneNamesByPage2.Length)
                    sceneName = sceneNamesByPage2[currentIslandIndex];
                break;
        }

        if (!string.IsNullOrEmpty(sceneName))
        {
            Debug.Log($"🌍 Loading Scene: {sceneName}");
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("⚠️ 해당 위치에 연결된 씬이 없습니다!");
        }
    }

    // ✅ 배경폭 기반 페이지 좌표 계산 (Pivot: Bottom Left)
    private void CalculatePagePositions()
    {
        pagePositions = new Vector3[backgrounds.Length];
        float currentX = 0f;

        for (int i = 0; i < backgrounds.Length; i++)
        {
            SpriteRenderer sr = backgrounds[i].GetComponent<SpriteRenderer>();
            float width = sr != null ? sr.bounds.size.x : 20f;
            pagePositions[i] = new Vector3(currentX, 0, 0);
            currentX += width;
        }
    }

    private void MoveRight()
    {
        Vector3[] islands = GetCurrentIslandArray();
        if (islands == null || islands.Length == 0) return;

        if (currentIslandIndex < islands.Length - 1)
        {
            currentIslandIndex++;
        }
        else if (currentPage < backgrounds.Length - 1)
        {
            currentPage++;
            currentIslandIndex = 0;
            targetWorldPos = -pagePositions[currentPage];
            UpdateBackground();
        }

        targetDaniPos = worldGroup.position + GetCurrentIslandArray()[currentIslandIndex];
    }

    private void MoveLeft()
    {
        Vector3[] islands = GetCurrentIslandArray();
        if (islands == null || islands.Length == 0) return;

        if (currentIslandIndex > 0)
        {
            currentIslandIndex--;
        }
        else if (currentPage > 0)
        {
            currentPage--;
            Vector3[] prevIslands = GetCurrentIslandArray();
            currentIslandIndex = prevIslands.Length - 1;
            targetWorldPos = -pagePositions[currentPage];
            UpdateBackground();
        }

        targetDaniPos = worldGroup.position + GetCurrentIslandArray()[currentIslandIndex];
    }

    private void SmoothMove()
    {
        worldGroup.position = Vector3.Lerp(worldGroup.position, targetWorldPos, Time.deltaTime * mapLerpSpeed);

        Vector3 daniTargetWorld = worldGroup.position + GetCurrentIslandArray()[currentIslandIndex];
        hoverTimer += Time.deltaTime * hoverFrequency;

        float hoverOffset = Mathf.Sin(hoverTimer) * hoverAmplitude;
        Vector3 hoverTarget = new Vector3(daniTargetWorld.x, daniTargetWorld.y + hoverOffset, daniTargetWorld.z);

        dani.position = Vector3.Lerp(dani.position, hoverTarget, Time.deltaTime * daniLerpSpeed);

        if (daniRenderer != null)
        {
            if (daniTargetWorld.x > dani.position.x + 0.05f) daniRenderer.flipX = false;
            else if (daniTargetWorld.x < dani.position.x - 0.05f) daniRenderer.flipX = true;
        }

        Vector3 pageCenter = GetPageCenter(currentPage);
        Vector3 desiredCameraPos = new Vector3(pageCenter.x, mainCamera.transform.position.y, cameraOffset.z);

        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, desiredCameraPos, Time.deltaTime * cameraLerpSpeed);
    }

    private Vector3 GetPageCenter(int pageIndex)
    {
        Vector3[] islands = null;
        switch (pageIndex)
        {
            case 0: islands = page0IslandPositions; break;
            case 1: islands = page1IslandPositions; break;
            case 2: islands = page2IslandPositions; break;
        }

        if (islands == null || islands.Length == 0)
            return worldGroup.position;

        Vector3 sum = Vector3.zero;
        foreach (var pos in islands)
            sum += pos;

        return targetWorldPos + (sum / islands.Length);
    }

    private Vector3[] GetCurrentIslandArray()
    {
        switch (currentPage)
        {
            case 0: return page0IslandPositions;
            case 1: return page1IslandPositions;
            case 2: return page2IslandPositions;
            default: return null;
        }
    }

    private void UpdateBackground()
    {
        for (int i = 0; i < backgrounds.Length; i++)
            backgrounds[i].SetActive(true);
    }
}

//using UnityEngine;
//using UnityEngine.InputSystem;
//using UnityEngine.SceneManagement;

//public class WorldMapController : MonoBehaviour
//{
//    [Header("References")]
//    [SerializeField] private Transform worldGroup;
//    [SerializeField] private Transform dani;
//    [SerializeField] private Camera mainCamera;
//    [SerializeField] private GameObject[] backgrounds;

//    [Header("Island Spots Per Page")]
//    [SerializeField] private Vector3[] page0IslandPositions;
//    [SerializeField] private Vector3[] page1IslandPositions;
//    [SerializeField] private Vector3[] page2IslandPositions;

//    [Header("Movement Settings")]
//    [SerializeField] private float mapLerpSpeed = 1f;
//    [SerializeField] private float daniLerpSpeed = 0.8f;
//    [SerializeField] private float cameraLerpSpeed = 1.5f;
//    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 0, -10);

//    [Header("Dani Hover Motion")]
//    [SerializeField] private float hoverAmplitude = 0.1f;
//    [SerializeField] private float hoverFrequency = 2f;

//    private Vector3[] pagePositions;
//    private int currentPage = 0;
//    private int currentIslandIndex = 0;
//    private Vector3 targetWorldPos;
//    private Vector3 targetDaniPos;

//    private SpriteRenderer daniRenderer;
//    private float hoverTimer;

//    private void Start()
//    {
//        CalculatePagePositions();

//        currentPage = 0;
//        currentIslandIndex = 0;
//        targetWorldPos = pagePositions[currentPage];
//        worldGroup.position = targetWorldPos;

//        Vector3[] islands = GetCurrentIslandArray();
//        targetDaniPos = worldGroup.position + islands[currentIslandIndex];
//        dani.position = targetDaniPos;

//        daniRenderer = dani.GetComponent<SpriteRenderer>();
//        hoverTimer = Random.Range(0f, Mathf.PI * 2f);

//        Vector3 startCenter = GetPageCenter(currentPage);
//        mainCamera.transform.position = startCenter + cameraOffset;

//        UpdateBackground();

//        // ✅ 해금 상태에 맞게 각 섬 스프라이트 갱신
//        foreach (var island in FindObjectsOfType<EpisodeIsland>())
//            island.RefreshIslandVisual();
//    }

//    private void Update()
//    {
//        HandleInput();
//        SmoothMove();
//    }

//    private void HandleInput()
//    {
//        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
//            MoveRight();

//        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
//            MoveLeft();

//        if (Keyboard.current.spaceKey.wasPressedThisFrame)
//            LoadCurrentIslandScene();
//    }

//    // ✅ 섬 클릭 → 연결된 씬 로드 (해금된 섬만)
//    private void LoadCurrentIslandScene()
//    {
//        string[] sceneNamesByPage0 = { "TUTO", "GR", "YL", };
//        string[] sceneNamesByPage1 = { "BL", "OR", "RD", "SK" };
//        string[] sceneNamesByPage2 = { "PR", "BOSE", "RAINBOW" };

//        string sceneName = null;

//        switch (currentPage)
//        {
//            case 0:
//                if (currentIslandIndex < sceneNamesByPage0.Length)
//                    sceneName = sceneNamesByPage0[currentIslandIndex];
//                break;
//            case 1:
//                if (currentIslandIndex < sceneNamesByPage1.Length)
//                    sceneName = sceneNamesByPage1[currentIslandIndex];
//                break;
//            case 2:
//                if (currentIslandIndex < sceneNamesByPage2.Length)
//                    sceneName = sceneNamesByPage2[currentIslandIndex];
//                break;
//        }

//        if (!string.IsNullOrEmpty(sceneName))
//        {
//            // 🚫 잠긴 섬은 이동 불가
//            if (PlayerPrefs.GetInt("Unlocked_" + sceneName, sceneName == "TUTO" ? 1 : 0) == 0)
//            {
//                Debug.Log($"🔒 {sceneName}은 아직 잠겨 있습니다.");
//                return;
//            }

//            Debug.Log($"🌍 Loading Scene: {sceneName}");
//            SceneManager.LoadScene(sceneName);
//        }
//        else
//        {
//            Debug.LogWarning("⚠️ 해당 위치에 연결된 씬이 없습니다!");
//        }
//    }

//    private void CalculatePagePositions()
//    {
//        pagePositions = new Vector3[backgrounds.Length];
//        float currentX = 0f;

//        for (int i = 0; i < backgrounds.Length; i++)
//        {
//            SpriteRenderer sr = backgrounds[i].GetComponent<SpriteRenderer>();
//            float width = sr != null ? sr.bounds.size.x : 20f;
//            pagePositions[i] = new Vector3(currentX, 0, 0);
//            currentX += width;
//        }
//    }

//    private void MoveRight()
//    {
//        Vector3[] islands = GetCurrentIslandArray();
//        if (islands == null || islands.Length == 0) return;

//        if (currentIslandIndex < islands.Length - 1)
//        {
//            currentIslandIndex++;
//        }
//        else if (currentPage < backgrounds.Length - 1)
//        {
//            currentPage++;
//            currentIslandIndex = 0;
//            targetWorldPos = -pagePositions[currentPage];
//            UpdateBackground();
//        }

//        targetDaniPos = worldGroup.position + GetCurrentIslandArray()[currentIslandIndex];
//    }

//    private void MoveLeft()
//    {
//        Vector3[] islands = GetCurrentIslandArray();
//        if (islands == null || islands.Length == 0) return;

//        if (currentIslandIndex > 0)
//        {
//            currentIslandIndex--;
//        }
//        else if (currentPage > 0)
//        {
//            currentPage--;
//            Vector3[] prevIslands = GetCurrentIslandArray();
//            currentIslandIndex = prevIslands.Length - 1;
//            targetWorldPos = -pagePositions[currentPage];
//            UpdateBackground();
//        }

//        targetDaniPos = worldGroup.position + GetCurrentIslandArray()[currentIslandIndex];
//    }

//    private void SmoothMove()
//    {
//        worldGroup.position = Vector3.Lerp(worldGroup.position, targetWorldPos, Time.deltaTime * mapLerpSpeed);

//        Vector3 daniTargetWorld = worldGroup.position + GetCurrentIslandArray()[currentIslandIndex];
//        hoverTimer += Time.deltaTime * hoverFrequency;

//        float hoverOffset = Mathf.Sin(hoverTimer) * hoverAmplitude;
//        Vector3 hoverTarget = new Vector3(daniTargetWorld.x, daniTargetWorld.y + hoverOffset, daniTargetWorld.z);

//        dani.position = Vector3.Lerp(dani.position, hoverTarget, Time.deltaTime * daniLerpSpeed);

//        if (daniRenderer != null)
//        {
//            if (daniTargetWorld.x > dani.position.x + 0.05f) daniRenderer.flipX = false;
//            else if (daniTargetWorld.x < dani.position.x - 0.05f) daniRenderer.flipX = true;
//        }

//        Vector3 pageCenter = GetPageCenter(currentPage);
//        Vector3 desiredCameraPos = new Vector3(pageCenter.x, mainCamera.transform.position.y, cameraOffset.z);

//        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, desiredCameraPos, Time.deltaTime * cameraLerpSpeed);
//    }

//    private Vector3 GetPageCenter(int pageIndex)
//    {
//        Vector3[] islands = null;
//        switch (pageIndex)
//        {
//            case 0: islands = page0IslandPositions; break;
//            case 1: islands = page1IslandPositions; break;
//            case 2: islands = page2IslandPositions; break;
//        }

//        if (islands == null || islands.Length == 0)
//            return worldGroup.position;

//        Vector3 sum = Vector3.zero;
//        foreach (var pos in islands)
//            sum += pos;

//        return targetWorldPos + (sum / islands.Length);
//    }

//    private Vector3[] GetCurrentIslandArray()
//    {
//        switch (currentPage)
//        {
//            case 0: return page0IslandPositions;
//            case 1: return page1IslandPositions;
//            case 2: return page2IslandPositions;
//            default: return null;
//        }
//    }

//    private void UpdateBackground()
//    {
//        for (int i = 0; i < backgrounds.Length; i++)
//            backgrounds[i].SetActive(true);
//    }
//}
