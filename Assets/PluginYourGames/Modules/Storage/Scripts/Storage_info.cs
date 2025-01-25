using System;
using UnityEngine;

namespace YG
{
    public partial class InfoYG
    {
        public StorageSettings Storage = new StorageSettings();

        [Serializable]
        public partial class StorageSettings
        {
#if RU_YG2
            [Tooltip("Вкл/Выкл локальные сохранения. При включённых облачных сохранениях локальные и облачные синхронизируются! Локальные сохранения могут быть полезны при замещении облачных для конкретной платформы. Для платформы Yandex Games лучше выключить локальные сохранения и использовать облачные.")]
#else
            [Tooltip("On/Off local saves. When cloud saves are enabled, local and cloud saves are synchronized! Local saves can be useful when replacing cloud-based ones for a specific platform. For the Yandex Games platform, it is better to turn off local saves and use cloud ones.")]
#endif
            public bool saveLocal;
#if RU_YG2
            [Tooltip("Вкл/Выкл облачные сохранения (облачные сохранения конкретной платформы). При включении облачных сохранений, локальные сохранения всё равно будут работать (если включены). При использовании метода сохранения «SaveProgress», сохранения будут происходить локально, если таймер не достиг значения «Save Cloud Interval». Если же таймер достиг интервала - то сохранения запишутся в облако. При загрузке сохранений, будут загружены более актуальные данные (из локальных или облачных сохранений).")]
#else
            [Tooltip("On/Off cloud saves (cloud platform saves). When cloud saves are enabled, local saves will still work (if enabled). When using the «SaveProgress» save method, saves will occur locally - if the timer has not reached the «Save Cloud Interval» value. If the timer has reached the interval, then the saves will be recorded in the cloud. When uploading saves, more up-to-date data will be uploaded (from local or cloud saves).")]
#endif
            public bool saveCloud = true;
#if RU_YG2
            [Tooltip("Интервал облачных сохранений (в секундах).\n При использовании метода сохранения «SaveProgress», сохранения будут происходить локально (если вклчены), если таймер не достиг значения (Save Cloud Interval. По умолчанию = 5). Если же таймер достиг интервала, то сохранения запишутся в облако.\n При загрузке сохранений, будут загружены более актуальные данные (из локальных или облачных сохранений).")]
#else
            [Tooltip("The interval of cloud saves (in seconds).\n When using the save method «SaveProgress», saves will occur locally (if enabled) if the timer has not reached the value (Save Cloud Interval. By default = 5). If the timer has reached the interval, then the saves will be recorded in the cloud.\n When uploading saves, more up-to-date data will be uploaded (from local or cloud saves).")]
#endif
            [NestedYG(nameof(saveCloud)), Min(0)]
            public int saveCloudInterval;
#if RU_YG2
            [Tooltip("Flush — определяет очередность отправки данных. При значении «true» данные будут отправлены на сервер немедленно; «false» (значение по умолчанию) — запрос на отправку данных будет поставлен в очередь. (Рекомендуется оставить данный параметр выключенным)")]
#else
            [Tooltip("Flush — determines the order in which data is sent. If the value is «true», the data will be sent to the server immediately; «false» (default value) — the request to send data will be queued. (It is recommended to leave this option disabled)")]
#endif
            [NestedYG(nameof(saveCloud))]
            public bool flush;

#if UNITY_EDITOR && YandexGamesPlatform_yg && !Authorization_yg
#if RU_YG2
            [SerializeField, NestedYG(false, "saveCloud"), LabelYG("Требуется модуль авторизации", "red")]
#else
            [SerializeField, NestedYG(false, "saveCloud"), LabelYG("An authorization module is required", "red")]
#endif
            private bool labelAdvSimLabel;
#endif
        }
    }
}