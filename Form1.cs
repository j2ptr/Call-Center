using System;
using System.Windows.Forms;
using System.Net;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Threading;

namespace CallCenter
{
    public partial class Form1 : Form
    {
        KeyboardHook hook = new KeyboardHook();


        public Form1()
        {
            InitializeComponent();
            hook.KeyPressed +=
                new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);
                hook.RegisterHotKey(ModifierKey.Alt, Keys.L);
        }

        void hook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            Thread.Sleep(200);
            SendKeys.Send("^c");
            Thread.Sleep(100);
            if (System.Windows.Clipboard.ContainsText())
            {
                var text = System.Windows.Clipboard.GetText();
                textBox1.Text = text;
            }
            BringToFront();
            Activate();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void N0_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == null)
            {
                textBox1.Text = "0";
            }
            else
            {
                textBox1.Text = textBox1.Text + "0";
            }
        }

        private void N1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == null)
            {
                textBox1.Text = "1";
            }
            else
            {
                textBox1.Text = textBox1.Text + "1";
            }
        }

        private void N2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == null)
            {
                textBox1.Text = "2";
            }
            else
            {
                textBox1.Text = textBox1.Text + "2";
            }
        }

        private void N3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == null)
            {
                textBox1.Text = "3";
            }
            else
            {
                textBox1.Text = textBox1.Text + "3";
            }
        }

        private void N4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == null)
            {
                textBox1.Text = "4";
            }
            else
            {
                textBox1.Text = textBox1.Text + "4";
            }
        }

        private void N5_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == null)
            {
                textBox1.Text = "5";
            }
            else
            {
                textBox1.Text = textBox1.Text + "5";
            }
        }

        private void N6_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == null)
            {
                textBox1.Text = "6";
            }
            else
            {
                textBox1.Text = textBox1.Text + "6";
            }
        }

        private void N7_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == null)
            {
                textBox1.Text = "7";
            }
            else
            {
                textBox1.Text = textBox1.Text + "7";
            }
        }

        private void N8_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == null)
            {
                textBox1.Text = "8";
            }
            else
            {
                textBox1.Text = textBox1.Text + "8";
            }
        }

        private void N9_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == null)
            {
                textBox1.Text = "9";
            }
            else
            {
                textBox1.Text = textBox1.Text + "9";
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == null)
            {
                textBox1.Text = "*";
            }
            else
            {
                textBox1.Text = textBox1.Text + "*";
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == null)
            {
                textBox1.Text = "#";
            }
            else
            {
                textBox1.Text = textBox1.Text + "#";
            }
        }

        private void Button_call_Click(object sender, EventArgs e)
        {
            using (var client = new WebClient())
            {
                string oldTextBox = textBox1.Text;
                string responseString = client.DownloadString("http://" + Properties.Settings.Default.Server + "/gui/freeswitch/originate?__auth_user=" + Properties.Settings.Default.Username + "&__auth_pass=" + Properties.Settings.Default.Password + "&destination=" + getNum());
            }
        }

        private void Button_settings_Click(object sender, EventArgs e)
        {
            Form2 settingsForm = new Form2();
            settingsForm.ShowDialog();
        }

        private string getNum()
        {
            string num = Regex.Replace(textBox1.Text, "[^.0-9]", "");
            return num;
        }
    }

    public sealed class KeyboardHook : IDisposable
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        private class Window : NativeWindow, IDisposable
        {
            private static int WM_HOTKEY = 0x0312;
            public Window()
            {
                this.CreateHandle(new CreateParams());
            }

            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);
                if (m.Msg == WM_HOTKEY)
                {
                    Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                    ModifierKey modifier = (ModifierKey)((int)m.LParam & 0xFFFF);
                    if (KeyPressed != null)
                        KeyPressed(this, new KeyPressedEventArgs(modifier, key));
                }
            }

            public event EventHandler<KeyPressedEventArgs> KeyPressed;
            #region IDisposable Members
            public void Dispose()
            {
                this.DestroyHandle();
            }
            #endregion
        }

        private Window _window = new Window();
        private int _currentId;

        public KeyboardHook()
        {
            _window.KeyPressed += delegate (object sender, KeyPressedEventArgs args)
            {
                if (KeyPressed != null)
                    KeyPressed(this, args);
            };
        }

        public void RegisterHotKey(ModifierKey modifier, Keys key)
        {
            _currentId = _currentId + 1;
            if (!RegisterHotKey(_window.Handle, _currentId, (uint)modifier, (uint)key))
                throw new InvalidOperationException("Couldn’t register the hot key.");
        }

        public event EventHandler<KeyPressedEventArgs> KeyPressed;

        #region IDisposable Members

        public void Dispose()
        {
            for (int i = _currentId; i > 0; i--)
            {
                UnregisterHotKey(_window.Handle, i);
            }
            _window.Dispose();
        }

        #endregion
    }

    public class KeyPressedEventArgs : EventArgs
    {
        private ModifierKey _modifier;
        private Keys _key;

        internal KeyPressedEventArgs(ModifierKey modifier, Keys key)
        {
            _modifier = modifier;
            _key = key;
        }

        public ModifierKey Modifier
        {
            get { return _modifier; }
        }

        public Keys Key
        {
            get { return _key; }
        }
    }

    [Flags]
    public enum ModifierKey : uint
    {
        Alt = 1,
        Control = 2,
        Shift = 4,
        Win = 8
    }
}
