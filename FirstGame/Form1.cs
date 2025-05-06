using System;
using System.Media;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace FirstGame
{
    public partial class Form1 : Form
    {
        private string soundFilePath = @"C:\VS_22_C#\FirstGame\shot.wav";
        private List<Button> buttons = new List<Button>();
        private Random rand = new Random();
        private double time = 60.0;
        private double timeBtn = 56.5;
        private double stepTime = 3.5;
        private int index = 0;

        private int killObject = 0;
        private int step = 25;

        private void startGame()
        {
            InitializeButton();
            nextPositionTarget(button3);
            buttons.Add(button3);
            nextPositionTarget(button4);
            buttons.Add(button4);
            nextPositionTarget(button5);
            buttons.Add(button5);
            nextPositionTarget(button6);
            buttons.Add(button6);
            nextPositionTarget(button7);
            buttons.Add(button7);
            nextPositionTarget(button8);
            buttons.Add(button8);
            nextPositionTarget(button9);
            buttons.Add(button9);
            nextPositionTarget(button10);
            buttons.Add(button10);
            button2.Location = new System.Drawing.Point(button1.Location.X + 25, button1.Location.Y);
            label2.Text = time.ToString();
            label4.Text = killObject.ToString();
            timer1.Interval = 1000;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Start();
            time -= 0.5;

            timer2.Interval = 30;
            timer2.Tick += new EventHandler(timer2_Tick);
        }

        public Form1()
        {
            InitializeComponent();
            startGame();
        }

        private void InitializeButton()
        {
            GraphicsPath myPath = new GraphicsPath();
            myPath.AddEllipse(0, 0, button2.Width, button2.Height); 
            button2.Region = new Region(myPath);
            button2.FlatStyle = FlatStyle.Flat;
            button2.FlatAppearance.BorderSize = 0;
        }
        private void nextPositionTarget(Button button)
        {
            int xRand = (rand.Next(50 / step, (450 / step) + 1)) * step;
            int yRand = (rand.Next(50 / step, (450 / step) + 1)) * step;
            var findButtonX = buttons.Where(button => button.Location.X == xRand).ToList();
            var findButtonY = buttons.Where(button => button.Location.Y == yRand).ToList();
            if (findButtonX.Count() != 0 || findButtonY.Count() != 0)
            {
                nextPositionTarget(button);
            }
            else
            {
                button.Location = new Point(xRand, yRand);
            }
        }

        private void kill(Button button)
        {
            button2.Location = new Point(button1.Location.X + 25, button1.Location.Y);
            nextPositionTarget(button);
            ++killObject;
            label4.Text = killObject.ToString();
            timer2.Stop();
        }

        private void soundGame()
        {
            if (!System.IO.File.Exists(soundFilePath))
            {
                MessageBox.Show("Звуковой файл не найден. Проверьте путь.");
                return;
            }
            try
            {
                using (SoundPlayer player = new SoundPlayer(soundFilePath))
                {
                    player.Load();
                    player.Play();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка воспроизведения звука: {ex.Message}");
            }
        }

        private void soundShot()
        {
            if (!System.IO.File.Exists(soundFilePath))
            {
                MessageBox.Show("Звуковой файл не найден. Проверьте путь.");
                return;
            }
            try
            {
                using (SoundPlayer player = new SoundPlayer(soundFilePath))
                {
                    player.Load(); 
                    player.Play(); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка воспроизведения звука: {ex.Message}");
            }
        }

        private void button1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left && button1.Left - step >= 0)
            {
                button1.Location = new Point(button1.Location.X - step, button1.Location.Y);
                if ((button1.Location.X == button2.Location.X - 50) &&
                    (button1.Location.Y == button2.Location.Y))
                {
                    button2.Location = new Point(button1.Location.X + 25, button1.Location.Y);
                }
            }
            else if (e.KeyCode == Keys.Right && button1.Right + step <= this.ClientSize.Width)
            {
                button1.Location = new Point(button1.Location.X + step, button1.Location.Y);
                if (button1.Location == button2.Location)
                {
                    button2.Location = new Point(button1.Location.X + 25, button1.Location.Y);
                }
            }
            else
            if (e.KeyCode == Keys.Space)
            {
                timer2.Start();
                timer2_Tick(sender, e);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (time > 0.0)
            {
                label2.Text = time.ToString();
                if (time == timeBtn)
                {
                    if (index < buttons.Count)
                    {
                        nextPositionTarget(buttons[index++]);
                    }
                    if (index == buttons.Count)
                    {
                        index = 0;
                    }
                    timeBtn -= stepTime;
                }
                if (time % 10 == 0)
                {
                    stepTime -= 0.5;
                }
                time -= 0.5;
            }
            else
            {
                label2.Text = "0";
                timer1.Stop();
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (time != 0 && timer2.Enabled)
            {
                foreach (Button button in buttons)
                {
                    if (button2.Location == button.Location)
                    {
                        kill(button);
                        timer2.Stop();
                    }
                }
                if (button2.Location.Y == 0 && timer2.Enabled)
                {
                    button2.Location = new Point(button1.Location.X + 25, button1.Location.Y);
                    timer2.Stop();
                }
                else
                if (button2.Location.Y != 0 && timer2.Enabled)
                {
                    if (button2.Location.Y == 480)
                    {
                        soundShot();
                    }
                    button2.Location = new Point(button2.Location.X, button2.Location.Y - 5);
                }
            }
        }
    }
}
