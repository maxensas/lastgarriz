using Run.Util;
using Run.ViewModels.Command;
using System;
using System.Windows;
using System.Windows.Media;

namespace Run.ViewModels
{
    public sealed class RocketViewModel : BaseViewModel
    {
        private string indicator = string.Empty;
        private string kind = string.Empty;
        private bool rocketZook;
        private bool showWindow;
        private bool showDisclaimer = true;
        private bool showNoHoldValues;
        private bool showMaxNoHoldSchreck;
        private string disclaimerText = string.Empty;
        private SolidColorBrush disclaimerColor = Brushes.Lime;
        private SolidColorBrush crosshairColor = Brushes.Lime;
        private int xhair = 0;
        private Crosshair crosshairValue;

        public string Indicator { get => indicator; set => SetProperty(ref indicator, value); }
        public string Kind { get => kind; set => SetProperty(ref kind, value); }
        public bool RocketZook { get => rocketZook; set => SetProperty(ref rocketZook, value); }
        public bool ShowWindow { get => showWindow; set => SetProperty(ref showWindow, value); }
        public bool ShowDisclaimer { get => showDisclaimer; set => SetProperty(ref showDisclaimer, value); }
        public bool ShowNoHoldValues { get => showNoHoldValues; set => SetProperty(ref showNoHoldValues, value); }
        public bool ShowMaxNoHoldSchreck { get => showMaxNoHoldSchreck; set => SetProperty(ref showMaxNoHoldSchreck, value); }
        public string DisclaimerText { get => disclaimerText; set => SetProperty(ref disclaimerText, value); }
        public SolidColorBrush DisclaimerColor { get => disclaimerColor; set => SetProperty(ref disclaimerColor, value); }
        public SolidColorBrush CrosshairColor { get => crosshairColor; set => SetProperty(ref crosshairColor, value); }
        public int Xhair { get => xhair; set => SetProperty(ref xhair, value); }
        public Crosshair CrosshairValue { get => crosshairValue; set => SetProperty(ref crosshairValue, value); }

        public RocketCommand Commands { get; private set; }

        public RocketViewModel(bool isSchreck)
        {
            Commands = new(this);
            RocketZook = isSchreck;
            DisclaimerText = isSchreck ? Strings.Aim.DISCLAIMER_PANZERSCHRECK_ON : Strings.Aim.DISCLAIMER_BAZOOKA_ON;
            //TaskManager.StartMouseCatcherTask(this);
            crosshairValue = new(isSchreck);
            ShowNoHoldValues = DataManager.Instance.Config.Options.ShowNoHoldValues;
            ShowMaxNoHoldSchreck = RocketZook && ShowNoHoldValues;
        }

        public sealed class Crosshair : BaseViewModel
        {
            private int height = Convert.ToInt32(SystemParameters.PrimaryScreenHeight/2);
            private int txtNoHold075;
            private int txtNoHold100;
            private int txtNoHold125;
            private int txtNoHold150;
            private int txtNoHold175;
            private int txtNoHold200;
            private int txtNoHold960;
            private int txtHold100;
            private int txtHold150;
            private int txtHold200;
            private int txtHold250;
            private int txtHold300;
            private int txtHold350;
            private int txtHold400;
            private int txtHold450;
            private int txtHold500;
            private int txtHold550;
            private int txtHold600;
            private int txtHold650;
            private int txtHold700;
            private int txtHold750;
            private int lineNoHold075;
            private int lineNoHold100;
            private int lineNoHold125;
            private int lineNoHold150;
            private int lineNoHold175;
            private int lineNoHold200;
            private int lineHold100;
            private int lineHold150;
            private int lineHold200;
            private int lineHold250;
            private int lineHold300;
            private int lineHold350;
            private int lineHold400;
            private int lineHold450;
            private int lineHold500;
            private int lineHold550;
            private int lineHold600;
            private int lineHold650;
            private int lineHold700;
            private int lineHold750;
            private string txtLast;

