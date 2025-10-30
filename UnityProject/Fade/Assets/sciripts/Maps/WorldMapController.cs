﻿using UnityEngine;
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
    [SerializeField] private float mapLerpSpeed = 1f;
    [SerializeField] private float daniLerpSpeed = 0.8f;
    [SerializeField] private float cameraLerpSpeed = 1.5f;
    [SerializeField] private Vector3 cameraOffset = new Vector3(0, 0, -10);

    [Header("Dani Hover Motion")]
    [SerializeField] private float hoverAmplitude = 0.1f;   // 위아래 흔들림 높이
    [SerializeField] private float hoverFrequency = 2f;     // 위아래 속도

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
        // 🌎 WorldGroup 이동
        worldGroup.position = Vector3.Lerp(
            worldGroup.position,
            targetWorldPos,
            Time.deltaTime * mapLerpSpeed
        );

        // 🧍‍♀️ Dani 이동 (둥실둥실 모션)
        Vector3 daniTargetWorld = worldGroup.position + GetCurrentIslandArray()[currentIslandIndex];
        hoverTimer += Time.deltaTime * hoverFrequency;

        // 위아래로 흔들리는 위치
        float hoverOffset = Mathf.Sin(hoverTimer) * hoverAmplitude;
        Vector3 hoverTarget = new Vector3(
            daniTargetWorld.x,
            daniTargetWorld.y + hoverOffset,
            daniTargetWorld.z
        );

        dani.position = Vector3.Lerp(
            dani.position,
            hoverTarget,
            Time.deltaTime * daniLerpSpeed
        );

        // 방향 전환 시 스프라이트 반전
        if (daniRenderer != null)
        {
            if (daniTargetWorld.x > dani.position.x + 0.05f) daniRenderer.flipX = false;
            else if (daniTargetWorld.x < dani.position.x - 0.05f) daniRenderer.flipX = true;
        }

        // 🎥 카메라 이동 (X만 이동하고 Y는 고정)
        Vector3 pageCenter = GetPageCenter(currentPage);
        Vector3 desiredCameraPos = new Vector3(
            pageCenter.x,
            mainCamera.transform.position.y,
            cameraOffset.z
        );

        mainCamera.transform.position = Vector3.Lerp(
            mainCamera.transform.position,
            desiredCameraPos,
            Time.deltaTime * cameraLerpSpeed
        );
    }

    // ✅ 페이지별 섬 중심 계산 (목표 위치 기준)
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
