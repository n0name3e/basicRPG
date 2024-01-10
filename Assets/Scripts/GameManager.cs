using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Player Player { get; private set; }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private IEnumerator Start()
    {
        yield return null;
        Player = FindObjectOfType<Player>();
        Inventory.Instance.AddItem(Items.Instance.stone);
        Inventory.Instance.AddItem(Items.Instance.healingPotion);
        Inventory.Instance.AddItem(Items.Instance.healingPotion);
    }
    public int GetNextLevelExperience(int nextLevel)
    {
        return nextLevel * 100;
    }
    public void Pause()
    {
        Time.timeScale = 0;
    }
    public void UnPause()
    {
        Time.timeScale = 1;
    }
}
