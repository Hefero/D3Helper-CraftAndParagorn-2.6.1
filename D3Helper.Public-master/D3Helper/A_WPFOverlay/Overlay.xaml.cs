using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Linq.Expressions;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using D3Helper.A_Collection;
using Enigma.D3;
using Enigma.D3.UI;
using Enigma.D3.Helpers;

using D3Helper.A_Tools;
using D3Helper.A_Collector;
using D3Helper.A_Enums;
using Enigma.D3.UI.Controls;
using SlimDX.DirectInput;
using SlimDX.DirectWrite;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Windows.Media.Color;
using Environment = System.Environment;
using FontFamily = System.Windows.Media.FontFamily;
using Point = System.Drawing.Point;
using Rectangle = System.Windows.Shapes.Rectangle;
using D3Helper.A_Handler.AutoCube;

namespace D3Helper.A_WPFOverlay
{
    /// <summary>
    /// Interaktionslogik für Overlay.xaml
    /// </summary>
    public partial class Overlay : Window
    {

        public static System.Drawing.Rectangle d3WndRect = new System.Drawing.Rectangle();

        public static Int32Rect d3clientrect;

        static string[] _postFixes = new string[] { "", "k", "M", "bn", "tr", "qr", "qt" };
        static string[] _postFixesKm = new string[] { "", "m", "Km" };

        public static int ExceptionCount = 0;
        private static int _tick;

        private static IntPtr _parentHandle;

        public static string selected_uxcontrol = null;

        public Overlay()
        {
            while (Engine.Current == null) { System.Threading.Thread.Sleep(1); }

            ResizeMode = ResizeMode.NoResize;
            WindowStyle = WindowStyle.None;
            ShowInTaskbar = false;
            //SnapsToDevicePixels = true;
            //SizeToContent = SizeToContent.WidthAndHeight;

            this.MakeTransparent();
            //SetParentHandle(Engine.Current.Process.MainWindowHandle);

            //this.Topmost = true;

            this.WindowStartupLocation = System.Windows.WindowStartupLocation.Manual;

            d3clientrect = A_Collection.D3Client.Window.D3ClientRect;

            this.Left = d3clientrect.X;//d3WndRect.Left;//mainScreen.Left;//Engine.Current.VideoPreferences.x0C_DisplayMode.x08_WinLeft;
            this.Top = d3clientrect.Y;//d3WndRect.Top;//mainScreen.Top;//Engine.Current.VideoPreferences.x0C_DisplayMode.x0C_WinTop;
            this.Width = d3clientrect.Width;//d3WndRect.Width;//mainScreen.Width;//Engine.Current.VideoPreferences.x0C_DisplayMode.x10_WinWidth;
            this.Height = d3clientrect.Height;//d3WndRect.Height;//mainScreen.Height;//Engine.Current.VideoPreferences.x0C_DisplayMode.x14_WinHeight;
            
            CompositionTarget.Rendering += Render_UI;

            InitializeComponent();

            //--
            TextBlock t = new TextBlock();
            
            Canvas.SetLeft(t, 0);
            Canvas.SetTop(t, 0);

            canvas1.Children.Add(t);
            //
            
            
        }

 

