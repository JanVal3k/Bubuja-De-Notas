// NotaWindow.xaml.cs
using System.Windows;
using System.Windows.Input;

namespace BurbujasDeNotas
{
    public partial class NotaWindow : Window
    {
        // Variables para controlar el arrastre de la ventana
        private bool estaArrastrando = false;
        private Point posicionInicial;

        // Constructor
        public NotaWindow()
        {
            InitializeComponent();
            this.Topmost = true; // la ventana quedaria sobre todas las demas
            this.WindowStyle = WindowStyle.None; // quita los menus predeterminados para las ventans
        }

        // Se ejecuta cuando se presiona el botón del mouse
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Solo si es el botón izquierdo
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                estaArrastrando = true;
                posicionInicial = e.GetPosition(this);  // Guarda la posición inicial
                this.CaptureMouse();  // Captura eventos del mouse
            }
        }

        // Se ejecuta cuando se mueve el mouse
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (estaArrastrando)
            {
                // Si se soltó el botón mientras se movía
                if (e.LeftButton == MouseButtonState.Released)
                {
                    estaArrastrando = false;
                    this.ReleaseMouseCapture();
                    return;
                }

                // Calcula la nueva posición
                var posicionActual = e.GetPosition(this);
                var offset = posicionActual - posicionInicial;

                // Mueve la ventana
                this.Left += offset.X;
                this.Top += offset.Y;
            }
        }

        // Minimiza la ventana
        private void MinimizarButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        // Cierra la ventana
        private void CerrarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}