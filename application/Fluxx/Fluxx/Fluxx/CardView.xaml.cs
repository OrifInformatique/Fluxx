using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Fluxx.Card;

namespace Fluxx
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CardView : ContentView
    {
        double leftTitleMarginLeft = 1;
        double leftTitleMarginTop = 1;
        public static readonly Color colorGoal = Color.FromHex("#D03080");
        public static readonly Color colorKeeper = Color.FromHex("#B0C010");
        public static readonly Color colorNewRule = Color.FromHex("#E0B000");
        public static readonly Color colorAction = Color.FromHex("#30A0D0");
        double CardHeight;
        public CardView(Card card, double cardHeight)
        {
            InitializeComponent();
            CardHeight = cardHeight;
            ChargeCard(card);

        }

        private void CardLayoutChangeSize(object sender, EventArgs e)
        {
            LeftTitleFrame.HeightRequest = CardLayout.Height * 490 / 610;
            LeftTitleFrame.WidthRequest = CardLayout.Height * 490 / 610;
            LeftTitleFrame.Rotation = -90;
            LeftTitleFrame.Padding = 0;
            LeftTitleFrame.Margin = new Thickness(CardLayout.Width * leftTitleMarginLeft, CardLayout.Height * leftTitleMarginTop, 0, 0);

            CardFrame.CornerRadius = Convert.ToInt32(40 * CardLayout.Height / 1100);
            FrameColorType.CornerRadius = Convert.ToInt32(24 * CardLayout.Height / 1100);
            FrameContent.CornerRadius = Convert.ToInt32(12 * CardLayout.Height / 1100);
        }
        private void ChargeCard(Card card)
        {
            /*
             ImageIconType
            - FrameColorType
            - LabelTitleType
            - LabelLeftTitle
             
                (new) 
             LabelTitle
             LabelDescription
             LabelSubTitle
             Image
             */
            double GeneralFontSize = CardHeight / 88;
            double TitleFontSize = GeneralFontSize * 4.8;
            double SubtitleFontSize = GeneralFontSize * 3;
            double DescriptionFontSize = GeneralFontSize * 2.7;
            Thickness TitleMargin = new Thickness(0,CardHeight/80,0, CardHeight / 80);




            LabelLeftTitle.Text = card.Title.ToUpper();
            LabelLeftTitle.FontFamily = "font_left_title.ttf#font_left_title";
            leftTitleMarginTop = 130D / 940D;


            if (card.Title.Count() > 25)
            {
                leftTitleMarginLeft = 60D / 652D;
                LabelLeftTitle.FontSize = GeneralFontSize * 4;
            }
            else if (card.Title.Count() > 20)
            {
                leftTitleMarginLeft = 55D / 652D;
                LabelLeftTitle.FontSize = GeneralFontSize * 4.6;
            }
            else
            {
                leftTitleMarginLeft = 50D / 652D;
                LabelLeftTitle.FontSize = GeneralFontSize * 5.2;
            }

            LabelTitleType.FontFamily = "font_type.ttf#font_type";
            LabelTitleType.FontSize = GeneralFontSize * 7;

            switch (card.GetType())
            {
                case TypeOfCard.Action:
                    string[] words = card.Description.Split('\n');
                    string testText = "";
                    foreach (string word in words)
                    {
                        testText += word + Environment.NewLine + Environment.NewLine;
                    }
                    LabelTitleType.Text = "ACTION";
                    FrameColorType.BackgroundColor = colorAction;
                    ImageIconType.Source = ImageSource.FromFile("icon_action.png");
                    leftTitleMarginTop = 165D / 940D;

                    GridContent.Children.Add(new StackLayout
                    {
                        Spacing = 0,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        Children =
                    {
                        new Label()
                        {
                            Text = card.Title,
                            TextColor = Color.Black,
                            Margin = TitleMargin,
                            FontSize =  TitleFontSize,
                            FontFamily = "font_title.ttf#font_title",
                        },
                        new Label()
                        {
                            Text = testText,
                            TextColor = Color.Black,
                            FontSize = DescriptionFontSize,
                            FontFamily = "font_description.otf#font_description"
                        }
                    }
                    }, 1, 3);
                    break;
                case TypeOfCard.NewRule:
                    LabelTitleType.Text = "NOUVELLE RÈGLE";
                    LabelTitleType.FontSize = GeneralFontSize * 4.6;
                    FrameColorType.BackgroundColor = colorNewRule;
                    ImageIconType.Source = ImageSource.FromFile("icon_new_rule.png");

                    GridContent.Children.Add(new StackLayout
                    {
                        Spacing = 0,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        Children =
                    {
                        new Image()
                        {
                            Source = card.GetImage(),
                            HorizontalOptions = LayoutOptions.Fill,
                            VerticalOptions = LayoutOptions.CenterAndExpand
                        },
                        new Label()
                        {
                            Text = card.Title,
                            TextColor = Color.Black,
                            Margin = TitleMargin,
                            FontSize =  TitleFontSize,
                            VerticalOptions = LayoutOptions.End,
                            FontFamily = "font_title.ttf#font_title",

                        },
                        new Label()
                        {
                            Text = card.SubTitle,
                            TextColor = Color.Black,
                            FontSize = SubtitleFontSize,
                            VerticalOptions = LayoutOptions.End,
                            FontFamily = "font_title.ttf#font_title"
                        },
                        new Label()
                        {
                            Text = card.Description,
                            TextColor = Color.Black,
                            FontSize = DescriptionFontSize,
                            VerticalOptions = LayoutOptions.End,
                            FontFamily = "font_description.otf#font_description"
                        }
                    }
                    }, 1, 3);
                    break;
                case TypeOfCard.Keeper:
                    LabelTitleType.Text = "ATOUT";
                    FrameColorType.BackgroundColor = colorKeeper;
                    ImageIconType.Source = ImageSource.FromFile("icon_keeper.png");

                    GridContent.Children.Add(new StackLayout
                    {
                        Spacing = 0,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        Children =
                    {
                        new Image()
                        {
                            Source = card.GetImage(),
                            HorizontalOptions = LayoutOptions.Fill,
                            VerticalOptions = LayoutOptions.CenterAndExpand
                        },
                        new Label()
                        {
                            Text = card.Title,
                            TextColor = Color.Black,
                            Margin = TitleMargin,
                            FontSize =  TitleFontSize,
                            VerticalOptions = LayoutOptions.End,
                            FontFamily = "font_title.ttf#font_title",

                        }
                    }
                    }, 1, 3);
                    break;
                case TypeOfCard.Goal:
                    LabelTitleType.Text = "OBJECTIF";
                    FrameColorType.BackgroundColor = colorGoal;
                    ImageIconType.Source = ImageSource.FromFile("icon_goal.png");
                    if(card.Description != null && card.Description.Length > 100)
                    {
                        GridContent.Children.Add(new StackLayout
                        {
                            Spacing = 0,
                            VerticalOptions = LayoutOptions.FillAndExpand,
                            Children =
                            {
                                new Image()
                                {
                                    Source = card.GetImage(),
                                    HorizontalOptions = LayoutOptions.Fill,
                                    VerticalOptions = LayoutOptions.CenterAndExpand
                                },
                                new Label()
                                {
                                    Text = card.Title,
                                    TextColor = Color.Black,
                                    Margin = TitleMargin,
                                    FontSize =  TitleFontSize,
                                    VerticalOptions = LayoutOptions.End,
                                    FontFamily = "font_title.ttf#font_title",

                                },
                                new Label()
                                {
                                    Text = card.Description,
                                    TextColor = Color.Black,
                                    FontSize = DescriptionFontSize,
                                    VerticalOptions = LayoutOptions.End,
                                    FontFamily = "font_description.otf#font_description"
                                }
                            }
                        }, 1, 3);
                    }
                    else
                    {
                        Label secondLabel = new Label();
                        if (card.Keepers1 != null && card.Keepers2 != null)
                        {
                            secondLabel.Text = card.Keepers1 + " + " + card.Keepers2;
                            secondLabel.TextColor = Color.Black;
                            secondLabel.FontSize = SubtitleFontSize;
                            secondLabel.VerticalOptions = LayoutOptions.Start;
                            secondLabel.FontFamily = "font_title.ttf#font_title";
                        }
                        else if (card.SubTitle != null)
                        {
                            secondLabel.Text = card.SubTitle;
                            secondLabel.TextColor = Color.Black;
                            secondLabel.FontSize = SubtitleFontSize;
                            secondLabel.VerticalOptions = LayoutOptions.Start;
                            secondLabel.FontFamily = "font_title.ttf#font_title";
                        }
                        else if (card.Description != null)
                        {
                            secondLabel.Text = card.Description;
                            secondLabel.TextColor = Color.Black;
                            secondLabel.FontSize = DescriptionFontSize;
                            secondLabel.VerticalOptions = LayoutOptions.Start;
                            secondLabel.FontFamily = "font_description.otf#font_description";
                        }
                    

                        GridContent.Children.Add(new StackLayout
                        {
                            Spacing = 0,
                            VerticalOptions = LayoutOptions.FillAndExpand,
                            Children =
                            {
                                new Label()
                                {
                                    Text = card.Title,
                                    TextColor = Color.Black,
                                    Margin = TitleMargin,
                                    FontSize =  TitleFontSize,
                                    VerticalOptions = LayoutOptions.Start,
                                    FontFamily = "font_title.ttf#font_title"
                                },
                                secondLabel,
                                new Image()
                                {
                                    Source = card.GetImage(),
                                    HorizontalOptions = LayoutOptions.Fill,
                                    VerticalOptions = LayoutOptions.EndAndExpand
                                }

                            }
                        }, 1, 3);
                    }
                    break;
                default:
                    break;
            }
            CardLayoutChangeSize(this, new EventArgs());


        }
    }
}