        private void Render_UI(object sender, EventArgs e)
        {

            while (Engine.Current == null) System.Threading.Thread.Sleep(50);

            var tick = Engine.Current.ApplicationLoopCount;
            if (tick != _tick)
            {
                _tick = tick;

                try
                {
                    if (Engine.Current == null) Environment.Exit(1);

                    if (A_Collection.D3Client.Window.isForeground && 
                        !A_Collection.D3UI.isOpenGameMenu &&
                        !A_Collection.D3UI.isOpenGuildMain &&
                        !A_Collection.D3UI.isOpenLeaderboardsMain &&
                        !A_Collection.D3UI.isOpenAchievementsMain)
                    {



                        d3clientrect = A_Collection.D3Client.Window.D3ClientRect;

                        double FontSize = (double) (d3clientrect.Height*14/1000);
                        FontFamily fontfamily = new FontFamily("Arial");

                        //System.Diagnostics.Stopwatch fpsSW = new System.Diagnostics.Stopwatch();
                        //fpsSW.Start();


                        this.Left = d3clientrect.X;
                            //d3WndRect.Left;//mainScreen.Left;//Engine.Current.VideoPreferences.x0C_DisplayMode.x08_WinLeft;
                        this.Top = d3clientrect.Y;
                            //d3WndRect.Top;//mainScreen.Top;//Engine.Current.VideoPreferences.x0C_DisplayMode.x0C_WinTop;
                        this.Width = d3clientrect.Width;
                            //d3WndRect.Width;//mainScreen.Width;//Engine.Current.VideoPreferences.x0C_DisplayMode.x10_WinWidth;
                        this.Height = d3clientrect.Height;
                            //d3WndRect.Height;//mainScreen.Height;//Engine.Current.VideoPreferences.x0C_DisplayMode.x14_WinHeight;

                        
                        this.WindowState = System.Windows.WindowState.Normal;
                        canvas1.Children.Clear();

                        if (A_Collection.Me.HeroStates.isInGame &&
                            A_Collection.Environment.Scene.Counter_CurrentFrame != 0 &&
                            A_Collection.Environment.Scene.GameTick > 1)
                        {
                            populateRectRenderTargets();
                            //populateSkillButtonRectRenderTargets();
                            populateTextRenderTargets();
                            //populateSkillButtonImages();



                            var rectTargets = Overlay.rectRenderTargets;
                            //var rectSkillButtonTargets = Overlay.rectSkillButtonRenderTargets;
                            var textTargets = Overlay.textRenderTargets;
                            //var canvasSkillButtonImages = canvasSkillButtonImageTargets;

                            //// Draw BackgroundImages
                            if (Properties.Settings.Default.overlayxptracker) // XP Tracker BackgroundImage
                            {
                                Canvas mycanvas = new Canvas();


                                mycanvas.Width = A_Collection.D3Client.Window.D3ClientRect.Width*18/100;
                                mycanvas.Height = A_Collection.D3Client.Window.D3ClientRect.Height*4/100;

                                ImageBrush ib = new ImageBrush();
                                ib.ImageSource =
                                    new BitmapImage(getResourceUri("TraitList_Button_Inactive"));
                                mycanvas.Background = ib;

                                Canvas.SetLeft(mycanvas, d3clientrect.Width*20/100);
                                Canvas.SetTop(mycanvas, 0);

                                canvas1.Children.Add(mycanvas);
                            }


                            //////////////////////////

                            if (A_Collection.D3UI.isOpenInventory)
                            {
                                UIRect Inventory_MainPage =
                                    A_Tools.T_D3UI.UIElement.getRect(A_Enums.UIElements.InventoryMainPage);

                                // Render Version Info


                                //TextBlock VersionInfo = new TextBlock();
                                //VersionInfo.FontFamily = fontfamily;
                                //VersionInfo.FontSize = FontSize;
                                //VersionInfo.Text = "D3Helper V" + A_Tools.Version.AppVersion.version;
                                //VersionInfo.Foreground = new SolidColorBrush(Colors.White);
                                //Canvas.SetLeft(VersionInfo, Inventory_MainPage.Left + (Inventory_MainPage.Width*60/100));
                                //Canvas.SetTop(VersionInfo, Inventory_MainPage.Top + (Inventory_MainPage.Height*5/1000));
                                //canvas1.Children.Add(VersionInfo);



                                // Render AutoGamble Status
                                if (Properties.Settings.Default.AutoGambleBool)
                                {
                                    TextBlock AutoGambleStatus = new TextBlock();
                                    AutoGambleStatus.FontFamily = fontfamily;
                                    AutoGambleStatus.FontSize = FontSize;
                                    AutoGambleStatus.Text = "AutoGamble On";
                                    AutoGambleStatus.Foreground = new SolidColorBrush(Colors.GreenYellow);
                                    Canvas.SetLeft(AutoGambleStatus,
                                        Inventory_MainPage.Left + (Inventory_MainPage.Width*60/100));
                                    Canvas.SetTop(AutoGambleStatus,
                                        Inventory_MainPage.Top + (Inventory_MainPage.Height*40/1000));
                                    canvas1.Children.Add(AutoGambleStatus);
                                }
                                else
                                {
                                    TextBlock AutoGambleStatus = new TextBlock();
                                    AutoGambleStatus.FontFamily = fontfamily;
                                    AutoGambleStatus.FontSize = FontSize;
                                    AutoGambleStatus.Text = "AutoGamble Off";
                                    AutoGambleStatus.Foreground = new SolidColorBrush(Colors.IndianRed);
                                    Canvas.SetLeft(AutoGambleStatus,
                                        Inventory_MainPage.Left + (Inventory_MainPage.Width*60/100));
                                    Canvas.SetTop(AutoGambleStatus,
                                        Inventory_MainPage.Top + (Inventory_MainPage.Height*40/1000));
                                    canvas1.Children.Add(AutoGambleStatus);
                                }

                                //*********************************************************
                                // mark ancient,primals in Inventory
                                //*********************************************************

                                if (Properties.Settings.Default.ShowItemQualityBool)
                                {
                                    var itemsInInventory = Tools.Get_Items("legendary");
                                    foreach (var item in itemsInInventory)
                                    {
                                        var ancientRank = item.GetAttributeValue(Enigma.D3.Enums.AttributeId.AncientRank);
                                        if (ancientRank > 0)
                                        {
                                            //get UIrect of actorcommondata item
                                            UIRect UIRect_item =
                                                A_Collection.D3UI.InventoryItemUIRectMesh.FirstOrDefault(
                                                    x => x.Key.ItemSlotX == item.x118_ItemSlotX && x.Key.ItemSlotY == item.x11C_ItemSlotY).Value;


                                            string txt = "A";
                                            if (ancientRank == 1)
                                            {
                                                txt = "A";
                                            }
                                            else if (ancientRank == 2)
                                            {
                                                txt = "P";
                                            }

                                            TextBlock t = new TextBlock();
                                            t.BeginInit();
                                            t.Text = txt;
                                            t.FontSize = 12;
                                            t.FontWeight = FontWeights.Bold;
                                            t.Foreground = Brushes.Orange; //textcolor

                                            Canvas.SetLeft(t, UIRect_item.Left + +4);
                                            Canvas.SetTop(t, UIRect_item.Top + +4);

                                            t.EndInit();

                                            canvas1.Children.Add(t);

                                        }



                                    }
                                }


                            }


                            foreach (var rect in rectTargets) // Render Rectangles
                            {
                                Rectangle r = rect.Rect;

                                Canvas.SetLeft(r, rect.PosLeft);
                                Canvas.SetTop(r, rect.PosTop);

                                canvas1.Children.Add(r);

                            }

                            #region SkillButtonDraws

                            if (A_Collection.Me.HeroStates.isInGame && A_Collection.Me.HeroGlobals.LocalACD != null)
                            {
                                Dictionary<int, int> ActiveSkills;
                                lock (A_Collection.Me.HeroDetails.ActiveSkills)
                                    ActiveSkills = A_Collection.Me.HeroDetails.ActiveSkills.ToDictionary(x => x.Key,
                                        y => y.Value);

                                if (ActiveSkills.FirstOrDefault(x => x.Key == 196974).Key == 0)
                                {

                                if (Properties.Settings.Default.overlayskillbuttons ||
                                    Properties.Settings.Default.overlayskillbuttonsastext)
                                {
                                    DrawSkillButtonCross();

                                    if (missingDefinitions)
                                    {
                                        string text =
                                            "There are one or more AutoCast Skills without a single defined CastDefinition.\nCheck if you specified cast to a specific rune.\nUse the SkillEditor to create Definitions.";
                                        float x = A_Collection.D3Client.Window.D3ClientRect.X +
                                                  (A_Collection.D3Client.Window.D3ClientRect.Width*2/100);
                                        float y = A_Collection.D3Client.Window.D3ClientRect.Y +
                                                  (A_Collection.D3Client.Window.D3ClientRect.Height/2);

                                        DrawScreenText(Colors.Red, 20, new FontFamily("Arial"), text, "", x, y);
                                    }
                                }

                                if (Properties.Settings.Default.overlayskillbuttons)
                                {
                                    if (A_Collection.Skills.UI_Controls.SkillControls[0].IsVisible())
                                    {
                                        DrawSkillButtonOutlines();

                                    }

                                }
                                if (Properties.Settings.Default.overlayskillbuttonsastext)
                                {
                                    if (A_Collection.Skills.UI_Controls.SkillControls[0].IsVisible())
                                    {
                                        DrawSkillButtonCorners();
                                    }

                                }

                            }
                        }

                            #endregion

                            foreach (var text in textTargets) // Render TextBlocks
                            {
                                TextBlock t = text.Text;

                                Canvas.SetLeft(t, text.PosLeft);
                                Canvas.SetTop(t, text.PosTop);

                                canvas1.Children.Add(t);


                            }


                            #region VersionInfo

                            DrawScreenText(Colors.OrangeRed, 15, new FontFamily("SansSerif"), "D3", "versioninfo",
                                d3clientrect.X + (d3clientrect.Width*17/1000),
                                d3clientrect.Y + (d3clientrect.Height*98/100), true);
                            DrawScreenText(Colors.DimGray, 15, new FontFamily("SansSerif"), "Helper", "versioninfo",
                                d3clientrect.X + (d3clientrect.Width*27/1000),
                                d3clientrect.Y + (d3clientrect.Height*98/100), true);

                            #endregion

                            #region MenuSettings
                            //DrawScreenImage(Properties.Resources.menu_iconsettings, d3clientrect.X + (d3clientrect.Width * 20/100), d3clientrect.Y);
                            #endregion

                            #region GRift Progress Draws

                            if (Properties.Settings.Default.overlayriftprogress &&
                                A_Collection.Environment.Areas.GreaterRift_Tier > 0 &&
                                !A_Collection.D3UI.isOpenInventory &&
                                A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.Bar_GRiftProgress))
                            {
                                UIRect bar_riftprogress =
                                    A_Tools.T_D3UI.UIElement.getRect(A_Enums.UIElements.Bar_GRiftProgress);

                                DrawScreenText(Colors.PeachPuff, 14, new FontFamily("Arial Black"),
                                    "(" + Properties.Settings.Default.riftprogress_radius + " yrd) " +
                                    A_Collection.Environment.Actors.RiftProgressInRange_Percentage.ToString("0.00") +
                                    "%", "riftprogress", bar_riftprogress.Left,
                                    bar_riftprogress.Bottom + (bar_riftprogress.Height*150/100));
                            }

                            #endregion

                            #region ConventionPanel

                            //--
                            if (Properties.Settings.Default.overlayconventiondraws)
                            {
                                for (int i = 0; i < 4; i++)
                                {
                                    switch (i)
                                    {
                                        case 0:
                                            if (A_Collection.Me.HeroDetails.CurrentConventionElement == DamageType.none)
                                                continue;
                                            break;

                                        case 1:
                                            if (A_Collection.Me.Party.PartyMember1_CurrentConventionElement ==
                                                DamageType.none)
                                                continue;
                                            break;

                                        case 2:
                                            if (A_Collection.Me.Party.PartyMember2_CurrentConventionElement ==
                                                DamageType.none)
                                                continue;
                                            break;

                                        case 3:
                                            if (A_Collection.Me.Party.PartyMember3_CurrentConventionElement ==
                                                DamageType.none)
                                                continue;
                                            break;
                                    }



                                    switch (i)
                                    {
                                        case 0:
                                            UIRect portrait0 =
                                                A_Tools.T_D3UI.UIElement.getRect(A_Enums.UIElements.portrait_0);
                                            DamageType current0 = A_Collection.Me.HeroDetails.CurrentConventionElement;
                                            DamageType next0 = A_Collection.Me.HeroDetails.NextConventionElement;
                                            double ticksleft0 = A_Collection.Me.HeroDetails.Convention_TicksLeft;

                                            Draw_CurrentConvention(portrait0, current0, ticksleft0);
                                            Draw_NextConvention(portrait0, next0);
                                            break;

                                        case 1:
                                            UIRect portrait1 =
                                                A_Tools.T_D3UI.UIElement.getRect(A_Enums.UIElements.portrait_1);
                                            DamageType current1 =
                                                A_Collection.Me.Party.PartyMember1_CurrentConventionElement;
                                            DamageType next1 = A_Collection.Me.Party.PartyMember1_NextConventionElement;
                                            double ticksleft1 = A_Collection.Me.Party.PartyMember1_Convention_TicksLeft;

                                            Draw_CurrentConvention(portrait1, current1, ticksleft1);
                                            Draw_NextConvention(portrait1, next1);
                                            break;

                                        case 2:
                                            UIRect portrait2 =
                                                A_Tools.T_D3UI.UIElement.getRect(A_Enums.UIElements.portrait_2);
                                            DamageType current2 =
                                                A_Collection.Me.Party.PartyMember2_CurrentConventionElement;
                                            DamageType next2 = A_Collection.Me.Party.PartyMember2_NextConventionElement;
                                            double ticksleft2 = A_Collection.Me.Party.PartyMember2_Convention_TicksLeft;

                                            Draw_CurrentConvention(portrait2, current2, ticksleft2);
                                            Draw_NextConvention(portrait2, next2);
                                            break;

                                        case 3:
                                            UIRect portrait3 =
                                                A_Tools.T_D3UI.UIElement.getRect(A_Enums.UIElements.portrait_3);
                                            DamageType current3 =
                                                A_Collection.Me.Party.PartyMember3_CurrentConventionElement;
                                            DamageType next3 = A_Collection.Me.Party.PartyMember3_NextConventionElement;
                                            double ticksleft3 = A_Collection.Me.Party.PartyMember3_Convention_TicksLeft;

                                            Draw_CurrentConvention(portrait3, current3, ticksleft3);
                                            Draw_NextConvention(portrait3, next3);
                                            break;
                                    }
                                }
                            }
                            //

                            #endregion

                            #region TotalAPS && Snapshoted TotalAPS Draws

                            if (Properties.Settings.Default.Overlay_APS)
                            {
                                string text = "CurAPS " + A_Collection.Me.HeroDetails.AttacksPerSecondTotal.ToString("0.00") + System.Environment.NewLine +
                                              "SnapAPS " + A_Collection.Me.HeroDetails.SnapShotted_APS.ToString("0.00");

                                DrawCursorText(Colors.LightSeaGreen, 14, new FontFamily("Arial Black"), text, 30, 30);
                                
                            }
                            #endregion

                            get_MouseIfHoveringUiElements();




                            //*******************************************
                            // debug ui rects
                            //*******************************************
                            if (Properties.Settings.Default.overlayfps)
                            {

                                foreach (UXControl uxcontrol in UXHelper.Enumerate())
                                {
                                    try
                                    {
                                        if (uxcontrol.IsVisible())
                                        {
                                            UIRect uirect = A_Tools.T_D3UI.UIElement.getRect(uxcontrol.ToString());



                                            Rectangle r = new Rectangle();
                                            r.BeginInit();
                                            r.Width = uirect.Width;
                                            r.Height = uirect.Height;
                                            r.StrokeThickness = 1;
                                            r.Stroke = Brushes.LightBlue;

                                            Canvas.SetLeft(r, uirect.Left);
                                            Canvas.SetTop(r, uirect.Top);

                                            r.EndInit();


                                            TextBlock t = new TextBlock();
                                            t.BeginInit();
                                            t.Text = uxcontrol.ToString();
                                            t.Foreground = Brushes.LightBlue; //textcolor

                                            Canvas.SetLeft(t, uirect.Left);
                                            Canvas.SetTop(t, uirect.Top);

                                            t.EndInit();


                                            //only render selected item
                                            if(selected_uxcontrol != null && selected_uxcontrol.Any())
                                            {
                                                if (t.Text.ToLower().Contains(selected_uxcontrol.ToLower()))
                                                {
                                                    canvas1.Children.Add(r);
                                                    canvas1.Children.Add(t);
                                                }
                                            }else
                                            {
                                                //render all
                                                canvas1.Children.Add(r);
                                                canvas1.Children.Add(t);
                                            }
                                        }
                                    }
                                    catch (Exception) { }
                                }
                            }






                            //**************************************
                            // print LevelAreaName of me and other Players
                            // not working :S
                            //**************************************

                            //string heroname = A_Collection.Me.HeroGlobals.LocalPlayerData.GetHeroName();
                            //int levelAreaSnoId = A_Collection.Me.HeroGlobals.LocalPlayerData.x9644_LevelAreaSnoId;
                            //A_Collection.LevelArea area = A_Collection.LevelArea.get_LevelAreaNameBySnoId(levelAreaSnoId);
                            //if (area != null)
                            //{
                            //    TextBlock t = new TextBlock();
                            //    t.BeginInit();
                            //    t.Text = area.Name;
                            //    t.Foreground = Brushes.LightBlue; //textcolor

                            //    Canvas.SetLeft(t, 10);
                            //    Canvas.SetTop(t, 10);

                            //    t.EndInit();
                            //    canvas1.Children.Add(t);
                            //}





                            //*******************************************
                            // elite circles in range overlay
                            // and normal monster minimap
                            //*******************************************

                            if (Properties.Settings.Default.Overlay_EliteCircles)
                            {

                                if (A_Collection.Me.HeroStates.isInGame && A_Collection.Me.HeroGlobals.LocalACD != null)
                                {
                                    try
                                    {
                                        List<ActorCommonData> eliteInRange = A_Tools.T_ACD.getEliteInRange(6000);

                                        List<ActorCommonData> nonEliteInRange = A_Tools.T_ACD.getNonEliteMonsterInRange(6000);

                                        List<ActorCommonData> monsterInRange = new List<ActorCommonData>();
                                        monsterInRange.AddRange(eliteInRange);
                                        monsterInRange.AddRange(nonEliteInRange);

                                        foreach (ActorCommonData monster in monsterInRange)
                                        {

                                            Ellipse el = new Ellipse();
                                            el.BeginInit();

                                            el.Width = 100;
                                            el.Height = 80;


                                            el.StrokeThickness = 8;

                                            if (T_ACD.isEliteYellow(monster))
                                            {
                                                el.Stroke = new SolidColorBrush(Color.FromArgb(180, 255, 148, 20));
                                            }

                                            if (T_ACD.isEliteBlue(monster))
                                            {
                                                el.Stroke = new SolidColorBrush(Color.FromArgb(180, 64, 128, 255));
                                            }

                                            if (T_ACD.isBoss(monster))
                                            {
                                                el.Stroke = new SolidColorBrush(Color.FromArgb(180, 255, 96, 0));
                                                el.StrokeThickness = 10;
                                            }

                                            if (!T_ACD.isElite(monster))
                                            {
                                                el.Stroke = new SolidColorBrush(Color.FromArgb(180, 188, 236, 138));
                                            }


                                            //******************************************
                                            // Calculate Monster Position on Screen
                                            //******************************************
                                            float rX = 0;
                                            float rY = 0;

                                            A_Tools.T_World.ToScreenCoordinate(monster.x0D0_WorldPosX, monster.x0D4_WorldPosY, monster.x0D8_WorldPosZ, out rX, out rY);


                                            //circle position = circle_center
                                            rX = rX - (float)el.Width / 2;
                                            rY = rY - (float)el.Height / 2;




                                            Canvas.SetLeft(el, rX);
                                            Canvas.SetTop(el, rY);

                                            el.EndInit();

                                            if (T_ACD.isElite(monster))
                                            {
                                                canvas1.Children.Add(el);
                                            }



                                            //-----------------------------------------
                                            //draw elite health in Percentage
                                            //-----------------------------------------
                                            try
                                            {
                                                if (T_ACD.isElite(monster))
                                                {

                                                    double hitpointsCurrent = monster.GetAttributeValue(Enigma.D3.Enums.AttributeId.HitpointsCur);
                                                    double hitpointsMax = monster.GetAttributeValue(Enigma.D3.Enums.AttributeId.HitpointsMax);
                                                    if(hitpointsMax > 0)
                                                    {
                                                        //center text in circle
                                                        float rX_center = rX + (float)el.Width / 2;
                                                        float rY_center = rY + (float)el.Height / 2;


                                                        int health_percentage = (int)((hitpointsCurrent / hitpointsMax) * 100);

                                                        TextBlock t = new TextBlock();
                                                        t.BeginInit();
                                                        t.Text = health_percentage + "%";
                                        
                                                        t.Foreground = el.Stroke; //textcolor same as circle
                                                        t.Background = Brushes.White;
                                                        t.FontSize = 16;

                                                        Canvas.SetLeft(t, rX_center);
                                                        Canvas.SetTop(t, rY_center);

                                                        t.EndInit();
                                                        canvas1.Children.Add(t);
                                            
                                                    }
                                                }

                                            }
                                            catch (Exception)
                                            {
       
                                            }







                                            //-----------------------------------------
                                            //draw elite/monster on minimap
                                            //-----------------------------------------
                                            UIRect minimap_rect = A_Tools.T_D3UI.UIElement.getRect("Root.NormalLayer.minimap_dialog_backgroundScreen.minimap_dialog_pve.minimap_frame");

                                            //int video_width = Engine.Current.VideoPreferences.x0C_DisplayMode.x20_Width;
                                            //int video_height = Engine.Current.VideoPreferences.x0C_DisplayMode.x24_Height;

                                            int video_width = A_Collection.D3Client.Window.D3ClientRect.Width;
                                            int video_height = A_Collection.D3Client.Window.D3ClientRect.Height;


                                            int rx_player = video_width / 2;
                                            int ry_player = video_height / 2;

                                            float rx_player_minimap = minimap_rect.Left + (minimap_rect.Width / 2);
                                            float ry_player_minimap = minimap_rect.Top + (minimap_rect.Height / 2);

                                            float delta_y = rY - ry_player;
                                            float delta_x = rX - rx_player;

                                            float factor = 0.075f;

                                            float rx_minimap = rx_player_minimap + delta_x * factor;
                                            float ry_minimap = ry_player_minimap + delta_y * factor;

                                            Ellipse e_minimap = new Ellipse();
                                            e_minimap.BeginInit();

                                            double diameter = 10;

                                            if (!T_ACD.isElite(monster))
                                            {
                                                diameter = 5;
                                            }

                                            e_minimap.Width = diameter;
                                            e_minimap.Height = diameter;

                                            e_minimap.Stroke = el.Stroke;
                                            e_minimap.StrokeThickness = diameter / 2;


                                            //circle position = circle_center
                                            rx_minimap = rx_minimap - (float)e_minimap.Width / 2;
                                            ry_minimap = ry_minimap - (float)e_minimap.Height / 2;

                                            Canvas.SetLeft(e_minimap, rx_minimap);
                                            Canvas.SetTop(e_minimap, ry_minimap);

                                            e_minimap.EndInit();
                                            canvas1.Children.Add(e_minimap);


                                        }
                                    }
                                    catch (Exception) { }
                              
                                }
                            }

               

                        }

                        this.Topmost = true;

                        //fpsSW.Stop();
                        //TimeSpan elapsedFpsSW = fpsSW.Elapsed;

                        //renderFramesPerSecond = /*1000 / */ elapsedFpsSW.TotalMilliseconds;

                        //double TimeLeftToNextTick = ((1000/Properties.Settings.Default.D3Helper_UpdateRate) -
                        //                             elapsedFpsSW.TotalMilliseconds);
                        //if (TimeLeftToNextTick > 0)
                        //    System.Threading.Thread.Sleep((int) TimeLeftToNextTick);

                        System.Threading.Thread.Sleep(16);
                    }
                    else
                    {
                        canvas1.Children.Clear();
                        System.Threading.Thread.Sleep(16);
                    }

                }
                catch (Exception _e)
                {
                    A_Handler.Log.ExceptionLogEntry newEntry = new A_Handler.Log.ExceptionLogEntry(_e, DateTime.Now,
                        A_Enums.ExceptionThread.Overlay);

                    lock (A_Handler.Log.Exception.ExceptionLog) A_Handler.Log.Exception.ExceptionLog.Add(newEntry);
                }
            }
            else
            {
                System.Threading.Thread.Sleep(16);
            }
        }




