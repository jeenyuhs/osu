// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Graphics.Sprites;
using osu.Game.Overlays;
using osu.Game.Overlays.Mods;
using osu.Game.Resources.Localisation.Web;
using osuTK;

namespace osu.Game.Graphics.UserInterface
{
    public class ShearedSearchTextBox : CompositeDrawable, IHasCurrentValue<string>
    {
        private const float corner_radius = 7;

        private readonly Box background;
        private readonly SearchTextBox textBox;

        public Bindable<string> Current
        {
            get => textBox.Current;
            set => textBox.Current = value;
        }

        public bool HoldFocus
        {
            get => textBox.HoldFocus;
            set => textBox.HoldFocus = value;
        }

        public void TakeFocus() => textBox.TakeFocus();

        public void KillFocus() => textBox.KillFocus();

        public ShearedSearchTextBox()
        {
            Height = 42;
            Shear = new Vector2(ShearedOverlayContainer.SHEAR, 0);
            Masking = true;
            CornerRadius = corner_radius;

            InternalChildren = new Drawable[]
            {
                background = new Box
                {
                    RelativeSizeAxes = Axes.Both
                },
                new GridContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Content = new[]
                    {
                        new Drawable[]
                        {
                            textBox = new InnerSearchTextBox
                            {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                RelativeSizeAxes = Axes.Both,
                                Size = Vector2.One
                            },
                            new Container
                            {
                                Name = @"Icon container",
                                RelativeSizeAxes = Axes.Y,
                                Width = 50,
                                Origin = Anchor.CentreRight,
                                Anchor = Anchor.CentreRight,
                                Children = new Drawable[]
                                {
                                    new SpriteIcon
                                    {
                                        Icon = FontAwesome.Solid.Search,
                                        Origin = Anchor.Centre,
                                        Anchor = Anchor.Centre,
                                        Size = new Vector2(16),
                                        Shear = -Shear
                                    }
                                }
                            }
                        }
                    },
                    ColumnDimensions = new[]
                    {
                        new Dimension(),
                        new Dimension(GridSizeMode.AutoSize)
                    }
                }
            };
        }

        [BackgroundDependencyLoader]
        private void load(OverlayColourProvider colourProvider)
        {
            background.Colour = colourProvider.Background3;
        }

        public override bool HandleNonPositionalInput => textBox.HandleNonPositionalInput;

        private class InnerSearchTextBox : SearchTextBox
        {
            [BackgroundDependencyLoader]
            private void load(OverlayColourProvider colourProvider)
            {
                BackgroundFocused = colourProvider.Background4;
                BackgroundUnfocused = colourProvider.Background4;

                Placeholder.Font = OsuFont.GetFont(size: CalculatedTextSize, weight: FontWeight.SemiBold);
                PlaceholderText = CommonStrings.InputSearch;

                CornerRadius = corner_radius;
                TextContainer.Shear = new Vector2(-ShearedOverlayContainer.SHEAR, 0);
            }

            protected override SpriteText CreatePlaceholder() => new SearchPlaceholder();

            internal class SearchPlaceholder : SpriteText
            {
                public override void Show()
                {
                    this
                        .MoveToY(0, 250, Easing.OutQuint)
                        .FadeIn(250, Easing.OutQuint);
                }

                public override void Hide()
                {
                    this
                        .MoveToY(3, 250, Easing.OutQuint)
                        .FadeOut(250, Easing.OutQuint);
                }
            }

            protected override Drawable GetDrawableCharacter(char c) => new FallingDownContainer
            {
                AutoSizeAxes = Axes.Both,
                Child = new OsuSpriteText { Text = c.ToString(), Font = OsuFont.GetFont(size: 20, weight: FontWeight.SemiBold) },
            };
        }
    }
}
