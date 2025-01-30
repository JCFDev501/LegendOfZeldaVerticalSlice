using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BSlot { None, Boomarang, Bomb, Bow, Potion}
public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject m_mainMenu;
    [SerializeField] private PlayerController m_playerController;
    [SerializeField] private CameraController m_cameraController;
    [SerializeField] private Health m_healthComp;
    [SerializeField] private EnemyManager m_enemyManager;
    [SerializeField] private GameObject m_overworld;
    [SerializeField] private GameObject m_overworldMiniMap;
    [SerializeField] private GameObject m_useBButton;
    [SerializeField] private GameObject m_boomarang;
    [SerializeField] private GameObject m_bomb;
    [SerializeField] private GameObject m_bowAndArrow;
    [SerializeField] private GameObject m_potion;
    [SerializeField] private GameObject m_selector;
    [SerializeField] private GameObject m_mapItem;
    [SerializeField] private GameObject m_triforce;
    [SerializeField] private GameObject m_playerMapIcon;
    [SerializeField] private GameObject m_playerMiniMapIcon;
    [SerializeField] private List<GameObject> m_dungeonTiles;
    [SerializeField] private List<GameObject> m_dungeonMiniMapTiles;
    [SerializeField] private GameObject m_rupieTens;
    [SerializeField] private GameObject m_rupieOnes;
    [SerializeField] private GameObject m_keysTens;
    [SerializeField] private GameObject m_keysOnes;
    [SerializeField] private GameObject m_bombsTens;
    [SerializeField] private GameObject m_bombsOnes;
    [SerializeField] private GameObject m_B;
    [SerializeField] private GameObject m_A;
    [SerializeField] private GameObject m_heartOne;
    [SerializeField] private GameObject m_heartTwo;
    [SerializeField] private GameObject m_heartThree;
    [SerializeField] private GameObject m_heartFour;

    [SerializeField] private Sprite[] m_numbers;
    [SerializeField] private Sprite[] m_hearts;
    [SerializeField] private Sprite m_boomarangSprite;
    [SerializeField] private Sprite m_bombSprite;
    [SerializeField] private Sprite m_bowAndArrowSprite;
    [SerializeField] private Sprite m_potionSprite;

    [SerializeField] private bool m_inventoryDown = false;
    [SerializeField] private bool m_inOverworld = true;
    [SerializeField] private float m_scrollSpeed = 1.0f;

    private BSlot m_bSlot = BSlot.None;

    private Vector2 m_inventoryDownPos = new Vector3(0, 0);
    private Vector2 m_inventoryUpPos = new Vector3(0, 535);

    private Vector3 m_overword1 = new Vector3(-0.28f, 7.74f, -10f);
    private Vector3 m_overword2 = new Vector3(10.47f, 7.74f, -10f);
    private Vector3 m_overword3 = new Vector3(-0.28f, 0.34f, -10f);
    private Vector3 m_overword4 = new Vector3(10.47f, 0.34f, -10f);

    private Vector3 m_dungeon1 = new Vector3(10.47f, 22.55f, -10f);
    private Vector3 m_dungeon2 = new Vector3(21.22f, 22.55f, -10f);
    private Vector3 m_dungeon3 = new Vector3(31.97f, 22.55f, -10f);
    private Vector3 m_dungeon4 = new Vector3(10.47f, 15.15f, -10f);
    private Vector3 m_dungeon5 = new Vector3(21.22f, 15.15f, -10f);

    private Vector3 m_miniMapLocation1 = new Vector3(109.5f, -243.65f);
    private Vector3 m_miniMapLocation2 = new Vector3(134.5f, -243.65f);
    private Vector3 m_miniMapLocation3 = new Vector3(158.5f, -243.65f);
    private Vector3 m_miniMapLocation4 = new Vector3(109.5f, -257.25f);
    private Vector3 m_miniMapLocation5 = new Vector3(134.5f, -257.25f);

    private Vector3 m_mapLocation1 = new Vector3(507.5f, 262f);
    private Vector3 m_mapLocation2 = new Vector3(535.5f, 262f);
    private Vector3 m_mapLocation3 = new Vector3(563.5f, 262f);
    private Vector3 m_mapLocation4 = new Vector3(507.5f, 234f);
    private Vector3 m_mapLocation5 = new Vector3(535.5f, 234f);

    private InventoryData m_inventoryData = new InventoryData();
    private bool m_once = true;

    public void SaveToJson()
    {
        string inventoryData = JsonUtility.ToJson(m_inventoryData);
        string filePath = Application.persistentDataPath + "/InventoryData.json";
        Debug.Log(filePath);
        System.IO.File.WriteAllText(filePath, inventoryData);
        Debug.Log("Saved");
    }

    public void LoadFromJson()
    {
        string filePath = Application.persistentDataPath + "/InventoryData.json";

        if (System.IO.File.Exists(filePath)) 
        {
            string inventoryData = System.IO.File.ReadAllText(filePath);
            m_inventoryData = JsonUtility.FromJson<InventoryData>(inventoryData);
            Debug.Log("Loaded");
        }
        else
            SaveToJson();
    }

    private void Start()
    {
        LoadFromJson();
        SetUINumber(m_inventoryData.m_rupies, m_rupieTens, m_rupieOnes);
        SetUINumber(m_inventoryData.m_keys, m_keysTens, m_keysOnes);
        SetUINumber(m_inventoryData.m_bombs, m_bombsTens, m_bombsOnes);
    }

    void Update()
    {
        if (m_inventoryDownPos == GetComponent<RectTransform>().anchoredPosition)
        {
            m_playerController.SetControllable(false);
            m_enemyManager.SetPaused(true);
        }
        else 
        {
            m_enemyManager.SetPaused(false);
        }

        if ( Input.GetKeyDown(KeyCode.Escape) && !m_inventoryDown && !m_mainMenu.activeSelf)
        {
            m_inventoryDown = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && m_inventoryDown && !m_mainMenu.activeSelf)
        {
            m_inventoryDown = false;
            m_playerController.SetControllable(true);
            m_enemyManager.SetPaused(false);
        }

        if (m_inventoryDownPos != GetComponent<RectTransform>().anchoredPosition  && m_inventoryDown)
        {
            GetComponent<RectTransform>().anchoredPosition = Vector2.MoveTowards(GetComponent<RectTransform>().anchoredPosition, m_inventoryDownPos, m_scrollSpeed * Time.deltaTime);
        }
        else if (m_inventoryUpPos != GetComponent<RectTransform>().anchoredPosition && !m_inventoryDown)
        {
            GetComponent<RectTransform>().anchoredPosition = Vector2.MoveTowards(GetComponent<RectTransform>().anchoredPosition, m_inventoryUpPos, m_scrollSpeed * Time.deltaTime);
        }

        if (m_inOverworld)
        {
            m_overworld.SetActive(true);
            m_overworldMiniMap.SetActive(true);
            if (!m_inventoryData.m_inventory.Contains("Triforce"))
            {
                m_triforce.SetActive(false);
            }
            else
            {
                m_triforce.SetActive(true);
            }
        }
        else
        {
            m_overworld.SetActive(false);
            m_overworldMiniMap.SetActive(false);
        }

        // Stuff to update always
        if (m_inventoryData.m_inventory.Contains("Sword"))
        {
            m_A.GetComponent<UnityEngine.UI.Image>().color = new Color32(255, 255, 255, 255);
        }
        if (m_inventoryData.m_inventory.Contains("Boomarang"))
        {
            m_boomarang.SetActive(true);
        }
        if (m_inventoryData.m_inventory.Contains("Bow"))
        {
            m_bowAndArrow.SetActive(true);
        }
        if (m_inventoryData.m_inventory.Contains("Potion"))
        {
            m_potion.SetActive(true);
        }
        if (m_inventoryData.m_inventory.Contains("Map"))
        {
            m_mapItem.GetComponent<UnityEngine.UI.Image>().color = new Color32(255, 255, 255, 255);
            foreach (GameObject tile in m_dungeonTiles)
            {
                tile.GetComponent<UnityEngine.UI.Image>().color = new Color32(255, 255, 255, 255);
            }
            foreach (GameObject tile in m_dungeonMiniMapTiles)
            {
                tile.GetComponent<UnityEngine.UI.Image>().color = new Color32(33, 56, 239, 255);
            }
        }
        if (m_inventoryData.m_inventory.Contains("Heartpeice"))
        {
            m_heartFour.GetComponent<UnityEngine.UI.Image>().color = new Color32(255, 255, 255, 255);
            if (m_once)
            {
                m_once = false;
                m_healthComp.SetMaxHealth(8);
                m_healthComp.UpdateHealth(8);
            }
        }

        int currnetHealth = m_healthComp.GetCurrentHealth();

        // Health UI
        if (currnetHealth == 8 && m_inventoryData.m_inventory.Contains("Heartpeice"))
        {
            m_heartOne.GetComponent<UnityEngine.UI.Image>().sprite = m_hearts[2];
            m_heartTwo.GetComponent<UnityEngine.UI.Image>().sprite = m_hearts[2];
            m_heartThree.GetComponent<UnityEngine.UI.Image>().sprite = m_hearts[2];
            m_heartFour.GetComponent<UnityEngine.UI.Image>().sprite = m_hearts[2];

        }
        else if (currnetHealth == 7 && m_inventoryData.m_inventory.Contains("Heartpeice"))
        {
            m_heartOne.GetComponent<UnityEngine.UI.Image>().sprite = m_hearts[2];
            m_heartTwo.GetComponent<UnityEngine.UI.Image>().sprite = m_hearts[2];
            m_heartThree.GetComponent<UnityEngine.UI.Image>().sprite = m_hearts[2];
            m_heartFour.GetComponent<UnityEngine.UI.Image>().sprite = m_hearts[1];

        }
        else if (currnetHealth == 6)
        {
            m_heartOne.GetComponent<UnityEngine.UI.Image>().sprite = m_hearts[2];
            m_heartTwo.GetComponent<UnityEngine.UI.Image>().sprite = m_hearts[2];
            m_heartThree.GetComponent<UnityEngine.UI.Image>().sprite = m_hearts[2];
            m_heartFour.GetComponent<UnityEngine.UI.Image>().sprite = m_hearts[0];

        }
        else if (currnetHealth == 5)
        {
            m_heartOne.GetComponent<UnityEngine.UI.Image>().sprite = m_hearts[2];
            m_heartTwo.GetComponent<UnityEngine.UI.Image>().sprite = m_hearts[2];
            m_heartThree.GetComponent<UnityEngine.UI.Image>().sprite = m_hearts[1];
            m_heartFour.GetComponent<UnityEngine.UI.Image>().sprite = m_hearts[0];

        }
        else if (currnetHealth == 4)
        {
            m_heartOne.GetComponent<UnityEngine.UI.Image>().sprite = m_hearts[2];
            m_heartTwo.GetComponent<UnityEngine.UI.Image>().sprite = m_hearts[2];
            m_heartThree.GetComponent<UnityEngine.UI.Image>().sprite = m_hearts[0];
            m_heartFour.GetComponent<UnityEngine.UI.Image>().sprite = m_hearts[0];

        }
        else if (currnetHealth == 3)
        {
            m_heartOne.GetComponent<UnityEngine.UI.Image>().sprite = m_hearts[2];
            m_heartTwo.GetComponent<UnityEngine.UI.Image>().sprite = m_hearts[1];
            m_heartThree.GetComponent<UnityEngine.UI.Image>().sprite = m_hearts[0];
            m_heartFour.GetComponent<UnityEngine.UI.Image>().sprite = m_hearts[0];

        }
        else if (currnetHealth == 2)
        {
            m_heartOne.GetComponent<UnityEngine.UI.Image>().sprite = m_hearts[2];
            m_heartTwo.GetComponent<UnityEngine.UI.Image>().sprite = m_hearts[0];
            m_heartThree.GetComponent<UnityEngine.UI.Image>().sprite = m_hearts[0];
            m_heartFour.GetComponent<UnityEngine.UI.Image>().sprite = m_hearts[0];

        }
        else if (currnetHealth == 1)
        {
            m_heartOne.GetComponent<UnityEngine.UI.Image>().sprite = m_hearts[1];
            m_heartTwo.GetComponent<UnityEngine.UI.Image>().sprite = m_hearts[0];
            m_heartThree.GetComponent<UnityEngine.UI.Image>().sprite = m_hearts[0];
            m_heartFour.GetComponent<UnityEngine.UI.Image>().sprite = m_hearts[0];
        }
        else if (currnetHealth == 0)
        {
            m_heartOne.GetComponent<UnityEngine.UI.Image>().sprite = m_hearts[0];
            m_heartTwo.GetComponent<UnityEngine.UI.Image>().sprite = m_hearts[0];
            m_heartThree.GetComponent<UnityEngine.UI.Image>().sprite = m_hearts[0];
            m_heartFour.GetComponent<UnityEngine.UI.Image>().sprite = m_hearts[0];
        }

        // Inventory open
        if (m_inventoryDown && m_inventoryDownPos == GetComponent<RectTransform>().anchoredPosition)
        {
            Vector2 selectorLocation = m_selector.GetComponent<RectTransform>().anchoredPosition;
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (selectorLocation.y + 55 == -41)
                {
                    selectorLocation.y += 55;
                }
            }
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (selectorLocation.y - 55 == -96)
                {
                    selectorLocation.y -= 55;
                }
            }
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (selectorLocation.x + 82 <= 271.5)
                {
                    selectorLocation.x += 82;
                }
            }
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (selectorLocation.x - 82 >= 25.5)
                {
                    selectorLocation.x -= 82;
                }
            }
            m_selector.GetComponent<RectTransform>().anchoredPosition = selectorLocation;


            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (selectorLocation == new Vector2(25.5f, -41) && m_boomarang.activeSelf)
                {
                    m_useBButton.SetActive(true);
                    m_B.SetActive(true);
                    m_useBButton.GetComponent<UnityEngine.UI.Image>().sprite = m_boomarangSprite;
                    m_B.GetComponent<UnityEngine.UI.Image>().sprite = m_boomarangSprite;
                    m_bSlot = BSlot.Boomarang;
                }
                else if (selectorLocation == new Vector2(107.5f, -41) && m_bomb.activeSelf)
                {
                    m_useBButton.SetActive(true);
                    m_B.SetActive(true);
                    m_useBButton.GetComponent<UnityEngine.UI.Image>().sprite = m_bombSprite;
                    m_B.GetComponent<UnityEngine.UI.Image>().sprite = m_bombSprite;
                    m_bSlot = BSlot.Bomb;
                }
                else if (selectorLocation == new Vector2(189.5f, -41) && m_bowAndArrow.activeSelf)
                {
                    m_useBButton.SetActive(true);
                    m_B.SetActive(true);
                    m_useBButton.GetComponent<UnityEngine.UI.Image>().sprite = m_bowAndArrowSprite;
                    m_B.GetComponent<UnityEngine.UI.Image>().sprite = m_bowAndArrowSprite;
                    m_bSlot = BSlot.Bow;
                }
                else if (selectorLocation == new Vector2(189.5f, -96) && m_potion.activeSelf)
                {
                    m_useBButton.SetActive(true);
                    m_B.SetActive(true);
                    m_useBButton.GetComponent<UnityEngine.UI.Image>().sprite = m_potionSprite;
                    m_B.GetComponent<UnityEngine.UI.Image>().sprite = m_potionSprite;
                    m_bSlot = BSlot.Potion;
                }
                else
                {
                    m_useBButton.SetActive(false);
                    m_B.SetActive(false);
                    m_bSlot = BSlot.None;
                }
            }
        }

        //
        if (m_cameraController.gameObject.transform.position == m_overword1)
        {
            m_playerMiniMapIcon.GetComponent<RectTransform>().anchoredPosition = m_miniMapLocation1;
        }
        else if (m_cameraController.gameObject.transform.position == m_overword2)
        {
            m_playerMiniMapIcon.GetComponent<RectTransform>().anchoredPosition = m_miniMapLocation2;

        }
        else if (m_cameraController.gameObject.transform.position == m_overword3)
        {
            m_playerMiniMapIcon.GetComponent<RectTransform>().anchoredPosition = m_miniMapLocation4;

        }
        else if (m_cameraController.gameObject.transform.position == m_overword4)
        {
            m_playerMiniMapIcon.GetComponent<RectTransform>().anchoredPosition = m_miniMapLocation5;

        }

        if (m_cameraController.gameObject.transform.position == m_dungeon1)
        {
            m_playerMiniMapIcon.GetComponent<RectTransform>().anchoredPosition = m_miniMapLocation1;
            m_playerMapIcon.GetComponent<RectTransform>().anchoredPosition = m_mapLocation1;
            m_dungeonTiles[0].GetComponent<UnityEngine.UI.Image>().color = new Color32(255, 255, 255, 255);
            m_dungeonMiniMapTiles[0].GetComponent<UnityEngine.UI.Image>().color = new Color32(33, 56, 239, 255);
        }
        else if (m_cameraController.gameObject.transform.position == m_dungeon2)
        {
            m_playerMiniMapIcon.GetComponent<RectTransform>().anchoredPosition = m_miniMapLocation2;
            m_playerMapIcon.GetComponent<RectTransform>().anchoredPosition = m_mapLocation2;
            m_dungeonTiles[1].GetComponent<UnityEngine.UI.Image>().color = new Color32(255, 255, 255, 255);
            m_dungeonMiniMapTiles[1].GetComponent<UnityEngine.UI.Image>().color = new Color32(33, 56, 239, 255);
        }
        else if (m_cameraController.gameObject.transform.position == m_dungeon3)
        {
            m_playerMiniMapIcon.GetComponent<RectTransform>().anchoredPosition = m_miniMapLocation3;
            m_playerMapIcon.GetComponent<RectTransform>().anchoredPosition = m_mapLocation3;
            m_dungeonTiles[2].GetComponent<UnityEngine.UI.Image>().color = new Color32(255, 255, 255, 255);
            m_dungeonMiniMapTiles[2].GetComponent<UnityEngine.UI.Image>().color = new Color32(33, 56, 239, 255);
        }
        else if (m_cameraController.gameObject.transform.position == m_dungeon4)
        {
            m_playerMiniMapIcon.GetComponent<RectTransform>().anchoredPosition = m_miniMapLocation4;
            m_playerMapIcon.GetComponent<RectTransform>().anchoredPosition = m_mapLocation4;
            m_dungeonTiles[3].GetComponent<UnityEngine.UI.Image>().color = new Color32(255, 255, 255, 255);
            m_dungeonMiniMapTiles[3].GetComponent<UnityEngine.UI.Image>().color = new Color32(33, 56, 239, 255);
        }
        else if (m_cameraController.gameObject.transform.position == m_dungeon5)
        {
            m_playerMiniMapIcon.GetComponent<RectTransform>().anchoredPosition = m_miniMapLocation5;
            m_playerMapIcon.GetComponent<RectTransform>().anchoredPosition = m_mapLocation5;
            m_dungeonTiles[4].GetComponent<UnityEngine.UI.Image>().color = new Color32(255, 255, 255, 255);
            m_dungeonMiniMapTiles[4].GetComponent<UnityEngine.UI.Image>().color = new Color32(33, 56, 239, 255);
        }
    }

    public BSlot UseBSlot()
    {
        if (m_bSlot == BSlot.None)
        {
            return BSlot.None;
        }
        else if (m_bSlot == BSlot.Boomarang)
        {
            return BSlot.Boomarang;
            // Handled by player
        }
        else if (m_bSlot == BSlot.Bomb)
        {
            if (m_inventoryData.m_bombs > 0)
            {
                UpdateBombs(-1);
                return BSlot.Bomb;
            }
            else 
                return BSlot.None;
            // Handled by player
        }
        else if (m_bSlot == BSlot.Bow)
        {
            if (m_inventoryData.m_rupies > 0)
            {
                UpdateRupies(-1);
                return BSlot.Bow;
            }
            return BSlot.None;
            // Handled by player
        }
        else if (m_bSlot == BSlot.Potion)
        {
            m_healthComp.UpdateHealth(8);
            m_inventoryData.m_inventory.Remove("Potion");
            m_potion.SetActive(false);
            m_useBButton.SetActive(false);
            m_B.SetActive(false);
            m_bSlot = BSlot.None;
            return BSlot.Potion;
        }

        return BSlot.None;
    }

    private void SetUINumber(int stat ,GameObject tens, GameObject ones)
    {
        int tensPlace = stat / 10;
        int onesPlace = stat % 10;

        for (int i = 0; i < 10; i++)
        {
            if (tensPlace == i)
            {
                tens.GetComponent<UnityEngine.UI.Image>().sprite = m_numbers[i];
            }
            if (onesPlace == i)
            {
                ones.GetComponent<UnityEngine.UI.Image>().sprite = m_numbers[i];
            }
        }
        
    }

    public void UpdateRupies(int num)
    {
        if (m_inventoryData.m_rupies + num >= 0 && m_inventoryData.m_rupies + num < 100)
        {
            m_inventoryData.m_rupies += num;
            SetUINumber(m_inventoryData.m_rupies, m_rupieTens, m_rupieOnes);
        }

    }
    public int GetRupies()
    {
        return m_inventoryData.m_rupies;
    }

    public void UpdateKeys(int num)
    {
        if (m_inventoryData.m_keys + num >= 0 && m_inventoryData.m_keys + num < 100)
        {
            m_inventoryData.m_keys += num;
            SetUINumber(m_inventoryData.m_keys, m_keysTens, m_keysOnes);
        }

    }
    public int GetKeys()
    {
        return m_inventoryData.m_keys;
    }
    public void UpdateBombs(int num)
    {
        if (m_inventoryData.m_bombs + num >= 0 && m_inventoryData.m_bombs + num < 100)
        {
            m_inventoryData.m_bombs += num;
            SetUINumber(m_inventoryData.m_bombs, m_bombsTens, m_bombsOnes);
            if (m_inventoryData.m_bombs > 0)
                m_bomb.SetActive(true);
                
            else
            {
                m_bomb.SetActive(false);
                if (m_useBButton.GetComponent<UnityEngine.UI.Image>().sprite == m_boomarangSprite && m_B.GetComponent<UnityEngine.UI.Image>().sprite == m_boomarangSprite)
                {
                    m_useBButton.SetActive(false);
                    m_B.SetActive(false);
                    m_bSlot = BSlot.None;
                }
            }
        }
    }
    public int GetBombs()
    {
        return m_inventoryData.m_rupies;
    }

    public void AddItem(string item)
    {
        m_inventoryData.m_inventory.Add(item);
    }
    public bool ContainsItem(string item)
    {
        return m_inventoryData.m_inventory.Contains(item);
    }

    public Health GetHealthComp()
    {  
        return m_healthComp; 
    }

    public void SetInOverworld(bool set)
    {
        m_inOverworld = set;
    }
    public void ResetStats()
    {
        m_inventoryData.m_inventory.Clear();
        m_inventoryData.m_rupies = 0;
        m_inventoryData.m_keys = 0;
        m_inventoryData.m_bombs = 0;
    }
}

[System.Serializable]
public class InventoryData
{
    public List<string> m_inventory = new List<string>();
    public int m_rupies = 0;
    public int m_keys = 0;
    public int m_bombs = 0;
}