// NotaWindow.xaml.cs
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


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
            this.ResizeMode = ResizeMode.NoResize; //Evita el cambio de tamaño de la ventana
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
        // Agrega una nueva nota
        private void AgregarNotaButton_Click(object sender, RoutedEventArgs e)
        {
            var nuevaNota = new NotaWindow();
            nuevaNota.Show();
        }
        // metodo para seleccionar color
        private void PersonalizarNotaButton_Click(object sender, RoutedEventArgs e)
        {
            colorPickerPopup.IsOpen = !colorPickerPopup.IsOpen;
        }
        // Evento que se dispara cuando se selecciona un color en el ColorPicker
        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color?> e)
        {
            if (e.NewValue.HasValue)
            {
                var border = (Border)this.Content;
                border.Background = new SolidColorBrush(e.NewValue.Value);
            }
        }
        private void ApplyColorButton_Click(object sender, RoutedEventArgs e)
        {
            if (colorPicker.SelectedColor.HasValue)
            {
                // Cambiar el color de fondo de la nota
                var border = (Border)this.Content;
                border.Background = new SolidColorBrush(colorPicker.SelectedColor.Value);

                // Cerrar el popup
                colorPickerPopup.IsOpen = false;
            }
        }
    }
}