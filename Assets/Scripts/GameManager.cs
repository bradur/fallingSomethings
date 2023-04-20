using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager main;
    void Awake()
    {
        main = this;
    }

    [SerializeField]
    private OptionsMenu menu;

    [SerializeField]
    private TextureDrawer textureDrawer;

    [SerializeField]
    private BrushManager brushManager;


    void Start()
    {
        brushManager.Initialize(textureDrawer);
    }

    private bool menuOpen = false;
    public bool MenuOpen { get { return menuOpen; } }

    public void OpenOptions()
    {
        menuOpen = true;
        menu.Show();
    }


    public void InitializeAndFill()
    {
        textureDrawer.InitializeAndFill();
    }
    public void Reset()
    {
        textureDrawer.Reset();
    }

    public int RandomChoice(int value1, int value2)
    {
        if (UnityEngine.Random.Range(0f, 1f) > 0.5f)
        {
            return value1;
        }
        return value2;
    }


    public void CloseOptions()
    {
        menuOpen = false;
        menu.Hide();
        UpdateOptions();
    }

    public void UpdateOptions()
    {
        textureDrawer.SetOptions(menu.Options);
    }

    public void SelectBrush(BrushConfig config)
    {
        brushManager.SelectBrush(config);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (menuOpen)
            {
                CloseOptions();
            }
            else
            {
                OpenOptions();
            }
        }
    }
}

