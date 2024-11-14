using System;
using System.Drawing;
using System.Windows.Forms;

namespace _2024._11._14.__Dolgozat
{
    public partial class Form1 : Form
    {
        Timer manMoveTimer = new Timer();
        Timer AppleDownTimer = new Timer();
        Timer KickTimer = new Timer();
        int ApplePCS = 0;
        int ManMove = 0;
        int BasketSize = 20;
        int kicks = 10;              //  hányszor üti meg a fát
        bool left = true;
        bool kopp = false;
        int utes = 0;
        bool AppleInTheHand = true;
        int biggerBasket = 10;
        int fasterKick = 30;
        public Form1()
        {
            InitializeComponent();
            tree1.BackColor = Color.Brown;
            tree2.BackColor = Color.Green;
            basket1.BackColor = Color.Brown;
            basket2.BackColor = Color.Brown;
            Apple.BackColor = Color.Red;
            manBody.BackColor = Color.Salmon;
            manHand.BackColor = Color.Salmon;
            label1.Text = $"Gyűjtött almák száma: {ApplePCS}";
            label2.Text = $"Kosár kapacitása: {BasketSize}";
            label3.Text = "Nagyobb kosár: +5 alma";
            label4.Text = "Gyorsabb szedés: -1 ütés";
            button1.Text = $"{biggerBasket} alma";
            button2.Text = $"{fasterKick} alma";
            Apple.Visible = false;
            this.Text = "Alma szedése";
            Start();
        }
        void Start()
        {
            StartTimer();
            ButtonEvents();
        }
        void StartTimer()
        {
            manMoveTimer.Interval = 30;
            AppleDownTimer.Interval = 15;
            KickTimer.Interval = 300;

            manMoveTimer.Start();

            manMoveTimer.Tick += ManMoveEvent;
            KickTimer.Tick += KickEvent;
        }
        void ButtonEvents()
        {
            button1.Click += (s, e) =>
            {
                if (ApplePCS >= biggerBasket)
                {
                    ApplePCS -= biggerBasket;
                    BasketSize += 5;
                    biggerBasket += 2;
                    PointDraw();
                }
            };
            button2.Click += (s, e) =>
            {
                if (ApplePCS >= fasterKick && kicks > 3)
                {
                    ApplePCS -= fasterKick;
                    kicks -= 1;
                    fasterKick += 30;
                    PointDraw();
                }
            };
        }
        void ManMoveEvent(object s, EventArgs e)
        {
            if (left)
            {
                ManMove = -3;
                AppleInTheHand = true;
                manHand.Left = manBody.Right - manBody.Width - manHand.Width;
                if (manBody.Left > tree1.Right + manHand.Width)
                {
                    manBody.Left += ManMove;
                    manHand.Left = manHand.Left + ManMove;
                    left = true;
                }
                else
                {
                    manMoveTimer.Stop();
                    KickTimer.Start();
                    left = false;
                }
            }
            else
            {
                ManMove = 3;
                manHand.Left = manBody.Right;
                Apple.Left = manBody.Right + manHand.Width / 2;
                Apple.Visible = true;
                if (manBody.Right < basket1.Left - manHand.Width / 2)
                {
                    manBody.Left = manBody.Left + ManMove;
                    manHand.Left = manHand.Left + ManMove;
                    Apple.Left = Apple.Left + ManMove;
                    left = false;
                }
                else
                {
                    manMoveTimer.Stop();
                    AppleInTheHand = false;
                    Apple.Visible = false;
                    AppleDownTimer.Start();
                    left = true;
                    if (BasketSize > ApplePCS)
                    {
                        ApplePCS += 1;          // ennyivel nő az alma a kosárban
                        PointDraw();
                    }
                }
            }
        }
        void KickEvent(object s, EventArgs e)
        {
            if (kicks == utes)
            {
                KickTimer.Stop();
                utes = 0;
                AppleEvent();
            }
            else
            {
                if (kopp)
                {
                    manHand.Left = manHand.Left - manHand.Width;
                    kopp = false;
                    utes += 1;
                }
                else
                {
                    manHand.Left = manHand.Left + manHand.Width;
                    kopp = true;
                }
            }
        }
        void AppleEvent()
        {
            PictureBox NewApple = new PictureBox();
            this.Controls.Add(NewApple);
            NewApple.Size = new Size(17, 17);
            NewApple.Left = 196;
            NewApple.Top = 221;
            NewApple.BackColor = Color.Red;

            AppleDownTimer.Start();
            AppleDownTimer.Tick += (s, e) =>
            {
                if (AppleInTheHand)
                {
                    if (NewApple.Bottom >= manHand.Top)
                    {
                        AppleDownTimer.Stop();
                        NewApple.Visible = false;
                        manMoveTimer.Start();
                    }
                    else
                    {
                        NewApple.Top += 3;
                    }
                }
                else
                {
                    NewApple.Left = Apple.Left;
                    NewApple.Visible = true;
                    if (NewApple.Bottom >= basket1.Top)
                    {
                        AppleDownTimer.Stop();
                        NewApple.Top = 221;
                        this.Controls.Remove(NewApple);
                        manMoveTimer.Start();
                    }
                    else
                    {
                        NewApple.Top += 3;
                    }
                }
            };
        }
        void PointDraw()
        {
            label1.Text = $"Gyűjtött almák száma: {ApplePCS}";
            label2.Text = $"Kosár kapacitása: {BasketSize}";
            button1.Text = $"{biggerBasket} alma";
            button2.Text = $"{fasterKick} alma";
        }
    }
}
