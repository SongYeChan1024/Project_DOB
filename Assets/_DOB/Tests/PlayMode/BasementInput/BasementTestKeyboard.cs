#if UNITY_EDITOR
using System;
using UnityEngine.InputSystem;

namespace DOB.Tests
{
    /// <summary>창 포커스가 없는 PlayMode 실행에도 가상 키보드를 게임 프레임으로 보낸다.</summary>
    internal sealed class BasementTestKeyboard : IDisposable
    {
        private readonly bool oldRunInBackground;
        private readonly InputSettings.BackgroundBehavior oldBackground;
        private readonly InputSettings.EditorInputBehaviorInPlayMode oldRouting;
        public Keyboard Keyboard { get; }

        public BasementTestKeyboard()
        {
            oldRunInBackground = InputSystem.runInBackground;
            oldBackground = InputSystem.settings.backgroundBehavior;
            oldRouting = InputSystem.settings.editorInputBehaviorInPlayMode;
            InputSystem.runInBackground = true;
            InputSystem.settings.backgroundBehavior = InputSettings.BackgroundBehavior.IgnoreFocus;
            InputSystem.settings.editorInputBehaviorInPlayMode = InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
            Keyboard = InputSystem.AddDevice<Keyboard>();
        }

        public void Dispose()
        {
            if (Keyboard.added) InputSystem.RemoveDevice(Keyboard);
            InputSystem.settings.editorInputBehaviorInPlayMode = oldRouting;
            InputSystem.settings.backgroundBehavior = oldBackground;
            InputSystem.runInBackground = oldRunInBackground;
        }
    }
}
#endif
