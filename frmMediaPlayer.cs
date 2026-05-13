using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using AxWMPLib;

namespace HW5_MediaPlayer
{
    public class frmMediaPlayer : Form
    {
        private AxWindowsMediaPlayer wmpVideo;
        private Panel palButton;
        private Button btnBrowser;
        private Button btnPlay;
        private Button btnPause;
        private Button btnStop;
        private Label lblRate;
        private ComboBox cmbRate;
        private CheckBox chkLoop;
        private Timer loopRestartTimer;
        private bool userStopped = true;
        private string currentMediaPath = "";

        public frmMediaPlayer()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "多媒體播放器";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(860, 520);

            wmpVideo = new AxWindowsMediaPlayer();
            ((System.ComponentModel.ISupportInitialize)(wmpVideo)).BeginInit();
            wmpVideo.Dock = DockStyle.Fill;
            wmpVideo.Enabled = true;
            wmpVideo.Name = "wmpVideo";
            wmpVideo.PlayStateChange += wmpVideo_PlayStateChange;
            ((System.ComponentModel.ISupportInitialize)(wmpVideo)).EndInit();

            palButton = new Panel();
            palButton.Name = "palButton";
            palButton.Dock = DockStyle.Bottom;
            palButton.Height = 70;
            palButton.Padding = new Padding(12);

            btnBrowser = new Button();
            btnBrowser.Text = "瀏覽";
            btnBrowser.Location = new Point(20, 18);
            btnBrowser.Size = new Size(85, 30);
            btnBrowser.Click += btnBrowser_Click;

            btnPlay = new Button();
            btnPlay.Text = "播放";
            btnPlay.Location = new Point(120, 18);
            btnPlay.Size = new Size(85, 30);
            btnPlay.Click += btnPlay_Click;

            btnPause = new Button();
            btnPause.Text = "暫停";
            btnPause.Location = new Point(220, 18);
            btnPause.Size = new Size(85, 30);
            btnPause.Click += btnPause_Click;

            btnStop = new Button();
            btnStop.Text = "停止";
            btnStop.Location = new Point(320, 18);
            btnStop.Size = new Size(85, 30);
            btnStop.Click += btnStop_Click;

            lblRate = new Label();
            lblRate.Text = "播放倍速：";
            lblRate.Location = new Point(430, 23);
            lblRate.Size = new Size(80, 20);

            cmbRate = new ComboBox();
            cmbRate.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbRate.Location = new Point(510, 20);
            cmbRate.Size = new Size(90, 24);
            cmbRate.Items.Add("0.5x");
            cmbRate.Items.Add("1.0x");
            cmbRate.Items.Add("1.5x");
            cmbRate.Items.Add("2.0x");
            cmbRate.SelectedIndex = 1;
            cmbRate.SelectedIndexChanged += cmbRate_SelectedIndexChanged;

            chkLoop = new CheckBox();
            chkLoop.Text = "循環播放";
            chkLoop.Location = new Point(625, 22);
            chkLoop.Size = new Size(100, 22);
            chkLoop.CheckedChanged += chkLoop_CheckedChanged;

            loopRestartTimer = new Timer();
            loopRestartTimer.Interval = 250;
            loopRestartTimer.Tick += loopRestartTimer_Tick;

            palButton.Controls.Add(btnBrowser);
            palButton.Controls.Add(btnPlay);
            palButton.Controls.Add(btnPause);
            palButton.Controls.Add(btnStop);
            palButton.Controls.Add(lblRate);
            palButton.Controls.Add(cmbRate);
            palButton.Controls.Add(chkLoop);

            this.Controls.Add(wmpVideo);
            this.Controls.Add(palButton);

            this.Load += frmMediaPlayer_Load;
        }

