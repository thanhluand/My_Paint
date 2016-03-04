using System;
using System.Collections.Generic;
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


using System.IO;


using Microsoft.Win32;
using System.Windows.Controls.Primitives;
//using Xceed.Wpf.Toolkit;


namespace WpfPaint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeMyPaint();
            
        }

        public void InitializeMyPaint()
        {
            size = 2;
            sizeText = 15;
            RBPENCIL.IsEnabled = false;
            cl = Colors.Black;
            brt = Brushes.Black;
            br = Colors.White;
           
            
            ClPickerText.SelectedColor = cl;
            ClPicker.SelectedColor = cl;
            Undo.IsEnabled = false;
            Redo.IsEnabled = false;
            RBCOPY.IsEnabled = false;

            Font.Items.Add("Times New Roman");
            Font.Items.Add("Snap ITC");
            Font.Items.Add("Gigi");
            Font.Items.Add("Gill Sans Ultra Bold");
            Font.Items.Add("DotumChe");
            Font.Text = "Times New Roman";

            SizeText.Items.Add("10");
            SizeText.Items.Add("15");
            SizeText.Items.Add("20");
            SizeText.Items.Add("25");
            SizeText.Items.Add("30");
            SizeText.Text = "15";

            CBBFill.Items.Add("No Fill");
            CBBFill.Items.Add("Solid Fill");
            CBBFill.Items.Add("HorizontalRadialGradient");
            CBBFill.Items.Add("VerticalRadialGradient");
            CBBFill.Items.Add("RadialGradientBrush");
            CBBFill.Items.Add("LinearGradientBrush");
            CBBFill.Text = "No Fill";

            CBBSize.Items.Add("2");
            CBBSize.Items.Add("5");
            CBBSize.Items.Add("10");
            CBBSize.Items.Add("15");
            CBBSize.Items.Add("20");
            CBBSize.Items.Add("25");
            CBBSize.Text = "2";


            myHorizontalGradient.StartPoint = new Point(0, 0.5);
            myHorizontalGradient.EndPoint = new Point(1, 0.5);
            myHorizontalGradient.GradientStops.Add(new GradientStop(Colors.Yellow, 0.0));
            myHorizontalGradient.GradientStops.Add(new GradientStop(Colors.Red, 0.25));
            myHorizontalGradient.GradientStops.Add(new GradientStop(Colors.Blue, 0.75));
            myHorizontalGradient.GradientStops.Add(new GradientStop(Colors.LimeGreen, 1.0));

            myVerticalGradient.StartPoint = new Point(0.5, 0);
            myVerticalGradient.EndPoint = new Point(0.5, 1);
            myVerticalGradient.GradientStops.Add(new GradientStop(Colors.Yellow, 0.0));
            myVerticalGradient.GradientStops.Add(new GradientStop(Colors.Red, 0.25));
            myVerticalGradient.GradientStops.Add(new GradientStop(Colors.Blue, 0.75));
            myVerticalGradient.GradientStops.Add(new GradientStop(Colors.LimeGreen, 1.0));

            myLinearGradientBrush.StartPoint = new Point(0, 0);
            myLinearGradientBrush.EndPoint = new Point(1, 1);
            myLinearGradientBrush.GradientStops.Add(new GradientStop(Colors.Yellow, 0.0));
            myLinearGradientBrush.GradientStops.Add(new GradientStop(Colors.Red, 0.25));
            myLinearGradientBrush.GradientStops.Add(new GradientStop(Colors.Blue, 0.75));
            myLinearGradientBrush.GradientStops.Add(new GradientStop(Colors.LimeGreen, 1.0));


            myRadialGradientBrush.GradientOrigin = new Point(0.5, 0.5);
            myRadialGradientBrush.Center = new Point(0.5, 0.5);
            myRadialGradientBrush.RadiusX = 0.5;
            myRadialGradientBrush.RadiusY = 0.5;
            myRadialGradientBrush.GradientStops.Add(new GradientStop(Colors.Yellow, 0.0));
            myRadialGradientBrush.GradientStops.Add(new GradientStop(Colors.Red, 0.25));
            myRadialGradientBrush.GradientStops.Add(new GradientStop(Colors.Blue, 0.75));
            myRadialGradientBrush.GradientStops.Add(new GradientStop(Colors.LimeGreen, 1.0));
        }

       // khởi tạo các biến
       //Stack<Image> undoImage = new Stack<Image>(), redoImage = new Stack<Image>();
       Image ImageSelect = new Image();
        BitmapSource bs;
        RenderTargetBitmap StackBitmap;
        BitmapImage bmfs = new BitmapImage();    
        Stack<RenderTargetBitmap> undoImage = new Stack<RenderTargetBitmap>(), redoImage = new Stack<RenderTargetBitmap>();
               
        Random randoms = new Random();
        Point startPoint;

        int sizeText = 15;
        int slud = 0, slrd = 0;
        int flag = 0;
        int flagBrush = 0;
        int flagFill = 0;
        int size;
        int TypeFill = 0;

       
       
        ContentControl ccm = new ContentControl();
        FontFamily f = new FontFamily("Times New Roman");
        TextBox Teb = new TextBox();
        Label lb;
        Polyline pll = new Polyline();
        Polygon plg = new Polygon();
        System.Windows.Shapes.Path path = new System.Windows.Shapes.Path();
        System.Windows.Shapes.Path path2 = new System.Windows.Shapes.Path();
        Line lastLine = new Line();
        Ellipse lastElip = new Ellipse();
        Rectangle LastRectangle = new Rectangle();
        Ellipse elip = new Ellipse();
        Rectangle rec = new Rectangle();
        Polyline pll2 = new Polyline();
        Line line = new Line();

        LinearGradientBrush myLinearGradientBrush = new LinearGradientBrush();
        RadialGradientBrush myRadialGradientBrush = new RadialGradientBrush();
        LinearGradientBrush myHorizontalGradient = new LinearGradientBrush();
        LinearGradientBrush myVerticalGradient = new LinearGradientBrush();
        Color cl, br;
        Brush brt;

        bool firstSave = true;
        bool isMouseDowned = false;
        bool isnew = true;
        bool isChangedType = false;
        bool Tbold = false, TItalic = false, Tunderline = false;

        // default các nut button
        public void DefaultButton()
        {
            RBPENCIL.IsEnabled = true;
            RBLINE.IsEnabled = true;
            RBELLIPSe.IsEnabled = true;
            RBRECTANGLE.IsEnabled = true;
            RBCIRCLE.IsEnabled = true;
            RBSQUARE.IsEnabled = true;
            RBSELECT.IsEnabled = true;
            RBLEFT.IsEnabled = true;
            RBRIGHT.IsEnabled = true;
            RBBOTTOM.IsEnabled = true;
            RBHEART.IsEnabled = true;
            RBSTAR.IsEnabled = true;
            TBTOP.IsEnabled = true;
            RBFILL.IsEnabled = true;
            RBTEXT.IsEnabled = true;
            PaintCanvas.Background = Brushes.White;

        }

        // thuật toán to loang
  

         public Color GetPixel(double x, double y, int stride, byte[] bm)
        {
            int index1 = ((int)x) * 4 + (int)y * stride;
            Color clrt = Color.FromRgb(bm[index1 + 2], bm[index1 + 1], bm[index1]);
            return clrt;
        }
     

        public void SetPixel(double x, double y, int stride, byte[] bm, Color Fill)
        {
            int index1 = ((int)x) * 4 + (int)y * stride;
            bm[index1] = Fill.B;
            bm[index1 + 1] = Fill.G;
            bm[index1 + 2] = Fill.R;
            Color clrt = Color.FromRgb(bm[index1 + 2], bm[index1 + 1], bm[index1]);
        }

        double nMinX = 0, nMaxX = 1350, nMinY = 150, nMaxY = 685;
   
        void floodFillScanline(double x, double y, Color Fill, Color seed_color,  int stride, byte[] bm)
        {
            if (seed_color == Fill) return;
            if (GetPixel(x, y, stride, bm) != seed_color) return;

            double y1;

            //draw current scanline from start position to the top
            y1 = y;
            while (y1 < nMaxY && GetPixel(x, y1, stride, bm) == seed_color)
            {
                SetPixel(x,y1,stride,bm,Fill);
                y1++;
            }

            //draw current scanline from start position to the bottom
            y1 = y - 1;
            while (y1 >= 0 && GetPixel(x, y1, stride, bm) == seed_color)
            {
                SetPixel(x, y1, stride, bm, Fill);
                y1--;
            }

            //test for new scanlines to the left
            y1 = y;
            while (y1 < nMaxY && GetPixel(x, y1, stride, bm) == Fill)
            {
                if (x > 0 && GetPixel(x- 1, y1, stride, bm) == seed_color)
                {
                    floodFillScanline(x - 1, y1, Fill, seed_color,stride,bm);
                }
                y1++;
            }
            y1 = y - 1;
            while (y1 >= 0 && GetPixel(x, y1, stride, bm) == Fill)
            {
                if (x > 0 && GetPixel(x- 1, y1, stride, bm) == seed_color)
                {
                    floodFillScanline(x - 1, y1, Fill, seed_color, stride, bm);
                }
                y1--;
            }

            //test for new scanlines to the right 
            y1 = y;
            while (y1 < nMaxY && GetPixel(x, y1, stride, bm) == Fill)
            {
                if (x < nMaxX - 1 && GetPixel(x + 1, y1, stride, bm) == seed_color)
                {
                    floodFillScanline(x + 1, y1, Fill, seed_color, stride, bm);
                }
                y1++;
            }
            y1 = y - 1;
            while (y1 >= 0 && GetPixel(x, y1, stride, bm) == Fill)
            {
                if (x < nMaxX - 1 && GetPixel(x + 1, y1, stride, bm) == seed_color)
                {
                    floodFillScanline(x + 1, y1, Fill, seed_color, stride, bm);
                }
                y1--;
            }
        }
        private void PaintCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (flag ==12)
            {
                nMaxX = PaintCanvas.Width;
                nMaxY = PaintCanvas.Height;
                startPoint = e.GetPosition((IInputElement)sender);
                RenderTargetBitmap rdbm  =  new RenderTargetBitmap((int)PaintCanvas.RenderSize.Width, (int)PaintCanvas.RenderSize.Height, 96, 96, PixelFormats.Pbgra32);
                rdbm.Render(PaintCanvas);
                
                int height = rdbm.PixelHeight;
                int width = rdbm.PixelWidth;
                int nStride = (rdbm.PixelWidth * rdbm.Format.BitsPerPixel + 7) / 8;
                byte[] pixelByteArray = new byte[rdbm.PixelHeight * nStride];
                rdbm.CopyPixels(pixelByteArray, nStride, 0);
              
                int index = (int)startPoint.X * 4 + (int)startPoint.Y* nStride;
              
                
               
               
                 Color crfs = Color.FromRgb(pixelByteArray[index + 2], pixelByteArray[index + 1], pixelByteArray[index ]);

        

                if (br != crfs)
                    floodFillScanline(startPoint.X, startPoint.Y, br, crfs, nStride, pixelByteArray);
                System.Windows.Int32Rect rcFrom = new System.Windows.Int32Rect();
                rcFrom.X = 0;
                rcFrom.Y = 0;
                rcFrom.Width = (int)PaintCanvas.RenderSize.Width;
                rcFrom.Height = (int)PaintCanvas.RenderSize.Height;
                WriteableBitmap wab = new WriteableBitmap((int)PaintCanvas.RenderSize.Width, (int)PaintCanvas.RenderSize.Height, 96, 96, PixelFormats.Pbgra32,null);
                wab.WritePixels(rcFrom, pixelByteArray, nStride, 0);

                Image imfs = new Image();
                imfs.Source = wab;

                PaintCanvas.Children.Add(imfs);

                return;
            }
            if (flag == 11)
            {
                if (isnew == true)
                {
                   

                    startPoint = e.GetPosition((IInputElement)sender);
                  
                    Teb.FontSize = sizeText;
                    Teb.IsHitTestVisible = true;
                  
                    if (TItalic)
                        Teb.FontStyle = FontStyles.Italic;
                    else
                        Teb.FontStyle = FontStyles.Normal;
                    if (Tbold)
                        Teb.FontWeight = FontWeights.Bold;
                    else
                        Teb.FontWeight = FontWeights.Normal;
                    SolidColorBrush brtf = new SolidColorBrush(br);
                    Teb.Background = brtf;
                    ccm.Content = Teb;
                    ccm.Style = this.FindResource("DesignerItemStyle") as Style;
                    ccm.Height = Math.Abs(23);
                    ccm.Width = Math.Abs(100);
                    Canvas.SetLeft(ccm, startPoint.X);
                    Canvas.SetTop(ccm, startPoint.Y);
                    Selector.SetIsSelected(ccm, true);
                   
                    PaintCanvas.Children.Add(ccm);
                    isnew = false;
                    isMouseDowned = false;
                   
                }
                else
                {
                    lb = new Label();
                    lb.FontFamily = f;
                    lb.FontSize = sizeText;
                    SolidColorBrush brtf = new SolidColorBrush(br);
                    lb.Background = brtf;
                    lb.Foreground = brt;
                    lb.Content = Teb.Text;
                    if (TItalic)
                        lb.FontStyle = FontStyles.Italic;
                    else
                        lb.FontStyle = FontStyles.Normal;
                    if (Tbold)
                        lb.FontWeight = FontWeights.Bold;
                    else
                        lb.FontWeight = FontWeights.Normal;

                    lb.RenderTransform = ccm.RenderTransform;

                    Canvas.SetLeft(lb, startPoint.X);
                    Canvas.SetTop(lb, startPoint.Y);
                    PaintCanvas.Children.Add(lb);
                    PaintCanvas.Children.Remove(ccm);
                    ccm.RenderTransform = Transform.Identity;


                    // MessageBox.Show(lb.RenderTransform.ToString());
                    Teb.Text = null;
                    //  SaveCanvasToBitmap(PaintCanvas);
                    isnew = true;

                }
                return;
            }
            if (flag == 10)
            {
                if (isnew == true)
                {
                    startPoint = e.GetPosition(PaintCanvas);
                    isMouseDowned = true;
                    ccm.RenderTransform = Transform.Identity;
                }
                else
                {
                    RBCOPY.IsEnabled = false;
                    RBCUT.IsEnabled = false;
                    Selector.SetIsSelected(ccm, false);
                    RenderTargetBitmap rdtgbm = new RenderTargetBitmap((int)PaintCanvas.RenderSize.Width, (int)PaintCanvas.RenderSize.Height, 96, 96, PixelFormats.Pbgra32);
                    rdtgbm.Render(PaintCanvas);
                    
                    PaintCanvas.Children.Remove(ccm);
                    ImageBrush ib = new ImageBrush(rdtgbm);
                    Image im = new Image();
                    im.Source = rdtgbm;


                    PaintCanvas.Children.Add(im);
                    isnew = true;
                    
                }
                return;
            }
            else
            {
                if (isnew == true)
                {
                    startPoint = e.GetPosition(PaintCanvas);
                   // MessageBox.Show(startPoint.ToString());
                    isMouseDowned = true;
                    ccm.RenderTransform = Transform.Identity;
                    if (firstSave)
                    {
                         StackBitmap = new RenderTargetBitmap((int)PaintCanvas.RenderSize.Width, (int)PaintCanvas.RenderSize.Height, 96, 96, PixelFormats.Pbgra32);
                        StackBitmap.Render(PaintCanvas);
                         firstSave = false;
                    }
                   
                    return;
                }
                else
                {


                    SaveCanvasToBitmap(PaintCanvas);
                    isnew = true;
                    return;
                }
            }
        }

        private void PaintCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (flag == 1)
            {
                if (isMouseDowned  )
                {
                    PaintCanvas.Children.Remove(lastLine);
                    Point endPoint = e.GetPosition((IInputElement)sender);



                    if (flagBrush == 1)
                        lastLine.StrokeDashArray.Add(4);
                    if (flagBrush == 0)
                        lastLine.StrokeDashArray.Clear();


                    SolidColorBrush brush = new SolidColorBrush();
                    brush.Color = cl;
                    lastLine.Stroke = brush;

                    lastLine.StrokeThickness = size;
                    lastLine.X1 = startPoint.X;
                    lastLine.Y1 = startPoint.Y;
                    lastLine.X2 = endPoint.X;
                    lastLine.Y2 = endPoint.Y;
                  
                       
                    PaintCanvas.Children.Add(lastLine);

                    

                }
            }
            if (flag == 2)
            {
                if (isMouseDowned && isnew)
                {
                    // bool checkSwappoint = false;
                    PaintCanvas.Children.Remove(lastElip);
                    Point endPoint = e.GetPosition((IInputElement)sender);
                    Point StartPointEllip = startPoint; ;


                    if (StartPointEllip.X > endPoint.X)
                    {
                        double t = StartPointEllip.X;
                        StartPointEllip.X = endPoint.X;
                        endPoint.X = t;
                    }

                    if (StartPointEllip.Y > endPoint.Y)
                    {
                        double t = StartPointEllip.Y;
                        StartPointEllip.Y = endPoint.Y;
                        endPoint.Y = t;
                    }
                    SolidColorBrush brush = new SolidColorBrush();
                    brush.Color = cl;
                    lastElip.Stroke = brush;
                    lastElip.Fill = brush;
                    // lastElip.Stroke = System.Windows.Media.Brushes.Blue;

                    SolidColorBrush brushBr = new SolidColorBrush();
                    brushBr.Color = br;
                    if (flagBrush == 1)
                        lastElip.StrokeDashArray.Add(4);
                    if (flagBrush == 0)
                        lastElip.StrokeDashArray.Clear();

                    if (TypeFill == 0)
                        lastElip.Fill = null;
                    if (TypeFill == 1)
                        lastElip.Fill = brushBr;
                    if (TypeFill == 2)
                        lastElip.Fill = myHorizontalGradient;
                    if (TypeFill == 3)
                        lastElip.Fill = myVerticalGradient;
                    if (TypeFill == 4)
                        lastElip.Fill = myRadialGradientBrush;
                    if (TypeFill == 5)
                        lastElip.Fill = myLinearGradientBrush;
                    


                    lastElip.StrokeThickness = size;
                    lastElip.Height = Math.Abs(StartPointEllip.Y - endPoint.Y);
                    lastElip.Width = Math.Abs(StartPointEllip.X - endPoint.X);
                    Canvas.SetLeft(lastElip, StartPointEllip.X);
                    Canvas.SetTop(lastElip, StartPointEllip.Y);
                    PaintCanvas.Children.Add(lastElip);

                }
            }
            if (flag == 3 )
            {
                if (isMouseDowned && isnew)
                {
                    Point endpoint = e.GetPosition((IInputElement)sender);
                    Point StarPointRectangle = startPoint;
                    PaintCanvas.Children.Remove(LastRectangle);
                   
                    if (StarPointRectangle.Y > endpoint.Y)
                    {
                        double t = endpoint.Y;
                        endpoint.Y = StarPointRectangle.Y;
                        StarPointRectangle.Y = t;
                    }
                    if (StarPointRectangle.X > endpoint.X)
                    {
                        double t = endpoint.X;
                        endpoint.X = StarPointRectangle.X;
                        StarPointRectangle.X = t;
                    }
                    // LastRectangle.Stroke = System.Windows.Media.Brushes.Blue;
                    SolidColorBrush brush = new SolidColorBrush();
                    brush.Color = cl;
                    LastRectangle.Stroke = brush;
                    LastRectangle.StrokeThickness = size;


                    SolidColorBrush brushBr = new SolidColorBrush();
                    brushBr.Color = br;
                    if (flagBrush == 1)
                        LastRectangle.StrokeDashArray.Add(4);
                    if (flagBrush == 0)
                        LastRectangle.StrokeDashArray.Clear();

                    if (TypeFill == 0)
                        LastRectangle.Fill = null;
                    if (TypeFill == 1)
                        LastRectangle.Fill = brushBr;
                    if (TypeFill == 2)
                        LastRectangle.Fill = myHorizontalGradient;
                    if (TypeFill == 3)
                        LastRectangle.Fill = myVerticalGradient;
                    if (TypeFill == 4)
                        LastRectangle.Fill = myRadialGradientBrush;
                    if (TypeFill == 5)
                        LastRectangle.Fill = myLinearGradientBrush;

                    LastRectangle.Height = Math.Abs(StarPointRectangle.Y - endpoint.Y);
                    LastRectangle.Width = Math.Abs(StarPointRectangle.X - endpoint.X);
                    Canvas.SetLeft(LastRectangle, StarPointRectangle.X);
                    Canvas.SetTop(LastRectangle, StarPointRectangle.Y);
                    PaintCanvas.Children.Add(LastRectangle);

                }

            }
            if (flag == 4)
            {
                if (isMouseDowned && isnew)
                {
                    PaintCanvas.Children.Remove(lastElip);
                    Point endPoint = e.GetPosition((IInputElement)sender);
                    int flagpoint = 1;
                    if (endPoint.Y < startPoint.Y)
                        if (endPoint.X < startPoint.X)
                            flagpoint = 1;
                        else
                            flagpoint = 2;
                    else
                    if (endPoint.X < startPoint.X)
                        flagpoint = 3;
                    else
                        flagpoint = 4;

                    Point StartPointEllip = startPoint; ;
                  

                    double LenghtMin;
                    if ((Math.Abs(endPoint.Y - StartPointEllip.Y)) < Math.Abs(endPoint.X - StartPointEllip.X))
                    {
                        LenghtMin = Math.Abs(endPoint.Y - StartPointEllip.Y);
                    }
                    else
                    {
                        LenghtMin = Math.Abs(endPoint.X - StartPointEllip.X);
                    }
                    if (flagpoint == 1)
                    {
                        StartPointEllip.X = (double)startPoint.X - LenghtMin;
                        StartPointEllip.Y = (double)startPoint.Y - LenghtMin;
                    }
                    if (flagpoint == 2)
                    {
                        StartPointEllip.X = startPoint.X;
                        StartPointEllip.Y = startPoint.Y - LenghtMin;
                    }
                    if (flagpoint == 3)
                    {
                        StartPointEllip.Y = startPoint.Y;
                        StartPointEllip.X = startPoint.X - LenghtMin;
                    }
                    if (flagpoint == 4)
                    {
                        StartPointEllip = startPoint;
                    }

                    SolidColorBrush brush = new SolidColorBrush();
                    brush.Color = cl;
                    lastElip.Stroke = brush;

                    SolidColorBrush brushBr = new SolidColorBrush();
                    brushBr.Color = br;
                    if (flagBrush == 1)
                        lastElip.StrokeDashArray.Add(4);
                    if (flagBrush == 0)
                        lastElip.StrokeDashArray.Clear();

                    if (TypeFill == 0)
                        lastElip.Fill = null;
                    if (TypeFill == 1)
                        lastElip.Fill = brushBr;
                    if (TypeFill == 2)
                        lastElip.Fill = myHorizontalGradient;
                    if (TypeFill == 3)
                        lastElip.Fill = myVerticalGradient;
                    if (TypeFill == 4)
                        lastElip.Fill = myRadialGradientBrush;
                    if (TypeFill == 5)
                        lastElip.Fill = myLinearGradientBrush;

                    lastElip.StrokeThickness = size;
                    lastElip.Height = LenghtMin;
                    lastElip.Width = LenghtMin;
                    Canvas.SetLeft(lastElip, StartPointEllip.X);
                    Canvas.SetTop(lastElip, StartPointEllip.Y);

                    PaintCanvas.Children.Add(lastElip);

                }
            }
            if (flag == 5)
            {
                if (isMouseDowned && isnew)
                {
                    PaintCanvas.Children.Remove(LastRectangle);
                    Point endPoint = e.GetPosition((IInputElement)sender);
                    int flagpoint = 1;
                    if (endPoint.Y < startPoint.Y)
                        if (endPoint.X < startPoint.X)
                            flagpoint = 1;
                        else
                            flagpoint = 2;
                    else
                    if (endPoint.X < startPoint.X)
                        flagpoint = 3;
                    else
                        flagpoint = 4;

                    Point StartPointRec = startPoint;
                    


                    double LenghtMin;
                    if ((Math.Abs(endPoint.Y - StartPointRec.Y)) < Math.Abs(endPoint.X - StartPointRec.X))
                    {
                        LenghtMin = Math.Abs(endPoint.Y - StartPointRec.Y);
                    }
                    else
                    {
                        LenghtMin = Math.Abs(endPoint.X - StartPointRec.X);
                    }
                    if (flagpoint == 1)
                    {
                        StartPointRec.X = (double)startPoint.X - LenghtMin;
                        StartPointRec.Y = (double)startPoint.Y - LenghtMin;
                    }
                    if (flagpoint == 2)
                    {
                        StartPointRec.X = startPoint.X;
                        StartPointRec.Y = startPoint.Y - LenghtMin;
                    }
                    if (flagpoint == 3)
                    {
                        StartPointRec.Y = startPoint.Y;
                        StartPointRec.X = startPoint.X - LenghtMin;
                    }
                    if (flagpoint == 4)
                    {
                        StartPointRec = startPoint;
                    }

                    SolidColorBrush brush = new SolidColorBrush();
                    brush.Color = cl;
                    LastRectangle.Stroke = brush;


                    SolidColorBrush brushBr = new SolidColorBrush();
                    brushBr.Color = br;
                    if (flagBrush == 1)
                        LastRectangle.StrokeDashArray.Add(4);
                    if (flagBrush == 0)
                        LastRectangle.StrokeDashArray.Clear();

                    if (TypeFill == 0)
                        LastRectangle.Fill = null;
                    if (TypeFill == 1)
                        LastRectangle.Fill = brushBr;
                    if (TypeFill == 2)
                        LastRectangle.Fill = myHorizontalGradient;
                    if (TypeFill == 3)
                        LastRectangle.Fill = myVerticalGradient;
                    if (TypeFill == 4)
                        LastRectangle.Fill = myRadialGradientBrush;
                    if (TypeFill == 5)
                        LastRectangle.Fill = myLinearGradientBrush;

                    LastRectangle.StrokeThickness = size;
                    LastRectangle.Height = LenghtMin;
                    LastRectangle.Width = LenghtMin;
                    Canvas.SetLeft(LastRectangle, StartPointRec.X);
                    Canvas.SetTop(LastRectangle, StartPointRec.Y);
                    PaintCanvas.Children.Add(LastRectangle);

                }
            }
            if(flag == 10)
            {
                if(isMouseDowned)
                {
                    PaintCanvas.Children.Remove(LastRectangle);
                    Point endpoint = e.GetPosition((IInputElement)sender);
                    Point StarPointRectangle = startPoint;
                   
                    if (StarPointRectangle.Y > endpoint.Y)
                    {
                        double t = endpoint.Y;
                        endpoint.Y = StarPointRectangle.Y;
                        StarPointRectangle.Y = t;
                    }
                    if (StarPointRectangle.X > endpoint.X)
                    {
                        double t = endpoint.X;
                        endpoint.X = StarPointRectangle.X;
                        StarPointRectangle.X = t;
                    }
                    // LastRectangle.Stroke = System.Windows.Media.Brushes.Blue;
                   
                    LastRectangle.Stroke = Brushes.Black;
                    LastRectangle.StrokeThickness = 1;


                   
                        LastRectangle.StrokeDashArray.Add(4);
                    
                    
                        LastRectangle.Fill = Brushes.Transparent;
                   

                    LastRectangle.Height = Math.Abs(StarPointRectangle.Y - endpoint.Y);
                    LastRectangle.Width = Math.Abs(StarPointRectangle.X - endpoint.X);
                    Canvas.SetLeft(LastRectangle, StarPointRectangle.X);
                    Canvas.SetTop(LastRectangle, StarPointRectangle.Y);

                  
                        PaintCanvas.Children.Add(LastRectangle);

                   

                }
            }

            if (flag == 21)
            {
                if (isMouseDowned && isnew)
                {
                    // bool checkSwappoint = false;
                    PaintCanvas.Children.Remove(pll);
                    Point endPoint = e.GetPosition((IInputElement)sender);
                  //  MessageBox.Show(startPoint.ToString());
                    Point StartPointTemp = startPoint;


                    if (StartPointTemp.X > endPoint.X)
                    {
                        double t = StartPointTemp.X;
                        StartPointTemp.X = endPoint.X;
                        endPoint.X = t;
                    }

                    if (StartPointTemp.Y > endPoint.Y)
                    {
                        double t = StartPointTemp.Y;
                        StartPointTemp.Y = endPoint.Y;
                        endPoint.Y = t;
                    }
                    double widthPll = endPoint.X - StartPointTemp.X;
                    double heightPll = endPoint.Y - StartPointTemp.Y;


                    Point point1 = new Point(StartPointTemp.X + widthPll / 4, StartPointTemp.Y);
                  //  MessageBox.Show(point1.ToString());
                    PointCollection polygonPoints = new PointCollection();
                    polygonPoints.Add(point1);
                    point1 = new Point(StartPointTemp.X + widthPll * 3 / 4, StartPointTemp.Y);
                    polygonPoints.Add(point1);
                    point1 = new Point(StartPointTemp.X + widthPll * 3 / 4, StartPointTemp.Y + heightPll / 2);
                    polygonPoints.Add(point1);
                    point1 = new Point(endPoint.X, StartPointTemp.Y + heightPll / 2);
                    polygonPoints.Add(point1);
                    point1 = new Point(StartPointTemp.X + widthPll / 2, endPoint.Y);
                    polygonPoints.Add(point1);
                    point1 = new Point(StartPointTemp.X, StartPointTemp.Y + heightPll / 2);
                    polygonPoints.Add(point1);
                    point1 = new Point(StartPointTemp.X + widthPll / 4, StartPointTemp.Y + heightPll / 2);
                    polygonPoints.Add(point1);
                    point1 = new Point(StartPointTemp.X + widthPll / 4, StartPointTemp.Y);
                    polygonPoints.Add(point1);


                    // pll.Points(50, 100);
                    pll.Points = polygonPoints;
                    SolidColorBrush brush = new SolidColorBrush();
                    brush.Color = cl;
                    pll.Stroke = brush;
                   pll.Fill = brush;


                    SolidColorBrush brushBr = new SolidColorBrush();
                   brushBr.Color = br;
                    if (flagBrush == 1)
                        pll.StrokeDashArray.Add(4);
                    if (flagBrush == 0)
                        pll.StrokeDashArray.Clear();

                    if (TypeFill == 0)
                        pll.Fill = null;
                    if (TypeFill == 1)
                        pll.Fill = brushBr;
                    if (TypeFill == 2)
                        pll.Fill = myHorizontalGradient;
                    if (TypeFill == 3)
                        pll.Fill = myVerticalGradient;
                    if (TypeFill == 4)
                        pll.Fill = myRadialGradientBrush;
                    if (TypeFill == 5)
                        pll.Fill = myLinearGradientBrush;

                    pll.StrokeThickness = size;
                 
                    PaintCanvas.Children.Add(pll);

                    

                }
            }
            if (flag == 22)
            {
                if (isMouseDowned && isnew)
                {
                    // bool checkSwappoint = false;
                    PaintCanvas.Children.Remove(pll);
                    Point endPoint = e.GetPosition((IInputElement)sender);
                    //  MessageBox.Show(startPoint.ToString());
                    Point StartPointTemp = startPoint;


                    if (StartPointTemp.X > endPoint.X)
                    {
                        double t = StartPointTemp.X;
                        StartPointTemp.X = endPoint.X;
                        endPoint.X = t;
                    }

                    if (StartPointTemp.Y > endPoint.Y)
                    {
                        double t = StartPointTemp.Y;
                        StartPointTemp.Y = endPoint.Y;
                        endPoint.Y = t;
                    }
                    double widthPll = endPoint.X - StartPointTemp.X;
                    double heightPll = endPoint.Y - StartPointTemp.Y;


                    Point point1 = new Point(StartPointTemp.X , StartPointTemp.Y + heightPll / 4);
                    //  MessageBox.Show(point1.ToString());
                    PointCollection polygonPoints = new PointCollection();
                    polygonPoints.Add(point1);
                    point1 = new Point(StartPointTemp.X + widthPll / 2, StartPointTemp.Y + heightPll / 4);
                    polygonPoints.Add(point1);
                    point1 = new Point(StartPointTemp.X + widthPll / 2, StartPointTemp.Y);
                    polygonPoints.Add(point1);
                    point1 = new Point(endPoint.X, StartPointTemp.Y + heightPll / 2);
                    polygonPoints.Add(point1);
                    point1 = new Point(StartPointTemp.X + widthPll / 2, endPoint.Y);
                    polygonPoints.Add(point1);
                    point1 = new Point(StartPointTemp.X + widthPll / 2, endPoint.Y - heightPll / 4 );
                    polygonPoints.Add(point1);
                    point1 = new Point(StartPointTemp.X , StartPointTemp.Y + heightPll * 3 / 4);
                    polygonPoints.Add(point1);
                    point1 = new Point(StartPointTemp.X, StartPointTemp.Y + heightPll / 4);
                    polygonPoints.Add(point1);


                    // pll.Points(50, 100);
                    pll.Points = polygonPoints;
                    SolidColorBrush brush = new SolidColorBrush();
                    brush.Color = cl;
                    pll.Stroke = brush;
                    pll.Fill = brush;


                    SolidColorBrush brushBr = new SolidColorBrush();
                    brushBr.Color = br;
                    if (flagBrush == 1)
                        pll.StrokeDashArray.Add(4);
                    if (flagBrush == 0)
                        pll.StrokeDashArray.Clear();


                    if (TypeFill == 0)
                        pll.Fill = null;
                    if (TypeFill == 1)
                        pll.Fill = brushBr;
                    if (TypeFill == 2)
                        pll.Fill = myHorizontalGradient;
                    if (TypeFill == 3)
                        pll.Fill = myVerticalGradient;
                    if (TypeFill == 4)
                        pll.Fill = myRadialGradientBrush;
                    if (TypeFill == 5)
                        pll.Fill = myLinearGradientBrush;

                    pll.StrokeThickness = size;

                    PaintCanvas.Children.Add(pll);



                }
            }
            if (flag == 23)
            {
                if (isMouseDowned && isnew)
                {
                    // bool checkSwappoint = false;
                    PaintCanvas.Children.Remove(pll);
                    Point endPoint = e.GetPosition((IInputElement)sender);
                    //  MessageBox.Show(startPoint.ToString());
                    Point StartPointTemp = startPoint;


                    if (StartPointTemp.X > endPoint.X)
                    {
                        double t = StartPointTemp.X;
                        StartPointTemp.X = endPoint.X;
                        endPoint.X = t;
                    }

                    if (StartPointTemp.Y > endPoint.Y)
                    {
                        double t = StartPointTemp.Y;
                        StartPointTemp.Y = endPoint.Y;
                        endPoint.Y = t;
                    }
                    double widthPll = endPoint.X - StartPointTemp.X;
                    double heightPll = endPoint.Y - StartPointTemp.Y;


                    Point point1 = new Point(StartPointTemp.X + widthPll / 4, endPoint.Y);
                    //  MessageBox.Show(point1.ToString());
                    PointCollection polygonPoints = new PointCollection();
                    polygonPoints.Add(point1);
                    point1 = new Point(StartPointTemp.X + widthPll * 3 / 4, endPoint.Y);
                    polygonPoints.Add(point1);
                    point1 = new Point(StartPointTemp.X + widthPll * 3 / 4, StartPointTemp.Y + heightPll / 2);
                    polygonPoints.Add(point1);
                    point1 = new Point(endPoint.X, StartPointTemp.Y + heightPll / 2);
                    polygonPoints.Add(point1);
                    point1 = new Point(StartPointTemp.X + widthPll / 2, StartPointTemp.Y);
                    polygonPoints.Add(point1);
                    point1 = new Point(StartPointTemp.X , StartPointTemp.Y + heightPll / 2);
                    polygonPoints.Add(point1);
                    point1 = new Point(StartPointTemp.X + widthPll / 4, StartPointTemp.Y + heightPll / 2);
                    polygonPoints.Add(point1);
                    point1 = new Point(StartPointTemp.X + widthPll / 4, endPoint.Y);
                    polygonPoints.Add(point1);


                    // pll.Points(50, 100);
                    pll.Points = polygonPoints;
                    SolidColorBrush brush = new SolidColorBrush();
                    brush.Color = cl;
                    pll.Stroke = brush;
                    pll.Fill = brush;


                    SolidColorBrush brushBr = new SolidColorBrush();
                    brushBr.Color = br;
                    if (flagBrush == 1)
                        pll.StrokeDashArray.Add(4);
                    if (flagBrush == 0)
                        pll.StrokeDashArray.Clear();


                    if (TypeFill == 0)
                        pll.Fill = null;
                    if (TypeFill == 1)
                        pll.Fill = brushBr;
                    if (TypeFill == 2)
                        pll.Fill = myHorizontalGradient;
                    if (TypeFill == 3)
                        pll.Fill = myVerticalGradient;
                    if (TypeFill == 4)
                        pll.Fill = myRadialGradientBrush;
                    if (TypeFill == 5)
                        pll.Fill = myLinearGradientBrush;

                    pll.StrokeThickness = size;

                    PaintCanvas.Children.Add(pll);



                }
            }
            if (flag == 24)
            {
                if (isMouseDowned && isnew)
                {
                    // bool checkSwappoint = false;
                    PaintCanvas.Children.Remove(pll);
                    Point endPoint = e.GetPosition((IInputElement)sender);
                   
                    Point StartPointTemp = startPoint;

                    if (StartPointTemp.X > endPoint.X)
                    {
                        double t = StartPointTemp.X;
                        StartPointTemp.X = endPoint.X;
                        endPoint.X = t;
                    }

                    if (StartPointTemp.Y > endPoint.Y)
                    {
                        double t = StartPointTemp.Y;
                        StartPointTemp.Y = endPoint.Y;
                        endPoint.Y = t;
                    }
                    double widthPll = endPoint.X - StartPointTemp.X;
                    double heightPll = endPoint.Y - StartPointTemp.Y;
                
                    Point point1 = new Point(endPoint.X , StartPointTemp.Y + heightPll / 4);
                    //  MessageBox.Show(point1.ToString());
                    PointCollection polygonPoints = new PointCollection();
                    polygonPoints.Add(point1);
                    point1 = new Point(endPoint.X, StartPointTemp.Y + heightPll * 3 / 4 );
                    polygonPoints.Add(point1);
                    point1 = new Point(StartPointTemp.X + widthPll  / 2, StartPointTemp.Y + heightPll * 3 / 4);
                    polygonPoints.Add(point1);
                    point1 = new Point(StartPointTemp.X + widthPll / 2, endPoint.Y);
                    polygonPoints.Add(point1);
                    point1 = new Point(StartPointTemp.X , StartPointTemp.Y + heightPll / 2);
                    polygonPoints.Add(point1);
                    point1 = new Point(StartPointTemp.X + widthPll / 2, StartPointTemp.Y);
                    polygonPoints.Add(point1);
                    point1 = new Point(StartPointTemp.X + widthPll / 2, StartPointTemp.Y + heightPll / 4);
                    polygonPoints.Add(point1);
                    point1 = new Point(endPoint.X, StartPointTemp.Y + heightPll / 4);
                    polygonPoints.Add(point1);
                    
                    pll.Points = polygonPoints;
                    SolidColorBrush brush = new SolidColorBrush();
                    brush.Color = cl;
                    pll.Stroke = brush;
                    pll.Fill = brush;
                    
                    SolidColorBrush brushBr = new SolidColorBrush();
                    brushBr.Color = br;
                    if (flagBrush == 1)
                        pll.StrokeDashArray.Add(4);
                    if (flagBrush == 0)
                        pll.StrokeDashArray.Clear();


                    if (TypeFill == 0)
                        pll.Fill = null;
                    if (TypeFill == 1)
                        pll.Fill = brushBr;
                    if (TypeFill == 2)
                        pll.Fill = myHorizontalGradient;
                    if (TypeFill == 3)
                        pll.Fill = myVerticalGradient;
                    if (TypeFill == 4)
                        pll.Fill = myRadialGradientBrush;
                    if (TypeFill == 5)
                        pll.Fill = myLinearGradientBrush;

                    pll.StrokeThickness = size;
                    PaintCanvas.Children.Add(pll);
                }
            }
            if (flag == 25)
            {
                if (isMouseDowned && isnew)
                {
                    // bool checkSwappoint = false;
                    PaintCanvas.Children.Remove(pll);
                    Point endPoint = e.GetPosition((IInputElement)sender);

                    Point StartPointTemp = startPoint;

                    if (StartPointTemp.X > endPoint.X)
                    {
                        double t = StartPointTemp.X;
                        StartPointTemp.X = endPoint.X;
                        endPoint.X = t;
                    }

                    if (StartPointTemp.Y > endPoint.Y)
                    {
                        double t = StartPointTemp.Y;
                        StartPointTemp.Y = endPoint.Y;
                        endPoint.Y = t;
                    }
                    double widthPll = endPoint.X - StartPointTemp.X;
                    double heightPll = endPoint.Y - StartPointTemp.Y;

                    Point point1 = new Point(StartPointTemp.X + widthPll /2, StartPointTemp.Y);
                    //  MessageBox.Show(point1.ToString());
                    PointCollection polygonPoints = new PointCollection();
                    polygonPoints.Add(point1);
                    point1 = new Point(StartPointTemp.X + widthPll * 3 / 8,  StartPointTemp.Y + heightPll * 11 / 30);
                    polygonPoints.Add(point1);
                    point1 = new Point(StartPointTemp.X , StartPointTemp.Y + heightPll * 115 / 300);
                    polygonPoints.Add(point1);
                    point1 = new Point(StartPointTemp.X + widthPll * 120 / 400, StartPointTemp.Y + heightPll * 185 / 300);
                    polygonPoints.Add(point1);
                    point1 = new Point(StartPointTemp.X + widthPll * 75 / 400, StartPointTemp.Y + heightPll );
                    polygonPoints.Add(point1);
                    point1 = new Point(StartPointTemp.X + widthPll / 2, StartPointTemp.Y + heightPll * 230 / 300);
                    polygonPoints.Add(point1);
                    point1 = new Point(StartPointTemp.X + widthPll * 325 / 400, StartPointTemp.Y + heightPll );
                    polygonPoints.Add(point1);
                    point1 = new Point(StartPointTemp.X + widthPll *280 / 400, StartPointTemp.Y + heightPll * 185/300);
                    polygonPoints.Add(point1);
                    point1 = new Point(endPoint.X, StartPointTemp.Y + heightPll * 115 / 300);
                    polygonPoints.Add(point1);
                    point1 = new Point(StartPointTemp.X + widthPll* 250/400, StartPointTemp.Y + heightPll* 115 / 300);
                    polygonPoints.Add(point1);
                    point1 = new Point(StartPointTemp.X + widthPll / 2, StartPointTemp.Y);
                    polygonPoints.Add(point1);

                    pll.Points = polygonPoints;
                    SolidColorBrush brush = new SolidColorBrush();
                    brush.Color = cl;
                    pll.Stroke = brush;
                    pll.Fill = brush;

                    SolidColorBrush brushBr = new SolidColorBrush();
                    brushBr.Color = br;
                    if (flagBrush == 1)
                        pll.StrokeDashArray.Add(4);
                    if (flagBrush == 0)
                        pll.StrokeDashArray.Clear();


                    if (TypeFill == 0)
                        pll.Fill = null;
                    if (TypeFill == 1)
                        pll.Fill = brushBr;
                    if (TypeFill == 2)
                        pll.Fill = myHorizontalGradient;
                    if (TypeFill == 3)
                        pll.Fill = myVerticalGradient;
                    if (TypeFill == 4)
                        pll.Fill = myRadialGradientBrush;
                    if (TypeFill == 5)
                        pll.Fill = myLinearGradientBrush;

                    pll.StrokeThickness = size;
                    PaintCanvas.Children.Add(pll);
                }
            }
            if (flag == 20)
            {
                if (isMouseDowned && isnew)
                {
                  
                    PaintCanvas.Children.Remove(path);
                    Point endPoint = e.GetPosition((IInputElement)sender);

                    Point StartPointTemp = startPoint;

                    if (StartPointTemp.X > endPoint.X)
                    {
                        double t = StartPointTemp.X;
                        StartPointTemp.X = endPoint.X;
                        endPoint.X = t;
                    }

                    if (StartPointTemp.Y > endPoint.Y)
                    {
                        double t = StartPointTemp.Y;
                        StartPointTemp.Y = endPoint.Y;
                        endPoint.Y = t;
                    }
                    double widthPll = endPoint.X - StartPointTemp.X;
                    double heightPll = endPoint.Y - StartPointTemp.Y;
                  
                    Point point1 = new Point(StartPointTemp.X + widthPll / 2, StartPointTemp.Y + heightPll * 70 / 300);
                 
                 
                  
                    Point point2 = new Point(StartPointTemp.X + widthPll * 70 / 300, StartPointTemp.Y );
                   
                    Point point3 = new Point(StartPointTemp.X, StartPointTemp.Y + heightPll * 85 / 300);

                    Point point4 = new Point(StartPointTemp.X + widthPll * 90 / 300 , StartPointTemp.Y + heightPll * 245 / 300);

                    Point point5 = new Point(StartPointTemp.X + widthPll  / 2, StartPointTemp.Y + heightPll);

                    Point point6 = new Point(StartPointTemp.X + widthPll *210/ 300, StartPointTemp.Y + heightPll * 245 / 300);

                    Point point7 = new Point(StartPointTemp.X + widthPll, StartPointTemp.Y + heightPll * 85 /300);
                    
                    Point point8 = new Point(StartPointTemp.X + widthPll * 230/ 300, StartPointTemp.Y);
                    
                    Point point9 = new Point(StartPointTemp.X + widthPll / 2, StartPointTemp.Y + heightPll * 70 / 300);
          

                    Point[] points = new[] {point1 ,point2,point3,point4,point5   ,point6 ,point7 ,point8, point9  };
                    var b = GetBezierApproximation(points, 256);
                    PathFigure pf = new PathFigure(b.Points[0], new[] { b }, true);
                    
                    PathFigureCollection pfc = new PathFigureCollection();
                    pfc.Add(pf);
                    var pge = new PathGeometry();
                    pge.Figures = pfc;
                    
                    path.Data = pge;

                    SolidColorBrush brush = new SolidColorBrush();
                    brush.Color = cl;
                    path.Stroke = brush;
                    

                    SolidColorBrush brushBr = new SolidColorBrush();
                    brushBr.Color = br;
                    if (flagBrush == 1)
                        path.StrokeDashArray.Add(4);
                    if (flagBrush == 0)
                        path.StrokeDashArray.Clear();


                    if (TypeFill == 0)
                        path.Fill = null;
                    if (TypeFill == 1)
                        path.Fill = brushBr;
                    if (TypeFill == 2)
                        path.Fill = myHorizontalGradient;
                    if (TypeFill == 3)
                        path.Fill = myVerticalGradient;
                    if (TypeFill == 4)
                        path.Fill = myRadialGradientBrush;
                    if (TypeFill == 5)
                        path.Fill = myLinearGradientBrush;

                    path.StrokeThickness = size;

                    PaintCanvas.Children.Add(path);


                    
                    //PaintCanvas.Children.Add(path);
                }
            }
        }



        private void PaintCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Point endPoint = e.GetPosition((IInputElement)sender);
            if(Math.Abs(endPoint.X - startPoint.X )< 20 && Math.Abs(endPoint.Y - startPoint.Y) < 20)
            {
                isMouseDowned = false;
                return;
            }


            if (flag == 1)
            {

                if (isMouseDowned && isnew == true)
                {
                    Point temp = startPoint;
                    if (startPoint.X > endPoint.X)
                    {
                        temp.X = endPoint.X;



                    }

                    if (startPoint.Y > endPoint.Y)
                    {
                        temp.Y = endPoint.Y;

                    }
                    
                    

                    SolidColorBrush brush = new SolidColorBrush();
                    brush.Color = cl;
                    line.Stroke = brush;
                    if (flagBrush == 1)
                        line.StrokeDashArray.Add(4);

                    line.StrokeThickness = size;
                    line.X1 = startPoint.X - temp.X;
                    line.Y1 = startPoint.Y - temp.Y;
                    line.X2 = endPoint.X - temp.X;
                    line.Y2 = endPoint.Y - temp.Y;

                    line.IsHitTestVisible = false;
                    line.Stretch = Stretch.Fill;
                    ccm.Content = line;
                    ccm.Style = this.FindResource("DesignerItemStyle") as Style;
                    ccm.Height = Math.Abs(startPoint.Y - endPoint.Y);
                    ccm.Width = Math.Abs(startPoint.X - endPoint.X);
                    Canvas.SetLeft(ccm, temp.X);
                    Canvas.SetTop(ccm, temp.Y);
                    Selector.SetIsSelected(ccm, true);
                    PaintCanvas.Children.Remove(lastLine);
                    PaintCanvas.Children.Add(ccm);
                    isnew = false;
                    isMouseDowned = false;
                  
                }
            }
            if (flag == 2)
            {
                if (isMouseDowned && isnew == true)
                {
                   // Point endPoint = e.GetPosition((IInputElement)sender);
                    if (startPoint.X > endPoint.X)
                    {
                        double t = startPoint.X;
                        startPoint.X = endPoint.X;
                        endPoint.X = t;



                    }

                    if (startPoint.Y > endPoint.Y)
                    {
                        double t = startPoint.Y;
                        startPoint.Y = endPoint.Y;
                        endPoint.Y = t;

                    }
                   // ContentControl ccm = new ContentControl();
                   
                    SolidColorBrush brush = new SolidColorBrush();
                    brush.Color = cl;
                    elip.Stroke = brush;
                    elip.StrokeThickness = size;

                    SolidColorBrush brushBr = new SolidColorBrush();
                    brushBr.Color = br;
                    if (flagBrush == 1)
                        elip.StrokeDashArray.Add(4);
                  

                    if (TypeFill == 0)
                        elip.Fill = null;
                    if (TypeFill == 1)
                        elip.Fill = brushBr;
                    if (TypeFill == 2)
                        elip.Fill = myHorizontalGradient;
                    if (TypeFill == 3)
                        elip.Fill = myVerticalGradient;
                    if (TypeFill == 4)
                        elip.Fill = myRadialGradientBrush;
                    if (TypeFill == 5)
                        elip.Fill = myLinearGradientBrush;

                    elip.IsHitTestVisible = true;
                    ccm.Content = elip;
                    ccm.Style = this.FindResource("DesignerItemStyle") as Style;
                    ccm.Height = Math.Abs(startPoint.Y - endPoint.Y);
                    ccm.Width = Math.Abs(startPoint.X - endPoint.X);
                    Canvas.SetLeft(ccm, startPoint.X);
                    Canvas.SetTop(ccm, startPoint.Y);
                    Selector.SetIsSelected(ccm, true);
                    PaintCanvas.Children.Remove(lastElip);
                    PaintCanvas.Children.Add(ccm);
                    isnew = false;
                    isMouseDowned = false;
                }
            }
            if (flag == 3)
            {
                if (isMouseDowned && isnew == true)
                {
                   // Point endPoint = e.GetPosition((IInputElement)sender);
                    if (startPoint.X > endPoint.X)
                    {
                        double t = startPoint.X;
                        startPoint.X = endPoint.X;
                        endPoint.X = t;



                    }

                    if (startPoint.Y > endPoint.Y)
                    {
                        double t = startPoint.Y;
                        startPoint.Y = endPoint.Y;
                        endPoint.Y = t;

                    }
                   
                    SolidColorBrush brush = new SolidColorBrush();
                    brush.Color = cl;
                    rec.Stroke = brush;

                    SolidColorBrush brushBr = new SolidColorBrush();
                    brushBr.Color = br;
                    if (flagBrush == 1)
                        rec.StrokeDashArray.Add(4);

                    if (TypeFill == 0)
                        rec.Fill = null;
                    if (TypeFill == 1)
                        rec.Fill = brushBr;
                    if (TypeFill == 2)
                        rec.Fill = myHorizontalGradient;
                    if (TypeFill == 3)
                        rec.Fill = myVerticalGradient;
                    if (TypeFill == 4)
                        rec.Fill = myRadialGradientBrush;
                    if (TypeFill == 5)
                        rec.Fill = myLinearGradientBrush;

                    rec.StrokeThickness = size;

                    rec.IsHitTestVisible = true;
                    ccm.Content = rec;
                    ccm.Style = this.FindResource("DesignerItemStyle") as Style;
                    ccm.Height = Math.Abs(startPoint.Y - endPoint.Y);
                    ccm.Width = Math.Abs(startPoint.X - endPoint.X);
                    Canvas.SetLeft(ccm, startPoint.X);
                    Canvas.SetTop(ccm, startPoint.Y);
                    Selector.SetIsSelected(ccm, true);
                    PaintCanvas.Children.Remove(LastRectangle);
                    PaintCanvas.Children.Add(ccm);
                    isnew = false;
                    isMouseDowned = false;

                    
                }
            }
            if (flag == 4)
            {
                if (isMouseDowned && isnew == true)
                {
                    //Point endPoint = e.GetPosition((IInputElement)sender);
                    int flagpoint = 1;
                    if (endPoint.Y < startPoint.Y)
                        if (endPoint.X < startPoint.X)
                            flagpoint = 1;
                        else
                            flagpoint = 2;
                    else
                    if (endPoint.X < startPoint.X)
                        flagpoint = 3;
                    else
                        flagpoint = 4;

                    Point StartPointEllip = startPoint;




                    double LenghtMin;
                    if ((Math.Abs(endPoint.Y - StartPointEllip.Y)) < Math.Abs(endPoint.X - StartPointEllip.X))
                    {
                        LenghtMin = Math.Abs(endPoint.Y - StartPointEllip.Y);
                    }
                    else
                    {
                        LenghtMin = Math.Abs(endPoint.X - StartPointEllip.X);
                    }
                    if (flagpoint == 1)
                    {
                        StartPointEllip.X = (double)startPoint.X - LenghtMin;
                        StartPointEllip.Y = (double)startPoint.Y - LenghtMin;
                    }
                    if (flagpoint == 2)
                    {
                        StartPointEllip.X = startPoint.X;
                        StartPointEllip.Y = startPoint.Y - LenghtMin;
                    }
                    if (flagpoint == 3)
                    {
                        StartPointEllip.Y = startPoint.Y;
                        StartPointEllip.X = startPoint.X - LenghtMin;
                    }
                    if (flagpoint == 4)
                    {
                        StartPointEllip = startPoint;
                    }
                   
                    SolidColorBrush brush = new SolidColorBrush();
                    brush.Color = cl;
                    elip.Stroke = brush;

                    SolidColorBrush brushBr = new SolidColorBrush();
                    brushBr.Color = br;
                    if (flagBrush == 1)
                        elip.StrokeDashArray.Add(4);

                    if (TypeFill == 0)
                        elip.Fill = null;
                    if (TypeFill == 1)
                        elip.Fill = brushBr;
                    if (TypeFill == 2)
                        elip.Fill = myHorizontalGradient;
                    if (TypeFill == 3)
                        elip.Fill = myVerticalGradient;
                    if (TypeFill == 4)
                        elip.Fill = myRadialGradientBrush;
                    if (TypeFill == 5)
                        elip.Fill = myLinearGradientBrush;

                    elip.StrokeThickness = size;

                    elip.IsHitTestVisible = true;
                    ccm.Content = elip;
                    ccm.Style = this.FindResource("DesignerItemStyle") as Style;
                    ccm.Height = LenghtMin;
                    ccm.Width = LenghtMin;
                    Canvas.SetLeft(ccm, StartPointEllip.X);
                    Canvas.SetTop(ccm, StartPointEllip.Y);
                    Selector.SetIsSelected(ccm, true);
                    PaintCanvas.Children.Remove(lastElip);
                    PaintCanvas.Children.Add(ccm);
                    isnew = false;
                    isMouseDowned = false;
                    
                }

            }
            if (flag == 5)
            {
                if (isMouseDowned && isnew == true)
                {
                 //   Point endPoint = e.GetPosition((IInputElement)sender);
                    int flagpoint = 1;
                    if (endPoint.Y < startPoint.Y)
                        if (endPoint.X < startPoint.X)
                            flagpoint = 1;
                        else
                            flagpoint = 2;
                    else
                    if (endPoint.X < startPoint.X)
                        flagpoint = 3;
                    else
                        flagpoint = 4;

                    Point StartPointEllip = startPoint;




                    double LenghtMin;
                    if ((Math.Abs(endPoint.Y - StartPointEllip.Y)) < Math.Abs(endPoint.X - StartPointEllip.X))
                    {
                        LenghtMin = Math.Abs(endPoint.Y - StartPointEllip.Y);
                    }
                    else
                    {
                        LenghtMin = Math.Abs(endPoint.X - StartPointEllip.X);
                    }
                    if (flagpoint == 1)
                    {
                        StartPointEllip.X = (double)startPoint.X - LenghtMin;
                        StartPointEllip.Y = (double)startPoint.Y - LenghtMin;
                    }
                    if (flagpoint == 2)
                    {
                        StartPointEllip.X = startPoint.X;
                        StartPointEllip.Y = startPoint.Y - LenghtMin;
                    }
                    if (flagpoint == 3)
                    {
                        StartPointEllip.Y = startPoint.Y;
                        StartPointEllip.X = startPoint.X - LenghtMin;
                    }
                    if (flagpoint == 4)
                    {
                        StartPointEllip = startPoint;
                    }
                    
                    SolidColorBrush brush = new SolidColorBrush();
                    brush.Color = cl;
                    rec.Stroke = brush;

                    SolidColorBrush brushBr = new SolidColorBrush();
                    brushBr.Color = br;
                    if (flagBrush == 1)
                        rec.StrokeDashArray.Add(4);
                    if (TypeFill == 0)
                        rec.Fill = null;
                    if (TypeFill == 1)
                        rec.Fill = brushBr;
                    if (TypeFill == 2)
                        rec.Fill = myHorizontalGradient;
                    if (TypeFill == 3)
                        rec.Fill = myVerticalGradient;
                    if (TypeFill == 4)
                        rec.Fill = myRadialGradientBrush;
                    if (TypeFill == 5)
                        rec.Fill = myLinearGradientBrush;

                    rec.StrokeThickness = size;


                    rec.IsHitTestVisible = true;
                    ccm.Content = rec;
                    ccm.Style = this.FindResource("DesignerItemStyle") as Style;
                    ccm.Height = LenghtMin;
                    ccm.Width = LenghtMin;
                    Canvas.SetLeft(ccm, StartPointEllip.X);
                    Canvas.SetTop(ccm, StartPointEllip.Y);
                    Selector.SetIsSelected(ccm, true);
                    PaintCanvas.Children.Remove(LastRectangle);
                    PaintCanvas.Children.Add(ccm);
                    isnew = false;
                    isMouseDowned = false;
                    
                }
            }

            if(flag == 10)
            {
                if (isMouseDowned && isnew == true)
                {
                    PaintCanvas.Children.Remove(LastRectangle);
                   // Point endPoint = e.GetPosition((IInputElement)sender);
                    if (startPoint.X > endPoint.X)
                    {
                        double t = startPoint.X;
                        startPoint.X = endPoint.X;
                        endPoint.X = t;

                    }

                    if (startPoint.Y > endPoint.Y)
                    {
                        double t = startPoint.Y;
                        startPoint.Y = endPoint.Y;
                        endPoint.Y = t;

                    }


               
                  
                   RenderTargetBitmap Bitmap = new RenderTargetBitmap((int)PaintCanvas.RenderSize.Width, (int)PaintCanvas.RenderSize.Height, 96, 96, PixelFormats.Pbgra32);
                    Bitmap.Render(PaintCanvas);

                   // ImageBrush ib = new ImageBrush(StackBitmap);
                  
                    ImageSelect.Source = Bitmap;


                    Rect rect1 = new Rect(Canvas.GetLeft(LastRectangle), Canvas.GetTop(LastRectangle), LastRectangle.Width, LastRectangle.Height);
                    System.Windows.Int32Rect rcFrom = new System.Windows.Int32Rect();
                    rcFrom.X = (int)Canvas.GetLeft(LastRectangle);
                    rcFrom.Y = (int)Canvas.GetTop(LastRectangle);
                    rcFrom.Width = (int)LastRectangle.Width;
                    rcFrom.Height = (int)LastRectangle.Height;
                    
                     bs = new CroppedBitmap(ImageSelect.Source as BitmapSource, rcFrom);
                    Rectangle dl = new Rectangle();
                    dl.Height = LastRectangle.Height;
                    dl.Width = LastRectangle.Width;
                    Canvas.SetLeft(dl, startPoint.X);
                    Canvas.SetTop(dl, startPoint.Y);
                    dl.Fill = Brushes.White;
                    dl.Stroke = Brushes.White;
                    PaintCanvas.Children.Add(dl);


                    ImageSelect.Source = bs;

                    ImageSelect.IsHitTestVisible = false;
                    ccm.Content = ImageSelect;
                    ccm.Style = this.FindResource("DesignerItemStyle") as Style;
                    ccm.Height = Math.Abs(startPoint.Y - endPoint.Y);
                    ccm.Width = Math.Abs(startPoint.X - endPoint.X);
                    Canvas.SetLeft(ccm, startPoint.X);
                    Canvas.SetTop(ccm, startPoint.Y);
                    Selector.SetIsSelected(ccm, true);
                   
                    PaintCanvas.Children.Add(ccm);

                   
                    isnew = false;
                    isMouseDowned = false;
                    RBCOPY.IsEnabled = true;
                    RBCUT.IsEnabled = true;
                }

            }
            if (flag == 21)
            {
                if (isMouseDowned && isnew == true)
                {
                    // Point endPoint = e.GetPosition((IInputElement)sender);
                    Point starPointTemp = new Point(0, 0);
                   
                    if (startPoint.X > endPoint.X)
                    {
                        double t = startPoint.X;
                        startPoint.X = endPoint.X;
                        endPoint.X = t;



                    }

                    if (startPoint.Y > endPoint.Y)
                    {
                        double t = startPoint.Y;
                        startPoint.Y = endPoint.Y;
                        endPoint.Y = t;

                    }
                    Point endPointTemp = new Point(endPoint.X - startPoint.X, endPoint.Y - startPoint.Y);

                    PathFigure pfg = new PathFigure();


                    double widthPll = endPointTemp.X - starPointTemp.X;
                    double heightPll = endPointTemp.Y - starPointTemp.Y;


                    Point point1 = new Point(starPointTemp.X + widthPll / 4, starPointTemp.Y);
                    //  MessageBox.Show(point1.ToString());
                    PointCollection polygonPoints = new PointCollection();
                    polygonPoints.Add(point1);
                    point1 = new Point(starPointTemp.X + widthPll * 3 / 4, starPointTemp.Y);
                    polygonPoints.Add(point1);
                    point1 = new Point(starPointTemp.X + widthPll * 3 / 4, starPointTemp.Y + heightPll / 2);
                    polygonPoints.Add(point1);
                    point1 = new Point(endPointTemp.X, starPointTemp.Y + heightPll / 2);
                    polygonPoints.Add(point1);
                    point1 = new Point(starPointTemp.X + widthPll / 2, endPointTemp.Y);
                    polygonPoints.Add(point1);
                    point1 = new Point(starPointTemp.X, starPointTemp.Y + heightPll / 2);
                    polygonPoints.Add(point1);
                    point1 = new Point(starPointTemp.X + widthPll / 4, starPointTemp.Y + heightPll / 2);
                    polygonPoints.Add(point1);
                    point1 = new Point(starPointTemp.X + widthPll / 4, starPointTemp.Y);
                    polygonPoints.Add(point1);

                    

                    pll2.Points = polygonPoints;

                    
                    //// ContentControl ccm = new ContentControl();
                    //Ellipse elip = new Ellipse();
                    SolidColorBrush brush = new SolidColorBrush();
                    brush.Color = cl;
                    pll2.Stroke = brush;
                    pll2.StrokeThickness = size;

                    SolidColorBrush brushBr = new SolidColorBrush();
                    brushBr.Color = br;
                    if (flagBrush == 1)
                        pll2.StrokeDashArray.Add(4);

                    if (TypeFill == 0)
                        pll2.Fill = null;
                    if (TypeFill == 1)
                        pll2.Fill = brushBr;
                    if (TypeFill == 2)
                        pll2.Fill = myHorizontalGradient;
                    if (TypeFill == 3)
                        pll2.Fill = myVerticalGradient;
                    if (TypeFill == 4)
                        pll2.Fill = myRadialGradientBrush;
                    if (TypeFill == 5)
                        pll2.Fill = myLinearGradientBrush;

                    pll2.IsHitTestVisible = false;
                    pll2.Stretch = Stretch.Fill;
                    ccm.Content = pll2;
                    ccm.Style = this.FindResource("DesignerItemStyle") as Style;
                    ccm.Height = Math.Abs(startPoint.Y - endPoint.Y);
                    ccm.Width = Math.Abs(startPoint.X - endPoint.X);
                    Canvas.SetLeft(ccm, startPoint.X);
                    Canvas.SetTop(ccm, startPoint.Y);
                    Selector.SetIsSelected(ccm, true);
                    PaintCanvas.Children.Remove(pll);
                    PaintCanvas.Children.Add(ccm);
                    isnew = false;
                    isMouseDowned = false;
                   
                }
            }
            if (flag == 22)
            {
                if (isMouseDowned && isnew == true)
                {
                    // Point endPoint = e.GetPosition((IInputElement)sender);
                    Point starPointTemp = new Point(0, 0);

                    if (startPoint.X > endPoint.X)
                    {
                        double t = startPoint.X;
                        startPoint.X = endPoint.X;
                        endPoint.X = t;



                    }

                    if (startPoint.Y > endPoint.Y)
                    {
                        double t = startPoint.Y;
                        startPoint.Y = endPoint.Y;
                        endPoint.Y = t;

                    }
                    Point endPointTemp = new Point(endPoint.X - startPoint.X, endPoint.Y - startPoint.Y);

                    PathFigure pfg = new PathFigure();


                    double widthPll = endPointTemp.X - starPointTemp.X;
                    double heightPll = endPointTemp.Y - starPointTemp.Y;


                    Point point1 = new Point(starPointTemp.X, starPointTemp.Y + heightPll / 4);
                    //  MessageBox.Show(point1.ToString());
                    PointCollection polygonPoints = new PointCollection();
                    polygonPoints.Add(point1);
                    point1 = new Point(starPointTemp.X + widthPll / 2, starPointTemp.Y + heightPll / 4);
                    polygonPoints.Add(point1);
                    point1 = new Point(starPointTemp.X + widthPll / 2, starPointTemp.Y);
                    polygonPoints.Add(point1);
                    point1 = new Point(endPointTemp.X, starPointTemp.Y + heightPll / 2);
                    polygonPoints.Add(point1);
                    point1 = new Point(starPointTemp.X + widthPll / 2, endPointTemp.Y);
                    polygonPoints.Add(point1);
                    point1 = new Point(starPointTemp.X + widthPll / 2, endPointTemp.Y - heightPll / 4);
                    polygonPoints.Add(point1);
                    point1 = new Point(starPointTemp.X, starPointTemp.Y + heightPll * 3 / 4);
                    polygonPoints.Add(point1);
                    point1 = new Point(starPointTemp.X, starPointTemp.Y + heightPll / 4);
                    polygonPoints.Add(point1);

                  

                    pll2.Points = polygonPoints;


                    //// ContentControl ccm = new ContentControl();
                    //Ellipse elip = new Ellipse();
                    SolidColorBrush brush = new SolidColorBrush();
                    brush.Color = cl;
                    pll2.Stroke = brush;
                    pll2.StrokeThickness = size;

                    SolidColorBrush brushBr = new SolidColorBrush();
                    brushBr.Color = br;
                    if (flagBrush == 1)
                        pll2.StrokeDashArray.Add(4);

                    if (TypeFill == 0)
                        pll2.Fill = null;
                    if (TypeFill == 1)
                        pll2.Fill = brushBr;
                    if (TypeFill == 2)
                        pll2.Fill = myHorizontalGradient;
                    if (TypeFill == 3)
                        pll2.Fill = myVerticalGradient;
                    if (TypeFill == 4)
                        pll2.Fill = myRadialGradientBrush;
                    if (TypeFill == 5)
                        pll2.Fill = myLinearGradientBrush;
                    pll2.Stretch = Stretch.Fill;
                    pll2.IsHitTestVisible = false;
                    ccm.Content = pll2;
                    ccm.Style = this.FindResource("DesignerItemStyle") as Style;
                    ccm.Height = Math.Abs(startPoint.Y - endPoint.Y);
                    ccm.Width = Math.Abs(startPoint.X - endPoint.X);
                    Canvas.SetLeft(ccm, startPoint.X);
                    Canvas.SetTop(ccm, startPoint.Y);
                    Selector.SetIsSelected(ccm, true);
                    PaintCanvas.Children.Remove(pll);
                    PaintCanvas.Children.Add(ccm);
                    isnew = false;
                    isMouseDowned = false;

                }
            }
            if (flag == 23)
            {
                if (isMouseDowned && isnew == true)
                {
                    // Point endPoint = e.GetPosition((IInputElement)sender);
                    Point starPointTemp = new Point(0, 0);

                    if (startPoint.X > endPoint.X)
                    {
                        double t = startPoint.X;
                        startPoint.X = endPoint.X;
                        endPoint.X = t;



                    }

                    if (startPoint.Y > endPoint.Y)
                    {
                        double t = startPoint.Y;
                        startPoint.Y = endPoint.Y;
                        endPoint.Y = t;

                    }
                    Point endPointTemp = new Point(endPoint.X - startPoint.X, endPoint.Y - startPoint.Y);

                   

                    double widthPll = endPointTemp.X - starPointTemp.X;
                    double heightPll = endPointTemp.Y - starPointTemp.Y;


                    Point point1 = new Point(starPointTemp.X + widthPll / 4, endPointTemp.Y);
                    //  MessageBox.Show(point1.ToString());
                    PointCollection polygonPoints = new PointCollection();
                    polygonPoints.Add(point1);
                    point1 = new Point(starPointTemp.X + widthPll * 3 / 4, endPointTemp.Y);
                    polygonPoints.Add(point1);
                    point1 = new Point(starPointTemp.X + widthPll * 3 / 4, starPointTemp.Y + heightPll / 2);
                    polygonPoints.Add(point1);
                    point1 = new Point(endPointTemp.X, starPointTemp.Y + heightPll / 2);
                    polygonPoints.Add(point1);
                    point1 = new Point(starPointTemp.X + widthPll / 2, starPointTemp.Y);
                    polygonPoints.Add(point1);
                    point1 = new Point(starPointTemp.X, starPointTemp.Y + heightPll / 2);
                    polygonPoints.Add(point1);
                    point1 = new Point(starPointTemp.X + widthPll / 4, starPointTemp.Y + heightPll / 2);
                    polygonPoints.Add(point1);
                    point1 = new Point(starPointTemp.X + widthPll / 4, endPointTemp.Y);
                    polygonPoints.Add(point1);

                   

                    pll2.Points = polygonPoints;


                    //// ContentControl ccm = new ContentControl();
                    //Ellipse elip = new Ellipse();
                    SolidColorBrush brush = new SolidColorBrush();
                    brush.Color = cl;
                    pll2.Stroke = brush;
                    pll2.StrokeThickness = size;

                    SolidColorBrush brushBr = new SolidColorBrush();
                    brushBr.Color = br;
                    if (flagBrush == 1)
                        pll2.StrokeDashArray.Add(4);

                    if (TypeFill == 0)
                        pll2.Fill = null;
                    if (TypeFill == 1)
                        pll2.Fill = brushBr;
                    if (TypeFill == 2)
                        pll2.Fill = myHorizontalGradient;
                    if (TypeFill == 3)
                        pll2.Fill = myVerticalGradient;
                    if (TypeFill == 4)
                        pll2.Fill = myRadialGradientBrush;
                    if (TypeFill == 5)
                        pll2.Fill = myLinearGradientBrush;
                    pll2.Stretch = Stretch.Fill;
                    pll2.IsHitTestVisible = false;
                    ccm.Content = pll2;
                    ccm.Style = this.FindResource("DesignerItemStyle") as Style;
                    ccm.Height = Math.Abs(startPoint.Y - endPoint.Y);
                    ccm.Width = Math.Abs(startPoint.X - endPoint.X);
                    Canvas.SetLeft(ccm, startPoint.X);
                    Canvas.SetTop(ccm, startPoint.Y);
                    Selector.SetIsSelected(ccm, true);
                    PaintCanvas.Children.Remove(pll);
                    PaintCanvas.Children.Add(ccm);
                    isnew = false;
                    isMouseDowned = false;

                }
            }
            if (flag == 24)
            {
                if (isMouseDowned && isnew == true)
                {
                    // Point endPoint = e.GetPosition((IInputElement)sender);
                    Point starPointTemp = new Point(0, 0);

                    if (startPoint.X > endPoint.X)
                    {
                        double t = startPoint.X;
                        startPoint.X = endPoint.X;
                        endPoint.X = t;



                    }

                    if (startPoint.Y > endPoint.Y)
                    {
                        double t = startPoint.Y;
                        startPoint.Y = endPoint.Y;
                        endPoint.Y = t;

                    }
                    Point endPointTemp = new Point(endPoint.X - startPoint.X, endPoint.Y - startPoint.Y);

                    PathFigure pfg = new PathFigure();


                    double widthPll = endPointTemp.X - starPointTemp.X;
                    double heightPll = endPointTemp.Y - starPointTemp.Y;


                    Point point1 = new Point(endPointTemp.X, starPointTemp.Y + heightPll / 4);
                    //  MessageBox.Show(point1.ToString());
                    PointCollection polygonPoints = new PointCollection();
                    polygonPoints.Add(point1);
                    point1 = new Point(endPointTemp.X, starPointTemp.Y + heightPll * 3 / 4);
                    polygonPoints.Add(point1);
                    point1 = new Point(starPointTemp.X + widthPll / 2, starPointTemp.Y + heightPll * 3 / 4);
                    polygonPoints.Add(point1);
                    point1 = new Point(starPointTemp.X + widthPll / 2, endPointTemp.Y);
                    polygonPoints.Add(point1);
                    point1 = new Point(starPointTemp.X, starPointTemp.Y + heightPll / 2);
                    polygonPoints.Add(point1);
                    point1 = new Point(starPointTemp.X + widthPll / 2, starPointTemp.Y);
                    polygonPoints.Add(point1);
                    point1 = new Point(starPointTemp.X + widthPll / 2, starPointTemp.Y + heightPll / 4);
                    polygonPoints.Add(point1);
                    point1 = new Point(endPointTemp.X, starPointTemp.Y + heightPll / 4);
                    polygonPoints.Add(point1);
                    

                    pll2.Points = polygonPoints;


                    //// ContentControl ccm = new ContentControl();
                    //Ellipse elip = new Ellipse();
                    SolidColorBrush brush = new SolidColorBrush();
                    brush.Color = cl;
                    pll2.Stroke = brush;
                    pll2.StrokeThickness = size;

                    SolidColorBrush brushBr = new SolidColorBrush();
                    brushBr.Color = br;
                    if (flagBrush == 1)
                        pll2.StrokeDashArray.Add(4);

                    if (TypeFill == 0)
                        pll2.Fill = null;
                    if (TypeFill == 1)
                        pll2.Fill = brushBr;
                    if (TypeFill == 2)
                        pll2.Fill = myHorizontalGradient;
                    if (TypeFill == 3)
                        pll2.Fill = myVerticalGradient;
                    if (TypeFill == 4)
                        pll2.Fill = myRadialGradientBrush;
                    if (TypeFill == 5)
                        pll2.Fill = myLinearGradientBrush;
                    pll2.Stretch = Stretch.Fill;
                    pll2.IsHitTestVisible = false;
                    ccm.Content = pll2;
                    ccm.Style = this.FindResource("DesignerItemStyle") as Style;
                    ccm.Height = Math.Abs(startPoint.Y - endPoint.Y);
                    ccm.Width = Math.Abs(startPoint.X - endPoint.X);
                    Canvas.SetLeft(ccm, startPoint.X);
                    Canvas.SetTop(ccm, startPoint.Y);
                    Selector.SetIsSelected(ccm, true);
                    PaintCanvas.Children.Remove(pll);
                    PaintCanvas.Children.Add(ccm);
                    isnew = false;
                    isMouseDowned = false;

                }
            }
            if (flag == 25)
            {
                if (isMouseDowned && isnew == true)
                {
                    // Point endPoint = e.GetPosition((IInputElement)sender);
                    Point starPointTemp = new Point(0, 0);

                    if (startPoint.X > endPoint.X)
                    {
                        double t = startPoint.X;
                        startPoint.X = endPoint.X;
                        endPoint.X = t;



                    }

                    if (startPoint.Y > endPoint.Y)
                    {
                        double t = startPoint.Y;
                        startPoint.Y = endPoint.Y;
                        endPoint.Y = t;

                    }
                    Point endPointTemp = new Point(endPoint.X - startPoint.X, endPoint.Y - startPoint.Y);

                    PathFigure pfg = new PathFigure();


                    double widthPll = endPointTemp.X - starPointTemp.X;
                    double heightPll = endPointTemp.Y - starPointTemp.Y;


                    Point point1 = new Point(starPointTemp.X + widthPll / 2, starPointTemp.Y);
                    //  MessageBox.Show(point1.ToString());
                    PointCollection polygonPoints = new PointCollection();
                    polygonPoints.Add(point1);
                    point1 = new Point(starPointTemp.X + widthPll * 3 / 8, starPointTemp.Y + heightPll * 11 / 30);
                    polygonPoints.Add(point1);
                    point1 = new Point(starPointTemp.X, starPointTemp.Y + heightPll * 115 / 300);
                    polygonPoints.Add(point1);
                    point1 = new Point(starPointTemp.X + widthPll * 120 / 400, starPointTemp.Y + heightPll * 185 / 300);
                    polygonPoints.Add(point1);
                    point1 = new Point(starPointTemp.X + widthPll * 75 / 400, starPointTemp.Y + heightPll);
                    polygonPoints.Add(point1);
                    point1 = new Point(starPointTemp.X + widthPll / 2, starPointTemp.Y + heightPll * 230 / 300);
                    polygonPoints.Add(point1);
                    point1 = new Point(starPointTemp.X + widthPll * 325 / 400, starPointTemp.Y + heightPll);
                    polygonPoints.Add(point1);
                    point1 = new Point(starPointTemp.X + widthPll * 280 / 400, starPointTemp.Y + heightPll * 185 / 300);
                    polygonPoints.Add(point1);
                    point1 = new Point(endPointTemp.X, starPointTemp.Y + heightPll * 115 / 300);
                    polygonPoints.Add(point1);
                    point1 = new Point(starPointTemp.X + widthPll * 250 / 400, starPointTemp.Y + heightPll * 115 / 300);
                    polygonPoints.Add(point1);
                    point1 = new Point(starPointTemp.X + widthPll / 2, starPointTemp.Y);
                    polygonPoints.Add(point1);

                    

                    pll2.Points = polygonPoints;


                    //// ContentControl ccm = new ContentControl();
                    //Ellipse elip = new Ellipse();
                    SolidColorBrush brush = new SolidColorBrush();
                    brush.Color = cl;
                    pll2.Stroke = brush;
                    pll2.StrokeThickness = size;

                    SolidColorBrush brushBr = new SolidColorBrush();
                    brushBr.Color = br;
                    if (flagBrush == 1)
                        pll2.StrokeDashArray.Add(4);

                    if (TypeFill == 0)
                        pll2.Fill = null;
                    if (TypeFill == 1)
                        pll2.Fill = brushBr;
                    if (TypeFill == 2)
                        pll2.Fill = myHorizontalGradient;
                    if (TypeFill == 3)
                        pll2.Fill = myVerticalGradient;
                    if (TypeFill == 4)
                        pll2.Fill = myRadialGradientBrush;
                    if (TypeFill == 5)
                        pll2.Fill = myLinearGradientBrush;
                    pll2.Stretch = Stretch.Fill;
                    pll2.IsHitTestVisible = false;
                    ccm.Content = pll2;
                    ccm.Style = this.FindResource("DesignerItemStyle") as Style;
                    ccm.Height = Math.Abs(startPoint.Y - endPoint.Y);
                    ccm.Width = Math.Abs(startPoint.X - endPoint.X);
                    Canvas.SetLeft(ccm, startPoint.X);
                    Canvas.SetTop(ccm, startPoint.Y);
                    Selector.SetIsSelected(ccm, true);
                    PaintCanvas.Children.Remove(pll);
                    PaintCanvas.Children.Add(ccm);
                    isnew = false;
                    isMouseDowned = false;

                }
            }
            if (flag == 20)
            {
                if (isMouseDowned && isnew == true)
                {
                    // Point endPoint = e.GetPosition((IInputElement)sender);
                    Point starPointTemp = new Point(0, 0);

                    if (startPoint.X > endPoint.X)
                    {
                        double t = startPoint.X;
                        startPoint.X = endPoint.X;
                        endPoint.X = t;



                    }

                    if (startPoint.Y > endPoint.Y)
                    {
                        double t = startPoint.Y;
                        startPoint.Y = endPoint.Y;
                        endPoint.Y = t;

                    }
                    Point endPointTemp = new Point(endPoint.X - startPoint.X, endPoint.Y - startPoint.Y);

                   


                    double widthPll = endPointTemp.X - starPointTemp.X;
                    double heightPll = endPointTemp.Y - starPointTemp.Y;


                    Point point1 = new Point(starPointTemp.X + widthPll / 2, starPointTemp.Y + heightPll * 70 / 300);



                    Point point2 = new Point(starPointTemp.X + widthPll * 70 / 300, starPointTemp.Y);

                    Point point3 = new Point(starPointTemp.X, starPointTemp.Y + heightPll * 85 / 300);

                    Point point4 = new Point(starPointTemp.X + widthPll * 90 / 300, starPointTemp.Y + heightPll * 245 / 300);

                    Point point5 = new Point(starPointTemp.X + widthPll / 2, starPointTemp.Y + heightPll);

                    Point point6 = new Point(starPointTemp.X + widthPll * 210 / 300, starPointTemp.Y + heightPll * 245 / 300);

                    Point point7 = new Point(starPointTemp.X + widthPll, starPointTemp.Y + heightPll * 85 / 300);

                    Point point8 = new Point(starPointTemp.X + widthPll * 230 / 300, starPointTemp.Y);

                    Point point9 = new Point(starPointTemp.X + widthPll / 2, starPointTemp.Y + heightPll * 70 / 300);


                    Point[] points = new[] { point1, point2, point3, point4, point5, point6, point7, point8, point9 };
                    var b = GetBezierApproximation(points, 256);
                    PathFigure pf = new PathFigure(b.Points[0], new[] { b }, true);

                    PathFigureCollection pfc = new PathFigureCollection();
                    pfc.Add(pf);
                    var pge = new PathGeometry();
                    pge.Figures = pfc;

                    
                    path2.Data = pge;

                    SolidColorBrush brush = new SolidColorBrush();
                    brush.Color = cl;
                    path2.Stroke = brush;


                    SolidColorBrush brushBr = new SolidColorBrush();
                    brushBr.Color = br;
                    if (flagBrush == 1)
                        path2.StrokeDashArray.Add(4);
                    if (flagBrush == 0)
                        path2.StrokeDashArray.Clear();

                    if (TypeFill == 0)
                        path2.Fill = null;
                    if (TypeFill == 1)
                        path2.Fill = brushBr;
                    if (TypeFill == 2)
                        path2.Fill = myHorizontalGradient;
                    if (TypeFill == 3)
                        path2.Fill = myVerticalGradient;
                    if (TypeFill == 4)
                        path2.Fill = myRadialGradientBrush;
                    if (TypeFill == 5)
                        path2.Fill = myLinearGradientBrush;

                    path2.StrokeThickness = size;
                    path2.Stretch = Stretch.Fill;
                    path2.IsHitTestVisible = false;
                    ccm.Content = path2;
                    ccm.Style = this.FindResource("DesignerItemStyle") as Style;
                    ccm.Height = Math.Abs(startPoint.Y - endPoint.Y);
                    ccm.Width = Math.Abs(startPoint.X - endPoint.X);
                    Canvas.SetLeft(ccm, startPoint.X);
                    Canvas.SetTop(ccm, startPoint.Y);
                    Selector.SetIsSelected(ccm, true);
                    PaintCanvas.Children.Remove(path);
                    PaintCanvas.Children.Add(ccm);
                    isnew = false;
                    isMouseDowned = false;

                }
            }
        }



        private void scrollBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }


      
     

    

        private void RBPENCIL_Click(object sender, RoutedEventArgs e)
        {
            flag = 0;
            PaintCanvas.IsEnabled = true;
           // PaintInk.IsEnabled = true;
            DefaultButton();
            RBPENCIL.IsEnabled = false;
            if (isnew == false)
                SaveCanvasToBitmap(PaintCanvas);
            isnew = true;
        }

        private void RBLINE_Click(object sender, RoutedEventArgs e)
        {
            flag = 1;
           // PaintInk.IsEnabled = false;
            PaintCanvas.IsEnabled = true;
            DefaultButton();
            RBLINE.IsEnabled = false;
            if (isnew == false)
                SaveCanvasToBitmap(PaintCanvas);
            isnew = true;
        }

        private void RBELLIPSe_Click(object sender, RoutedEventArgs e)
        {
            flag = 2;
            if (isnew == false)
                SaveCanvasToBitmap(PaintCanvas);
            
           // PaintInk.IsEnabled = false;
            PaintCanvas.IsEnabled = true;
            DefaultButton();
            RBELLIPSe.IsEnabled = false;
            isnew = true;
        }

        private void RBRECTANGLE_Click(object sender, RoutedEventArgs e)
        {
            flag = 3;
         //   PaintInk.IsEnabled = false;
            PaintCanvas.IsEnabled = true;
            DefaultButton();
            RBRECTANGLE.IsEnabled = false;
            if (isnew == false)
                SaveCanvasToBitmap(PaintCanvas);
            isnew = true;
        }

        private void RBCIRCLE_Click(object sender, RoutedEventArgs e)
        {
            flag = 4;
            //PaintInk.IsEnabled = false;
            PaintCanvas.IsEnabled = true;
            DefaultButton();
            RBCIRCLE.IsEnabled = false;
            if (isnew == false)
                SaveCanvasToBitmap(PaintCanvas);
            isnew = true;
        }

        private void RBTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (RBTB.Text.ToString() != "")
            {
                size = int.Parse(RBTB.Text.ToString());
                SetChangerResize();
            }
           
        }

        public void SetChangerBrush()
        {
            if(flag ==1)
            {
                if (flagBrush == 1)
                    line.StrokeDashArray.Add(4);
                if (flagBrush == 0)
                    line.StrokeDashArray.Clear();
            }
            if(flag == 2 || flag ==4)
            {
                if (flagBrush == 1)
                    elip.StrokeDashArray.Add(4);
                if (flagBrush == 0)
                    elip.StrokeDashArray.Clear();
            }
            if(flag == 3 || flag == 5)
            {
                if (flagBrush == 1)
                    rec.StrokeDashArray.Add(4);
                if (flagBrush == 0)
                    rec.StrokeDashArray.Clear();
            }
            if(flag == 21 || flag == 22 || flag == 23 || flag == 24 || flag == 25)
            {
                if (flagBrush == 1)
                    pll2.StrokeDashArray.Add(4);
                if (flagBrush == 0)
                    pll2.StrokeDashArray.Clear();
            }
            if(flag == 20)
            {
                if (flagBrush == 1)
                    path2.StrokeDashArray.Add(4);
                if (flagBrush == 0)
                    path2.StrokeDashArray.Clear();
            }
        }

        public void SetChangerResize()
        {
            if (flag == 1)
            {
                line.StrokeThickness = size;
            }
            if (flag == 2 || flag == 4)
            {
                elip.StrokeThickness = size;
            }
            if (flag == 3 || flag == 5)
            {
                rec.StrokeThickness = size;
            }
            if (flag == 21 || flag == 22 || flag == 23 || flag == 24 || flag == 25)
            {
                pll2.StrokeThickness = size;
            }
            if (flag == 20)
            {
                path2.StrokeThickness = size;
            }
            RBTB.Text = size.ToString();
        }

        //save picture
        public void save(Canvas paintcanvas)
        {
            RenderTargetBitmap bitmap = new RenderTargetBitmap((int)paintcanvas.RenderSize.Width, (int)paintcanvas.RenderSize.Height, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(paintcanvas);
            BitmapFrame fram = BitmapFrame.Create(bitmap);
            BitmapEncoder Encoder = new PngBitmapEncoder();
            Encoder.Frames.Add(fram);
            SaveFileDialog sa = new SaveFileDialog();
            sa.Filter = "PNG|*.png|" +
                        "JPG|*.jpg|" +
                        "BITMAP|*.bmp";
            sa.FileName = "New Image";
            sa.DefaultExt = ".png";
            if (sa.ShowDialog() == true)
            {
                using (var fs = System.IO.File.OpenWrite(sa.FileName))
                {
                    Encoder.Save(fs);
                }
            }
        }

        private void RBSAVE_Click(object sender, RoutedEventArgs e)
        {
            save(PaintCanvas);
        }


// open picture

        private void button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                Image im = new Image();
                BitmapImage bim = new BitmapImage(new Uri(op.FileName));

                im.Source = bim;
                PaintCanvas.Children.Add(im);
                InitializeComponent();
            }
        }


        private void RBOPEN_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                Image im = new Image();
                BitmapImage bim = new BitmapImage(new Uri(op.FileName));

                im.Source = bim;
                PaintCanvas.Children.Add(im);

            }
        }




     

