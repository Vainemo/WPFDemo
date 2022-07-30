using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFDemoControls
{
    /// <summary>
    /// Instrument.xaml 的交互逻辑
    /// </summary>
    public partial class Instrument : UserControl
    {
        //依赖属性,依赖对象
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty=DependencyProperty.Register("Value",typeof(int),typeof(Instrument),new PropertyMetadata( default(int),new PropertyChangedCallback(OnPorpertyCganged)));
        public static void OnPorpertyCganged(DependencyObject d,DependencyPropertyChangedEventArgs e)
        {
            (d as Instrument).Refresh();
        }


        public Brush ScaleBrush
        {
            get { return (Brush)GetValue(ScaleBrushProperty); }
            set { SetValue(ScaleBrushProperty, value); }
        }
        public static readonly DependencyProperty ScaleBrushProperty =
            DependencyProperty.Register("ScaleBrush", typeof(Brush), typeof(Instrument), new PropertyMetadata(default(Brush), new PropertyChangedCallback(OnPorpertyCganged)));




        public int ScaleText
        {
            get { return (int)GetValue(ScaleTextProperty); }
            set { SetValue(ScaleTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScaleText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScaleTextProperty =
            DependencyProperty.Register("ScaleText", typeof(int), typeof(Instrument), new PropertyMetadata(default(int), new PropertyChangedCallback(OnPorpertyCganged)));


        public Brush PlateBackGround
        {
            get { return (Brush)GetValue(PlateBackGroundProperty); }
            set { SetValue(PlateBackGroundProperty, value); }
        }

        public static readonly DependencyProperty PlateBackGroundProperty =
            DependencyProperty.Register("PlateBackGround", typeof(Brush), typeof(Instrument), new PropertyMetadata(default(Brush)));




        public int Minimum
        {
            get { return (int)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

       
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(int), typeof(Instrument), new PropertyMetadata(OnPorpertyCganged));



        public int Maxmum
        {
            get { return (int)GetValue(MaxmumProperty); }
            set { SetValue(MaxmumProperty, value); }
        }

      
        public static readonly DependencyProperty MaxmumProperty =
            DependencyProperty.Register("Maxmum", typeof(int), typeof(Instrument), new PropertyMetadata(OnPorpertyCganged));



        public int Interval
        {
            get { return (int)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Interval.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IntervalProperty =
            DependencyProperty.Register("Interval", typeof(int), typeof(Instrument), new PropertyMetadata(default(int),new
                 PropertyChangedCallback(OnPorpertyCganged)));


        public Instrument()
        {
            InitializeComponent();
            this.SizeChanged += Instrument_SizeChanged;
        }

        private void Instrument_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double minSize=Math.Min(this.RenderSize.Width, this.RenderSize.Height);
            this.backEllipse.Width =minSize;
            this.backEllipse.Height =minSize;
        }
        public void Refresh()
        {
            double radius = this.backEllipse.Width / 2;
            if (double.IsNaN(radius))
            {
                return;
            }
            this.mainCanvas.Children.Clear();
            double step = 270.0 / (this.Maxmum - this.Minimum);
            for (int i = 0; i < (this.Maxmum - this.Minimum); i++)
            {
                Line lineScale=new Line();
                lineScale.X1 = radius-(radius-13) * Math.Cos((i * step-45) * Math.PI / 180);
                lineScale.Y1 = radius-(radius - 13) * Math.Sin((i * step-45) * Math.PI / 180);
                lineScale.X2 = radius- (radius - 8) * Math.Cos((i * step -45) * Math.PI / 180);
                lineScale.Y2 = radius- (radius - 8) * Math.Sin((i * step -45) * Math.PI / 180);
                lineScale.Stroke = this.ScaleBrush;
                lineScale.StrokeThickness=1;
                this.mainCanvas.Children.Add(lineScale);

            }
            step = 270.0 / Interval;
            int scaleText =Minimum;
            //绘制十位整数格和相应数值
            for (int i = 0; i <=Interval; i++)
            {
                Line lineScale = new Line();
                lineScale.X1 = radius - (radius - 20) * Math.Cos((i * step - 45) * Math.PI / 180);
                lineScale.Y1 = radius - (radius - 20) * Math.Sin((i * step - 45) * Math.PI / 180);
                lineScale.X2 = radius - (radius - 8) * Math.Cos((i * step - 45) * Math.PI / 180);
                lineScale.Y2 = radius - (radius - 8) * Math.Sin((i * step - 45) * Math.PI / 180);
                lineScale.Stroke =this.ScaleBrush;
                lineScale.StrokeThickness = 1;
                this.mainCanvas.Children.Add(lineScale);
                TextBlock textScale = new TextBlock();
                textScale.Width = 34;
                textScale.TextAlignment = TextAlignment.Center;
                textScale.FontSize = this.ScaleText;
                textScale.Text = (scaleText + ((this.Maxmum - this.Minimum) / Interval) * i).ToString();
                textScale.Foreground=this.ScaleBrush;
                Canvas.SetLeft(textScale, radius - (radius - 36) * Math.Cos((i * step - 45) * Math.PI / 180)-17);
                Canvas.SetTop(textScale, radius - (radius - 36) * Math.Sin((i * step - 45) * Math.PI / 180)-10);
                this.mainCanvas.Children.Add(textScale);
            }
            string Data = "M{0} {1} A{0} {0} 0 1 1 {1} {2}";
            Data=String.Format(Data, radius / 2,radius,radius*1.5);
            var converter = TypeDescriptor.GetConverter(typeof(Geometry));
            this.circle.Data = (Geometry)converter.ConvertFrom(Data);
            DoubleAnimation da = new DoubleAnimation((int)(Value-Minimum) * step - 45, new Duration(TimeSpan.FromMilliseconds(200)));
            this.rtPointer.BeginAnimation(RotateTransform.AngleProperty, da);
            Data = "M{0} {1},{1} {2},{1} {3} Z";
            Data = String.Format(Data, radius*0.4,radius,radius-5, radius+5);
            this.pointer.Data = (Geometry)converter.ConvertFrom(Data);
        }
    }
}
