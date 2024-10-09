using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BurbujasDeNotas
{
    public partial class MainWindow : Window
    {
        private int widgetCount = 0;
        private bool isDragging = false;
        private List<SecondaryWidget> secondaryWidgets = new List<SecondaryWidget>();
        private Point offset;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                isDragging = true;
                var mousePos = e.GetPosition(this);
                offset = new Point(mousePos.X, mousePos.Y);
                this.CaptureMouse();

                if (widgetCount == 0)
                {
                    widgetCount++;
                    var newWidget = new SecondaryWidget
                    {
                        Title = "Widget Flotante " + widgetCount,
                        Left = this.Left + this.Width + 10,
                        Top = this.Top
                    };
                    secondaryWidgets.Add(newWidget);
                    newWidget.Show();
                }
            }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                if (e.LeftButton == MouseButtonState.Released)
                {
                    Window_MouseUp(sender, null);
                    return;
                }

                // Obtener la posición actual del cursor en coordenadas de pantalla
                Point currentMousePosition = PointToScreen(e.GetPosition(this));

                // Ajustar la posición del widget principal
                this.Left = currentMousePosition.X - offset.X;
                this.Top = currentMousePosition.Y - offset.Y;

                // Mover todos los widgets secundarios
                foreach (var widget in secondaryWidgets)
                {
                    widget.Left = this.Left + this.Width + 10;
                    widget.Top = this.Top;
                }
            }
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (isDragging)
            {
                isDragging = false;
                this.ReleaseMouseCapture();
            }
        }
    }

    public class SecondaryWidget : Window
    {
        public SecondaryWidget()
        {
            WindowStyle = WindowStyle.None;
            AllowsTransparency = true;
            Background = Brushes.Transparent;
            Topmost = true;
            Height = 50;
            Width = 50;

            var border = new Border
            {
                Background = Brushes.LightGreen,
                CornerRadius = new CornerRadius(20),
                BorderThickness = new Thickness(2),
                BorderBrush = Brushes.DarkGreen
            };

            Content = border;
        }
    }
}