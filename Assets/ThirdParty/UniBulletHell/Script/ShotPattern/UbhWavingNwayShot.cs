﻿using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Ubh waving nway shot.
/// </summary>
[AddComponentMenu("UniBulletHell/Shot Pattern/Waving nWay Shot")]
public class UbhWavingNwayShot : UbhBaseShot
{
    [Header("===== WavingNwayShot Settings =====")]
    // "Set a number of shot way."
    [FormerlySerializedAs("_WayNum")]
    public int m_wayNum = 5;
    // "Set a center angle of wave range. (0 to 360)"
    [Range(0f, 360f), FormerlySerializedAs("_WaveCenterAngle")]
    public float m_waveCenterAngle = 180f;
    // "Set a size of wave range. (0 to 360)"
    [Range(0f, 360f), FormerlySerializedAs("_WaveRangeSize")]
    public float m_waveRangeSize = 40f;
    // "Set a speed of wave. (0 to 10)"
    [Range(0f, 10f), FormerlySerializedAs("_WaveSpeed")]
    public float m_waveSpeed = 5f;
    // "Set a angle between bullet and next bullet. (0 to 360)"
    [Range(0f, 360f), FormerlySerializedAs("_BetweenAngle")]
    public float m_betweenAngle = 5f;
    // "Set a delay time between shot and next line shot. (sec)"
    [FormerlySerializedAs("_NextLineDelay")]
    public float m_nextLineDelay = 0.1f;

    public override void Shot()
    {
        StartCoroutine(ShotCoroutine());
    }

    private IEnumerator ShotCoroutine()
    {
        if (m_bulletNum <= 0 || m_bulletSpeed <= 0f || m_wayNum <= 0)
        {
            Debug.LogWarning("Cannot shot because BulletNum or BulletSpeed or WayNum is not set.");
            yield break;
        }
        if (m_shooting)
        {
            yield break;
        }
        m_shooting = true;

        int wayIndex = 0;

        for (int i = 0; i < m_bulletNum; i++)
        {
            if (m_wayNum <= wayIndex)
            {
                wayIndex = 0;
                if (0f < m_nextLineDelay)
                {
                    FiredShot();
                    yield return UbhUtil.WaitForSeconds(m_nextLineDelay);
                }
            }
            var bullet = GetBullet(transform.position, transform.rotation);
            if (bullet == null)
            {
                break;
            }

            float centerAngle = m_waveCenterAngle + (m_waveRangeSize / 2f * Mathf.Sin(UbhTimer.instance.frameCount * m_waveSpeed / 100f));

            float baseAngle = m_wayNum % 2 == 0 ? centerAngle - (m_betweenAngle / 2f) : centerAngle;

            float angle = UbhUtil.GetShiftedAngle(wayIndex, baseAngle, m_betweenAngle);

            ShotBullet(bullet, m_bulletSpeed, angle);

            AutoReleaseBullet(bullet);

            wayIndex++;
        }

        FiredShot();

        FinishedShot();

        yield break;
    }
}