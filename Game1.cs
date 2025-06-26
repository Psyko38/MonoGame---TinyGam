using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace game;

public class Game1 : Game
{
    private float _Upscale;
    private float _PlayerSpeed;
    private float _IsMenu;
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _PlayerTexture;
    private Texture2D _PlayerBasicTexture;
    private Texture2D _PlayerDerrierTexture;
    private Texture2D _PlayerProfileTexture;
    private Vector2 _PlayerPos;
    private Vector2 _TreePos;
    private Texture2D _Tree;
    private Texture2D _GrideGrass;
    private Texture2D _GrideSend;
    private Texture2D _GrideWater;
    private Texture2D _GrideBasicTextur;
    private Vector2 _CamXY;
    private List<string[]> Map = [];
    private SpriteFont font;
    private Vector2 _GrideXY;
    private Vector2 _GrideSize;
    private KeyboardState previousKeyboardState;
    private Song _BagrondAudio;
    private Texture2D _Backgrond;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        _GrideXY = new Vector2(0, 0);

        _PlayerPos = new Vector2(
            _graphics.PreferredBackBufferWidth / 2,
            _graphics.PreferredBackBufferHeight / 2
        );

        _TreePos = new Vector2(0, 0);

        font = Content.Load<SpriteFont>("MyFont");

        _Upscale = 3;

        _PlayerSpeed = 5;

        if (File.Exists("map.txt"))
        {
            try
            {
                using StreamReader reader = new("map.txt");
                string text = reader.ReadToEnd();

                string[] lines = text.Split('\n', StringSplitOptions.RemoveEmptyEntries);

                foreach (string line in lines)
                {
                    string[] items = line.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    Map.Add(items);
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        _GrideSize = new Vector2(Map[0].Length, Map.Count);

        _IsMenu = 1;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        // TODO: use this.Content to load your game content here

        _PlayerBasicTexture = Content.Load<Texture2D>("Basic");

        _PlayerDerrierTexture = Content.Load<Texture2D>("Derrier");

        _PlayerProfileTexture = Content.Load<Texture2D>("Profile");

        _PlayerTexture = _PlayerBasicTexture;

        _Tree = Content.Load<Texture2D>("Tree");

        _GrideGrass = Content.Load<Texture2D>("Grass");

        _GrideSend = Content.Load<Texture2D>("Send");

        _GrideWater = Content.Load<Texture2D>("Water");

        _GrideBasicTextur = _GrideGrass;

        _CamXY = new Vector2(
            2 * _GrideBasicTextur.Width * _Upscale * -1,
            2 * _GrideBasicTextur.Height * _Upscale * -1
        );

        _BagrondAudio = Content.Load<Song>("Cherry Coke - Cody O'Quinn - 01 Cherry Coke");

        MediaPlayer.Play(_BagrondAudio);
        MediaPlayer.IsRepeating = true;

        _Backgrond = new Texture2D(GraphicsDevice, 1, 1);
        _Backgrond.SetData([Color.White]);
    }

    protected override void Update(GameTime gameTime)
    {
        if (
            GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            || Keyboard.GetState().IsKeyDown(Keys.Escape)
        )
            Exit();

        // TODO: Add your update logic here

        void SaveGame()
        {
            List<string> lines = new List<string>();
            foreach (var l in Map)
            {
                string line = string.Join(" ", l);
                lines.Add(line);
            }

            File.WriteAllText("map.txt", string.Join("\n", lines));
        }

        void Edit(int block)
        {
            if (Map[(int)_GrideXY.Y][(int)_GrideXY.X] != block.ToString())
            {
                Map[(int)_GrideXY.Y][(int)_GrideXY.X] = block.ToString();
                SaveGame();
            }
        }
        {
            List<string> lines = new List<string>();
            foreach (var l in Map)
            {
                string line = string.Join(" ", l);
                lines.Add(line);
            }

            File.WriteAllText("map.txt", string.Join("\n", lines));
        }

        KeyboardState currentKeyboardState = Keyboard.GetState();

        _GrideXY.X = (int)
            Math.Abs(
                Math.Round(
                    _CamXY.X / (_GrideBasicTextur.Width * _Upscale),
                    MidpointRounding.AwayFromZero
                )
            );

        _GrideXY.Y = (int)
            Math.Abs(
                Math.Round(
                    _CamXY.Y / (_GrideBasicTextur.Height * _Upscale),
                    MidpointRounding.AwayFromZero
                )
            );

        _PlayerTexture = _PlayerBasicTexture;

        if (_IsMenu == 0)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                System.Diagnostics.Debug.WriteLine("test");
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Z))
            {
                if (_GrideXY.Y > 0)
                {
                    _CamXY.Y += _PlayerSpeed;
                    _PlayerTexture = _PlayerDerrierTexture;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                if (_GrideXY.Y < _GrideSize.Y - 1)
                {
                    _CamXY.Y -= _PlayerSpeed;
                    _PlayerTexture = _PlayerBasicTexture;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                if (_GrideXY.X > 0)
                {
                    _CamXY.X += _PlayerSpeed;
                    _PlayerTexture = _PlayerProfileTexture;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                if (_GrideXY.X < _GrideSize.X - 1)
                {
                    _CamXY.X -= _PlayerSpeed;
                    _PlayerTexture = _PlayerProfileTexture;
                }
            }

            if (
                currentKeyboardState.IsKeyDown(Keys.Left)
                && previousKeyboardState.IsKeyUp(Keys.Left)
            )
            {
                if (_Upscale > 0.6)
                {
                    _Upscale -= (float)0.1;
                }
            }

            if (
                currentKeyboardState.IsKeyDown(Keys.Right)
                && previousKeyboardState.IsKeyUp(Keys.Right)
            )
            {
                if (_Upscale < 10)
                {
                    _Upscale += (float)0.1;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                Edit(0);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D2))
            {
                Edit(1);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D3))
            {
                Edit(2);
            }
        }
        else
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                _IsMenu = 0;
            }
        }