        void get_MouseIfHoveringUiElements()
        {
            try
            {
                Point Cursor = System.Windows.Forms.Cursor.Position;

                Rect VersionInfo = new Rect();
                VersionInfo.X = d3clientrect.X;
                VersionInfo.Y = d3clientrect.Y + (d3clientrect.Height*97/100);
                VersionInfo.Height = d3clientrect.Height*2/100;
                VersionInfo.Width = d3clientrect.Width*4/100;

                //Rect Settings = new Rect();
                //Settings.X = d3clientrect.X + (d3clientrect.Width * 20 / 100);
                //Settings.Y = d3clientrect.Y;
                //Settings.Height = Properties.Resources.menu_iconsettings.Height;
                //Settings.Width = Properties.Resources.menu_iconsettings.Width;

                if (VersionInfo.Contains(new System.Windows.Point(Cursor.X, Cursor.Y)))
                {
                    DrawScreenText(Colors.DarkGray, 15, new FontFamily("SansSerif"),
                        "[" + A_Tools.Version.AppVersion.version + "]", "versioninfo",
                        d3clientrect.X + (d3clientrect.Width*53/1000),
                        d3clientrect.Y + (d3clientrect.Height*98/100), false);
                }

                //if (Settings.Contains(new System.Windows.Point(Cursor.X, Cursor.Y)))
                //{
                //    DrawScreenImage(Properties.Resources.menu1, (float)Settings.Left, (float)Settings.Bottom);
                //}
            }
            catch { }
        }
        void Draw_CurrentConvention(UIRect portrait, A_Enums.DamageType element, double _ticksleft)
        {
            ImageBrush background_current = new ImageBrush(getConventionElement_BitmapImage(element));

            Canvas element_current = new Canvas();
            element_current.Width = portrait.Width;
            element_current.Height = portrait.Width;
            element_current.Background = background_current;

            Canvas.SetLeft(element_current, portrait.Right);
            Canvas.SetTop(element_current, portrait.Bottom);

            //--

            //TextBlock ticksleft_bg = new TextBlock();
            //ticksleft_bg.FontFamily = new FontFamily("Arial Black");
            //ticksleft_bg.Text = (_ticksleft / 60).ToString("0");
            //ticksleft_bg.Foreground = new SolidColorBrush(Colors.Black);
            //ticksleft_bg.Background = new SolidColorBrush(Colors.Transparent);
            //ticksleft_bg.FontSize = FontSize * 2.2;

            //Canvas.SetTop(ticksleft_bg, (portrait.Width / 2) * 40 / 100);
            //Canvas.SetLeft(ticksleft_bg, (portrait.Width / 2) * 50 / 100);
            //element_current.Children.Add(ticksleft_bg);

            //--

            TextBlock ticksleft = new TextBlock();
            ticksleft.FontFamily = new FontFamily("Arial Black");
            ticksleft.Text = (_ticksleft / 60).ToString("0");
            ticksleft.Foreground = new SolidColorBrush(Colors.WhiteSmoke);
            ticksleft.Background = new SolidColorBrush(Colors.Transparent);
            ticksleft.FontSize = FontSize * 1.5;
            
            Canvas.SetTop(ticksleft, (portrait.Width / 2) * 65 / 100);
            Canvas.SetLeft(ticksleft, (portrait.Width / 2) * 65 / 100);
            element_current.Children.Add(ticksleft);
            
            //--
            canvas1.Children.Add(element_current);
        }

