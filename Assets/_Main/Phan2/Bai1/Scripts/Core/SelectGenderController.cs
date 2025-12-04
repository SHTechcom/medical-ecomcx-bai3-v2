using Frank;
using UnityEngine;

namespace Bai11
{
    public enum Gender
    {
        Male,
        Female
    }

    public class SelectGenderController : Singleton<SelectGenderController>
    {
        public GameObject malePrefab;
        public GameObject femalePrefab;
        private GameObject spawned;
        private Gender gender;

        public Gender Gender => gender;

        private UISelectGender UISelectGender => GameViewManager.Instance.GetView<UISelectGender>();

        private void Start()
        {
            UISelectGender.OnClickSelect1(OnSelectMale);
            UISelectGender.OnClickSelect1(OnSelecteFemale);
        }

        private void OnSelectMale()
        {
            Select(Gender.Male);
            UISelectGender.Hide();
        }

        private void OnSelecteFemale()
        {
            Select(Gender.Female);
            UISelectGender.Hide();
        }

        public void Select(Gender gender)
        {
            this.gender = gender;
            if (spawned != null)
            {
                Destroy(spawned.gameObject);
            }
            if (gender == Gender.Male)
            {
                spawned = CreateMale();
            }
            else
            {
                spawned = CreateFemale();
            }
        }

        private GameObject CreateMale()
        {
            return Instantiate(malePrefab);
        }

        private GameObject CreateFemale()
        {
            return Instantiate(femalePrefab);
        }
    }
}