        _TreePos = new Vector2(
            _CamXY.X
                + _graphics.PreferredBackBufferWidth / 2
                + (3 * _GrideBasicTextur.Width * _Upscale),
            _CamXY.Y
                + _graphics.PreferredBackBufferHeight / 2
                + (3 * _GrideBasicTextur.Height * _Upscale)
                - _Tree.Height
        );

        previousKeyboardState = currentKeyboardState;
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        _spriteBatch.Begin(
            SpriteSortMode.Deferred,
            BlendState.AlphaBlend,
            SamplerState.PointClamp,
            DepthStencilState.None,
            RasterizerState.CullCounterClockwise
        );

        for (int i = 0; i < Map.Count; i++)
        {
            for (int a = 0; a < Map[i].Length; a++)
            {
                if (Map[i][a] == "0")
                {
                    _GrideBasicTextur = _GrideWater;
                }

                if (Map[i][a] == "1")
                {
                    _GrideBasicTextur = _GrideGrass;
                }

                if (Map[i][a] == "2")
                {
                    _GrideBasicTextur = _GrideSend;
                }
                _spriteBatch.Draw(
                    _GrideBasicTextur,
                    new Vector2(
                        a * _GrideBasicTextur.Width * _Upscale
                            + _CamXY.X
                            + _graphics.PreferredBackBufferWidth / 2,
                        i * _GrideBasicTextur.Height * _Upscale
                            + _CamXY.Y
                            + _graphics.PreferredBackBufferHeight / 2
                    ),
                    null,
                    Color.White,
                    0f,
                    new Vector2(_GrideBasicTextur.Width / 2f, _GrideBasicTextur.Height / 2f),
                    _Upscale,
                    SpriteEffects.None,
                    0f
                );
            }
        }