        void Draw_NextConvention(UIRect portrait, A_Enums.DamageType element)
        {
            ImageBrush background_next = new ImageBrush(getConventionElement_BitmapImage(element));


            Canvas element_next = new Canvas();
            element_next.Opacity = 0.4;
            element_next.Width = portrait.Width;
            element_next.Height = portrait.Width;
            element_next.Background = background_next;

            Canvas.SetLeft(element_next, portrait.Right + portrait.Width);
            Canvas.SetTop(element_next, portrait.Bottom);

            canvas1.Children.Add(element_next);
        }
        BitmapImage getConventionElement_BitmapImage(A_Enums.DamageType damagetype)
        {
            switch (damagetype)
            {
                    case DamageType.Arcane:
                    return new BitmapImage(getResourceUri("Buff_Legendary_Ring_Arcane_p2-3405442230"));

                    case DamageType.Cold:
                    return new BitmapImage(getResourceUri("Buff_Legendary_Ring_Frost_p2-1541236666"));

                    case DamageType.Fire:
                    return new BitmapImage(getResourceUri("Buff_Legendary_Ring_Fire_p2-4122635698"));

                    case DamageType.Holy:
                    return new BitmapImage(getResourceUri("Buff_Legendary_Ring_Holy_p2-2639019912"));

                    case DamageType.Lightning:
                    return new BitmapImage(getResourceUri("Buff_Legendary_Ring_Lightning_p2-569405584"));

                    case DamageType.Physical:
                    return new BitmapImage(getResourceUri("Buff_Legendary_Ring_Physical_p2-928374825"));

                    case DamageType.Poison:
                    return new BitmapImage(getResourceUri("Buff_Legendary_Ring_Poison_p2-3513867492"));

                default:
                    return null;
            }
        }
        void DrawLine(float startX, float startY, float stopX, float stopY, int thickness, Color color)
        {

                       
            
            Line Line = new Line();
            Line.Stroke = new SolidColorBrush(color);
            Line.StrokeThickness = thickness;
            Line.X1 = startX;
            Line.X2 = stopX;
            Line.Y1 = startY;
            Line.Y2 = stopY;

            this.canvas1.Children.Add(Line);

        }
        void DrawCursorText(Color Color, double FontSize, FontFamily FontFamily, string Text, double offsetX, double offsetY)
        {
            TextBlock CursorText = new TextBlock();
            CursorText.FontFamily = FontFamily;
            CursorText.FontSize = FontSize;
            CursorText.Text = Text;
            CursorText.Foreground = new SolidColorBrush(Color);

            var CursorPos = System.Windows.Forms.Cursor.Position;

            Canvas.SetLeft(CursorText, CursorPos.X - (d3clientrect.Width * offsetX / 1000));
            Canvas.SetTop(CursorText, CursorPos.Y - (d3clientrect.Height * offsetY / 1000));

            canvas1.Children.Add(CursorText);

        }
        void DrawScreenText(Color Color, double FontSize, FontFamily FontFamily, string Text, string name, float x, float y, bool bold = false)
        {
            TextBlock ScreenText = new TextBlock();
            ScreenText.FontFamily = FontFamily;
            ScreenText.FontSize = FontSize;
            if(!bold)
                ScreenText.Text = Text;
            else
                ScreenText.Inlines.Add(new Bold(new Run(Text)));
            ScreenText.Foreground = new SolidColorBrush(Color);
            ScreenText.Name = name;
            
            Canvas.SetLeft(ScreenText, x - (d3clientrect.Width * 15 / 1000));
            Canvas.SetTop(ScreenText, y - (d3clientrect.Height * 10 / 1000));

            canvas1.Children.Add(ScreenText);

        }

