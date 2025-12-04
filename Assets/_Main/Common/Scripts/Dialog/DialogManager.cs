using UnityEngine;

public class DialogManager : MonoBehaviour
{
    private static DialogManager _instance;

    public static DialogManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<DialogManager>();
            }
            if (_instance == null)
            {
                _instance = new GameObject(nameof(DialogManager)).AddComponent<DialogManager>();
            }
            return _instance;
        }
    }

    private Dialog dialog;

    public Dialog Get()
    {
        if (dialog == null)
        {
            var prefab = Resources.Load<Dialog>($"prefab_dialog");
            dialog = Instantiate(prefab, transform);
        }
        return dialog;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
