using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private int GoldReward = 10;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.DecreaseFoodAmount();
            GameManager.Instance.OnFoodEaten();
            gameObject.SetActive(false);
        }
    }

    public int GetGoldReward()
    {
        return GoldReward;
    }
}