        void DrawSkillButtonOutlines()
        {
            try
            {
                List<UXIcon> AllSkillButtons;
                lock(A_Collection.Skills.UI_Controls.SkillControls) AllSkillButtons = A_Collection.Skills.UI_Controls.SkillControls;

                for (int i = 0; i < AllSkillButtons.Count; i++)
                {
                    UIRect SkillButton = A_Tools.T_D3UI.UIElement.getRect(AllSkillButtons[i].ToString());

                    if(AllSkillButtons[i].GetPowerSnoId() == -1)
                        continue;

                    Rectangle Outline = new Rectangle();
                    Outline.Width = SkillButton.Width;
                    Outline.Height = SkillButton.Height;
                    Outline.StrokeThickness = 3;
                    //Outline.Fill = new SolidColorBrush(Colors.Black);
                    
                    switch (i)
                    {
                        case 0: // HotBar1
                            switch (A_Collection.Me.AutoCastOverrides.AutoCast1Override)
                            {
                                case true:
                                    Outline.Stroke = new SolidColorBrush(Colors.Red);
                                    break;
                                case false:
                                    Outline.Stroke = new SolidColorBrush(Colors.Green);
                                    break;
                            }
                            break;
                        case 1: // HotBar2
                            switch (A_Collection.Me.AutoCastOverrides.AutoCast2Override)
                            {
                                case true:
                                    Outline.Stroke = new SolidColorBrush(Colors.Red);
                                    break;
                                case false:
                                    Outline.Stroke = new SolidColorBrush(Colors.Green);
                                    break;
                            }
                            break;
                        case 2: // HotBar3
                            switch (A_Collection.Me.AutoCastOverrides.AutoCast3Override)
                            {
                                case true:
                                    Outline.Stroke = new SolidColorBrush(Colors.Red);
                                    break;
                                case false:
                                    Outline.Stroke = new SolidColorBrush(Colors.Green);
                                    break;
                            }
                            break;
                        case 3: // HotBar4
                            switch (A_Collection.Me.AutoCastOverrides.AutoCast4Override)
                            {
                                case true:
                                    Outline.Stroke = new SolidColorBrush(Colors.Red);
                                    break;
                                case false:
                                    Outline.Stroke = new SolidColorBrush(Colors.Green);
                                    break;
                            }
                            break;
                        case 4: // HotBarRight
                            switch (A_Collection.Me.AutoCastOverrides.AutoCastRMBOverride)
                            {
                                case true:
                                    Outline.Stroke = new SolidColorBrush(Colors.Red);
                                    break;
                                case false:
                                    Outline.Stroke = new SolidColorBrush(Colors.Green);
                                    break;
                            }
                            break;
                        case 5: // HotBarLeft
                            switch (A_Collection.Me.AutoCastOverrides.AutoCastLMBOverride)
                            {
                                case true:
                                    Outline.Stroke = new SolidColorBrush(Colors.Red);
                                    break;
                                case false:
                                    Outline.Stroke = new SolidColorBrush(Colors.Green);
                                    break;
                            }
                            break;
                    }

                    Canvas.SetTop(Outline, SkillButton.Top);
                    Canvas.SetLeft(Outline, SkillButton.Left);


                    canvas1.Children.Add(Outline);
                }
            }
            catch (Exception)
            {
                
            }
        }
        void DrawSkillButtonCorners()
        {
            try
            {
                List<UXIcon> AllSkillButtons;
                lock (A_Collection.Skills.UI_Controls.SkillControls) AllSkillButtons = A_Collection.Skills.UI_Controls.SkillControls;

                for (int i = 0; i < AllSkillButtons.Count; i++)
                {
                    UIRect SkillButton = A_Tools.T_D3UI.UIElement.getRect(AllSkillButtons[i].ToString());

                    if (AllSkillButtons[i].GetPowerSnoId() == -1)
                        continue;

                    Rectangle Outline = new Rectangle();
                    Outline.Width = SkillButton.Width * 15/100;
                    Outline.Height = SkillButton.Height * 15/100;
                    Outline.StrokeThickness = 1;
                    //Outline.Fill = new SolidColorBrush(Colors.Black);

                    switch (i)
                    {
                        case 0: // HotBar1
                            switch (A_Collection.Me.AutoCastOverrides.AutoCast1Override)
                            {
                                case true:
                                    Outline.Stroke = new SolidColorBrush(Colors.Red);
                                    break;
                                case false:
                                    Outline.Stroke = new SolidColorBrush(Colors.Green);
                                    break;
                            }
                            break;
                        case 1: // HotBar2
                            switch (A_Collection.Me.AutoCastOverrides.AutoCast2Override)
                            {
                                case true:
                                    Outline.Stroke = new SolidColorBrush(Colors.Red);
                                    break;
                                case false:
                                    Outline.Stroke = new SolidColorBrush(Colors.Green);
                                    break;
                            }
                            break;
                        case 2: // HotBar3
                            switch (A_Collection.Me.AutoCastOverrides.AutoCast3Override)
                            {
                                case true:
                                    Outline.Stroke = new SolidColorBrush(Colors.Red);
                                    break;
                                case false:
                                    Outline.Stroke = new SolidColorBrush(Colors.Green);
                                    break;
                            }
                            break;
                        case 3: // HotBar4
                            switch (A_Collection.Me.AutoCastOverrides.AutoCast4Override)
                            {
                                case true:
                                    Outline.Stroke = new SolidColorBrush(Colors.Red);
                                    break;
                                case false:
                                    Outline.Stroke = new SolidColorBrush(Colors.Green);
                                    break;
                            }
                            break;
                        case 4: // HotBarRight
                            switch (A_Collection.Me.AutoCastOverrides.AutoCastRMBOverride)
                            {
                                case true:
                                    Outline.Stroke = new SolidColorBrush(Colors.Red);
                                    break;
                                case false:
                                    Outline.Stroke = new SolidColorBrush(Colors.Green);
                                    break;
                            }
                            break;
                        case 5: // HotBarLeft
                            switch (A_Collection.Me.AutoCastOverrides.AutoCastLMBOverride)
                            {
                                case true:
                                    Outline.Stroke = new SolidColorBrush(Colors.Red);
                                    break;
                                case false:
                                    Outline.Stroke = new SolidColorBrush(Colors.Green);
                                    break;
                            }
                            break;
                    }
                    
                    Outline.Fill = Outline.Stroke;

                    
                    Canvas.SetTop(Outline, SkillButton.Top);
                    Canvas.SetLeft(Outline, SkillButton.Left);
                    
                    canvas1.Children.Add(Outline);

                    
                }
            }
            catch (Exception)
            {

            }
        }

