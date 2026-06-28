using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Localization.IME;
using ReLogic.OS;
using System;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.UI;
using Terraria.UI.Chat;

namespace NewBeginnings.Common.UI;

#nullable enable

public enum InputType
{
    Text,
    Integer,
    Number
}

/// <summary>
/// Ported from DragonLens: https://github.com/ScalarVector1/DragonLens/blob/master/Content/GUI/FieldEditors/TextField.cs <br/>
/// Cleaned up and modified to not use DragonLens UI.
/// </summary>
internal class UIEditableText(InputType inputType = InputType.Text, string back = "", Action<string>? onEnter = null, bool enterClick = true, bool empty = true, bool panel = true) : UIElement
{
    public readonly InputType InputType = inputType;

    private readonly string _backingString = back;
    private readonly Action<string>? _enterAction = onEnter;
    private readonly bool EnterOnOffClick = enterClick;
    private readonly bool AllowEmptyEnter = empty;
    private readonly bool Panel = panel;

    public string Value { get; private set; } = "";
    public bool Typing { get; private set; }

    private bool _updated;
    private bool _reset;

    // Composition string is handled at the very beginning of the update
    // In order to check if there is a composition string before backspace is typed, we need to check the previous state
    private bool _oldHasCompositionString;

    public void SetTyping()
    {
        Typing = true;
        Main.blockInput = true;
    }

    public void SetNotTyping()
    {
        Typing = false;
        Main.blockInput = false;
    }

    public override void LeftClick(UIMouseEvent evt) => SetTyping();

    public override void RightClick(UIMouseEvent evt)
    {
        SetTyping();
        Value = "";
        _updated = true;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        if (_reset)
        {
            _updated = false;
            _reset = false;
        }

        if (_updated)
            _reset = true;

        if (Main.mouseLeft && !IsMouseHovering)
        {
            if (EnterOnOffClick && Typing)
                OnEnter();

            SetNotTyping();
        }
    }

    public void HandleText()
    {
        if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
            SetNotTyping();
        else if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter))
            OnEnter();

        PlayerInput.WritingText = true;
        Main.instance.HandleIME();

        string newText = Main.GetInputText(Value);

        // GetInputText() handles typing operation, but there is a issue that it doesn't handle backspace correctly when the composition string is not empty.
        // It will delete a character both in the text and the composition string instead of only the one in composition string.
        // We'll fix the issue here to provide a better user experience
        if (_oldHasCompositionString && Main.inputText.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Back))
            newText = Value; // force text not to be changed

        if (InputType == InputType.Integer && (newText == string.Empty || int.TryParse(newText, out int _)))
        {
            if (newText != Value)
            {
                Value = newText;
                _updated = true;
            }
        }
        else if (InputType == InputType.Number && (newText == string.Empty || double.TryParse(newText, out double _)))
        {
            if (newText != Value)
            {
                Value = newText;
                _updated = true;
            }
            else if (newText != Value)
            {
                Value = newText;
                _updated = true;
            }
        }
        else if (InputType == InputType.Text)
            Value = newText;

        while (ChatManager.GetStringSize(FontAssets.MouseText.Value, Value, Vector2.One).X + 8 > GetDimensions().Width && Value.Length > 0)
            Value = Value[..^1];

        _oldHasCompositionString = Platform.Get<IImeService>().CompositionString is { Length: > 0 };
    }

    private void OnEnter()
    {
        if (AllowEmptyEnter || Value != "")
            _enterAction?.Invoke(Value);

        Value = "";
        _updated = true;
        SetNotTyping();

        SoundEngine.PlaySound(SoundID.MenuTick);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        var rect = GetDimensions().ToRectangle();

        if (Panel)
            Utils.DrawInvBG(spriteBatch, rect with { Y = rect.Y - 4 });

        if (Typing)
        {
            HandleText();

            // Draw ime panel, note that if there's no composition string then it won't draw anything
            if (Panel)
                Utils.DrawInvBG(spriteBatch, rect with { Y = rect.Y - 4 }, Color.Yellow * (0.15f + (float)Math.Sin(Main.timeForVisualEffects * 0.08f) * 0.05f));

            Main.instance.DrawWindowsIMEPanel(GetDimensions().Position());
        }

        Vector2 pos = GetDimensions().Position() + Vector2.One * 4;
        Color color = Color.White;

        if (rect.Contains(Main.MouseScreen.ToPoint()))
            color = new Color(180, 180, 180);

        const float Scale = 1f;
        string displayed = Value ?? "";

        if (displayed != string.Empty)
            Utils.DrawBorderString(spriteBatch, displayed, pos, color, Scale);
        else
            Utils.DrawBorderString(spriteBatch, _backingString, pos, Color.Gray.MultiplyRGB(color), Scale);

        // Composition string + cursor drawing below
        if (!Typing)
            return;

        pos.X += FontAssets.MouseText.Value.MeasureString(displayed).X * Scale;
        string compositionString = Platform.Get<IImeService>().CompositionString;

        if (compositionString is { Length: > 0 })
        {
            Utils.DrawBorderString(spriteBatch, compositionString, pos, new Color(255, 240, 20), Scale);
            pos.X += FontAssets.MouseText.Value.MeasureString(compositionString).X * Scale;
        }

        if (Main.GameUpdateCount % 20 < 10)
            Utils.DrawBorderString(spriteBatch, "|", pos, Color.White, Scale);
    }
}