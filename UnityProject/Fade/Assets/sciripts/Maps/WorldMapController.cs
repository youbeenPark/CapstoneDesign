using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField] private float mapLerpSpeed = 5f;
    [SerializeField] private float daniLerpSpeed = 8f;
    [SerializeField] private float cameraLerpSpeed = 6f;
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 0, -10);

    // 내부 계산용
    private Vector3[] pagePositions;
    private int currentPage = 0;
    private int currentIslandIndex = 0;
    private Vector3 targetWorldPos;
    private Vector3 targetDaniPos;
    private Vector3 targetCameraPos;

    private void Start()
    {
        // ✅ pagePositions 자동 계산
        CalculatePagePositions();

        // ✅ 초기화
        currentPage = 0;
        currentIslandIndex = 0;
        targetWorldPos = pagePositions[currentPage];
        worldGroup.position = targetWorldPos;

        Vector3[] islands = GetCurrentIslandArray();
        targetDaniPos = worldGroup.position + islands[currentIslandIndex];
        dani.position = targetDaniPos;

        targetCameraPos = pagePositions[currentPage] + cameraOffset;
        mainCamera.transform.position = targetCameraPos;

        UpdateBackground();
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
    }

    // ✅ 배경 폭 기반 자동 배치 계산
    private void CalculatePagePositions()
    {
        pagePositions = new Vector3[backgrounds.Length];
        float currentX = 0f;

        for (int i = 0; i < backgrounds.Length; i++)
        {
            SpriteRenderer sr = backgrounds[i].GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                float width = sr.bounds.size.x;

                // ✅ Pivot이 Bottom Left 기준이므로 중앙 보정 필요 없음
                pagePositions[i] = new Vector3(currentX, 0, 0);

                // 다음 배경 시작점을 현재 배경의 오른쪽 끝으로 이동
                currentX += width;
            }
            else
            {
                // SpriteRenderer 없을 때 대비
                pagePositions[i] = new Vector3(currentX, 0, 0);
                currentX += 20f;
            }
        }

        // ✅ 마지막 페이지 오른쪽 끝 부분이 잘리지 않도록 여유 추가
        currentX += backgrounds[^1].GetComponent<SpriteRenderer>().bounds.size.x * 0.5f;
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
            targetWorldPos = -pagePositions[currentPage]; // ✅ worldGroup 이동
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
            targetWorldPos = -pagePositions[currentPage]; // ✅ worldGroup 이동
            UpdateBackground();
        }

        targetDaniPos = worldGroup.position + GetCurrentIslandArray()[currentIslandIndex];
    }

    private void SmoothMove()
    {
        // 🌎 WorldGroup 이동
        worldGroup.position = Vector3.Lerp(
            worldGroup.position,
            targetWorldPos,
            Time.deltaTime * mapLerpSpeed
        );

        // 🧍‍♀️ Dani 이동
        Vector3 daniTargetWorld = worldGroup.position + GetCurrentIslandArray()[currentIslandIndex];
        dani.position = Vector3.Lerp(
            dani.position,
            daniTargetWorld,
            Time.deltaTime * daniLerpSpeed
        );

        // 🎥 Camera 이동 — 반드시 페이지 중심 기준으로 이동
        targetCameraPos = new Vector3(
            pagePositions[currentPage].x,   // ✅ 현재 페이지 중심
            0,
            cameraOffset.z
        );

        mainCamera.transform.position = Vector3.Lerp(
            mainCamera.transform.position,
            targetCameraPos,
            Time.deltaTime * cameraLerpSpeed
        );
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

    // ✅ 모든 배경 항상 켜두기 (이어지는 방식일 때)
    private void UpdateBackground()
    {
        for (int i = 0; i < backgrounds.Length; i++)
            backgrounds[i].SetActive(true);
    }

}
