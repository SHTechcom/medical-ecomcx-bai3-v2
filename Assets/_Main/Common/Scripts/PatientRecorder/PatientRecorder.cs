using TMPro;
using UnityEngine;

public class PatientRecorderData
{
    public string name;
    public string age;
    public string patient;
}

public class PatientRecorder : MonoBehaviour
{
    private PatientRecorderData data;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text ageText;
    [SerializeField] private SimpleDropdown simpleDropdown;

    public PatientRecorder Init()
    {
        data = new PatientRecorderData();
        SetPatientOptions();
        return this;
    }

    public PatientRecorder SetName(string name)
    {
        nameText?.SetText(name);
        data.name = name;
        return this;
    }

    public PatientRecorder SetAge(string age)
    {
        ageText?.SetText($"{age} tuổi");
        data.age = age;
        return this;
    }

    public PatientRecorder SetPatientOptions()
    {
        simpleDropdown.Init(PatientConfig.PATIENTS, SetPatient);
        return this;
    }

    private void SetPatient(string value)
    {
        data.patient = value;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
