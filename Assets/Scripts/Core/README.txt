# 🧠 Score System Refactor — Dependency Injection + SRP

Цей модуль реалізує систему очок з чітким розділенням відповідальностей (SRP) та впровадженням Dependency Injection через VContainer. Основна мета — зробити логіку масштабованою, тестованою та візуально-зрозумілою.

---
## 📦 Структура `/Scripts/Core`
Core/
├── DI/
│ └── GameScope.cs # DI-контейнер, точка входу VContainer.
├── Interfaces/
│ └── ScoreService.cs # Реалізація логіки очок (без MonoBehaviour).
├── Services/
│ └── IScoreService.cs # Інтерфейс, що описує API сервісу очок.
└── UI/
└── ScoreUI # Відображення очок і рекорду на UI, реагує на зміни в ScoreService.
---
## 🔧 Архітектурні зміни
- ✅ `ScoreService` більше не MonoBehaviour, не має UI-логіки, не використовує Singleton.
- ✅ `IScoreService` — інтерфейс, який описує API сервісу очок.
- ✅ `ScoreUI` — слухає `OnScoreChanged` і оновлює `TextMeshProUGUI`.
- ✅ `CoinGenerator` отримує `IScoreService` через DI і передає його в `MyCollectibleScript`.
- ✅ `MyCollectibleScript` більше не використовує `ScoreUI.Instance`, а працює з сервісом напряму.
---
## ⚙️ Реєстрація в `GameScope.cs`
```csharp
builder.Register<ScoreService>(Lifetime.Singleton).As<IScoreService>();
builder.RegisterEntryPoint<ScoreUI>();
builder.RegisterEntryPoint<CoinGenerator>();