        private static bool missingDefinitions = false;

        void DrawSkillButtonCross()
        {
            try
            {
                missingDefinitions = false;

                List<UXIcon> AllSkillButtons;
                lock (A_Collection.Skills.UI_Controls.SkillControls) AllSkillButtons = A_Collection.Skills.UI_Controls.SkillControls.ToList();

                Dictionary<int, int> EquippedSkills;
                lock (A_Collection.Me.HeroDetails.ActiveSkills)
                    EquippedSkills = A_Collection.Me.HeroDetails.ActiveSkills.Where(x => x.Key != 0).ToDictionary(x => x.Key, y => y.Value);

                List<SkillData> AllDefinitions;
                lock (A_Collection.SkillCastConditions.Custom.CustomDefinitions)
                    AllDefinitions = A_Collection.SkillCastConditions.Custom.CustomDefinitions.ToList();

                foreach (var equippedSkill in EquippedSkills)
                {
                    if(equippedSkill.Key == -1)
                        continue;

                    int PowerSNO = equippedSkill.Key;
                    UXIcon ButtonControl = AllSkillButtons.FirstOrDefault(x => x.GetPowerSnoId() == PowerSNO);
                    if (ButtonControl != null)
                    {
                        string HotbarSlot = ButtonControl.ToString();
                        bool isAutoCast = true;

                        switch (HotbarSlot)
                        {
                            case A_Enums.UIElements.SkillHotBar1:
                                isAutoCast = !A_Collection.Me.AutoCastOverrides.AutoCast1Override;
                                break;

                            case A_Enums.UIElements.SkillHotBar2:
                                isAutoCast = !A_Collection.Me.AutoCastOverrides.AutoCast2Override;
                                break;

                            case A_Enums.UIElements.SkillHotBar3:
                                isAutoCast = !A_Collection.Me.AutoCastOverrides.AutoCast3Override;
                                break;

                            case A_Enums.UIElements.SkillHotBar4:
                                isAutoCast = !A_Collection.Me.AutoCastOverrides.AutoCast4Override;
                                break;

                            case A_Enums.UIElements.SkillHotBarRightclick:
                                isAutoCast = !A_Collection.Me.AutoCastOverrides.AutoCastRMBOverride;
                                break;

                            case A_Enums.UIElements.SkillHotBarLeftclick:
                                isAutoCast = !A_Collection.Me.AutoCastOverrides.AutoCastLMBOverride;
                                break;
                        }

                        if (isAutoCast &&
                            AllDefinitions.FirstOrDefault(
                                x => x.Power.PowerSNO == PowerSNO && (x.SelectedRune.RuneIndex == -1 || x.SelectedRune.RuneIndex == equippedSkill.Value) && x.CastConditions.Count > 0) == null)
                        {
                            missingDefinitions = true;

                            UIRect HotbarButton = A_Tools.T_D3UI.UIElement.getRect(ButtonControl.ToString());
                            DrawLine(HotbarButton.Left, HotbarButton.Top, HotbarButton.Right, HotbarButton.Bottom, 2,
                                Colors.Red);
                            DrawLine(HotbarButton.Right, HotbarButton.Top, HotbarButton.Left, HotbarButton.Bottom, 2,
                                Colors.Red);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        public static Uri getResourceUri(string filename)
        {
            Uri uri = new Uri("pack://application:,,,/Resources/" + filename + ".png");
            return uri;
        }
        
        public static List<RenderObjectRectangle> rectRenderTargets = new List<RenderObjectRectangle>();
        public static List<RenderObjectTextBlock> textRenderTargets = new List<RenderObjectTextBlock>();

        public static List<RenderObjectRectangle> rectSkillButtonRenderTargets = new List<RenderObjectRectangle>();
        public static Dictionary<Canvas, UIRect> canvasSkillButtonImageTargets = new Dictionary<Canvas, UIRect>();

        public static double renderFramesPerSecond = 0;


        void populateTextRenderTargets()
        {

            double FontSize = (double)(d3clientrect.Height * 13 / 1000);

            textRenderTargets.Clear();

            if (Properties.Settings.Default.overlayfps && A_Collection.Me.HeroStates.isInGame)
            {
                Color TextColor = Colors.Red;
                Color BackgroundColor = Colors.Black;

                TextBlock tb_overlay = new TextBlock();
                tb_overlay.FontFamily = new FontFamily("Arial Black");
                tb_overlay.Text = "Overlay: " + renderFramesPerSecond.ToString("0") + /*"FPS"*/ "ms";
                tb_overlay.Foreground = new SolidColorBrush(TextColor);
                tb_overlay.Background = new SolidColorBrush(BackgroundColor);
                tb_overlay.FontSize = FontSize;

                textRenderTargets.Add(new RenderObjectTextBlock(tb_overlay, 0, 0));

                lock (A_Initialize.Th_ICollector.ProcessingTimes)
                {
                    var ICollector_ProcessingTimes = A_Initialize.Th_ICollector.ProcessingTimes.ToList();
                    if (ICollector_ProcessingTimes.Count() > 0)
                    {
                        TextBlock tb_ICollector = new TextBlock();
                        tb_ICollector.FontFamily = new FontFamily("Arial Black");
                        tb_ICollector.Text = "ICollector: " + ICollector_ProcessingTimes.Average(x => x).ToString("0.00") + /*"FPS"*/ "ms";
                        tb_ICollector.Foreground = new SolidColorBrush(TextColor);
                        tb_ICollector.Background = new SolidColorBrush(BackgroundColor);
                        tb_ICollector.FontSize = FontSize;

                        textRenderTargets.Add(new RenderObjectTextBlock(tb_ICollector, 0, (float)(0 + ((FontSize * 110 / 100)) * 1)));
                    }
                }

                //lock (A_Initialize.Th_ECollector.ProcessingTimes)
                //{
                //    var ECollector_ProcessingTimes = A_Initialize.Th_ECollector.ProcessingTimes.ToList();
                //    if (ECollector_ProcessingTimes.Count() > 0)
                //    {
                //        TextBlock tb_ECollector = new TextBlock();
                //        tb_ECollector.FontFamily = new FontFamily("Arial Black");
                //        tb_ECollector.Text = "ECollector: " + ECollector_ProcessingTimes.Average(x => x).ToString("0.00") + /*"FPS"*/ "ms";
                //        tb_ECollector.Foreground = new SolidColorBrush(TextColor);
                //        tb_ECollector.Background = new SolidColorBrush(BackgroundColor);
                //        tb_ECollector.FontSize = FontSize;

                //        textRenderTargets.Add(new RenderObjectTextBlock(tb_ECollector, 0, (float)(0 + ((FontSize * 110 / 100)) * 2)));
                //    }
                //}

                lock (A_Initialize.Th_Handler.ProcessingTimes)
                {
                    var Handler_ProcessingTimes = A_Initialize.Th_Handler.ProcessingTimes.ToList();
                    if (Handler_ProcessingTimes.Count() > 0)
                    {
                        TextBlock tb_Handler = new TextBlock();
                        tb_Handler.FontFamily = new FontFamily("Arial Black");
                        tb_Handler.Text = "Handler: " + Handler_ProcessingTimes.Average(x => x).ToString("0.00") + /*"FPS"*/ "ms";
                        tb_Handler.Foreground = new SolidColorBrush(TextColor);
                        tb_Handler.Background = new SolidColorBrush(BackgroundColor);
                        tb_Handler.FontSize = FontSize;

                        textRenderTargets.Add(new RenderObjectTextBlock(tb_Handler, 0, (float)(0 + ((FontSize * 110 / 100)) * 2)));
                    }
                }

                TimeSpan UppTime = new TimeSpan(DateTime.Now.Ticks - Window_Main.Start.Ticks);

                TextBlock tb_AppTime = new TextBlock();
                tb_AppTime.FontFamily = new FontFamily("Arial Black");
                tb_AppTime.Text = "App Uptime: " + UppTime.Hours + "h" + UppTime.Minutes + "m" + UppTime.Seconds + "s\t\t\tExceptions(" + A_Handler.Log.Exception.ExceptionCount + ")";
                tb_AppTime.Foreground = new SolidColorBrush(TextColor);
                tb_AppTime.Background = new SolidColorBrush(BackgroundColor);
                tb_AppTime.FontSize = FontSize;

                textRenderTargets.Add(new RenderObjectTextBlock(tb_AppTime, 0, (float)(0 + ((FontSize * 110 / 100)) * 3)));

                //lock(A_Initialize.Th_HotkeyProcessor.ProcessingTimes)
                //{
                //    var HotkeyProcessor_ProcessingTimes = A_Initialize.Th_HotkeyProcessor.ProcessingTimes.ToList();
                //    if (HotkeyProcessor_ProcessingTimes.Count() > 0)
                //    {
                //        TextBlock tb_HotkeyProcessor = new TextBlock();
                //        tb_HotkeyProcessor.FontFamily = new FontFamily("Arial Black");
                //        tb_HotkeyProcessor.Text = "HotkeyProcessor: " + HotkeyProcessor_ProcessingTimes.Average(x => x).ToString("0.00") + /*"FPS"*/ "ms";
                //        tb_HotkeyProcessor.Foreground = new SolidColorBrush(Colors.GreenYellow);
                //        tb_HotkeyProcessor.FontSize = FontSize;

                //        textRenderTargets.Add(new RenderObjectTextBlock(tb_HotkeyProcessor, 0, (float)(0 + ((FontSize * 110 / 100)) * 4)));
                //    }
                //}






            }
            //---------------------
            if (Properties.Settings.Default.overlayxptracker)
            {
                UIRect resGlobe = tryGetResourceGlobeUIRect();

                int PlayerParagon = A_Collection.Me.HeroGlobals.Alt_Lvl;
                double CurrentTotalXP = A_Collection.Stats.Player.TotalXP;
                double Progression = A_Collection.Stats.Player.Progression;
                double NextRoundedHundred = A_Collection.Stats.Player.NextRoundedHundred;
                double NextRoundedHundredTotalXP = A_Collection.Stats.Player.NextRoundedHundredTotalXP;

                TextBlock tbtotalxpforeground = new TextBlock();
                tbtotalxpforeground.FontFamily = new FontFamily("Arial");
                tbtotalxpforeground.Text = "P" + PlayerParagon + "(" + ToStringBn(CurrentTotalXP, _postFixes) + ") =" + Progression.ToString("0.0") + "%=> P" + NextRoundedHundred + "(" + ToStringBn(NextRoundedHundredTotalXP, _postFixes) + ")";
                tbtotalxpforeground.Foreground = new SolidColorBrush(Colors.White);
                tbtotalxpforeground.FontSize = FontSize;


                textRenderTargets.Add(new RenderObjectTextBlock(tbtotalxpforeground, (float)canvas1.Margin.Left + (A_Collection.D3Client.Window.D3ClientRect.Width * 22/100), (float)canvas1.Margin.Top + (A_Collection.D3Client.Window.D3ClientRect.Height * 15/1000)));
            }




        }
        public static UIRect tryGetResourceGlobeUIRect()
        {
            if (A_Collection.Me.HeroStates.isInGame)
            {
                string hudoverlay = "Root.NormalLayer.game_dialog_backgroundScreenPC.game_window_manaAreaEater";

                UIRect rect = A_Tools.T_D3UI.UIElement.getRect(hudoverlay);

                return rect;
            }
            return new UIRect();
        }

        public static void populateRectRenderTargets()
        {
            //try
            //{
                rectRenderTargets.Clear();

                if (A_Collection.D3UI.isOpenInventory && A_Collection.Me.HeroStates.isInGame)
                {
                    //Collector.InventoryCollector.createStashMesh();
                    //Collector.InventoryCollector.createInventoryMesh();
                    //Collector.InventoryCollector.highlightInventoryItems();
                    
                    var inventoryMesh = A_Collection.D3UI.InventoryItemUIRectMesh;
                    /*
                    var stashMesh = Collector.InventoryCollector.StashSlotMesh;

                    foreach (var item in Collector.InventoryCollector.HighlightedInventoryItems)
                    {
                        UIRect itemRect = inventoryMesh.FirstOrDefault(x => x.Key.ItemSlotX == item.ItemSlotX && x.Key.ItemSlotY == item.ItemSlotY).Value;

                        Rectangle rect = new Rectangle();
                        rect.Width = itemRect.Right - itemRect.Left;
                        rect.Height = itemRect.Bottom - itemRect.Top;
                        rect.Stroke = new SolidColorBrush(Colors.White);
                        rect.StrokeThickness = 1;

                        rectRenderTargets.Add(new RenderObjectRectangle(rect, itemRect.Left, itemRect.Top));
                    }
                     */ 
                    /*
                    UIRect stash = Collector.InventoryCollector.getStashPageUIRect();

                    Rectangle stashrect = new Rectangle();
                    stashrect.Width = stash.Right - stash.Left;
                    stashrect.Height = stash.Bottom - stash.Top;
                    stashrect.Stroke = new SolidColorBrush(Colors.Green);
                    stashrect.StrokeThickness = 4;

                    rectRenderTargets.Add(new RenderObjectRectangle(stashrect, stash.Left, stash.Top));
                    
                    foreach(var stashslot in stashMesh)
                    {
                        UIRect itemRect = stashslot.Value;

                        Rectangle rect = new Rectangle();
                        rect.Width = itemRect.Right - itemRect.Left;
                        rect.Height = itemRect.Bottom - itemRect.Top;
                        rect.Stroke = new SolidColorBrush(Colors.White);
                        rect.StrokeThickness = 0.5;

                        rectRenderTargets.Add(new RenderObjectRectangle(rect, itemRect.Left, itemRect.Top));
                    }
                    */


                }
            //}
            //catch (Exception e) { Console.WriteLine(e.Message); }
        }
        static string ToStringBn(double value, string[] postFixes)
        {
            int index = 0;
            while (value >= 1000)
            {
                value /= 1000;
                if (++index == postFixes.Length - 1)
                    break;
            }
            return value.ToString("0.00", CultureInfo.InvariantCulture) + postFixes[index];
        }
    }
    
    public class RenderObjectRectangle
    {
        public RenderObjectRectangle(Rectangle rect, float posleft, float postop)
        {
            this.Rect = rect;
            this.PosLeft = posleft;
            this.PosTop = postop;

        }

        public Rectangle Rect { get; set; }
        public float PosLeft { get; set; }
        public float PosTop { get; set; }

    }
    public class RenderObjectTextBlock
    {
        public RenderObjectTextBlock(TextBlock text, float posleft, float postop)
        {
            this.Text = text;
            this.PosLeft = posleft;
            this.PosTop = postop;

        }

        public TextBlock Text { get; set; }
        public float PosLeft { get; set; }
        public float PosTop { get; set; }

    }
    public static class TransparentWindowHelper
    {
        public static void MakeTransparent(this Window window)
        {
            if (window.IsInitialized)
                throw new InvalidOperationException("Can only make a window transparent before it has been initialized.");

            window.AllowsTransparency = true;
            window.WindowStyle = WindowStyle.None;
            if (window.Background == null) window.Background = Brushes.Transparent;

            window.SourceInitialized += (s, e) =>
            {
                var hwnd = new WindowInteropHelper((Window)s).Handle;
                SetWindowStyleTransparent(hwnd);
            };
        }

        private static void SetWindowStyleTransparent(IntPtr windowHandle)
        {
            const int WS_EX_TRANSPARENT = 0x00000020;
            const int GWL_EXSTYLE = -20;

            var exStyle = Win32.GetWindowLong(windowHandle, GWL_EXSTYLE);
            Win32.SetWindowLong(windowHandle, GWL_EXSTYLE, exStyle | WS_EX_TRANSPARENT);
        }
        private static class Win32
        {
            private const string User32 = "user32.dll";

            [DllImport(User32)]
            internal static extern int GetWindowLong(IntPtr windowHandle, int index);

            [DllImport(User32)]
            internal static extern int SetWindowLong(IntPtr windowHandle, int index, int newStyle);
        }


        
    }
}
