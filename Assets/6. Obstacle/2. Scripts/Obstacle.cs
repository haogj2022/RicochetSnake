using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private int BounceCost = 1;
    [SerializeField] private bool IsFragile;
    [SerializeField] private List<GameObject> BrokenShards = new();
    [SerializeField] private bool IsBouncy;
    private int MaxFragileCount = 1;
    private int CurrentFragileCount;
    private BoxCollider2D SelfCollider;
    private SpriteRenderer SelfRenderer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (IsFragile)
            {
                if (CurrentFragileCount < MaxFragileCount)
                {
                    CurrentFragileCount++;
                    SelfCollider = GetComponent<BoxCollider2D>();
                    SelfRenderer = GetComponent<SpriteRenderer>();
                    gameObject.tag = "Untagged";
                    gameObject.layer = 2;
                    BounceCost = 0;
                }
                else
                {
                    for (int i = 0; i < BrokenShards.Count; i++)
                    {
                        BrokenShards[i].SetActive(true);
                    }
                    SelfRenderer.enabled = false;
                    SelfCollider.enabled = false;
                    Invoke(nameof(DespawnBrokenShards), 1);
                    return;
                }
            }

            if (IsBouncy)
            {
                transform.DOScale(0.3f, 0.1f).SetLoops(2, LoopType.Yoyo);
                return;
            }

            collision.gameObject.GetComponent<Snake>().DecreaseBounceCount(BounceCost);
        }
    }

    private void DespawnBrokenShards()
    {
        for (int i = 0; i < BrokenShards.Count; i++)
        {
            BrokenShards[i].SetActive(false);
        }
    }
}
