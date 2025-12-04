using _Main.Phan1.Bai1.Scripts.TaskSystem;
using _Main.Phan1.Bai1.Scripts.UI;
using _Main.Phan1.Bai1.Scripts.UI.WarningUI;
using _Main.Phan1.Bai1.StepSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _Main.Phan1.Bai1.Scripts
{
    public class Bai1Controller : MonoBehaviour
    {
        public void ShowCheckListCheckDrug(CheckListData data)
        {
            CheckListUI.Instance.Show(data, OnCheckDrug);
        }

        private void OnCheckDrug(bool isCorrect)
        {
            if (isCorrect)
            {
                CheckListUI.Instance.Hide();
                StepConditionController.MinusConditionCount();
            }
            else
            {
            }
        }

        [FoldoutGroup("Chuẩn bị kim tiêm")] public BaseActionStep duaKimVaoOngStep;
        [FoldoutGroup("Chuẩn bị kim tiêm")] public BaseActionStep rutThuocStep;
        [FoldoutGroup("Chuẩn bị kim tiêm")] public BaseActionStep xoayTrucVatStep;
        [FoldoutGroup("Chuẩn bị kim tiêm")] public GameObject trucVatCheckImage;

        public void CheckDuaKimVaoOng(Slider slider)
        {
            if (slider.value / (slider.maxValue - slider.minValue) >= 0.9f)
            {
                duaKimVaoOngStep.CompleteAction();
            }
            else
            {
                WarningUI.Instance?.Show("Sai vị trí");
            }
        }

        public void CheckRutThuoc(Slider slider)
        {
            if (slider.value / (slider.maxValue - slider.minValue) <= 0.1f)
            {
                rutThuocStep.CompleteAction();
            }
            else
            {
                WarningUI.Instance?.Show("Sai vị trí kim");
            }
        }

        public void XoayTrucVat(float value)
        {
            trucVatCheckImage.SetActive(value >= 0.4f && value <= 0.6f);
        }

        public void CheckTrucVatTrue(Slider slider)
        {
            if (slider.value >= 0.4f && slider.value <= 0.6f)
            {
                xoayTrucVatStep.CompleteAction();
            }
            else
            {
                WarningUI.Instance?.Show("Sai vị trí");
            }
        }


        [FoldoutGroup("Xé vỏ kim tiêm")] public BaseActionStep xeVoBomKimTiemStep;

        public void CheckXeVoBomKimTiem(Transform inspectTransform)
        {
            float zAngle = inspectTransform.localEulerAngles.z;
            if (zAngle > 180) zAngle -= 360;

            float yAngle = inspectTransform.localEulerAngles.y;
            if (yAngle > 180) yAngle -= 360;

            if (yAngle >= -20 && yAngle <= 20 &&
                zAngle >= -20 && zAngle <= 20)
            {
                xeVoBomKimTiemStep.CompleteAction();
            }
            else
            {
                WarningUI.Instance?.Show("Xé vỏ sai kỹ thuật");
            }
        }

        [FoldoutGroup("Đuổi khí")] public BaseActionStep keoPittingXuongStep;
        [FoldoutGroup("Đuổi khí")] public BaseActionStep dayPittingLenStep;

        public void CheckKeoPittongXuong(Slider slider)
        {
            if (slider.value / (slider.maxValue - slider.minValue) <= 0.1f)
            {
                keoPittingXuongStep.CompleteAction();
            }
            else
            {
                WarningUI.Instance?.Show("Sai vị trí");
            }
        }

        public void CheckDayPittongLen(Slider slider)
        {
            if (slider.value / (slider.maxValue - slider.minValue) >= 0.9f)
            {
                dayPittingLenStep.CompleteAction();
                WarningUI.Instance?.Show("Đã đuổi khí");
            }
        }

        [FoldoutGroup("Đâm kim")] public BaseActionStep damKimStep;

        public void CheckDamKim(Slider slider)
        {
            if (slider.value / (slider.maxValue - slider.minValue) >= 28 / 180f
                && slider.value / (slider.maxValue - slider.minValue) <= 32 / 180f)
            {
                damKimStep.CompleteAction();
            }
            else
            {
                WarningUI.Instance?.Show("Sai góc đâm!");
            }
        }
    }
}