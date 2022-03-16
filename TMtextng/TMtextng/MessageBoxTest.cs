using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace TMtextng
{
    public class MessageBoxTest
    {
        public void ShowMessage()
        {
            MessageBox.Show("Hallo, ich bin eine C# MessageBox");
            StartSTATask(() => ShowThreadInfo());
        }

        void ShowThreadInfo()
        {
            try
            {
                Application app = new Application();
                app.Run(new MainWindow());
            }

            catch (Exception c)
            {
                MessageBox.Show(c.ToString());
            }
        }

        public static Task StartSTATask(Action func)
        {
            var tcs = new TaskCompletionSource<object>();
            var thread = new Thread(() =>
            {
                try
                {
                    func();
                    tcs.SetResult(null);
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return tcs.Task;
        }
    }
}
