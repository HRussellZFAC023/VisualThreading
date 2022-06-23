﻿using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using VisualThreading.ToolWindows;

namespace VisualThreading
{
    public class BuildingWindow : BaseToolWindow<BuildingWindow>
    {
        public override string GetTitle(int toolWindowId) => "BuildingWindow";

        public override Type PaneType => typeof(Pane);

        public static BuildingWindowControl Instance;

        public override async Task<FrameworkElement> CreateAsync(int toolWindowId, CancellationToken cancellationToken)
        {
            var commands = await Schema.Schema.LoadAsync();
            var buffer = await VS.Documents.GetActiveDocumentViewAsync();
            var fileExt = "";

            // note reading from files should be done async or we will have lots of issues
            var root = Path.GetDirectoryName(typeof(VisualStudioServices).Assembly.Location);
            var blockly = await ReadFileAsync(Path.Combine(root!, "Resources", "html", "blocklyHTML.html"));
            var toolbox = await ReadFileAsync(Path.Combine(root!, "Resources", "xml", "blocklyToolbox.xml"));
            var workspace = await ReadFileAsync(Path.Combine(root!, "Resources", "xml", "blocklyWorkspace.xml"));

            if (buffer?.TextBuffer != null)
            {
                fileExt =
                    Path.GetExtension(buffer.TextBuffer.GetFileName());
            }

            Instance = new BuildingWindowControl(commands, fileExt, blockly, toolbox, workspace);
            return Instance;
        }

        private static async Task<string> ReadFileAsync(string file)
        {
            using var reader = new StreamReader(file);
            var content = await reader.ReadToEndAsync();
            return content;
        }

        [Guid("6a0155f8-b16a-4fba-90bb-8c9fab68de1b")]
        internal class Pane : ToolWindowPane
        {
            public Pane()
            {
                BitmapImageMoniker = KnownMonikers.ToolWindow;
            }
        }
    }
}