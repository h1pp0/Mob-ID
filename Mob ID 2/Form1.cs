/*
 * 
 * This code is a modified version of Ticko's Mob ID
 * http://www.ffevo.net/user/2753-ticko/
 * All copyrights to this code belong to him
 * It has been modified to work with fface and ffacetools
 * It should only break if fface.dll breaks after an FFXI update
 * 
 * I will not be managing this code anymore and I'm only providing this version
 * so that it doesn't break after updates.
 */


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using FFACETools;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        //Declare Variables
        int selectedApp;
        IntPtr idaddress;
        IntPtr nameaddress;
        public IntPtr ffximain, polhandle;
        public string playername;
        int pID, KBH, TH, target;
        public string name { get; set; }
        public Thread t1 { get; set; }



        ////// New shit //////////
        public FFACETools.FFACE _FFACE { get; private set; }

        //Variables for Settings
        Form2 f = new Form2();
        public int X = 0;
        public int Y = 500;
        public byte AG = 124;
        public byte RG = 20;
        public byte GG = 20;
        public byte BG = 20;
        public bool G = true;
        public byte AT = 255;
        public byte RT = 255;
        public byte GT = 255;
        public byte BT = 255;
        public byte top = 0;
        
        public Form1()
        {
            InitializeComponent();
        }

        //Import Kernal32 for Processes, WindowerHelper for TextOverlay.
        #region imports
        [DllImport("kernel32.dll")]
        static extern IntPtr OpenProcess(int dwDesiredAccess, [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);
        [DllImport("kernel32.dll", SetLastError = true, PreserveSig = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, out int lpNumberOfBytesRead);
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, out int lpNumberOfBytesWritten);
        [DllImport("WindowerHelper.dll")]
        public static extern int CreateConsoleHelper(string name);
        [DllImport("WindowerHelper.dll")]
        public static extern void DeleteConsoleHelper(int helper);
        [DllImport("WindowerHelper.dll")]
        public static extern bool CCHIsNewCommand(int helper);
        [DllImport("WindowerHelper.dll")]
        public static extern short CCHGetArgCount(int helper);
        [DllImport("WindowerHelper.dll")]
        public static extern void CCHGetArg(int helper, short index, byte[] buffer);
        [DllImport("WindowerHelper.dll")]
        public static extern int CCHGetCommandID(int helper);
        [DllImport("WindowerHelper.dll")]
        public static extern void CKHSendString(int helper, string data);
        [DllImport("WindowerHelper.dll")]
        public static extern int CreateKeyboardHelper(string name);
        [DllImport("WindowerHelper.dll")]
        public static extern int DeleteKeyboardHelper(int helper);
        [DllImport("WindowerHelper.dll")]
        public static extern int CreateTextHelper(string name);
        [DllImport("WindowerHelper.dll")]
        public static extern int DeleteTextHelper(int helper);
        [DllImport("WindowerHelper.dll")]
        public static extern int CTHCreateTextObject(int helper, string name);
        [DllImport("WindowerHelper.dll")]
        public static extern int CTHDeleteTextObject(int helper, string name);
        [DllImport("WindowerHelper.dll")]
        public static extern int CTHSetText(int helper, string name, string text);
        [DllImport("WindowerHelper.dll")]
        public static extern int CTHSetVisibility(int helper, string name, Boolean visible);
        [DllImport("WindowerHelper.dll")]
        public static extern int CTHSetFont(int helper, string name, string font, int height);
        [DllImport("WindowerHelper.dll")]
        public static extern int CTHSetColor(int helper, string name, byte a, byte r, byte g, byte b);
        [DllImport("WindowerHelper.dll")]
        public static extern int CTHSetLocation(int helper, string name, float x, float y);
        [DllImport("WindowerHelper.dll")]
        public static extern int CTHSetBGColor(int helper, string name, byte a, byte r, byte g, byte b);
        [DllImport("WindowerHelper.dll")]
        public static extern int CTHSetBGVisibility(int helper, string name, Boolean visibile);
        [DllImport("WindowerHelper.dll")]
        public static extern int CTHFlushCommands(int helper);
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
            //Get pol processes, add them to a dropdown combobox for users to change process.
            Process[] pol = Process.GetProcessesByName("pol");
            for (int i = 0; i < pol.Length; i++)
            {
                comboBox1.Items.Add("[" + pol[i].Id + "]" + pol[i].MainWindowTitle);
            }
            //Incase none, one, or multiple pols are running.
            switch (pol.Length)
            {
                case 0:
                    //0 pol's
                    MessageBox.Show("No instance of Final Fantasy XI found.");
                    Application.Exit();
                    return;
                    break;
                case 1:
                    //1 pol
                    selectedApp = 0;
                    break;
                case 2:
                    //Multiple (2+)
                    selectedApp = comboBox1.SelectedIndex;
                    break;
            }

            //Setting pID variable for Windowerhelper.
            pID = pol[selectedApp].Id;
            //Giving Read, Write, etc access to the process.

            //setting playername for the icon near your clock
            playername = pol[selectedApp].MainWindowTitle;
            notifyIcon1.Text = "Mob ID - " + playername;

            comboBox1.SelectedIndex = selectedApp;

            //Starting the thread that reads the memory values.
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Changes the pol process. Does everything that above does, and a few more things to WindowerHelper
            selectedApp = comboBox1.SelectedIndex;
            Process[] pol = Process.GetProcessesByName("pol");
            pID = pol[selectedApp].Id;
            
            if (pID != 0)
            {
                InitializeFFACE();
            }
            else
            {
                MessageBox.Show("Error getting PID");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //Opens settings box.
            Form2 f = new Form2();
            if (checkBox1.Checked == true)
            {
                f.top = 1;
                this.TopMost = true;
            }
            else
            {
                f.top = 0;
                this.TopMost = false;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            //Resets items in the combo box incase a pol is started after the Mob ID is.
            comboBox1.Items.Clear();
            Process[] pol = Process.GetProcessesByName("pol");
            for (int i = 0; i < pol.Length; i++)
            {
                comboBox1.Items.Add("[" + pol[i].Id + "]" + pol[i].MainWindowTitle);
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            //Opacity Slide Bar
            this.Opacity = trackBar1.Value * 0.01;
            this.numericUpDown1.Value = trackBar1.Value;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (trackBar1.Enabled == false)
            {
                trackBar1.Enabled = true;
                label5.Enabled = true;
                label6.Enabled = true;
                numericUpDown1.Enabled = true;
            }
            else
            {
                trackBar1.Enabled = false;
                label5.Enabled = false;
                label6.Enabled = false;
                numericUpDown1.Enabled = false;
            }

            if (checkBox2.Checked == false)
            {
                this.Opacity = 100;
            }
            else
            {
                this.Opacity = trackBar1.Value * 0.01;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                int anInt;

                anInt = Convert.ToInt32(numericUpDown1.Value);
                this.Opacity = anInt * 0.01;
                trackBar1.Value = anInt;

            }
        }

        private void resetTransparencyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Opacity = 100;
            trackBar1.Value = 100;
            numericUpDown1.Value = 100;
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            //WindowerHelper overlay
            if (checkBox3.Checked == true)
            {
               CTHCreateTextObject(TH, "Mob ID");
               CTHSetVisibility(TH, "Mob ID", true);
               CTHFlushCommands(TH);
            }
            if (checkBox3.Checked == false)
            {
                CTHSetVisibility(TH, "Mob ID", false);
                CTHDeleteTextObject(TH, "MobID");
                CTHFlushCommands(TH);
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            checkBox3.Checked = false;
            if (TH != 0)
            {
                CTHDeleteTextObject(TH, "Mob ID");
                CTHFlushCommands(TH);
            }
            notifyIcon1.Visible = false;
            notifyIcon1.Dispose();
            if (t1 != null)
            {
                t1.Abort();
                while (t1.IsAlive)
                {
                    Application.DoEvents();
                }
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Overlay Settings
            f.ShowDialog();
            X = f.xLocation;
            Y = f.yLocation;
            AG = f.AlphaBG;
            RG = f.RedBG;
            GG = f.GreenBG;
            BG = f.BlueBG;
            G = f.BG;
            AT = f.AlphaText;
            RT = f.RedText;
            GT = f.GreenText;
            BT = f.BlueText; 
        }

        private void InitializeFFACE()
        {
            try
            {
                _FFACE = new FFACETools.FFACE(pID);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Error: Could not attach to process.\n" + ex.Message, "Error");
            }
            finally
            {
                playername = _FFACE.Player.Name;
                notifyIcon1.Text = "Mob ID - " + playername;
                
                checkBox3.Checked = false;

                t1 = new Thread(new ThreadStart(GetTargetData));
                t1.IsBackground = true;
                t1.Start();
            }
        }

        public void GetTargetData()
        {
            // This code will always raise an exception due to the way it was implemented. I have not corrected the implementation just added try/catch
            KBH = CreateKeyboardHelper("WindowerMMFKeyboardHandler_" + pID);
            TH = CreateTextHelper("WindowerMMFTextHandler_" + pID);
            while (true)
            {
                if (label3.InvokeRequired)
                {
                    try
                    {
                        name = _FFACE.Target.Name;
                        target = _FFACE.Target.ID;
                        // Target Name
                        label3.Invoke(new MethodInvoker(delegate { label3.Text = name; }));
                        // Target ID
                        label4.Invoke(new MethodInvoker(delegate { label4.Text = target.ToString("X"); }));
                        CTHSetBGColor(TH, "Mob ID", AG, RG, GG, BG);
                        if (G == true)
                        {
                            CTHSetBGVisibility(TH, "Mob ID", true);
                        }
                        else
                        {
                            CTHSetBGVisibility(TH, "Mob ID", false);
                        }
                        CTHSetFont(TH, "Mob ID", "Comic Sans MS", 10);
                        CTHSetColor(TH, "Mob ID", AT, RT, GT, BT);
                        CTHSetLocation(TH, "Mob ID", X, Y);
                        CTHSetText(TH, "Mob ID", "" + label1.Text + " " + _FFACE.Target.Name + Environment.NewLine + label2.Text + " " + target.ToString("X"));
                        CTHFlushCommands(TH);
                    }
                    catch
                    {
                        MessageBox.Show("Error processing target info!", "Error");
                    }
                }
                Thread.Sleep(100);
            }
        }
    }
}
