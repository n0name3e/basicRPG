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
        /*Inventory.Instance.AddItem(ItemManager.Instance.FindItem("stone"));
        Inventory.Instance.AddItem(ItemManager.Instance.FindItem("healing potion"));
        Inventory.Instance.AddItem(ItemManager.Instance.FindItem("healing potion"));
        Inventory.Instance.AddItem(ItemManager.Instance.FindItem("helm"));
        Inventory.Instance.AddItem(ItemManager.Instance.FindItem("boots"));
        Inventory.Instance.AddItem(ItemManager.Instance.FindItem("basic staff"));
        Inventory.Instance.AddItem(ItemManager.Instance.FindItem("fire staff"));
        Inventory.Instance.AddItem(ItemManager.Instance.FindItem("vampiric staff"));
        Inventory.Instance.AddItem(ItemManager.Instance.FindItem("chainmail"));
        Inventory.Instance.AddItem(ItemManager.Instance.FindItem("sword"));
        Inventory.Instance.AddItem(ItemManager.Instance.FindItem("magic boots"));*/
        //Inventory.Instance.AddItem(ItemManager.Instance.FindItem("boots"));
        Player.ChangeMeleeWeapon(ItemManager.Instance.FindItem("sword") as Sword);
        Player.ChangeMagicWeapon(ItemManager.Instance.FindItem("fire staff") as Staff);
        //Player.ChangeMagicWeapon(ItemManager.Instance.FindItem("basic staff") as Staff);
        //UI.Instance.ChangeWeapon(Player.inventory.equippedStaff);
        UI.Instance.ChangeWeapon(Player.inventory.equippedSword);
        Player.AddAbility(AbilityManager.Instance.FindAbility("fireball"));
        Player.AddAbility(AbilityManager.Instance.FindAbility("heal"));
        SpawnEnemy(5, 5);
    }
    public void SpawnEnemy(float x, float y)
    {
        GameObject loadedEnemy = Resources.Load<GameObject>("Enemy");
        Enemy enemy = Instantiate(loadedEnemy, new Vector2(x, y), Quaternion.identity).GetComponent<Enemy>();
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