// save canvas thành bitmap and load this bitmap thành canvas
        public void SaveCanvasToBitmap(Canvas surface)
        {

            if (flag == 11)
                PaintCanvas.Children.Remove(ccm);
            Selector.SetIsSelected(ccm, false);
            undoImage.Push(StackBitmap);
            slud++;
            StackBitmap = new RenderTargetBitmap((int)surface.RenderSize.Width, (int)surface.RenderSize.Height, 96, 96, PixelFormats.Pbgra32);
            StackBitmap.Render(surface);
            if (flag != 11)
                PaintCanvas.Children.Remove(ccm);
            ImageBrush ib = new ImageBrush(StackBitmap);
            Image im = new Image();
            im.Source = StackBitmap;
            redoImage.Clear();
            slrd = 0;
            Redo.IsEnabled = false;
            

            surface.Children.Add(im);
            if (slud > 0)
                Undo.IsEnabled = true;
            if (flag == 1)
                PaintCanvas.Children.Remove(line);
            if (flag == 2 || flag == 4)
                PaintCanvas.Children.Remove(elip);
            if (flag == 3 || flag == 5)
                PaintCanvas.Children.Remove(rec);
            if (flag == 21 || flag == 22 || flag == 23 || flag == 24 || flag == 25)
                PaintCanvas.Children.Remove(pll2);
            if (flag == 20)
                PaintCanvas.Children.Remove(path2);
        }

