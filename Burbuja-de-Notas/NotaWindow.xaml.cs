// NotaWindow.xaml.cs
using System;
using System.Collections.Generic;
using System.IO;
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
        private List<Note> notas; // para listar las notas creadas
        private string rutaDirectorio = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\BurbujasDeNotas";

        // Constructor
        public NotaWindow()
        {
            InitializeComponent();
            this.Topmost = true; // la ventana quedaria sobre todas las demas
            this.WindowStyle = WindowStyle.None; // quita los menus predeterminados para las ventans
            this.ResizeMode = ResizeMode.NoResize; //Evita el cambio de tamaño de la ventana
            CargarNotas();
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
        // Metodo para guardar notas
        private void GuardarNotaButton_Click(object sender, RoutedEventArgs e)
        {
            
            var nota = new Note
            {
                Color = ((SolidColorBrush)((Border)this.Content).Background).Color
            };
            notas.Add(nota);
            GuardarNotas();
            MessageBox.Show("Nota guardada correctamente.");
        }
        // mostrar las notas creadas
        private void VerNotasButton_Click(object sender, RoutedEventArgs e)
        {
            var notasWindow = new NotaWindow();
            notasWindow.SetNotas(notas);
            notasWindow.Show();
        }
        //
        public void SetNotas(List<Note> notas)
        {
            this.notas = notas;
        }

        // Cargar las notas existentes desde el directorio local
        private void CargarNotas()
        {
            
            if (!Directory.Exists(rutaDirectorio))
                Directory.CreateDirectory(rutaDirectorio);

            var archivos = Directory.GetFiles(rutaDirectorio, "*.note");
            foreach (var archivo in archivos)
            {
                var color = File.ReadAllText(archivo).Split(',');
                var nota = new Note
                {
                    Color = Color.FromRgb(byte.Parse(color[0]), byte.Parse(color[1]), byte.Parse(color[2]))
                };
                notas.Add(nota);
            }
        }
        // Guardar las notas en archivos individuales en el directorio local
        private void GuardarNotas()
        {
            
            if (!Directory.Exists(rutaDirectorio))
                Directory.CreateDirectory(rutaDirectorio);

            for (int i = 0; i < notas.Count; i++)
            {
                var nota = notas[i];
                var archivo = Path.Combine(rutaDirectorio, $"nota_{i}.note");
                File.WriteAllText(archivo, $"{nota.Color.R},{nota.Color.G},{nota.Color.B}");
            }
        }
        //
    }
    public class Note
    {
        public Color Color { get; set; }
    }
}