            public int Height { get => height; set => SetProperty(ref height, value); }
            public int TxtNoHold075 { get => txtNoHold075; set => SetProperty(ref txtNoHold075, value); }
            public int TxtNoHold100 { get => txtNoHold100; set => SetProperty(ref txtNoHold100, value); }
            public int TxtNoHold125 { get => txtNoHold125; set => SetProperty(ref txtNoHold125, value); }
            public int TxtNoHold150 { get => txtNoHold150; set => SetProperty(ref txtNoHold150, value); }
            public int TxtNoHold175 { get => txtNoHold175; set => SetProperty(ref txtNoHold175, value); }
            public int TxtNoHold200 { get => txtNoHold200; set => SetProperty(ref txtNoHold200, value); }
            public int TxtNoHold960 { get => txtNoHold960; set => SetProperty(ref txtNoHold960, value); }
            public int TxtHold100 { get => txtHold100; set => SetProperty(ref txtHold100, value); }
            public int TxtHold150 { get => txtHold150; set => SetProperty(ref txtHold150, value); }
            public int TxtHold200 { get => txtHold200; set => SetProperty(ref txtHold200, value); }
            public int TxtHold250 { get => txtHold250; set => SetProperty(ref txtHold250, value); }
            public int TxtHold300 { get => txtHold300; set => SetProperty(ref txtHold300, value); }
            public int TxtHold350 { get => txtHold350; set => SetProperty(ref txtHold350, value); }
            public int TxtHold400 { get => txtHold400; set => SetProperty(ref txtHold400, value); }
            public int TxtHold450 { get => txtHold450; set => SetProperty(ref txtHold450, value); }
            public int TxtHold500 { get => txtHold500; set => SetProperty(ref txtHold500, value); }
            public int TxtHold550 { get => txtHold550; set => SetProperty(ref txtHold550, value); }
            public int TxtHold600 { get => txtHold600; set => SetProperty(ref txtHold600, value); }
            public int TxtHold650 { get => txtHold650; set => SetProperty(ref txtHold650, value); }
            public int TxtHold700 { get => txtHold700; set => SetProperty(ref txtHold700, value); }
            public int TxtHold750 { get => txtHold750; set => SetProperty(ref txtHold750, value); }
            public int LineNoHold075 { get => lineNoHold075; set => SetProperty(ref lineNoHold075, value); }
            public int LineNoHold100 { get => lineNoHold100; set => SetProperty(ref lineNoHold100, value); }
            public int LineNoHold125 { get => lineNoHold125; set => SetProperty(ref lineNoHold125, value); }
            public int LineNoHold150 { get => lineNoHold150; set => SetProperty(ref lineNoHold150, value); }
            public int LineNoHold175 { get => lineNoHold175; set => SetProperty(ref lineNoHold175, value); }
            public int LineNoHold200 { get => lineNoHold200; set => SetProperty(ref lineNoHold200, value); }
            public int LineHold100 { get => lineHold100; set => SetProperty(ref lineHold100, value); }
            public int LineHold150 { get => lineHold150; set => SetProperty(ref lineHold150, value); }
            public int LineHold200 { get => lineHold200; set => SetProperty(ref lineHold200, value); }
            public int LineHold250 { get => lineHold250; set => SetProperty(ref lineHold250, value); }
            public int LineHold300 { get => lineHold300; set => SetProperty(ref lineHold300, value); }
            public int LineHold350 { get => lineHold350; set => SetProperty(ref lineHold350, value); }
            public int LineHold400 { get => lineHold400; set => SetProperty(ref lineHold400, value); }
            public int LineHold450 { get => lineHold450; set => SetProperty(ref lineHold450, value); }
            public int LineHold500 { get => lineHold500; set => SetProperty(ref lineHold500, value); }
            public int LineHold550 { get => lineHold550; set => SetProperty(ref lineHold550, value); }
            public int LineHold600 { get => lineHold600; set => SetProperty(ref lineHold600, value); }
            public int LineHold650 { get => lineHold650; set => SetProperty(ref lineHold650, value); }
            public int LineHold700 { get => lineHold700; set => SetProperty(ref lineHold700, value); }
            public int LineHold750 { get => lineHold750; set => SetProperty(ref lineHold750, value); }
            public string TxtLast { get => txtLast; set => SetProperty(ref txtLast, value); }