        private void frmMediaPlayer_Load(object sender, EventArgs e)
        {
            // Windows Media Player 是 ActiveX 控制項，部分屬性要等表單載入後再設定。
            wmpVideo.uiMode = "none";
            wmpVideo.stretchToFit = true;
            wmpVideo.settings.autoStart = false;
            ApplyRate();
            ApplyLoop();

            string defaultFile = Path.Combine(Application.StartupPath, "sample_media", "Dog.wmv");
            if (File.Exists(defaultFile))
            {
                currentMediaPath = defaultFile;
                wmpVideo.URL = currentMediaPath;
                wmpVideo.Ctlcontrols.stop();
            }
        }

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Video files (*.wmv;*.mp4;*.avi)|*.wmv;*.mp4;*.avi|WMV files (*.wmv)|*.wmv|MP4 files (*.mp4)|*.mp4|AVI files (*.avi)|*.avi|All files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    userStopped = true;
                    loopRestartTimer.Stop();
                    currentMediaPath = ofd.FileName;
                    wmpVideo.URL = currentMediaPath;
                    wmpVideo.Ctlcontrols.stop();
                    ApplyRate();
                    ApplyLoop();
                }
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(currentMediaPath) && string.IsNullOrWhiteSpace(wmpVideo.URL))
            {
                MessageBox.Show("請先選擇影片檔。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            userStopped = false;
            loopRestartTimer.Stop();
            ApplyRate();
            ApplyLoop();
            wmpVideo.Ctlcontrols.play();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            loopRestartTimer.Stop();
            wmpVideo.Ctlcontrols.pause();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            userStopped = true;
            loopRestartTimer.Stop();
            wmpVideo.Ctlcontrols.stop();
        }

        private void cmbRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyRate();
        }

        private void chkLoop_CheckedChanged(object sender, EventArgs e)
        {
            ApplyLoop();
        }

        private void ApplyRate()
        {
            if (wmpVideo == null || cmbRate == null) return;

            double rate = 1.0;
            switch (cmbRate.SelectedIndex)
            {
                case 0:
                    rate = 0.5;
                    break;
                case 1:
                    rate = 1.0;
                    break;
                case 2:
                    rate = 1.5;
                    break;
                case 3:
                    rate = 2.0;
                    break;
            }

            try
            {
                wmpVideo.settings.rate = rate;
            }
            catch
            {
                MessageBox.Show("此影片或目前播放器不支援這個倍速。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cmbRate.SelectedIndex = 1;
            }
        }

        private void ApplyLoop()
        {
            if (wmpVideo == null || chkLoop == null) return;

            try
            {
                // 先使用 Windows Media Player 內建 loop 模式。
                wmpVideo.settings.setMode("loop", chkLoop.Checked);
            }
            catch
            {
                // 若某些環境不支援 setMode，下面的 MediaEnded + Timer 會補做循環播放。
            }
        }

        private void wmpVideo_PlayStateChange(object sender, _WMPOCXEvents_PlayStateChangeEvent e)
        {
            // 8 = MediaEnded。影片結束時，如果有勾選循環播放，就延遲一小段時間重播。
            // 用 Timer 重播比直接在 PlayStateChange 裡 play() 更穩，避免 WMP 還停在結束狀態而重播失敗。
            if (e.newState == 8 && chkLoop.Checked && !userStopped)
            {
                loopRestartTimer.Stop();
                loopRestartTimer.Start();
            }
        }

        private void loopRestartTimer_Tick(object sender, EventArgs e)
        {
            loopRestartTimer.Stop();

            if (!chkLoop.Checked || userStopped) return;

            string url = !string.IsNullOrWhiteSpace(currentMediaPath) ? currentMediaPath : wmpVideo.URL;
            if (string.IsNullOrWhiteSpace(url)) return;

            try
            {
                ApplyRate();
                wmpVideo.Ctlcontrols.currentPosition = 0;
                wmpVideo.Ctlcontrols.play();
            }
            catch
            {
                try
                {
                    // 若直接 currentPosition + play 失敗，就重新指定 URL 後再播放。
                    wmpVideo.URL = url;
                    ApplyRate();
                    wmpVideo.Ctlcontrols.play();
                }
                catch
                {
                    MessageBox.Show("循環播放時發生問題，請重新選擇影片檔。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
