#if UNITY_EDITOR
// Заглушка, чтобы код AI Toolkit компилировался,
// даже если самого пакета/класса AIDropdownConfig нет.
namespace Unity.AI.Toolkit.Accounts.Services
{
    internal sealed class AIDropdownConfig
    {
        private static readonly AIDropdownConfig _instance = new AIDropdownConfig();
        internal static AIDropdownConfig instance => _instance;

        // Пакет вызывает RegisterController(new { button = ..., content = ... })
        // Нам не важно что внутри — просто проглатываем вызов.
        internal void RegisterController(object controller) { }
    }
}
#endif
