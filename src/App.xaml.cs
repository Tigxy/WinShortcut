using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace win_short_cut {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        // for more info, see https://stackoverflow.com/a/23730146

        private const string UniqueMutexName = "win-shortcut-mutex-name";
        private const string UniqueEventName = "win-shortcut-background-event";

        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0052", 
            Justification = "Object is needed for lifetime of application, however no read is necessary.")]
        private Mutex? mutex;
        private EventWaitHandle? multiInstanceEventWaitHandle;

        private void Application_Startup(object sender, StartupEventArgs e) {
            mutex = new Mutex(true, UniqueMutexName, out bool isAppNewInstance);
            multiInstanceEventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, UniqueEventName);

            // In case app is a new instance...
            if (isAppNewInstance) {
                // ... spawn a thread that monitors whether another instance is being launched.
                var thread = new Thread(
                    () => {
                        while (multiInstanceEventWaitHandle.WaitOne()) {
                            Current.Dispatcher.BeginInvoke(
                                (Action)(() => ((MainWindow)Current.MainWindow).BringToForeground()));
                        }
                    }) {
                    // Mark thread as background, as otherwise it will prevent app from exiting.
                    IsBackground = true
                };

                thread.Start();
                return;
            }

            // Another instance was already running...
            else {
                // ... notify it of current launch
                multiInstanceEventWaitHandle.Set();
                // and shutdown this instance
                App.Current.Shutdown();
            }
        }
    }
}