            public Crosshair(bool isSchreck)
            {
                if (isSchreck)
                {
                    txtNoHold075 = Common.AdjustAbscissaValue(18);
                    txtNoHold100 = Common.AdjustAbscissaValue(34);
                    txtNoHold125 = Common.AdjustAbscissaValue(49);
                    txtNoHold150 = Common.AdjustAbscissaValue(59);
                    txtNoHold175 = Common.AdjustAbscissaValue(69);
                    txtNoHold200 = Common.AdjustAbscissaValue(89);
                    txtNoHold960 = Common.AdjustAbscissaValue(530);
                    txtHold100 = Common.AdjustAbscissaValue(42);
                    txtHold150 = Common.AdjustAbscissaValue(81);
                    txtHold200 = Common.AdjustAbscissaValue(116);
                    txtHold250 = Common.AdjustAbscissaValue(148);
                    txtHold300 = Common.AdjustAbscissaValue(186);
                    txtHold350 = Common.AdjustAbscissaValue(219);
                    txtHold400 = Common.AdjustAbscissaValue(252);
                    txtHold450 = Common.AdjustAbscissaValue(287);
                    txtHold500 = Common.AdjustAbscissaValue(322);
                    txtHold550 = Common.AdjustAbscissaValue(359);
                    txtHold600 = Common.AdjustAbscissaValue(395);
                    txtHold650 = Common.AdjustAbscissaValue(435);
                    txtHold700 = Common.AdjustAbscissaValue(474);
                    txtHold750 = Common.AdjustAbscissaValue(518);

                    lineNoHold075 = Common.AdjustAbscissaValue(24);
                    lineNoHold100 = Common.AdjustAbscissaValue(40);
                    lineNoHold125 = Common.AdjustAbscissaValue(55);
                    lineNoHold150 = Common.AdjustAbscissaValue(65);
                    lineNoHold175 = Common.AdjustAbscissaValue(75);
                    lineNoHold200 = Common.AdjustAbscissaValue(95);
                    lineHold100 = Common.AdjustAbscissaValue(48);
                    lineHold150 = Common.AdjustAbscissaValue(87);
                    lineHold200 = Common.AdjustAbscissaValue(122);
                    lineHold250 = Common.AdjustAbscissaValue(154);
                    lineHold300 = Common.AdjustAbscissaValue(192);
                    lineHold350 = Common.AdjustAbscissaValue(225);
                    lineHold400 = Common.AdjustAbscissaValue(258);
                    lineHold450 = Common.AdjustAbscissaValue(293);
                    lineHold500 = Common.AdjustAbscissaValue(328);
                    lineHold550 = Common.AdjustAbscissaValue(365);
                    lineHold600 = Common.AdjustAbscissaValue(401);
                    lineHold650 = Common.AdjustAbscissaValue(441);
                    lineHold700 = Common.AdjustAbscissaValue(480);
                    lineHold750 = Common.AdjustAbscissaValue(524);

                    txtLast = "750";
                    return;
                }
                txtNoHold075 = Common.AdjustAbscissaValue(18);
                txtNoHold100 = Common.AdjustAbscissaValue(32);
                txtNoHold125 = Common.AdjustAbscissaValue(46);
                txtNoHold150 = Common.AdjustAbscissaValue(57);
                txtNoHold175 = Common.AdjustAbscissaValue(66);
                txtNoHold200 = Common.AdjustAbscissaValue(84);
                txtNoHold960 = Common.AdjustAbscissaValue(0); //not used in US
                txtHold100 = Common.AdjustAbscissaValue(41);
                txtHold150 = Common.AdjustAbscissaValue(81);
                txtHold200 = Common.AdjustAbscissaValue(114);
                txtHold250 = Common.AdjustAbscissaValue(148);
                txtHold300 = Common.AdjustAbscissaValue(182);
                txtHold350 = Common.AdjustAbscissaValue(215);
                txtHold400 = Common.AdjustAbscissaValue(247);
                txtHold450 = Common.AdjustAbscissaValue(283);
                txtHold500 = Common.AdjustAbscissaValue(318);
                txtHold550 = Common.AdjustAbscissaValue(355);
                txtHold600 = Common.AdjustAbscissaValue(393);
                txtHold650 = Common.AdjustAbscissaValue(431);
                txtHold700 = Common.AdjustAbscissaValue(469);
                txtHold750 = Common.AdjustAbscissaValue(494); // 730 in US

                lineNoHold075 = Common.AdjustAbscissaValue(24);
                lineNoHold100 = Common.AdjustAbscissaValue(38);
                lineNoHold125 = Common.AdjustAbscissaValue(52);
                lineNoHold150 = Common.AdjustAbscissaValue(63);
                lineNoHold175 = Common.AdjustAbscissaValue(72);
                lineNoHold200 = Common.AdjustAbscissaValue(90);
                lineHold100 = Common.AdjustAbscissaValue(47);
                lineHold150 = Common.AdjustAbscissaValue(87);
                lineHold200 = Common.AdjustAbscissaValue(120);
                lineHold250 = Common.AdjustAbscissaValue(154);
                lineHold300 = Common.AdjustAbscissaValue(188);
                lineHold350 = Common.AdjustAbscissaValue(221);
                lineHold400 = Common.AdjustAbscissaValue(253);
                lineHold450 = Common.AdjustAbscissaValue(289);
                lineHold500 = Common.AdjustAbscissaValue(324);
                lineHold550 = Common.AdjustAbscissaValue(361);
                lineHold600 = Common.AdjustAbscissaValue(399);
                lineHold650 = Common.AdjustAbscissaValue(437);
                lineHold700 = Common.AdjustAbscissaValue(475);
                lineHold750 = Common.AdjustAbscissaValue(500); // 730 in US

                txtLast = "730";
            }
        }
    }
}
