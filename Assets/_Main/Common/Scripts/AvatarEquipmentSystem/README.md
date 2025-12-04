# Avatar Equipment System

## Sử dụng nhanh với hệ thống UI

### Setup

#### 1. Setup trong scene

- Kéo prefab (*prefab_choose_avatar_equipment_control*)  vào scene
- Script để gọi API: **AvatarEquipmentControl**
- Kéo avatar_equipment_preset vào ref: để kiểm tra đồ của bài đó
    - Tạo mới cho từng bài
- Kéo avatar_equipment_database vào ref: để hiển thị các items có thể kéo thả để equip
    - Có thể thừa
    - Tạo mới cho từng bài
    - Có helper để áp dụng preset trước rồi kéo thêm đồ nếu muốn
- Tick chọn *Accept Missing* hoặc *Accept Extra* nếu cần thiết (mặc định true)
- Tick chọn *startWithCloth*: True (Cloth => ToolAndMedicine); False (ngược lại)
- *onCompleteChooseEquipment*: UnityEvent gọi khi hoàn thành phase

#### 2. Setup SO

- **AvatarEquipment** (_Main/Common/SO/AvatarEquipment/(Cloths||Tools||Medicines))
    - key:
        - sử dụng trong hệ thống AvatarEquipmentSystem
        - cần phải khác với các key khác
        - Nên đặt theo: equpiment_{type}_{name}: VD: equipment_cloth_mask; equipment_tool_kimtiem
    - equipmentName: Hiển thị trên UI
    - type: Type của equipment
    - modal: Object spawn để kéo thả trên UI
    - scaleOnUI: scale ở UI
    - rotate: rotate ở UI
    - *Cách xem để sửa thông số scale, rotate*:
        - Vào scene AvatarEquipmentView (_Main/Common/Scenes/AvatarEquipmentView.scene)
        - Kéo đồ làm child của choose_avatar_equipment_control/choose_equipment_object_drag/model_parent với transform
          mặc định
        - Chỉnh sửa rotation và scale của model_parent sao cho nhìn thấy trên UI => Nếu oke thì sửa vào SO
        - Nếu vị trí bị lệch thì nên tạo 1 prefab (Lưu prefab ở _Main/Common/Prefabs/AvatarEquipmentSystem) mới làm cha
          của renderer đó và chỉnh sửa vị trí của renderer sao cho nhìn được trên UI
        - Rotate và scale Renderer và object con của model_parent nên để là mặc định hết
        - **Đã có ví dụ trong scene**

---

- **AvatarEquipmentPreset** (_Main/Common/SO/AvatarEquipment/Presets)
    - cloths: list cloth equipments
    - toolsAndMedicines: list tools and medicines equipments
    - exactlyClothWarning: Text hiển thị trên UI khi đủ đồ
    - missingClothWarning: Text hiển thị trên UI khi thiếu đồ
    - extraClothWarning: Text hiển thị trên UI khi thừa đồ
    - exactlyToolAndMedicineWarning: Text hiển thị trên UI khi đủ đồ
    - missingToolAndMedicineWarning: Text hiển thị trên UI khi thiếu đồ
    - extraToolAndMedicineWarning: Text hiển thị trên UI khi thừa đồ

---

- **AvatarEquipmentDatabase** (_Main/Common/SO/AvatarEquipment/Databases)
    - cloths: list cloth equipments
    - toolsAndMedicines: list tools and medicines equipments
    - HELPER: *ImportFromPreset*: Đổi các list như trong preset

### Lưu ý

- Hệ thống Control trong prefab đã có thứ tự nên nếu ko đổi luồng thì chỉ cần kéo vào scene, đổi ref SO và active khi
  cần thiết
- Nếu Cloth hoặc ToolAndMedicine trong preset rỗng thì tự động skip
- Nếu đổi luồng thì copy script sang script mới và thay đổi
- KO APPLY PREFAB Ở SCENE

### API script AvatarEquipmentControl

