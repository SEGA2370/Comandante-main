using UnityEngine;

#if UNITY_ANDROID
using Unity.Notifications.Android;
#endif

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager I;

    private const string ReminderChannelId = "reminder_channel";

    private void Awake()
    {
        // Singleton
        if (I != null && I != this)
        {
            Destroy(gameObject);
            return;
        }
        I = this;
        DontDestroyOnLoad(gameObject);

#if UNITY_ANDROID
        CreateAndroidChannel();
#endif
    }

    private void Start()
    {
#if UNITY_ANDROID
        // Очистить старые уведомления, когда игрок заходит в игру
        AndroidNotificationCenter.CancelAllScheduledNotifications();
        AndroidNotificationCenter.CancelAllDisplayedNotifications();
#endif

        HandleFirstLaunchAfterUpdate();
    }

#if UNITY_ANDROID
    private void CreateAndroidChannel()
    {
        var channel = new AndroidNotificationChannel
        {
            Id = ReminderChannelId,
            Name = "Game Reminders",
            Importance = Importance.Default,
            Description = "Reminders to come back and play"
        };

        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }
#endif

    private void OnApplicationPause(bool pauseStatus)
    {
#if UNITY_ANDROID
        if (pauseStatus)
        {
            // App goes to background → schedule reminders
            ScheduleComeBackReminder();
        }
        else
        {
            // App comes to foreground → cancel reminders
            AndroidNotificationCenter.CancelAllScheduledNotifications();
        }
#endif
    }

    private void OnApplicationQuit()
    {
#if UNITY_ANDROID
        // App is closed → schedule reminders
        ScheduleComeBackReminder();
#endif
    }

#if UNITY_ANDROID
    private void ScheduleComeBackReminder()
    {
        AndroidNotificationCenter.CancelAllScheduledNotifications();

        var notification = new AndroidNotification
        {
            Title = "Come back to Comandante!",
            Text = "Your squad needs you. Jump back into battle!",

            // для теста сделаем 10 секунд
            //FireTime = System.DateTime.Now.AddSeconds(10),
            FireTime = System.DateTime.Now.AddHours(6),

            RepeatInterval = System.TimeSpan.FromHours(6),

            SmallIcon = "notification_icon",   // <-- как в Project Settings
            LargeIcon = "large_icon"
        };

        AndroidNotificationCenter.SendNotification(notification, ReminderChannelId);
    }
#endif


    /// <summary>
    /// Simple "update" detection: first launch after install/update.
    /// </summary>
    private void HandleFirstLaunchAfterUpdate()
    {
        string currentVersion = Application.version;
        string savedVersion = PlayerPrefs.GetString("app_version", "");

        if (savedVersion != currentVersion)
        {
            // First run after install or update
            PlayerPrefs.SetString("app_version", currentVersion);
            PlayerPrefs.Save();

            // TODO: here you show some UI about the update
            Debug.Log("[NotificationManager] First launch after install/update. Show 'What’s new' popup here.");
        }
    }
}
