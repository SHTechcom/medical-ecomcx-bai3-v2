using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VoiceManager : MonoBehaviour
{
    public bool isMale;
    public List<VoiceItem> items = new List<VoiceItem>();

    private UISetting UISetting => GameViewManager.Instance.GetView<UISetting>();

    private void Reset()
    {
        items = FindObjectsOfType<VoiceItem>(true).ToList();
    }

    private void Start()
    {
        isMale = false;
        items.ForEach(i => i?.Change(isMale));
        UISetting.SetIconVoice(isMale);
        UISetting.OnClickedVoice(ChangeVoice);
    }

    public void ChangeVoice()
    {
        isMale = !isMale;
        items.ForEach(i => i?.Change(isMale));
        UISetting.SetIconVoice(isMale);
    }
}
