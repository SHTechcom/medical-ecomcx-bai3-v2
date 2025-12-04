using UnityEngine;

public class PatientRecorderManager : MonoBehaviour
{
    private static PatientRecorderManager _instance;

    public static PatientRecorderManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<PatientRecorderManager>();
            }
            if (_instance == null)
            {
                _instance = new GameObject(nameof(PatientRecorderManager)).AddComponent<PatientRecorderManager>();
            }
            return _instance;
        }
    }

    private PatientRecorder patientRecorder;

    public PatientRecorder Get()
    {
        if(patientRecorder == null)
        {
            var prefab = Resources.Load<PatientRecorder>($"prefab_patient-recorder");
            patientRecorder = Instantiate(prefab, transform);
        }
        return patientRecorder;
    }

    private void Awake()
    {
        if(Instance != null)
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
