using UnityEngine;

public class SnakeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject PreviewSnake;
    [SerializeField] private Snake SnakeHeadPrefab;
    private bool CanSpawn = true;
    private Snake SnakeHead;

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            SpawnSnake();
        }
    }

    private void SpawnSnake()
    {
        if (CanSpawn)
        {
            SnakeHead = PoolingSystem.Spawn<Snake>(
                SnakeHeadPrefab.gameObject,
                transform,
                SnakeHeadPrefab.transform.localScale,
                Vector2.zero,
                PreviewSnake.transform.rotation);

            SnakeHead.MoveTowardsDirection(PreviewSnake.transform.right);
            PreviewSnake.SetActive(false);
            CanSpawn = false;
        }
    }
}