        if (_PlayerPos.Y + _PlayerTexture.Height / (_Upscale / 2) < _TreePos.Y + _Tree.Height)
        {
            _spriteBatch.Draw(
                _PlayerTexture,
                _PlayerPos,
                null,
                Color.White,
                0f,
                new Vector2(_PlayerTexture.Width / 2, _PlayerTexture.Height / 2),
                _Upscale,
                SpriteEffects.None,
                0f
            );
            _spriteBatch.Draw(
                _Tree,
                _TreePos,
                null,
                Color.White,
                0f,
                new Vector2(_Tree.Width / 2, _Tree.Height / 2),
                _Upscale + 1,
                SpriteEffects.None,
                0f
            );
        }
        else
        {
            _spriteBatch.Draw(
                _Tree,
                _TreePos,
                null,
                Color.White,
                0f,
                new Vector2(_Tree.Width / 2, _Tree.Height / 2),
                _Upscale + 1,
                SpriteEffects.None,
                0f
            );
            _spriteBatch.Draw(
                _PlayerTexture,
                _PlayerPos,
                null,
                Color.White,
                0f,
                new Vector2(_PlayerTexture.Width / 2, _PlayerTexture.Height / 2),
                _Upscale,
                SpriteEffects.None,
                0f
            );
        }

        if (_IsMenu == 0)
        {
            _spriteBatch.DrawString(
                font,
                $"Cam - X: {_CamXY.X}, Y: {_CamXY.Y}",
                new Vector2(10, 10), //ok
                Color.White
            );

            _spriteBatch.DrawString(
                font,
                $"Block - X: {_GrideXY.X}, Y: {_GrideXY.Y}",
                new Vector2(10, 50), //ok
                Color.White
            );

            _spriteBatch.DrawString(
                font,
                $"Map Scale - X: {_graphics.PreferredBackBufferWidth / 2}, Y: {_graphics.PreferredBackBufferHeight / 2}",
                new Vector2(10, 100), //ok
                Color.White
            );

            _spriteBatch.DrawString(
                font,
                $"Tree Pos - X: {_TreePos.X}, Y: {_TreePos.Y}",
                new Vector2(10, 150), //ok
                Color.White
            );

            _spriteBatch.DrawString(
                font,
                $"Gride Size - X: {_GrideSize.X}, Y: {_GrideSize.Y}",
                new Vector2(10, 200), //ok
                Color.White
            );

            _spriteBatch.DrawString(
                font,
                $"Upscale - {_Upscale}",
                new Vector2(10, 250), //ok
                Color.White
            );
        }

        if (_IsMenu == 1)
        {
            _spriteBatch.End();
            _spriteBatch.Begin();

            _spriteBatch.Draw(
                _Backgrond,
                new Rectangle(
                    0,
                    0,
                    _graphics.PreferredBackBufferWidth,
                    _graphics.PreferredBackBufferHeight
                ),
                Color.Black * 0.9f
            );

            string text = "Editor Game";
            Vector2 textSize = font.MeasureString(text);

            _spriteBatch.DrawString(
                font,
                $"{text}",
                new Vector2(
                    (_graphics.PreferredBackBufferWidth - textSize.X * _Upscale) / 2,
                    (_graphics.PreferredBackBufferHeight - textSize.Y * _Upscale) / 2
                ), //ok
                Color.White,
                0f,
                Vector2.Zero,
                _Upscale,
                SpriteEffects.None,
                0f
            );

            text = "Press Enter to Play";
            textSize = font.MeasureString(text);

            _spriteBatch.DrawString(
                font,
                $"{text}",
                new Vector2(
                    (_graphics.PreferredBackBufferWidth - textSize.X * (_Upscale - 1)) / 2,
                    (_graphics.PreferredBackBufferHeight - textSize.Y * _Upscale - 1) / (int)1.1
                ), //ok
                Color.White,
                0f,
                Vector2.Zero,
                _Upscale - 1,
                SpriteEffects.None,
                0f
            );
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
