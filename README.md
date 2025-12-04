# 🎮 Base Project

Một bộ khung Unity cơ bản

------------------------------------------------------------------

## Chú ý khi build game:
- Build:
    + WebGL
    + Test:
        Code optimizztion: Shorter Build Time
    + Release:
        Dùng Scene nào thì add scene đấy (ví dụ: bài 1 -> scene Bai1)
        Manager Stripping Level: High (Ignore những thứ ko được dùng khi build)
        Code optimizztion: Runtime Speed with LTO

- Folder Practices:
    + Chung 20 bài trong 1 dự án
    + Chia các bài thành các folder riêng
    _Main:
        Common: 
            Script 
            Model
            ...
        Phan1:
            Bai1
                Script 
                Model
                ...
            Bai2
            ...
        Phan2:
            Bai1
                Script 
                Model
                ...
            Bai2
            ...
    + Folder Common sẽ chứa những hệ thống, model,... dùng chung xuyên suốt các  bài

------------------------------------------------------------------

## 📁 Quy ước đặt tên Asset

| Loại Asset           | Tiền tố     | Ví dụ                         |
|----------------------|-------------|-------------------------------|
| Sprite               | `spr_`      | `spr_coin`                    |
| UI Sprite            | `ui_`       | `ui_popupmission_title`      |
| Texture              | `tex_`      | `tex_block`                   |
| 3D Model             | `model_`    | `model_block`                 |
| Material             | `mat_`      | `mat_block`                   |
| Sound Effect (SFX)   | `sfx_`      | `sfx_win`                     |
| Background Music     | `bgm_`      | `bgm_gameplay`                |
| Animation Clip       | `anim_`     | `anim_jump`                   |
| Animator Controller  | `ac_`       | `ac_player`                   |
| Prefab               | `prefab_`   | `prefab_block`                |
| ScriptableObject     | `so_`       | `so_config_game`              |
| Shader               | `shader_`   | `shader_liquid`               |
| Font                 | `font_`     | `font_time_new_romans`       |
| Timeline Asset       | `tl_`       | `tl_action`                   |

------------------------------------------------------------------

# Các hệ thống trong projects
- Dotween Pro
- Quick Outline

------------------------------------------------------------------

# Design Partten
- Singleton

------------------------------------------------------------------

# **SoundManager**

### **1. Chuẩn bị**

1. Tạo **SoundData** (ScriptableObject) và thêm danh sách `entries` gồm:
    - `key`: tên định danh âm thanh.
    - `clip`: file `AudioClip`.
    - `type`: loại âm thanh (`Music`, `Effect`, `UI`, `Voice`, `Ambient`).
2. Thêm `SoundManager` vào scene. (sample: SampleFolders/Prefabs/Manager/prefab_manager_sound)
3. Gán các trường serialized trong **Inspector** (Đã gán sẵn trong prefab):
    - **soundData** – dữ liệu âm thanh. (SampleFolders/SO/Sound/so_data_sound)
    - **mixer** – `AudioMixer` chính. (SampleFolders/SO/Sound/_mixer)
    - **musicSource** – `AudioSource` riêng cho nhạc nền.
    - **mixerMusicGroup**, **mixerFXGroup**, **mixerUIGroup**, **mixerVoiceGroup**, **mixerAmbientGroup** – gán đúng
      group tương ứng trong `AudioMixer`.
4. UI group sample: SampleFolders/Prefabs/UI/prefab_ui_setting_sound

* Các loại sound:
    - **Music** - Nhạc nền
    - **Effect** - Sound hiệu ứng
    - **Ambient** - Sound môi trường
    - **Voice** - Sound giọng nói
    - **UI** - Sound UI

------------------------------------------------------------------

### **2. Gọi trong code**

Xem ví dụ tại script: *SoundExample.cs*

| Hành động                    | Cách dùng                                                                      |
|------------------------------|--------------------------------------------------------------------------------|
| Phát âm thanh theo key       | `SoundManager.Instance.Play("button_click");`                                  |
| Phát âm thanh bằng AudioClip | `SoundManager.Instance.Play(myClip);`                                          |
| Dừng âm thanh theo key       | `SoundManager.Instance.Stop("bgm_main");`                                      |
| Dừng âm thanh của AudioClip  | `SoundManager.Instance.Stop(myClip);`                                          |
| Phát nhạc nền                | `SoundManager.Instance.PlayMusic("bgm_main", AudioPlayType.Loop);`             |
| Dừng nhạc nền                | `SoundManager.Instance.StopMusic();`                                           |
| Tạm dừng / Tiếp tục nhạc     | `SoundManager.Instance.PauseMusic();` / `SoundManager.Instance.ResumeMusic();` |
| Phát nhạc ngẫu nhiên         | `SoundManager.Instance.PlayRandomMusic();`                                     |
| Set âm lượng từng loại       | `SoundManager.Instance.SetVolume((float)value, SoundType);`                    |

------------------------------------------------------------------

## **Các trường quan trọng**

| Trường                | Loại              | Chức năng                                    |
|-----------------------|-------------------|----------------------------------------------|
| **soundData**         | `SoundData`       | Danh sách các âm thanh có key, clip và type. |
| **musicSource**       | `AudioSource`     | Dùng riêng cho nhạc nền (Music).             |
| **mixer**             | `AudioMixer`      | Bộ trộn âm thanh, chứa các tham số volume.   |
| **mixerMusicGroup**   | `AudioMixerGroup` | Group cho nhạc nền.                          |
| **mixerFXGroup**      | `AudioMixerGroup` | Group cho hiệu ứng.                          |
| **mixerUIGroup**      | `AudioMixerGroup` | Group cho âm thanh UI.                       |
| **mixerVoiceGroup**   | `AudioMixerGroup` | Group cho giọng nói.                         |
| **mixerAmbientGroup** | `AudioMixerGroup` | Group cho âm thanh môi trường.               |

------------------------------------------------------------------

### **Ghi chú**

- Volume range: `0` → `1f`. (Đã tự động lerp(-80f,0f) trong SoundManager)
- Khi `soundData` không có key, hàm sẽ return ngay mà ko gây lỗi.
- Nếu sound dùng đơn lẻ có thể gọi play bằng AudioClip mà ko cần thêm key vào SoundData

------------------------------------------------------------------

* Hồ sơ bệnh nhân: Patient Recorder Manager
- khởi tạo prefab patient_recorder
- API:
    + Instance
    + Get - PartientRecorder
        + Init
        + SetName
        + SetAge
- Select Option từ list để chọn loại bệnh

------------------------------------------------------------------

* Dialog Manager:
- API:
    + Instance
    + Get
        + Set(name, content)

------------------------------------------------------------------

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

- Hệ thống Control trong prefab đã có thứ tự nên nếu ko đổi luồng thì chỉ cần kéo vào scene, đổi ref SO và active khi cần thiết
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
