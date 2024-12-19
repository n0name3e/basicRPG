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
        Player = FindObjectOfType<Player>();
        yield return null;
        Inventory.Instance.AddItem(ItemManager.Instance.FindItem("stone"));
        Inventory.Instance.AddItem(ItemManager.Instance.FindItem("healing potion"));
        Inventory.Instance.AddItem(ItemManager.Instance.FindItem("healing potion"));
        Inventory.Instance.AddItem(ItemManager.Instance.FindItem("helm"));
        SpawnEnemy();
    }
    public void SpawnEnemy()
    {
        GameObject loadedEnemy = Resources.Load<GameObject>("Enemy");
        Enemy enemy = Instantiate(loadedEnemy, new Vector2(3, 2), Quaternion.identity).GetComponent<Enemy>();
        enemy.dropTable = loadedEnemy.GetComponent<Enemy>().dropTable;
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
