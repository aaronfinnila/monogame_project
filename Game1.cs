using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyMonoGame;

public class Game1 : Game {
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    Circle targetCircle;
    SpriteFont font;
    SoundEffect targetHitSound;
    Texture2D targetSprite;
    Texture2D crosshairSprite;
    Texture2D backgroundSprite;
    KeyboardState previousKeyboard;
    MouseState currentMouse;
    MouseState previousMouse;
    bool targetExists;
    bool gameTimerActive;
    bool soundOn;
    double fpsTimer;
    int framesCounter;
    int targetStartingX;
    int targetStartingY;
    double printTimer;
    double gameTimer;
    Random random;
    int targetsHit;
    String text;
    double bestTime;

    public Game1() {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = false;
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        targetExists = true;
        gameTimerActive = false;
        fpsTimer = 0d;
        gameTimer = 0d;
        framesCounter = 0;
        targetStartingX = 595;
        targetStartingY = 230;
        targetsHit = 0;
        soundOn = true;
        text = "Shoot the target to begin\n\n\n\n\n\n\n\n\nPress ESC to toggle fullscreen\n\nPress M to toggle sound";
        bestTime = 0d;
        random = new Random();
        targetCircle = new Circle(new Vector2(targetStartingX, targetStartingY), 45f);
    }

    protected override void Initialize() {
        IsFixedTimeStep = true;
        TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 500.0);
        _graphics.SynchronizeWithVerticalRetrace = false;
        _graphics.ApplyChanges();

        base.Initialize();
    }

    protected override void LoadContent() {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        font = Content.Load<SpriteFont>("galleryFont");
        targetSprite = Content.Load<Texture2D>("target");
        crosshairSprite = Content.Load<Texture2D>("crosshair");
        backgroundSprite = Content.Load<Texture2D>("sky");
        targetHitSound = Content.Load<SoundEffect>("targethit");
    }

    struct Circle(Vector2 Center, float radius) {
        public Vector2 Center = Center;
        public float radius = radius;
    }

    bool isTargetHit(Circle circle, Vector2 point) {
        float distance = Vector2.DistanceSquared(circle.Center, point);
        if (distance < circle.radius*circle.radius) {
            return true;
        } else {
            return false;
        }
    }

    protected override void Update(GameTime gameTime) {
        KeyboardState keyboard = Keyboard.GetState();
        fpsTimer += gameTime.ElapsedGameTime.TotalSeconds;
        printTimer += gameTime.ElapsedGameTime.TotalSeconds;
        if (gameTimerActive == true) {
            gameTimer += gameTime.ElapsedGameTime.TotalSeconds;
        }
        framesCounter++;

        if (keyboard.IsKeyDown(Keys.Escape) && previousKeyboard.IsKeyUp(Keys.Escape)) {
            _graphics.IsFullScreen = !_graphics.IsFullScreen;
            _graphics.ApplyChanges();
        }

        if (keyboard.IsKeyDown(Keys.R) && previousKeyboard.IsKeyUp(Keys.R)) {
            text = "Shoot the target to begin";
            targetExists = true;
            targetCircle.Center.X = targetStartingX;
            targetCircle.Center.Y = targetStartingY;
        }

        if (keyboard.IsKeyDown(Keys.M) && previousKeyboard.IsKeyUp(Keys.M)) {
            soundOn = !soundOn; 
        }

        currentMouse = Mouse.GetState();

        Vector2 mouseLocation = new Vector2(currentMouse.X, currentMouse.Y);

        bool leftClicked = false;

        if (currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Pressed) {
            leftClicked = true;
        }

        if (leftClicked == true && targetExists == true && isTargetHit(targetCircle, mouseLocation) == true) {
            gameTimerActive = true;
            text = "";
            targetCircle.Center.X = random.Next(45, _graphics.PreferredBackBufferWidth-45);
            targetCircle.Center.Y = random.Next(45, _graphics.PreferredBackBufferHeight-45);
            targetsHit++;
            if (soundOn == true) {
            targetHitSound.Play(0.05f, 0f, 0f);
            }

            if (targetsHit >= 30) {
                targetsHit = 0;
                gameTimerActive = false;
                if (gameTimer < bestTime || bestTime == 0d) {
                    bestTime = gameTimer;
                }
                text = $"Final time: {Math.Round(gameTimer, 2)}\n\nBest Time: {Math.Round(bestTime, 2)}\n\nPress R to play again";
                targetExists = false;
                gameTimer = 0d;
            }
        }

        if (printTimer >= 3.0) {
            printTimer -= 3.0;
        }
        
        previousKeyboard = keyboard;
        previousMouse = currentMouse;
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        _spriteBatch.Draw(backgroundSprite, new Vector2(0, 0), Color.White);
        if (targetExists == true) {
        _spriteBatch.Draw(targetSprite, new Vector2(targetCircle.Center.X-45, targetCircle.Center.Y-45), Color.White);
        }
        _spriteBatch.Draw(crosshairSprite, new Vector2(currentMouse.X-25, currentMouse.Y-25), Color.White);

        _spriteBatch.DrawString(font, text, new Vector2(_graphics.PreferredBackBufferWidth/2-215, 40), Color.Black);

        _spriteBatch.End();

        if (fpsTimer >= 1) {
            Window.Title = $"FPS: {framesCounter}";
            framesCounter = 0;
            fpsTimer = 0;
        }

        base.Draw(gameTime);
    }
}