- Action(int): đăng ký để sử dụng; luôn gọi khi số lượng > 0 kể cả *Accept Missing* và *Accept Extra* bật
    - **OnMissingCloth**: Trả về số lượng item thiếu trong Cloth
    - **OnMissingToolsAndMedicines**: Trả về số lượng item thiếu trong ToolsAndMedicines
    - **OnExtraCloth**: Trả về số lượng item thừa trong Cloth
    - **OnExtraToolsAndMedicines**: Trả về số lượng item thừa trong ToolsAndMedicines

---

- Method: (Mặc định gọi theo thứ tự Cloth -> ToolAndMedicine khi Start() )
    - **ResetAvatarEquipmentSystem**: Reset hệ thống AvatarEquipmentSystem
    - **StartChooseFlow**: Show theo flow đã chọn
    - **ShowClothChooseUI**: Hiển thị UI chọn Cloth
    - **ShowToolAndMedicineChooseUI**: Hiển thị UI chọn ToolAndMedicine
    - **HideChooseEquipmentUI**: ẩn UI
    - **CompleteChooseEquipmentPhase**: Gọi event hoàn thành phase và ẩn UI

## Tính năng chính

- Quản lý 2 loại trang bị: **Cloths** và **ToolAndMedicine**
- Equip/UnEquip items với hệ thống events
- Kiểm tra trạng thái trang bị
- So sánh và validate với preset (khớp hoàn toàn hoặc chứa đủ items)
- Áp dụng nhanh preset
- Tìm items thiếu/thừa so với preset
- Logging system có thể bật/tắt

## Cấu trúc

### Scriptable Objects

1. **`AvatarEquipment`** - ScriptableObject chứa thông tin item
    - `equipmentName`: Tên trang bị
    - `type`: Loại trang bị (EquipmentType)

2. **`AvatarEquipmentPreset`** - ScriptableObject chứa danh sách items
    - `cloths`: Danh sách quần áo
    - `toolsAndMedicines`: Danh sách công cụ và thuốc

3. **`EquipmentType`** - Enum định nghĩa loại trang bị
    - `Cloth`: Quần áo
    - `ToolAndMedicine`: Công cụ và thuốc

## Sử dụng cơ bản

### Khởi tạo

```csharp
// Cách 1: Khởi tạo system trống
var equipmentSystem = new AvatarEquipmentSystem();

// Cách 2: Khởi tạo với preset (Tự động equip các item trong preset)
var equipmentSystem = new AvatarEquipmentSystem(myPreset);

// Cách 3: Reset hệ thống về trạng thái ban đầu
AvatarEquipmentSystem.Init();

// Cách 4: Reset chỉ một loại trang bị
AvatarEquipmentSystem.Init(EquipmentType.Cloth);
```

### Equip/UnEquip Items

```csharp
// Equip một item
AvatarEquipmentSystem.Equip(clothItem);

// UnEquip một item
AvatarEquipmentSystem.UnEquip(clothItem);

// Kiểm tra xem item có đang được equip không
bool isEquipped = AvatarEquipmentSystem.IsEquipped(clothItem);

// Xóa tất cả trang bị
AvatarEquipmentSystem.ClearEquipment();
```

### Làm việc với Preset

```csharp
// Áp dụng nhanh preset (xóa hết và equip theo preset)
AvatarEquipmentSystem.QuickApplyPreset(myPreset);

// Kiểm tra xem tất cả trang bị hiện tại có khớp hoàn toàn với preset không
// (Không thiếu, không thừa)
bool isValid = AvatarEquipmentSystem.CheckValidAllPreset(myPreset);

// Kiểm tra từng loại trang bị có khớp với preset không
bool clothsValid = AvatarEquipmentSystem.CheckValidPreset(EquipmentType.Cloth, myPreset);

// Kiểm tra xem đã equip đủ items trong preset chưa (có thể thừa)
bool hasAllClothes = AvatarEquipmentSystem.CheckContainPreset(EquipmentType.Cloth, myPreset);
```

### Tìm items thiếu/thừa

```csharp
// Lấy danh sách items thiếu so với preset
var missingClothes = AvatarEquipmentSystem.GetMissingItems(EquipmentType.Cloth, myPreset);
if (missingClothes != null && missingClothes.Count > 0)
{
    Debug.Log("Cần thêm: " + string.Join(", ", missingClothes));
}

// Lấy danh sách items thừa so với preset
var extraTools = AvatarEquipmentSystem.GetExtraItems(EquipmentType.ToolAndMedicine, myPreset);
if (extraTools != null && extraTools.Count > 0)
{
    Debug.Log("Đang thừa: " + string.Join(", ", extraTools));
}
```