// changer color shape

        public void SetChangerColor()
        {
            if (flag == 2 || flag == 4)
            {
                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = cl;
                elip.Stroke = brush;
            }
            if (flag == 5 || flag == 3)
            {
                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = cl;
                rec.Stroke = brush;
            }
            if(flag == 21 || flag == 22 || flag == 23 || flag == 24 || flag == 25)
            {
                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = cl;
                pll2.Stroke = brush;
            }
            if(flag == 20)
            {
                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = cl;
                path2.Stroke = brush;
            }
        }



        private void RBBLACK_Click(object sender, RoutedEventArgs e)
        {

            cl = Colors.Black;
            SetChangerColor();
        }

        private void RBRED_Click(object sender, RoutedEventArgs e)
        {

            cl = Colors.Red;
            SetChangerColor();
        }

        private void ClPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {

            cl = (Color)ClPicker.SelectedColor;
            SetChangerColor();
        }

 // changer color text
        private void ClPickerText_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Color clText = (Color)ClPickerText.SelectedColor;
            SolidColorBrush brushText = new SolidColorBrush();
            brushText.Color = clText;
            brt = brushText;
            Teb.Foreground = brt;
        }
// changer color brush
        public void SetChangerFill()
        {
            SolidColorBrush brushBr = new SolidColorBrush();
            brushBr.Color = br;
            if (flag == 2 || flag == 4)
            {
                if (TypeFill == 0)
                    elip.Fill = null;
                if (TypeFill == 1)
                    elip.Fill = brushBr;
                if (TypeFill == 2)
                    elip.Fill = myHorizontalGradient;
                if (TypeFill == 3)
                    elip.Fill = myVerticalGradient;
                if (TypeFill == 4)
                    elip.Fill = myRadialGradientBrush;
                if (TypeFill == 5)
                    elip.Fill = myLinearGradientBrush;
            }
            if (flag == 5 || flag == 3)
            {
                if (TypeFill == 0)
                    rec.Fill = null;
                if (TypeFill == 1)
                    rec.Fill = brushBr;
                if (TypeFill == 2)
                    rec.Fill = myHorizontalGradient;
                if (TypeFill == 3)
                    rec.Fill = myVerticalGradient;
                if (TypeFill == 4)
                    rec.Fill = myRadialGradientBrush;
                if (TypeFill == 5)
                    rec.Fill = myLinearGradientBrush;

            }
            if (flag == 21 || flag == 22 || flag == 23 || flag == 24 || flag == 25)
            {
                if (TypeFill == 0)
                    pll2.Fill = null;
                if (TypeFill == 1)
                    pll2.Fill = brushBr;
                if (TypeFill == 2)
                    pll2.Fill = myHorizontalGradient;
                if (TypeFill == 3)
                    pll2.Fill = myVerticalGradient;
                if (TypeFill == 4)
                    pll2.Fill = myRadialGradientBrush;
                if (TypeFill == 5)
                    pll2.Fill = myLinearGradientBrush;
            }
            if (flag == 20)
            {
                if (TypeFill == 0)
                    path2.Fill = null;
                if (TypeFill == 1)
                    path2.Fill = brushBr;
                if (TypeFill == 2)
                    path2.Fill = myHorizontalGradient;
                if (TypeFill == 3)
                    path2.Fill = myVerticalGradient;
                if (TypeFill == 4)
                    path2.Fill = myRadialGradientBrush;
                if (TypeFill == 5)
                    path2.Fill = myLinearGradientBrush;
            }
            if (flag == 11)
                Teb.Background = brushBr;

        }

        private void ClPickerBrush_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            br = (Color)ClPickerBrush.SelectedColor;
            SetChangerFill();
        }

