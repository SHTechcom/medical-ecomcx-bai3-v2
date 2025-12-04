namespace _Main.Common.Scripts.Avatar
{
    public interface IAvatarEquipmentObject
    {
        void OnEquip(AvatarEquipment equipment);
        void OnUnEquip(AvatarEquipment equipment);
    }
}