### Events System

```csharp
public class EquipmentController : MonoBehaviour
{
    void OnEnable()
    {
        // Đăng ký sự kiện khi equip item
        AvatarEquipmentSystem.OnEquipItem += OnItemEquipped;
        
        // Đăng ký sự kiện khi unequip item
        AvatarEquipmentSystem.OnUnEquipItem += OnItemUnEquipped;
    }

    void OnDisable()
    {
        // ⚠️ QUAN TRỌNG: Luôn hủy đăng ký events
        AvatarEquipmentSystem.OnEquipItem -= OnItemEquipped;
        AvatarEquipmentSystem.OnUnEquipItem -= OnItemUnEquipped;
    }

    private void OnItemEquipped(AvatarEquipment item)
    {
        Debug.Log($"✅ Equipped: {item.equipmentName}");
        // Thực hiện logic khi equip (ví dụ: hiển thị model)
    }

    private void OnItemUnEquipped(AvatarEquipment item)
    {
        Debug.Log($"❌ UnEquipped: {item.equipmentName}");
        // Thực hiện logic khi unequip (ví dụ: ẩn model)
    }
}
```

### Logging System

```csharp
// Bật logging để debug
AvatarEquipmentSystem.ShowLog = true;

// Tắt logging trong production
AvatarEquipmentSystem.ShowLog = false;
```

## 📚 API Reference

### Properties (Static)

| Property                             | Type                    | Mô tả                                          |
|--------------------------------------|-------------------------|------------------------------------------------|
| `CurrentClothsEquipments`            | `List<AvatarEquipment>` | Danh sách quần áo đang equip (read-only)       |
| `CurrentToolsAndMedicinesEquipments` | `List<AvatarEquipment>` | Danh sách công cụ/thuốc đang equip (read-only) |
| `ShowLog`                            | `bool`                  | Bật/tắt logging system                         |

### Events (Static)

| Event           | Type                      | Mô tả                                |
|-----------------|---------------------------|--------------------------------------|
| `OnEquipItem`   | `Action<AvatarEquipment>` | Được gọi khi equip item thành công   |
| `OnUnEquipItem` | `Action<AvatarEquipment>` | Được gọi khi unequip item thành công |

### Methods (Static)

#### Khởi tạo

**`Init()`**

- Khởi tạo/reset toàn bộ hệ thống
- Xóa tất cả trang bị hiện tại (cả Cloths và ToolAndMedicine)

**`Init(EquipmentType type)`**

- Khởi tạo/reset chỉ một loại trang bị cụ thể
- **Parameters:**
    - `type`: Loại trang bị cần reset

---

#### Quản lý trang bị

**`Equip(AvatarEquipment item)`**

- Equip một item vào hệ thống
- Tự động phân loại theo `item.type`
- Không equip lại nếu item đã được equip
- Trigger event `OnEquipItem` nếu equip thành công
- **Parameters:**
    - `item`: Item cần equip (không null)

**`UnEquip(AvatarEquipment item)`**

- Gỡ bỏ một item khỏi hệ thống
- Trigger event `OnUnEquipItem` nếu unequip thành công
- **Parameters:**
    - `item`: Item cần unequip (không null)

**`IsEquipped(AvatarEquipment item)`**

- Kiểm tra xem item có đang được equip hay không
- **Parameters:**
    - `item`: Item cần kiểm tra
- **Returns:** `bool` - `true` nếu đang equip, `false` nếu không

**`ClearEquipment()`**

- Xóa tất cả trang bị hiện tại
- Không trigger events

---

#### Làm việc với Preset

**`QuickApplyPreset(AvatarEquipmentPreset preset)`**

- Xóa tất cả trang bị hiện tại và áp dụng preset mới
- Equip tất cả items trong preset (cả cloths và toolsAndMedicines)
- **Parameters:**
    - `preset`: Preset cần áp dụng (có thể null, sẽ bỏ qua)

**`CheckValidAllPreset(AvatarEquipmentPreset preset)`**