// set click các button
        private void RBSQUARE_Click(object sender, RoutedEventArgs e)
        {
            flag = 5;
            PaintCanvas.IsEnabled = true;
            DefaultButton();
            RBSQUARE.IsEnabled = false;


        }

        public void DefaultBrush()
        {
            RBNOBRUSH.IsEnabled = true;
            RBDASH.IsEnabled = true;
        }

  


        private void RBNOBRUSH_Click(object sender, RoutedEventArgs e)
        {
            DefaultBrush();
            flagBrush = 0;
            SetChangerBrush();
            RBNOBRUSH.IsEnabled = false;
        }

        private void RBDASH_Click(object sender, RoutedEventArgs e)
        {
            DefaultBrush();
            flagBrush = 1;
            SetChangerBrush();
            RBDASH.IsEnabled = false;
        }


       
      

        private void RBSELECT_Click(object sender, RoutedEventArgs e)
        {
           
            if(isnew == false)
                SaveCanvasToBitmap(PaintCanvas);
           
            flag = 10;
            DefaultButton();
            RBSELECT.IsEnabled = false;
            RBPENCIL.IsEnabled = true;
            isnew = true; 
        }
        
     

        private void RBCUT_Click(object sender, RoutedEventArgs e)
        {
            SaveCanvasToBitmap(PaintCanvas);
            Rectangle dl = new Rectangle();
            dl.Height = ccm.Height;
            dl.Width = ccm.Width;
            Canvas.SetLeft(dl, startPoint.X);
            Canvas.SetTop(dl, startPoint.Y);
            dl.Fill = Brushes.White;
            dl.Stroke = Brushes.White;
            PaintCanvas.Children.Add(dl);

            PaintCanvas.Children.Remove(ccm);
            Clipboard.SetImage(bs);
        }

        private void RBTEXT_Click(object sender, RoutedEventArgs e)
        {
            DefaultButton();
            RBTEXT.IsEnabled = false;
            flag = 11;
          //  PaintCanvas.Background = Brushes.Transparent;
            
        }

        private void RBTextRED_Click(object sender, RoutedEventArgs e)
        {
            brt = Brushes.Red;
            Teb.Foreground = brt;
            
        }

        private void RBTextBLACK_Click(object sender, RoutedEventArgs e)
        {
            brt = Brushes.Black;
            Teb.Foreground = brt;
        }



        private void RBCOPY_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetImage(bs);
        }

        private void RBPASTE_Click(object sender, RoutedEventArgs e)
        {
            if (Clipboard.GetImage() != null)
            {
                SaveCanvasToBitmap(PaintCanvas);
                Image imp = new Image();
                var ims = Clipboard.GetImage();
                imp.Source = new FormatConvertedBitmap(ims, PixelFormats.Bgr32, null, 0);
                ContentControl ccmp = new ContentControl();

                imp.IsHitTestVisible = false;
                ccm.Content = imp;
                ccm.Style = this.FindResource("DesignerItemStyle") as Style;
                ccm.Height = imp.Height + 10;
                ccm.Width = imp.Width + 10;
                Canvas.SetLeft(ccm, 10);
                Canvas.SetTop(ccm, 160);
                Selector.SetIsSelected(ccm, true);
                PaintCanvas.Children.Add(ccm);
                isnew = false;
            }
        }

        private void RBFILL_Click(object sender, RoutedEventArgs e)
        {
            flag = 12;
            SaveCanvasToBitmap(PaintCanvas);

            //PaintInk.IsEnabled = false;
            PaintCanvas.IsEnabled = true;
            DefaultButton();
            RBFILL.IsEnabled = false;
            isnew = true;

        }

        private void RBHEART_Click(object sender, RoutedEventArgs e)
        {
            flag = 20;
            //PaintInk.IsEnabled = false;
            PaintCanvas.IsEnabled = true;
            DefaultButton();
            RBHEART.IsEnabled = false;
        }

        private void RBBOTTOM_Click(object sender, RoutedEventArgs e)
        {
            flag = 21;
            //PaintInk.IsEnabled = false;
            PaintCanvas.IsEnabled = true;
            DefaultButton();
            RBBOTTOM.IsEnabled = false;
        }

        private void RBRIGHT_Click(object sender, RoutedEventArgs e)
        {
            flag = 22;
            //PaintInk.IsEnabled = false;
            PaintCanvas.IsEnabled = true;
            DefaultButton();
            RBRIGHT.IsEnabled = false;
        }

        private void TBTOP_Click(object sender, RoutedEventArgs e)
        {
            flag = 23;
            //PaintInk.IsEnabled = false;
            PaintCanvas.IsEnabled = true;
            DefaultButton();
            TBTOP.IsEnabled = false;
        }

        private void RBLEFT_Click(object sender, RoutedEventArgs e)
        {
            flag = 24;
            //PaintInk.IsEnabled = false;
            PaintCanvas.IsEnabled = true;
            DefaultButton();
            RBLEFT.IsEnabled = false;
        }

        private void RBSTAR_Click(object sender, RoutedEventArgs e)
        {
            flag = 25;
            //PaintInk.IsEnabled = false;
            PaintCanvas.IsEnabled = true;
            DefaultButton();
            RBSTAR.IsEnabled = false;
        }

        private void TBold_Click(object sender, RoutedEventArgs e)
        {
            if (Tbold)
            {
                Tbold = false;
                Teb.FontWeight = FontWeights.Normal;
                TBold.Background = Brushes.White;
            }
            else
            {
                TBold.Background = Brushes.Gray;
                Tbold = true;
                Teb.FontWeight = FontWeights.Bold;
               

            }
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            RenderTargetBitmap bitmap = new RenderTargetBitmap((int)PaintCanvas.RenderSize.Width, (int)PaintCanvas.RenderSize.Height, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(PaintCanvas);
            redoImage.Push(bitmap);
            bitmap = undoImage.Pop();

            Image imp = new Image();

            imp.Source = bitmap;
            
            PaintCanvas.Children.Add(imp);
            slrd++;
            slud--;
            if (slud < 1)
                Undo.IsEnabled = false;
            if (slrd > 0)
                Redo.IsEnabled = true;
            //MessageBox.Show(slud.ToString());
           
        }

        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            RenderTargetBitmap bitmap = new RenderTargetBitmap((int)PaintCanvas.RenderSize.Width, (int)PaintCanvas.RenderSize.Height, 96, 96, PixelFormats.Pbgra32);
            bitmap.Render(PaintCanvas);
            undoImage.Push(bitmap);
            bitmap = redoImage.Pop();

            Image imp = new Image();

            imp.Source = bitmap;

            PaintCanvas.Children.Add(imp);
            slud++;
            slrd--;
            if (slud > 0)
                Undo.IsEnabled = true;
            if (slrd < 1)
                Redo.IsEnabled = false;
        }

        private void TTitanic_Click(object sender, RoutedEventArgs e)
        {
            if (TItalic)
            {
                TItalic = false;
                Teb.FontStyle = FontStyles.Normal;
                TTitanic.Background = Brushes.White;
            }
            else
            {
                TItalic = true;
                Teb.FontStyle = FontStyles.Italic;
                TTitanic.Background = Brushes.Gray;
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            AdornerLayer aLayer = AdornerLayer.GetAdornerLayer(PaintCanvas);
            Canvas.SetLeft(aLayer, 0);
            Canvas.SetTop(aLayer, 0);
            PaintCanvas.Height = 560;
            PaintCanvas.Width = 1000;

             aLayer.Add(new ResizingAdorner(PaintCanvas));

            
            
        }

        private void TUnderline_Click(object sender, RoutedEventArgs e)
        {
            if (Tunderline)
            {
                Tunderline = false;
            }
            else
            {
                Tunderline = true;
            }

        }

        private void ribbon_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {

        }
// các Combobox
        private void Font_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChangedType = true;
            switch (Font.SelectedItem.ToString())
            {

                case "Times New Roman":
                    {
                        f = new FontFamily("Times New Roman");
                        Teb.FontFamily = f;
                        break;
                    }
                case "Snap ITC":
                    {
                        f = new FontFamily("Snap ITC");
                        Teb.FontFamily = f;
                        break;
                    }
                case "Gigi":
                    {
                        f = new FontFamily("Gigi");
                        Teb.FontFamily = f;
                        break;
                    }
                case "Gill Sans Ultra Bold":
                    {
                        f = new FontFamily("Gill Sans Ultra Bold");
                        Teb.FontFamily = f;
                        break;
                    }
                case "DotumChe":
                    {
                        f = new FontFamily("DotumChe");
                        Teb.FontFamily = f;
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }
        private void SizeText_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChangedType = true;
            switch (SizeText.SelectedItem.ToString())
            {

                case "10":
                    {
                        sizeText = 10;
                        Teb.FontSize = 10;
                        break;
                    }
                case "15":
                    {
                        sizeText = 15;
                        Teb.FontSize = 15;
                        break;
                    }
                case "20":
                    {
                        sizeText = 20;
                        Teb.FontSize = 20;
                        break;
                    }
                case "25":
                    {
                        sizeText = 25;
                        Teb.FontSize = 25;
                        break;
                    }
                case "30":
                    {
                        sizeText = 30;
                        Teb.FontSize = 30;
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }
        private void CBBSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChangedType = true;
            switch (CBBSize.SelectedItem.ToString())
            {
                case "2":
                    {

                        size = 2;
                        SetChangerResize();
                        break;
                    }
                case "5":
                    {

                        size = 5;
                        SetChangerResize();
                        break;
                    }
                case "10":
                    {
                        size = 10;
                        SetChangerResize();
                        break;
                    }
                case "15":
                    {
                        size = 15;
                        SetChangerResize(); 
                        break;
                    }
                case "20":
                    {
                        size = 20;
                        SetChangerResize();
                        break;
                    }
                case "25":
                    {
                        size = 25;
                        SetChangerResize();
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }
        private void CBBFill_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isChangedType = true;
            switch (CBBFill.SelectedItem.ToString())
            {
                case "No Fill":
                    {

                        TypeFill = 0;
                        SetChangerFill();
                        break;
                    }
                case "Solid Fill":
                    {

                        TypeFill = 1;
                        SetChangerFill();
                        break;
                    }
                case "HorizontalRadialGradient":
                    {
                        TypeFill = 2;
                        SetChangerFill();
                        break;
                    }
                case "VerticalRadialGradient":
                    {
                        TypeFill = 3;
                        SetChangerFill();
                        break;
                    }
                case "RadialGradientBrush":
                    {
                        TypeFill = 4;
                        SetChangerFill();
                        break;
                    }
                case "LinearGradientBrush":
                    {
                        TypeFill = 5;
                        SetChangerFill();
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }
        

 // hàm tao polyline segment
        PolyLineSegment GetBezierApproximation(Point[] controlPoints, int outputSegmentCount)
        {
            Point[] points = new Point[outputSegmentCount + 1];
            for (int i = 0; i <= outputSegmentCount; i++)
            {
                double t = (double)i / outputSegmentCount;
                points[i] = GetBezierPoint(t, controlPoints, 0, controlPoints.Length);
            }
            return new PolyLineSegment(points, true);
        }

        Point GetBezierPoint(double t, Point[] controlPoints, int index, int count)
        {
            if (count == 1)
                return controlPoints[index];
            var P0 = GetBezierPoint(t, controlPoints, index, count - 1);
            var P1 = GetBezierPoint(t, controlPoints, index + 1, count - 1);
            return new Point((1 - t) * P0.X + t * P1.X, (1 - t) * P0.Y + t * P1.Y);
        }
    }
}
