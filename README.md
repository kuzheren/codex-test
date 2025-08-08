# Unity Reinforcement Learning Project (ML-Agents)

Этот репозиторий подготовлен как Unity-проект для обучения и инференса игровых агентов с помощью ML-Agents.

## Что внутри
- `Assets/Scripts/SimpleAgent.cs`: пример агента на ML-Agents
- `Assets/Scripts/SimpleArea.cs`: простая среда для агента
- `ml-agents/configs/simple_ppo.yaml`: базовый конфиг PPO для обучения
- `python/requirements.txt`: зависимости Python для тренировки
- `.gitignore`: типичный список исключений для Unity

## Предварительные требования
- Unity (LTS, рекомендую 2021.3+ или 2022.3+)
- Установить пакет `ML-Agents` в Unity: Package Manager → Add package by name → `com.unity.ml-agents`
- Python 3.9–3.11 для тренировки (локально)

## Быстрый старт
1. Открой проект в Unity (корень репозитория — корень проекта)
2. Установи пакет `ML-Agents` в Unity (см. выше)
3. Создай сцену `Assets/Scenes/Simple.unity` и добавь в неё:
   - Плоскость (Plane) как пол
   - Пустой объект `SimpleArea` с компонентом `SimpleArea`
   - Объект `Agent` (например, Capsule), добавь на него компонент `SimpleAgent`
   - Объект-цель `Target` (например, Sphere) и установи его в поле `Target Transform` у `SimpleAgent`
   - Добавь компонент `Behavior Parameters` на `Agent`:
     - Behavior Name: `SimpleAgent`
     - Space Size: Continuous, Actions: 2
   - (Опционально) `Decision Requester` c частотой 5–10 шагов
4. Сохрани сцену и запусти — агент будет двигаться к цели. Если всё корректно, переходи к обучению.

## Обучение (Python)
1. Локально установи зависимости:
   ```bash
   python -m venv .venv
   source .venv/bin/activate
   pip install -U pip
   pip install -r python/requirements.txt
   ```
2. Запусти обучение:
   ```bash
   mlagents-learn ml-agents/configs/simple_ppo.yaml --run-id simple-ppo
   ```
3. Когда появится приглашение «Start training by pressing the Play button in the Unity Editor», нажми Play в Unity. По завершении смотри результаты в `results/` и TensorBoard.

## Инференс
- Чтобы проверить поведение обученной политики, укажи `Behavior Type = Inference Only` в `Behavior Parameters` и подгрузи `.onnx` модель из результатов обучения.

## Примечания по версиям
- Версии `mlagents` (Python) и `com.unity.ml-agents` (Unity) должны быть совместимы. Если используешь другую версию Unity-пакета, при необходимости поправь версию `mlagents` в `python/requirements.txt`.

## Дальше
- Хочешь сцену и префабы «под ключ»? Я могу сгенерировать базовую сцену и префаб агента/цели, но их удобнее собрать в редакторе. Готов добавить больше сред и агентов, логирование, CI, Docker для тренировки.