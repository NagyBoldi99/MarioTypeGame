

using System.Numerics;

namespace Mario
{
    public partial class Form1 : Form
    {

        bool goLeft, goRight, jumping;

        int jumpSpeed = 10;
        int force = 8;

        int playerSpeed = 5;
        int backgroundSpeed = 20;
        int time = 0;
        DateTime startTime;
        bool isGrounded = false;
        bool jumpPressed = false;

        public Form1()
        {
            InitializeComponent();
            startTime = DateTime.Now;

        }


        private void MainTimerEvent(object sender, EventArgs e)
        {


            TimeSpan elapsed = DateTime.Now - startTime;
            label1.Text = "Time: " + Math.Floor(elapsed.TotalSeconds) + " mp";
            bool canMoveLeft = true;
            bool canMoveRight = true;
            isGrounded = false;
            player.Top += jumpSpeed;
            


            Rectangle playerLeft = new Rectangle(player.Left - 1, player.Top + 5, 1, player.Height - 10);
            Rectangle playerRight = new Rectangle(player.Right, player.Top + 5, 1, player.Height - 10);


            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && ((string)x.Tag == "platform" || (string)x.Tag == "backgroundPlatform"))
                {
                    if (playerRight.IntersectsWith(x.Bounds))
                    {
                        canMoveRight = false;
                    }

                    if (playerLeft.IntersectsWith(x.Bounds))
                    {
                        canMoveLeft = false;
                    }
                }
            }


            if (goLeft && player.Left > 100 && canMoveLeft)
            {
                player.Left -= playerSpeed;
            }
            if (goRight && player.Left + (player.Width + 100) < this.ClientSize.Width && canMoveRight)
            {
                player.Left += playerSpeed;
            }

            if (goLeft && background.Left < 0 && canMoveLeft)
            {
                background.Left += backgroundSpeed;
                MoveGameElements("forward");
            }
            if (goRight && background.Left > -2241 && canMoveRight)
            {
                background.Left -= backgroundSpeed;
                MoveGameElements("back");
            }
            if (jumping == true)
            {
                jumpSpeed = -20;
                force -= 2;
            }
            else
            {
                jumpSpeed = 20;
            }


            if (jumping == true && force < 0)
            {
                jumping = false;
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && ((string)x.Tag == "platform" || (string)x.Tag == "backgroundPlatform"))
                {

                    Rectangle platformRect = x.Bounds;
                    Rectangle platformTop = new Rectangle(platformRect.X, platformRect.Y, platformRect.Width, 2);

                    if (player.Bounds.IntersectsWith(platformTop) && jumpSpeed > 0)
                    {
                        force = 10;
                        player.Top = x.Top - player.Height;
                        jumpSpeed = 0;
                        jumping = false;
                        isGrounded = true;
                    }
                    if (jumpPressed && isGrounded)
                    {
                        jumping = true;
                        force = 10; // vagy amennyi kell
                        jumpPressed = false; // reseteljük
                    }

                    if (player.Bounds.IntersectsWith(x.Bounds) && !player.Bounds.IntersectsWith(platformTop))
                    {
                        // bal oldalról jön a player
                        if (player.Right > x.Left && player.Left < x.Left)
                        {
                            player.Left = x.Left - player.Width;
                        }
                        // jobb oldalról jön
                        else if (player.Left < x.Right && player.Right > x.Right)
                        {
                            player.Left = x.Right;
                        }
                    }

                    x.BringToFront();
                }
            }

            // ütközésvizsgálat oldalirányban
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && ((string)x.Tag == "platform" || (string)x.Tag == "backgroundPlatform"))
                {
                    // csak ha a player szintjén van, hogy ne mindig blokkoljon
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        // jobbra ütközik
                        if (goRight && player.Right > x.Left && player.Left < x.Left)
                        {
                            canMoveRight = false;
                        }

                        // balra ütközik
                        if (goLeft && player.Left < x.Right && player.Right > x.Right)
                        {
                            canMoveLeft = false;
                        }
                    }
                }
            }


            if (player.Bounds.IntersectsWith(pictureBox13.Bounds))
            {
                GameTimer.Stop();
                MessageBox.Show("You won !" + Environment.NewLine + "Your Time : " + Math.Round(elapsed.TotalSeconds,1) + " mp" + Environment.NewLine + "Click OK to play again");
                RestartGame();
            }

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "spike")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        GameTimer.Stop();
                        MessageBox.Show("You Died !" + Environment.NewLine + "Click OK to play again");
                        RestartGame();
                    }
                }
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = true;
            }
            if (e.KeyCode == Keys.Space)
            {
                jumpPressed = true;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (jumping == true)
            {

            }
        }


        private void CloseGame(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void RestartGame()
        {
            Form1 newWindow = new Form1();
            newWindow.Show();
            this.Hide();
        }

        private void MoveGameElements(string direction)
        {
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "platform" || x is PictureBox && (string)x.Tag == "spike" || x is PictureBox && (string)x.Tag == "finishFlag")
                {

                    if (direction == "back")
                    {
                        x.Left -= backgroundSpeed;
                    }
                    if (direction == "forward")
                    {
                        x.Left += backgroundSpeed;
                    }


                }
            }
        }
    }
}
