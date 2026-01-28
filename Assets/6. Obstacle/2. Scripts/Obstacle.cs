using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private int BounceCost = 1;
    [SerializeField] private bool IsFragile;
    [SerializeField] private List<GameObject> BrokenShards = new();
    [SerializeField] private bool IsBouncy;
    private BoxCollider2D SelfCollider;
    private SpriteRenderer SelfRenderer;

    private void Start()
    {
        if (IsFragile)
        {
            SelfCollider = GetComponent<BoxCollider2D>();
            SelfRenderer = GetComponent<SpriteRenderer>();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (IsFragile)
            {
                BounceCost = 0;
                SelfRenderer.enabled = false;
                SelfCollider.enabled = false;

                for (int i = 0; i < BrokenShards.Count; i++)
                {
                    BrokenShards[i].SetActive(true);
                }

                Invoke(nameof(DespawnBrokenShards), 5);
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
