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
        public event EventHandler AnimationCompleted = delegate { };
        double leftTitleMarginLeft = 1;
        double leftTitleMarginTop = 1;
        public static readonly Color colorGoal = Color.FromHex("#D03080");
        public static readonly Color colorKeeper = Color.FromHex("#B0C010");
        public static readonly Color colorNewRule = Color.FromHex("#E0B000");
        public static readonly Color colorAction = Color.FromHex("#30A0D0");
    

        


        AbsoluteLayout animLayout;
        RelativeLayout invisible; //for remplace card during animation 
        RelativeLayout initialLayout;
        double xInitial, yInitial, sInitial, xFinal, yFinal, sFinal, xCurrent, yCurrent, sCurrent;//(for animation)
        bool face;  // true = face up, false = face down
        bool showed;// true = over all for lecture, false = default 

        public CardView(Card card, double cardHeight, AbsoluteLayout AnimLayout)
        {
            InitializeComponent();
            HeightRequest = cardHeight;//default Height = 25% of screen height
            this.animLayout = AnimLayout;
            face = true;
            showed = false;
            invisible = new RelativeLayout()
            {
                HeightRequest = Height,
                WidthRequest = Width,
                BackgroundColor = Color.Transparent
            };
            ChargeCard(card);
            
        }

        private void Test1(object sender, EventArgs e)
        {

            AnimShowCard(!showed);
        }
        private void Test2(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("2---------------------");
        }
        private void Test3(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("3---------------------");
        }
        private void Test4(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("4---------------------");
        }


        public double GetWidth(double height)
        {
            return (height / 11) * 7;
        }

        public async void AnimShowCard(bool show)// show = destination, showed = origine;
        {

            if (show && !showed)
            {
                if (!this.AnimationIsRunning("ViewCardAnimations"))
                {
                    double hmem = Height, wmem = Width;
                    double sOrigine = Scale, sDestination = Scale * ((Application.Current.MainPage.Height - 40) / Height);
                    (double xOrigine, double yOrigine) = GetXY(this);
                    double xDestination = (Application.Current.MainPage.Width - Width) / 2, yDestination = (Application.Current.MainPage.Height - Height) / 2;

                    //1 initial state
                    animLayout.BackgroundColor = Color.FromHex("#00000000");
                    animLayout.WidthRequest = Application.Current.MainPage.Width;
                    animLayout.HeightRequest = Application.Current.MainPage.Height;
                    HeightRequest = hmem; WidthRequest = wmem;

                    invisible.HeightRequest = Height;
                    invisible.WidthRequest = Width;
                    // (Parent as RelativeLayout).Children.Remove(this);

                    initialLayout = (Parent as RelativeLayout);

                    animLayout.Children.Insert(0, this);
                    HeightRequest = hmem; WidthRequest = wmem;
                    TranslationX = xOrigine; TranslationY = yOrigine;

                    //result.Children.Add(CardsViews[cardId], Constraint.Constant(0), Constraint.Constant(0), Constraint.RelativeToParent((parent) => { return (result.Height / verticalFormat) * horizontalFormat; }), Constraint.RelativeToParent((parent) => { return result.Height; }));
                    initialLayout.Children.Add(invisible, Constraint.Constant(0), Constraint.Constant(0), Constraint.Constant(Width), Constraint.Constant(Height));
                    //(Parent.Parent as StackLayout).Children.Insert((Parent as StackLayout).Children.IndexOf(this), invisible);
                    animLayout.IsVisible = true;

                    //2
                    var translationx = new Animation(callback: x => TranslationX = x, start: xOrigine, end: xDestination, easing: Easing.Linear);
                    var translationy = new Animation(callback: y => TranslationY = y, start: yOrigine, end: yDestination, easing: Easing.Linear);
                    var scale = new Animation(callback: s => Scale = s, start: sOrigine, end: sDestination, easing: Easing.Linear);

                    //3
                    var animation = new Animation
                    {
                        { 0, 1, translationx },
                        { 0, 1, translationy },
                        { 0, 1, scale }
                    };

                    animation.Commit(this, "ViewCardAnimations", 16, 1000);
                    await Task.Delay(1000);
                    showed = show;
                }
            }
            if (showed && !show)
            {
                if (!this.AnimationIsRunning("ViewCardAnimations"))
                {
                    this.AbortAnimation("ViewCardAnimations");
                }
                if (!this.AnimationIsRunning("!ViewCardAnimations"))
                {
                    
                    double sOrigine = Scale, sDestination = invisible.Scale;
                    double xOrigine = (Application.Current.MainPage.Width - Width) / 2, yOrigine = (Application.Current.MainPage.Height - Height) / 2;
                    (double xDestination, double yDestination) = GetXY(invisible);

                    var translationx = new Animation(callback: x => TranslationX = x, start: xOrigine, end: xDestination, easing: Easing.Linear);
                    var translationy = new Animation(callback: y => TranslationY = y, start: yOrigine, end: yDestination, easing: Easing.Linear);
                    var scale = new Animation(callback: s => Scale = s, start: sOrigine, end: sDestination, easing: Easing.Linear);

                    Animation animation = new Animation {
                        { 0, 1, translationx },
                        { 0, 1, translationy },
                        { 0, 1, scale }
                    };
                    
                    animation.Commit(this, "!ViewCardAnimations", 16, 500);

                    await Task.Delay(500);
                    int i = initialLayout.Children.IndexOf(invisible);
                    //_parent.Children.RemoveAt(i);
                    TranslationX = 0;
                    TranslationY = 0;
                    initialLayout.Children.Insert(i, this);

                    //_parent.Children.Remove(invisible);
                    animLayout.Children.Remove(this);
                    animLayout.IsVisible = false;

                    showed = show;
                }
            }
            AnimationCompleted(this, EventArgs.Empty);
        
    

        }
        private async void AnimMoveCard(StackLayout final, double final_height)
        {
            if (!this.AnimationIsRunning("MoveCardAnimations"))
            {
                (double xOrigine, double yOrigine) = GetXY(this);
                (double xDestination, double yDestination ) = GetXY(final);
                double hOrigine = Height, wOrigine = Width, hDestination = final_height, wDestination = GetWidth(final_height);

                var translationx = new Animation(callback: x => TranslationX = x, start: xOrigine, end: xDestination, easing: Easing.Linear);
                var translationy = new Animation(callback: y => TranslationY = y, start: yOrigine, end: yDestination, easing: Easing.Linear);
                var scaleH = new Animation(callback: s => HeightRequest = s, start: hOrigine, end: hDestination, easing: Easing.Linear);
                var scaleW = new Animation(callback: s => HeightRequest = s, start: wOrigine, end: wDestination, easing: Easing.Linear);

                //3
                var animation = new Animation
                    {
                        { 0, 1, translationx },
                        { 0, 1, translationy },
                        { 0, 1, scaleH },
                        { 0, 1, scaleW }
                    };

                //avant animation
                animation.Commit(this, "ViewCardAnimations", 16, 1000);
                await Task.Delay(1000);
                //après animation

            }
        }
        public (double, double) GetXY(View view)
        {
            double x = view.X, y = view.Y;
            var parent = (view.Parent as View);
            while (parent != null)
            {
                if (parent is ScrollView)
                {
                    if ((parent as ScrollView).Orientation == ScrollOrientation.Horizontal)
                        x -= (parent as ScrollView).ScrollX;
                    else
                        y -= (parent as ScrollView).ScrollY;
                }
                x += parent.X;
                y += parent.Y;
                parent = (parent.Parent as View);
            }
            return (x, y);
        }

        public async void AnimTurnCard(bool final_face)//face true = face up //face false = face down
        {
          
            if (!Card.AnimationIsRunning("RotateYTo"))
            {
                int rotate = 90;
                if (face == final_face) rotate = 20;
                await Card.RotateYTo(face?-rotate: rotate, 100);//0  to -90 = up left, 0   to  90 = up right
                Card.RotationY = (final_face)?-rotate : rotate;      //90 to   0 = right, -90 to   0 = left 
                CardFrame.IsVisible = final_face;
                LeftTitleFrame.IsVisible = final_face;
                CardFaceDown.IsVisible = !final_face;
                await Card.RotateYTo(0, 100);
                Card.RotationY = 0;
                AnimationCompleted(this, EventArgs.Empty);
                face = final_face;
            }
        }
   
        private void CardTouch(object sender, EventArgs e)
        {
           
            AnimShowCard(!showed);
        }
        public void CardLayoutChangeSize(object sender, EventArgs e)
        {
            LeftTitleFrame.HeightRequest = CardLayout.Height * 490 / 610;
            LeftTitleFrame.WidthRequest = CardLayout.Height * 490 / 610;
            CardFaceDown.HeightRequest = CardLayout.Height;
            LeftTitleFrame.Rotation = -90;
            LeftTitleFrame.Padding = 0;
            LeftTitleFrame.Margin = new Thickness(CardLayout.Width * leftTitleMarginLeft, CardLayout.Height * leftTitleMarginTop, 0, 0);

            CardFrame.CornerRadius = Convert.ToInt32(40 * CardLayout.Height / 1100);
            CardFaceDown.CornerRadius = Convert.ToInt32(40 * CardLayout.Height / 1100);
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
            double GeneralFontSize = Height / 88;
            double TitleFontSize = GeneralFontSize * 4.8;
            double SubtitleFontSize = GeneralFontSize * 3;
            double DescriptionFontSize = GeneralFontSize * 2.7;
            Thickness TitleMargin = new Thickness(0, Height / 80, 0, Height / 80);

            LabelLeftTitle.Text = card.Title.ToUpper();
            LabelLeftTitle.FontFamily = "left_title_font.ttf#left_title_font";
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

            LabelTitleType.FontFamily = "type_title_font.ttf#type_title_font";
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
                            FontFamily = "title_font.ttf#title_font",
                        },
                        new Label()
                        {
                            Text = testText,
                            TextColor = Color.Black,
                            FontSize = DescriptionFontSize,
                            FontFamily = "description_font.ttf#description_font"
                        }
                    }
                    }, 1, 3);
                    break;
                case TypeOfCard.NewRule:
                    LabelTitleType.Text = "NOUVELLE RÈGLE";
                    LabelTitleType.FontSize = GeneralFontSize * 4.3;
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
                                FontFamily = "title_font.ttf#title_font",

                            },
                            new Label()
                            {
                                Text = card.SubTitle,
                                TextColor = Color.Black,
                                FontSize = SubtitleFontSize,
                                VerticalOptions = LayoutOptions.End,
                                FontFamily = "title_font.ttf#title_font"
                            },
                            new Label()
                            {
                                Text = card.Description,
                                TextColor = Color.Black,
                                FontSize = DescriptionFontSize,
                                VerticalOptions = LayoutOptions.End,
                                FontFamily = "description_font.ttf#description_font"
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
                            FontFamily = "title_font.ttf#title_font",

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
                                    FontFamily = "title_font.ttf#title_font",

                                },
                                new Label()
                                {
                                    Text = card.Description,
                                    TextColor = Color.Black,
                                    FontSize = DescriptionFontSize,
                                    VerticalOptions = LayoutOptions.End,
                                    FontFamily = "description_font.ttf#description_font"
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
                            secondLabel.FontFamily = "title_font.ttf#title_font";
                        }
                        else if (card.SubTitle != null)
                        {
                            secondLabel.Text = card.SubTitle;
                            secondLabel.TextColor = Color.Black;
                            secondLabel.FontSize = SubtitleFontSize;
                            secondLabel.VerticalOptions = LayoutOptions.Start;
                            secondLabel.FontFamily = "title_font.ttf#title_font";
                        }
                        else if (card.Description != null)
                        {
                            secondLabel.Text = card.Description;
                            secondLabel.TextColor = Color.Black;
                            secondLabel.FontSize = DescriptionFontSize;
                            secondLabel.VerticalOptions = LayoutOptions.Start;
                            secondLabel.FontFamily = "description_font.ttf#description_font";
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
                                    FontFamily = "title_font.ttf#title_font"
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