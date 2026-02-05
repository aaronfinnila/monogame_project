using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MyMonoGame;

public class Game1 : Game {
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

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

    public Game1() {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = false;
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        targetX = 595;
        targetY = 120;
        targetExists = true;
        timeCounter = 0;
        framesCounter = 0;
        timeCounter = 0;
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

        int mouseX = currentMouse.X;
        int mouseY = currentMouse.Y;

        bool leftClicked = false;

        if (currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Pressed) {
            leftClicked = true;
        }

        if (leftClicked == true && targetExists == true && mouseX > targetX-45 && mouseX <= targetX + 45 && mouseY > targetY-45 && mouseY <= targetY+45) {
            Console.WriteLine("Target hit!");
            targetExists = false;
            Console.WriteLine("mouseX: " + mouseX);
            Console.WriteLine("mouseY: " + mouseY);
        }

        if (targetExists && mouseX > targetX-45 && mouseX <= targetX + 45 && mouseY > targetY-45 && mouseY <= targetY+45) {

        }

        if (timeCounter % 3 == 0) {
            Console.WriteLine(mouseX);
            Console.WriteLine(mouseY);
        }

        previousMouse = currentMouse;
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        _spriteBatch.Draw(backgroundSprite, new Vector2(0, 0), Color.White);
        if (targetExists == true) {
        _spriteBatch.Draw(targetSprite, new Vector2(targetX, targetY), Color.White);
        }
        _spriteBatch.Draw(crosshairSprite, new Vector2(currentMouse.X, currentMouse.Y), Color.White);

        _spriteBatch.End();

        if (timeCounter >= 1) {
            Window.Title = $"FPS: {framesCounter}";
            framesCounter = 0;
            timeCounter = 0;
        }

        base.Draw(gameTime);
    }
}
