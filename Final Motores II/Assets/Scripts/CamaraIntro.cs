using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraIntro : MonoBehaviour
{
    public CinemachineVirtualCamera VirtualCamera;
    public Transform startPosition;
    public Transform PlayerPosition;
    public float zoomOutSize = 15f;
    public float zoomInSize = 5f;
    public float transitionSpeed = 2f;
    public float waitTime = 1.5f;

    private void Start()
    {
        Time.timeScale = 0f;

        VirtualCamera.Follow = null;
        VirtualCamera.transform.position = startPosition.position;
        VirtualCamera.m_Lens.OrthographicSize = zoomInSize;

        StartCoroutine(CameraIntroSequence());
    }

    private IEnumerator CameraIntroSequence()
    {
        yield return new WaitForSecondsRealtime(waitTime);

        yield return StartCoroutine(MoveAndZoom(PlayerPosition.position, zoomOutSize));

        yield return new WaitForSecondsRealtime(waitTime);

        yield return StartCoroutine(ZoomIntoPlayer());

        VirtualCamera.Follow = PlayerPosition;

        yield return new WaitForSecondsRealtime(waitTime);

        Time.timeScale = 1f;
    }

    private IEnumerator MoveAndZoom(Vector3 targetPosition, float targetSize)
    {
        Vector3 PositionOne = VirtualCamera.transform.position;
        float startSize = VirtualCamera.m_Lens.OrthographicSize;
        float elapsedTime = 0;

        while (elapsedTime < transitionSpeed)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = elapsedTime / transitionSpeed;

            VirtualCamera.transform.position = Vector3.Lerp(PositionOne, targetPosition,t);
            VirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(startSize, targetSize,t);
            yield return null;
        }

        VirtualCamera.transform.position = targetPosition;
        VirtualCamera.m_Lens.OrthographicSize = targetSize;
    }

    private IEnumerator ZoomIntoPlayer()
    {
        float startSize = VirtualCamera.m_Lens.OrthographicSize;
        float elapsedTime = 0;

        while (elapsedTime < transitionSpeed)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = elapsedTime / transitionSpeed;

            VirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(startSize, zoomInSize, t);
            yield return null;
        }

        VirtualCamera.m_Lens.OrthographicSize = zoomInSize;
    }
}
