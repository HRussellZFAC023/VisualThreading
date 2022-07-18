﻿using VisualThreading.ToolWindows;

namespace VisualThreading.Commands
{
    [Command(PackageIds.OpenBuildingWindow)]
    internal sealed class OpenBuildingWindow : BaseCommand<OpenBuildingWindow>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await BuildingWindow.ShowAsync();
        }
    }
}