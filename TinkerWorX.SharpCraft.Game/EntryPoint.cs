﻿using EasyHook;
using TinkerWorX.SharpCraft.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace TinkerWorX.SharpCraft.Game
{
    public class EntryPoint : IEntryPoint
    {
        private PluginManager pluginManager;

        public EntryPoint(RemoteHooking.IContext context, String hackPath, String installPath)
        {
            try
            {
                // Settings passed from the launcher.
                WarcraftIII.HackPath = hackPath;
                WarcraftIII.InstallPath = installPath;

                Settings.Load(Path.Combine(WarcraftIII.HackPath, "settings.xml"));

                if (Settings.Current.IsDebugging)
                {
                    Kernel32.AllocConsole();
                    Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
                }
                Trace.AutoFlush = true;
                Trace.Listeners.Add(new TextWriterTraceListener(Path.Combine(WarcraftIII.HackPath, "debug.log")));
                Trace.WriteLine("-------------------");
                Trace.WriteLine(DateTime.Now);

                this.pluginManager = new PluginManager();
                Trace.WriteLine("Loading plugins . . . ");
                this.pluginManager.LoadPlugins();
                Trace.WriteLine(" - Done!");
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "Low-Level exception!" + Environment.NewLine +
                    exception + Environment.NewLine +
                    "Aborting execution!",
                    this.GetType() + " (Constructor)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Process.GetCurrentProcess().Kill();
            }
        }

        public void Run(RemoteHooking.IContext context, String hackPath, String installPath)
        {
            try
            {
                Trace.WriteLine("Initializing WarcraftIII . . . ");
                WarcraftIII.Initialize();
                Trace.WriteLine(" - Done!");

                Trace.WriteLine("Initializing plugins . . . ");
                this.pluginManager.InitializePlugins();
                Trace.WriteLine(" - Done!");

                RemoteHooking.WakeUpProcess();
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    "Low-Level exception!" + Environment.NewLine +
                    exception + Environment.NewLine +
                    "Aborting execution!",
                    this.GetType() + " (Run)", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Process.GetCurrentProcess().Kill();
            }
        }
    }
}