- Kiểm tra xem tất cả trang bị hiện tại có **khớp hoàn toàn** với preset hay không
- Khớp hoàn toàn = không thiếu và không thừa items
- **Parameters:**
    - `preset`: Preset cần so sánh
- **Returns:** `bool` - `true` nếu khớp hoàn toàn hoặc preset null, `false` nếu có thiếu/thừa

**`CheckValidPreset(EquipmentType type, AvatarEquipmentPreset preset)`**

- Kiểm tra một loại trang bị cụ thể có khớp với preset hay không
- **Parameters:**
    - `type`: Loại trang bị cần kiểm tra
    - `preset`: Preset cần so sánh
- **Returns:** `bool` - `true` nếu khớp, `false` nếu có thiếu/thừa hoặc preset null

**`CheckContainPreset(EquipmentType type, AvatarEquipmentPreset preset)`**

- Kiểm tra xem đã equip **đủ** items trong preset chưa
- Trả về `true` nếu có đủ items (có thể thừa items không có trong preset)
- Trả về `false` nếu thiếu items
- **Parameters:**
    - `type`: Loại trang bị cần kiểm tra
    - `preset`: Preset cần so sánh
- **Returns:** `bool` - `true` nếu có đủ items, `false` nếu thiếu hoặc preset null

---

#### Kiểm tra

**`GetMissingItems(EquipmentType type, AvatarEquipmentPreset preset)`**

- Lấy danh sách items thiếu so với preset
- **Parameters:**
    - `type`: Loại trang bị cần kiểm tra
    - `preset`: Preset cần so sánh
- **Returns:** `List<AvatarEquipment>` - Danh sách items thiếu, hoặc `null` nếu preset null

**`GetExtraItems(EquipmentType type, AvatarEquipmentPreset preset)`**

- Lấy danh sách items thừa so với preset
- **Parameters:**
    - `type`: Loại trang bị cần kiểm tra
    - `preset`: Preset cần so sánh
- **Returns:** `List<AvatarEquipment>` - Danh sách items thừa, hoặc `null` nếu preset null

## Ví dụ

### Ví dụ 1: Kiểm tra và tự động bổ sung trang bị thiếu

```csharp
public void EnsurePresetEquipped(AvatarEquipmentPreset requiredPreset)
{
    // Kiểm tra items thiếu
    var missingClothes = AvatarEquipmentSystem.GetMissingItems(
        EquipmentType.Cloth, 
        requiredPreset
    );
    
    // Tự động equip items thiếu
    if (missingClothes != null)
    {
        foreach (var item in missingClothes)
        {
            AvatarEquipmentSystem.Equip(item);
        }
    }
}
```

### Ví dụ 2: Xóa items thừa

```csharp
public void RemoveExtraItems(AvatarEquipmentPreset allowedPreset)
{
    // Lấy items thừa
    var extraTools = AvatarEquipmentSystem.GetExtraItems(
        EquipmentType.ToolAndMedicine, 
        allowedPreset
    );
    
    // Unequip items thừa
    if (extraTools != null)
    {
        foreach (var item in extraTools)
        {
            AvatarEquipmentSystem.UnEquip(item);
        }
    }
}
```

## ⚠️ Lưu ý

### Events

- **Luôn hủy đăng ký events** nếu đã đăng ký để tránh:
    - Memory leaks
    - Null reference exceptions
    - Duplicate event calls

### Null Safety

- Tất cả methods đều có xử lý null an toàn
- Methods trả về list có thể trả về `null` nếu preset null
- Nên kiểm tra `!= null` trước khi dùng kết quả

### Static Members

- System sử dụng static members để dễ truy cập
- Chỉ có một instance duy nhất trong toàn ứng dụng
- Dữ liệu không bị reset khi scene thay đổi (trừ khi gọi `Init()`)

### Performance

- `CurrentClothsEquipments` và `CurrentToolsAndMedicinesEquipments` là read-only
- Không thể modify trực tiếp, phải dùng `Equip()`/`UnEquip()`
- Logging có thể ảnh hưởng performance, nên tắt trong production

## Tham khảo

Xem file **AvatarEquipmentCallExample.cs** để có thêm ví dụ chi tiết về cách sử dụng.