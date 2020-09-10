using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MaxBorg
{
    /// <summary>
    /// The main type for the game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D square;
        Texture2D torpRight;
        Texture2D torpLeft;
        Texture2D torpUp;
        Texture2D torpDown;
        Texture2D ship;
        Texture2D torpBorg;

        Texture2D powerBar;
        Texture2D lsuBar;
        Texture2D fill;
        Texture2D lsuFill;

        Texture2D boom;
        Texture2D pVert;
        Texture2D pHor;

        Rectangle rightRect;
        Rectangle leftRect;
        Rectangle upRect;
        Rectangle downRect;
        Rectangle torpRect;

        KeyboardState oldkb;
        GamePadState oldPad;

        Boolean right;
        Boolean left;
        Boolean up;
        Boolean down;
        Boolean fire;

        SpriteFont font1;
        int lsu;
        int lsuTimer;
        int take;
        int take2;

        int timer;
        int expTimer;

        Random random = new Random();
        Boolean drawShip;
        int time;
        int screenTime;
        int direction;

        Boolean red;

        private static int distUp;
        private static int distDown;
        private static int distLeft;
        private static int distRight;

        Boolean oldFire;
        Boolean removeLsu;

        Boolean drawTorp;

        Boolean explode;
        int X;
        int Y;

        Color pColor;
        Boolean pFire;
        int pTimer;
        int take3;

        SoundEffect borgSound;
        SoundEffect torpSound;
        SoundEffect pSound;

        int rem;
        float oldLeftY;
        float oldLeftX;
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content. Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            IsMouseVisible = true;
            oldkb = Keyboard.GetState();
            oldPad = GamePad.GetState(PlayerIndex.One);

            timer = 0;
            time = 0;
            direction = 0;
            red = false;

            lsuTimer = 0;
            lsu = 100;
            take = 0;
            take3 = 0;

            right = false;
            left = false;
            up = true;
            down = false;
            fire = false;

            drawTorp = false;

            leftRect = new Rectangle(280, 208, 50, 50);
            rightRect = new Rectangle(440, 208, 50, 50);
            upRect = new Rectangle(363, 130, 50, 50);
            downRect = new Rectangle(363, 290, 50, 50);

            torpRect = new Rectangle(0, 0, 10, 10);

            oldFire = false;
            removeLsu = false;

            explode = false;
            expTimer = 0;

            pColor = Color.Black;
            pFire = false;
            pTimer = 0;

            oldLeftY = 0;
            oldLeftX = 0;
            

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of the content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            square = this.Content.Load<Texture2D>("square");
            torpRight = this.Content.Load<Texture2D>("tRight");
            torpLeft = this.Content.Load<Texture2D>("tLeft");
            torpUp = this.Content.Load<Texture2D>("tUp");
            torpDown = this.Content.Load<Texture2D>("tDown");
            ship = this.Content.Load<Texture2D>("badGuyShip");
            torpBorg = this.Content.Load<Texture2D>("tBorg");
            powerBar = this.Content.Load<Texture2D>("Bitmap1");
            lsuBar = this.Content.Load<Texture2D>("Bitmap3");
            fill = this.Content.Load<Texture2D>("Bitmap2");
            lsuFill = this.Content.Load<Texture2D>("Bitmap4");
            boom = this.Content.Load<Texture2D>("explosion");
            pVert = this.Content.Load<Texture2D>("pVert");
            pHor = this.Content.Load<Texture2D>("pHor");


            font1 = this.Content.Load<SpriteFont>("SpriteFont1");

            borgSound = this.Content.Load<SoundEffect>("borg_cut_clean");
            torpSound = this.Content.Load<SoundEffect>("klingon_torpedo_clean");
            pSound = this.Content.Load<SoundEffect>("tng_phaser3_clean");
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();
            GamePadState pad1 = GamePad.GetState(PlayerIndex.One);

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // --- Update logic ---

            timer++;
            lsuTimer++;

            if (lsuTimer == 60) // inc +3 every 1 sec
            {
                lsu += 3;

                if (lsu > 100)
                    lsu = 100;

                lsuTimer = 0;
            }


            if (explode)
                expTimer++;

            if (pFire)
                pTimer++;

            if (expTimer == 60)
            {
                explode = false;
                expTimer = 0;
            }

            // randoms for borg ship
            if (timer == 1)
            {
                time = random.Next(1, 181); //1 to 3s                 // what time appears
                direction = random.Next(4); //0 to 3

                distUp = random.Next(71); //0 to 70 Y
                distDown = random.Next(330, 411); //330 to 410 Y
                distLeft = random.Next(180); //0 to 180 X
                distRight = random.Next(500, 701); //500 to 700 X

                screenTime = random.Next(120, 361); //2s to 6s       // how long on screen
            }

            if (timer == time)
            {
                drawShip = true;
                drawTorp = true;
            }
            else if (timer <= time + screenTime && timer >= time)
            {
                drawShip = true;
            }
            else
            {
                drawShip = false;
            }

            if (timer == 180 + screenTime)
            {
                timer = 0;
            }


            // moves borg torpedo
            if (drawTorp)
            {
                if (direction == 0) //up
                {
                    if (timer == time)
                    {
                        torpRect.X = 380;
                        torpRect.Y = distUp;
                    }
                    torpRect.Y += 4;

                    if (torpRect.Y >= 200)
                        drawTorp = false;
                }
                else if (direction == 1) //down
                {
                    if (timer == time)
                    {
                        torpRect.X = 380;
                        torpRect.Y = distDown;
                    }
                    torpRect.Y -= 4;

                    if (torpRect.Y <= 250)
                        drawTorp = false;
                }
                else if (direction == 2) //left
                {
                    if (timer == time)
                    {
                        torpRect.X = distLeft;
                        torpRect.Y = 200;
                    }
                    torpRect.X += 4;

                    if (torpRect.X >= 350)
                        drawTorp = false;
                }
                else if (direction == 3) //right
                {
                    if (timer == time)
                    {
                        torpRect.X = distRight;
                        torpRect.Y = 200;
                    }
                    torpRect.X -= 4;

                    if (torpRect.X <= 400)
                        drawTorp = false;
                }
            }


            // select gun
            if (pad1.DPad.Up == ButtonState.Pressed && !fire && !explode && !pFire)
            {
                up = true;
                right = false;
                left = false;
                down = false;
            }
            else if (pad1.DPad.Down == ButtonState.Pressed && !fire && !explode && !pFire)
            {
                down = true;
                right = false;
                left = false;
                up = false;
            }
            else if (pad1.DPad.Right == ButtonState.Pressed && !fire && !explode && !pFire)
            {
                right = true;
                left = false;
                up = false;
                down = false;
            }
            else if (pad1.DPad.Left == ButtonState.Pressed && !fire && !explode && !pFire)
            {
                left = true;
                right = false;
                up = false;
                down = false;
            }


            // select explosive (take) and propulsive (take2)
            if (!fire)
            {
                if (pad1.ThumbSticks.Left.Y > 0 && pad1.ThumbSticks.Left.Y > oldLeftY + 0.000001)
                    take++;
                else if (pad1.ThumbSticks.Left.Y < 0 && pad1.ThumbSticks.Left.Y < oldLeftY - 0.000001)
                    take--;

                if (take < 0)
                    take = 0;

                if (take > 9)
                    take = 9;

                if (pad1.ThumbSticks.Left.X > 0 && pad1.ThumbSticks.Left.X > oldLeftX + 0.000001)
                    take2++;
                else if (pad1.ThumbSticks.Left.X < 0 && pad1.ThumbSticks.Left.X < oldLeftX - 0.000001)
                    take2--;

                if (take2 < 1)
                    take2 = 1;

                if (take2 > 9)
                    take2 = 9;
            }
            

            if (pad1.Triggers.Left > 0 && oldPad.Triggers.Left == 0)
                fire = true;
            
            // if not enought lsu
            if (lsu - (take + take2 + take3) < 0 && fire == true && oldFire == false)
            {
                fire = false;
            }

            if (lsu - (take + take2 + take3) < 0)
                red = true;
            else
                red = false;


            // moves torpedo
            if (fire)
            {
                fire = true;
                if (up)
                {
                    upRect.Y -= 5; // moves by 5s

                    if (upRect.Y <= -51)
                    {
                        upRect.Y = 130;
                        fire = false;
                    }
                }
                else if (down)
                {
                    downRect.Y += 5;

                    if (downRect.Y >= 480)
                    {
                        downRect.Y = 290;
                        fire = false;
                    }
                }
                else if (right)
                {
                    rightRect.X += 5;

                    if (rightRect.X >= 800)
                    {
                        rightRect.X = 440;
                        fire = false;
                    }
                }
                else if (left)
                {
                    leftRect.X -= 5;

                    if (leftRect.X <= -50)
                    {
                        leftRect.X = 280;
                        fire = false;
                    }
                }

            }

            // phasor color
            if (pad1.Buttons.X == ButtonState.Pressed && !pFire)
                pColor = Color.Blue;
            else if (pad1.Buttons.Y == ButtonState.Pressed && !pFire)
                pColor = Color.Yellow;
            else if (pad1.Buttons.B == ButtonState.Pressed && !pFire)
                pColor = Color.Red;
            else if (pad1.Buttons.A == ButtonState.Pressed && !pFire)
                pColor = Color.Green;

            if (pad1.Triggers.Right > 0 && oldPad.Triggers.Right == 0)
                pFire = true;

            if (pTimer == 60) // on screen for 1 sec
            {
                pFire = false;
                pTimer = 0;
            }


            // remove lsu when start
            if (fire == true && oldFire == false)
            {
                removeLsu = true;
            }
            

            // update vars
            oldFire = fire;
            oldLeftY = pad1.ThumbSticks.Left.Y;
            oldLeftX = pad1.ThumbSticks.Left.X;
            oldPad = pad1;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(square, new Rectangle(350, 200, 70, 70), Color.Gray);
            spriteBatch.DrawString(font1, lsu.ToString(), new Vector2(375, 220), Color.Blue);
            
            spriteBatch.Draw(square, new Rectangle(380, 269, 15, 60), Color.Red); //down
            spriteBatch.Draw(square, new Rectangle(380, 140, 15, 60), Color.Green); //up
            spriteBatch.Draw(square, new Rectangle(420, 225, 60, 15), Color.Red); //right
            spriteBatch.Draw(square, new Rectangle(290, 225, 60, 15), Color.Red); //left

            int sub = 0;
            if (take == 9)
                sub = -30;
            else if (take == 8)
                sub = -25;
            else if (take == 7)
                sub = -20;
            else if (take == 6)
                sub = -15;
            else if (take == 5)
                sub = -10;
            else if (take == 4)
                sub = -5;
            else if (take == 3)
                sub = 0;
            else if (take == 2)
                sub = 5;
            else if (take == 1)
                sub = 10;
            else if (take == 0)
                sub = 15;

            // based on which gun:
            if (down)
            {
                // gun color
                if (red)
                    spriteBatch.Draw(square, new Rectangle(380, 269, 15, 60), Color.Red); //down
                else
                    spriteBatch.Draw(square, new Rectangle(380, 269, 15, 60), Color.Green); //down

                spriteBatch.Draw(square, new Rectangle(380, 140, 15, 60), Color.Red); //up
                spriteBatch.Draw(square, new Rectangle(420, 225, 60, 15), Color.Red); //right
                spriteBatch.Draw(square, new Rectangle(290, 225, 60, 15), Color.Red); //left

                if (removeLsu)
                {
                    lsu = lsu - (take + take2);
                    removeLsu = false;
                    torpSound.Play();
                }

                // torpedo
                if (fire)
                {
                    spriteBatch.Draw(square, new Rectangle(380, 269, 15, 60), Color.Red); //down

                    Rectangle rect = new Rectangle(downRect.X + sub, downRect.Y, take * 10 + 20, take * 10 + 20); // size based on explosive
                    if (rect.Y <= (110 * take2 / 9 + 290) + 40) // distance based on propulsive
                    {
                        spriteBatch.Draw(torpDown, rect, Color.White); 
                        X = rect.X;
                        Y = rect.Y;
                    }
                    explode = true; // explosion at end
                    
                }

                // explosion
                if (!fire && explode)
                {
                    spriteBatch.Draw(boom, new Rectangle(X - 35, Y - 30, take * 10 + 100, take * 10 + 100), Color.White); // size based on explosive
                }

                // phasor
                if (pFire && pColor != Color.Black) // black if none selected
                {
                    spriteBatch.Draw(pHor, new Rectangle (385, 330, 5, 750), pColor);
                    if (pTimer == 1)
                    {
                        pSound.Play(); 

                        //ABXY controls
                        if (pColor == Color.Green)
                        {
                            rem = (int)(lsu * (25 / 100.0));
                        }
                        else if (pColor == Color.Blue)
                        {
                            rem = (int)(lsu * (75 / 100.0));
                        }
                        else if (pColor == Color.Yellow)
                        {
                            rem = 0;
                        }
                        else if (pColor == Color.Red)
                        {
                            rem = (int)(lsu * (50 / 100.0));
                        }

                        if (rem == 0)
                            lsu = 0;
                        else
                            lsu = lsu - rem;

                    }

                    
                }
            }
            if (up)
            {
                if (red)
                    spriteBatch.Draw(square, new Rectangle(380, 140, 15, 60), Color.Red); //up
                else
                    spriteBatch.Draw(square, new Rectangle(380, 140, 15, 60), Color.Green); //up

                spriteBatch.Draw(square, new Rectangle(380, 269, 15, 60), Color.Red); //down
                spriteBatch.Draw(square, new Rectangle(420, 225, 60, 15), Color.Red); //right
                spriteBatch.Draw(square, new Rectangle(290, 225, 60, 15), Color.Red); //left

                if (removeLsu)
                {
                    lsu = lsu - (take + take2);
                    removeLsu = false;
                    torpSound.Play();
                }

                if (fire)
                {
                    spriteBatch.Draw(square, new Rectangle(380, 140, 15, 60), Color.Red); //up
                    

                    if (take > 4)
                    {
                        Rectangle rect = new Rectangle(upRect.X + sub, upRect.Y - 50, take * 10 + 20, take * 10 + 20);
                        if (rect.Y >= 80 - (80 * take2 / 9))
                        {
                            spriteBatch.Draw(torpUp, rect, Color.White);
                            X = rect.X;
                            Y = rect.Y;
                        }
                        
                    }
                    else
                    {
                        Rectangle rect = new Rectangle(upRect.X + sub, upRect.Y, take * 10 + 20, take * 10 + 20);
                        if (rect.Y >= 130 - (130 * take2 / 9))
                        {
                            spriteBatch.Draw(torpUp, rect, Color.White);
                            X = rect.X;
                            Y = rect.Y;
                        }
                    }

                    explode = true; 

                }

                if (!fire && explode)
                {
                    spriteBatch.Draw(boom, new Rectangle(X - 35, Y - 40, take * 10 + 100, take * 10 + 100), Color.White);
                }

                if (pFire && pColor != Color.Black)
                {
                    spriteBatch.Draw(pHor, new Rectangle(385, -110, 5, 250), pColor);
                    if (pTimer == 1)
                    {
                        pSound.Play();

                        //ABXY
                        if (pColor == Color.Green)
                        {
                            rem = (int)(lsu * (25 / 100.0));
                        }
                        else if (pColor == Color.Blue)
                        {
                            rem = (int)(lsu * (75 / 100.0));
                        }
                        else if (pColor == Color.Yellow)
                        {
                            rem = 0;
                        }
                        else if (pColor == Color.Red)
                        {
                            rem = (int)(lsu * (50 / 100.0));
                        }

                        if (rem == 0)
                            lsu = 0;
                        else
                            lsu = lsu - rem;
                    }
                }
            }
            if (right)
            {
                if (red)
                    spriteBatch.Draw(square, new Rectangle(420, 225, 60, 15), Color.Red); //right
                else
                    spriteBatch.Draw(square, new Rectangle(420, 225, 60, 15), Color.Green); //right

                spriteBatch.Draw(square, new Rectangle(380, 269, 15, 60), Color.Red); //down
                spriteBatch.Draw(square, new Rectangle(380, 140, 15, 60), Color.Red); //up
                spriteBatch.Draw(square, new Rectangle(290, 225, 60, 15), Color.Red); //left

                if (removeLsu)
                {
                    lsu = lsu - (take + take2);
                    removeLsu = false;
                    torpSound.Play();
                }

                if (fire)
                {

                    spriteBatch.Draw(square, new Rectangle(420, 225, 60, 15), Color.Red); //right

                    Rectangle rect = new Rectangle(rightRect.X, rightRect.Y + sub, take * 10 + 20, take * 10 + 20);
                    if (rect.X <= (260 * take2 / 9 + 440) + 50)
                    {
                        spriteBatch.Draw(torpRight, rect, Color.White);
                        X = rect.X;
                        Y = rect.Y;
                    }


                        explode = true;
                }

                
                if (!fire && explode)
                {
                    spriteBatch.Draw(boom, new Rectangle(X - 30, Y - 35, take * 10 + 100, take * 10 + 100), Color.White);
                }

                
                if (pFire && pColor != Color.Black)
                {
                    spriteBatch.Draw(pVert, new Rectangle(480, 230, 755, 5), pColor); 
                    if (pTimer == 1)
                    {
                        pSound.Play();

                        //ABXY
                        if (pColor == Color.Green)
                        {
                            rem = (int)(lsu * (25 / 100.0));
                        }
                        else if (pColor == Color.Blue)
                        {
                            rem = (int)(lsu * (75 / 100.0));
                        }
                        else if (pColor == Color.Yellow)
                        {
                            rem = 0;
                        }
                        else if (pColor == Color.Red)
                        {
                            rem = (int)(lsu * (50 / 100.0));
                        }

                        if (rem == 0)
                            lsu = 0;
                        else
                            lsu = lsu - rem;
                    }

                }
            }
            if (left)
            {
                if (red)
                    spriteBatch.Draw(square, new Rectangle(290, 225, 60, 15), Color.Red); //left
                else
                    spriteBatch.Draw(square, new Rectangle(290, 225, 60, 15), Color.Green); //left

                spriteBatch.Draw(square, new Rectangle(380, 269, 15, 60), Color.Red); //down
                spriteBatch.Draw(square, new Rectangle(380, 140, 15, 60), Color.Red); //up
                spriteBatch.Draw(square, new Rectangle(420, 225, 60, 15), Color.Red); //right

                if (removeLsu)
                {
                    lsu = lsu - (take + take2);
                    removeLsu = false;
                    torpSound.Play();
                }

                if (fire)
                {
                    spriteBatch.Draw(square, new Rectangle(290, 225, 60, 15), Color.Red); //left
                    if (take > 4)
                    {
                        Rectangle rect = new Rectangle(leftRect.X - 50, leftRect.Y + sub, take * 10 + 20, take * 10 + 20);
                        if (rect.X >= 230 - (230 * take2 / 9))
                        {
                            spriteBatch.Draw(torpLeft, rect, Color.White);
                            X = rect.X;
                            Y = rect.Y;
                        }

                    }
                    else
                    {
                        Rectangle rect = new Rectangle(leftRect.X, leftRect.Y + sub, take * 10 + 20, take * 10 + 20);
                        if (rect.X >= 280 - (280 * take2 / 9))
                        {
                            spriteBatch.Draw(torpLeft, rect, Color.White);
                            X = rect.X;
                            Y = rect.Y;
                        }
                    }

                    explode = true;
                }

                if (!fire && explode)
                {
                    spriteBatch.Draw(boom, new Rectangle(X - 40, Y - 35, take * 10 + 100, take * 10 + 100), Color.White);
                }

                if (pFire && pColor != Color.Black)
                {
                    spriteBatch.Draw(pVert, new Rectangle(-110, 230, 400, 5), pColor);
                    if (pTimer == 1)
                    {
                        pSound.Play();

                        //ABXY
                        if (pColor == Color.Green)
                        {
                            rem = (int)(lsu * (25 / 100.0));
                        }
                        else if (pColor == Color.Blue)
                        {
                            rem = (int)(lsu * (75 / 100.0));
                        }
                        else if (pColor == Color.Yellow)
                        {
                            rem = 0;
                        }
                        else if (pColor == Color.Red)
                        {
                            rem = (int)(lsu * (50 / 100.0));
                        }

                        if (rem == 0)
                            lsu = 0;
                        else
                            lsu = lsu - rem;

                    }
                }

            }


            // draw borg ship
            if (drawShip)
            {
                if (direction == 0) //up
                {
                    spriteBatch.Draw(ship, new Rectangle(340, distUp, 100, 70), Color.White);
                }
                else if (direction == 1) //down
                {
                    spriteBatch.Draw(ship, new Rectangle(340, distDown, 100, 70), Color.White);
                }
                else if (direction == 2) //left
                {
                    spriteBatch.Draw(ship, new Rectangle(distLeft, 200, 100, 70), Color.White);
                }
                else if (direction == 3) //right
                {
                    spriteBatch.Draw(ship, new Rectangle(distRight, 200, 100, 70), Color.White);
                }
            }

            // draw borg torpedo
            if (drawTorp)
            {
                spriteBatch.Draw(torpBorg, torpRect, Color.White);
                if (timer == time)
                    borgSound.Play();
            }
            

            // power bars
            spriteBatch.Draw(powerBar, new Rectangle(5, 5, 182, 40), Color.White);
            spriteBatch.DrawString(font1, "Explosive MJ", new Vector2(200, 10), Color.Black);
            spriteBatch.Draw(powerBar, new Rectangle(5, 50, 182, 40), Color.White);
            spriteBatch.DrawString(font1, "Propulsive MJ", new Vector2(200, 55), Color.Black);
            spriteBatch.Draw(fill, new Rectangle(7, 52, 18, 36), Color.White);

            spriteBatch.Draw(lsuBar, new Rectangle(590, 5, 204, 40), Color.White);
            spriteBatch.DrawString(font1, "LSU", new Vector2(550, 10), Color.Black);

            spriteBatch.Draw(lsuFill, new Rectangle(592, 7, lsu * 2, 36), Color.White);

            
            // power bar 1
            switch (take)
            {
                case 1:
                    spriteBatch.Draw(fill, new Rectangle(7, 7, 18, 36), Color.White);
                    break;
                case 2:
                    spriteBatch.Draw(fill, new Rectangle(27, 7, 18, 36), Color.White);
                    goto case 1;
                case 3:
                    spriteBatch.Draw(fill, new Rectangle(47, 7, 18, 36), Color.White);
                    goto case 2;
                case 4:
                    spriteBatch.Draw(fill, new Rectangle(67, 7, 18, 36), Color.White);
                    goto case 3;
                case 5:
                    spriteBatch.Draw(fill, new Rectangle(87, 7, 18, 36), Color.White);
                    goto case 4;
                case 6:
                    spriteBatch.Draw(fill, new Rectangle(107, 7, 18, 36), Color.White);
                    goto case 5;
                case 7:
                    spriteBatch.Draw(fill, new Rectangle(127, 7, 18, 36), Color.White);
                    goto case 6;
                case 8:
                    spriteBatch.Draw(fill, new Rectangle(147, 7, 18, 36), Color.White);
                    goto case 7;
                case 9:
                    spriteBatch.Draw(fill, new Rectangle(167, 7, 18, 36), Color.White);
                    goto case 8;
            }

            // power bar 2
            switch (take2)
            {
                case 1:
                    spriteBatch.Draw(fill, new Rectangle(7, 52, 18, 36), Color.White);
                    break;
                case 2:
                    spriteBatch.Draw(fill, new Rectangle(27, 52, 18, 36), Color.White);
                    goto case 1;
                case 3:
                    spriteBatch.Draw(fill, new Rectangle(47, 52, 18, 36), Color.White);
                    goto case 2;
                case 4:
                    spriteBatch.Draw(fill, new Rectangle(67, 52, 18, 36), Color.White);
                    goto case 3;
                case 5:
                    spriteBatch.Draw(fill, new Rectangle(87, 52, 18, 36), Color.White);
                    goto case 4;
                case 6:
                    spriteBatch.Draw(fill, new Rectangle(107, 52, 18, 36), Color.White);
                    goto case 5;
                case 7:
                    spriteBatch.Draw(fill, new Rectangle(127, 52, 18, 36), Color.White);
                    goto case 6;
                case 8:
                    spriteBatch.Draw(fill, new Rectangle(147, 52, 18, 36), Color.White);
                    goto case 7;
                case 9:
                    spriteBatch.Draw(fill, new Rectangle(167, 52, 18, 36), Color.White);
                    goto case 8;
            }


            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}




