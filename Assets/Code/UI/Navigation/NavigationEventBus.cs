using System;

namespace UI.Navigation
{
    public class NavigationEventBus
    {
        public Action<TabType> OnTabChanged;
        public TabType CurrentTab { get; private set; } = TabType.Clicker;

        public void SwitchTab(TabType tab)
        {
            CurrentTab = tab;
            OnTabChanged?.Invoke(tab);
        }
    }
}
