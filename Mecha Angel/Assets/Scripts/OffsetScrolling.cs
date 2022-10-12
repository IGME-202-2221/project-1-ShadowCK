using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// References
/// https://gist.github.com/satanas/5766d9d9d34f94be25cb0f85ffc50ad1
/// https://unity3d.com/learn/tutorials/topics/2d-game-creation/2d-scrolling-backgrounds
/// https://stackoverflow.com/questions/36947732/scroll-2d-3d-background-via-texture-offset
/// </summary>
public class OffsetScrolling : MonoBehaviour
{
    public float scrollSpeed = 1f;
    private float previousScrollSpeed = 1f;
    private float timer = 0f;

    private Renderer renderer;
    private Vector2 savedOffset;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        previousScrollSpeed = scrollSpeed;
        // Updates scrollSpeed based on player's health (the lower the higher)
        Player player = Game.Instance.Player;
        // Maps HP/maxHP (0~1) to 2~0.1
        scrollSpeed = (float)math.lerp(2, 0.1, Mathf.InverseLerp(0, player.MaxHealth, player.Health));
        // Keeps scrolling progress by changing timer's value; otherwise there will be an offeset every time the speed changes
        if (scrollSpeed != previousScrollSpeed)
        {
            timer = timer * previousScrollSpeed / scrollSpeed;
        }
        float x = Mathf.Repeat(timer * scrollSpeed, 1);
        // Changes texture offset
        Vector2 offset = new Vector2(x, 0);
        renderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
        timer += Time.deltaTime;
    }
}