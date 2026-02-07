using System;
using System.Security.Cryptography;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyMonoGame;

public class Game1 : Game {
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    Circle targetCircle;
    Texture2D targetSprite;
    Texture2D crosshairSprite;
    Texture2D backgroundSprite;
    float targetX;
    float targetY;
    MouseState currentMouse;
    MouseState previousMouse;
    bool targetExists;
    double timeCounter;
    int framesCounter;
    Random random;
    int targetsHit;
    String text;

    public Game1() {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = false;
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        targetExists = true;
        timeCounter = 0;
        framesCounter = 0;
        timeCounter = 0;
        targetsHit = 0;
        text = "Shoot the target to begin";
        random = new Random();
        targetCircle = new Circle(new Vector2(595, 120), 45f);
    }

    protected override void Initialize() {
        IsFixedTimeStep = false;
        _graphics.SynchronizeWithVerticalRetrace = false;
        _graphics.ApplyChanges();

        base.Initialize();
    }

    protected override void LoadContent() {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        targetSprite = Content.Load<Texture2D>("target");
        crosshairSprite = Content.Load<Texture2D>("crosshair");
        backgroundSprite = Content.Load<Texture2D>("sky");
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
        timeCounter += gameTime.ElapsedGameTime.TotalSeconds;
        framesCounter++;

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) {
            Exit();
        }

        if (Keyboard.GetState().IsKeyDown(Keys.D)) {
            targetX++;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.A)) {
            targetX--;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.W)) {
            targetY--;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.S)) {
            targetY++;
        }

        currentMouse = Mouse.GetState();

        Vector2 mouseLocation = new Vector2(currentMouse.X, currentMouse.Y);

        bool leftClicked = false;

        if (currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Pressed) {
            leftClicked = true;
        }

        if (leftClicked == true && targetExists == true && isTargetHit(targetCircle, mouseLocation) == true) {
            Console.WriteLine("Target hit!");
            targetCircle.Center.X = random.Next(45, _graphics.PreferredBackBufferWidth-45);
            targetCircle.Center.Y = random.Next(45, _graphics.PreferredBackBufferHeight-45);
            targetsHit++;

            if (targetsHit == 50) {
                text = $"Final time: {timeCounter}";
            }
        }

        if (timeCounter % 3 == 0) {
            Console.WriteLine(mouseLocation.X);
            Console.WriteLine(mouseLocation.Y);
        }

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

        _spriteBatch.End();

        if (timeCounter >= 1) {
            Window.Title = $"FPS: {framesCounter}";
            framesCounter = 0;
            timeCounter = 0;
        }

        base.Draw(gameTime);
    }
}
