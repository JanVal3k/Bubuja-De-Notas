using System;                                  // Proporciona clases fundamentales y clases base
using System.Windows;                          // Contiene clases para crear aplicaciones Windows
using System.Windows.Controls;                 // Proporciona controles de UI como Button, Grid, etc.
using System.Windows.Controls.Primitives;      // Contiene controles base como Popup
using System.Windows.Input;                    // Manejo de entrada del usuario (mouse, teclado)
using System.Windows.Media;                    // Proporciona funcionalidad para gráficos y dibujo

namespace BurbujasDeNotas
{
    public partial class MainWindow : Window    // Clase principal que hereda de Window
    {
        // Variables miembro de la clase
        private bool estaArrastrando = false;   // Controla si la ventana está siendo arrastrada
        private Point posicionInicial;          // Almacena la posición inicial del mouse al arrastrar
        private Popup menuEmergente;            // El menú popup que aparece al soltar el mouse

        // Constructor de la clase
        public MainWindow()
        {
            InitializeComponent();              // Inicializa los componentes definidos en XAML
            InicializarMenuEmergente();         // Configura el menú emergente
        }

        // Método que crea y configura el menú emergente
        private void InicializarMenuEmergente()
        {
            // Crear el popup con propiedades de transparencia y animación
            menuEmergente = new Popup
            {
                AllowsTransparency = true,      // Permite transparencia en el popup
                PopupAnimation = PopupAnimation.Fade  // Añade animación de fundido
            };

            // Crear el contenedor principal del popup
            var bordePrincipal = new Border
            {
                Background = new SolidColorBrush(Colors.White),  // Fondo blanco
                CornerRadius = new CornerRadius(15),            // Esquinas redondeadas
                BorderBrush = new SolidColorBrush(Colors.LightGray), // Color del borde
                BorderThickness = new Thickness(1),             // Grosor del borde
                Width = 160,                                    // Ancho fijo
                Height = 160,                                   // Alto fijo
                Margin = new Thickness(10)                      // Margen exterior
            };

            // Agregar efecto de sombra al borde
            bordePrincipal.Effect = new System.Windows.Media.Effects.DropShadowEffect
            {
                BlurRadius = 10,    // Desenfoque de la sombra
                ShadowDepth = 5,    // Profundidad de la sombra
                Opacity = 0.3       // Transparencia de la sombra
            };

            // Crear grid para organizar los botones en 2x2
            var cuadricula = new Grid();

            // Configurar 2 filas y 2 columnas
            for (int i = 0; i < 2; i++)
            {
                cuadricula.RowDefinitions.Add(new RowDefinition());     // Añade fila
                cuadricula.ColumnDefinitions.Add(new ColumnDefinition()); // Añade columna
            }

            // Crear los botones con emojis
            string[] opcionesMenu = { "📝", "📅", "📷", "⚙️" };  // Array de emojis para los botones
            for (int i = 0; i < 4; i++)
            {
                var boton = CrearBotonCircular(opcionesMenu[i]);
                Grid.SetRow(boton, i / 2);     // Calcula la fila (0 para i=0,1 y 1 para i=2,3)
                Grid.SetColumn(boton, i % 2);   // Calcula la columna (alterna entre 0 y 1)
                cuadricula.Children.Add(boton); // Añade el botón a la cuadrícula
            }

            // Ensamblar la jerarquía visual
            bordePrincipal.Child = cuadricula;        // Añade la cuadrícula al borde
            menuEmergente.Child = bordePrincipal;     // Añade el borde al popup
        }

        // Método para crear botones circulares
        private Button CrearBotonCircular(string contenido)
        {
            return new Button
            {
                Content = contenido,            // Emoji del botón
                Width = 60,                     // Ancho fijo
                Height = 60,                    // Alto fijo
                Margin = new Thickness(5),      // Margen alrededor del botón
                Style = FindResource("EstiloBotonCircular") as Style  // Aplica estilo definido en XAML
            };
        }

        // Manejador del evento MouseDown de la ventana
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // Si el menú está abierto, lo cierra al hacer clic
                if (menuEmergente.IsOpen)
                {
                    menuEmergente.IsOpen = false;
                    return;
                }

                // Inicia el proceso de arrastre
                estaArrastrando = true;
                posicionInicial = e.GetPosition(this);  // Guarda posición inicial del mouse
                this.CaptureMouse();  // Captura eventos del mouse para esta ventana
            }
        }

        // Manejador del evento MouseMove de la ventana
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (estaArrastrando)
            {
                // Si se soltó el botón mientras se movía
                if (e.LeftButton == MouseButtonState.Released)
                {
                    Window_MouseUp(sender, null);
                    return;
                }

                // Calcula el desplazamiento desde la posición inicial
                var posicionActual = e.GetPosition(this);
                var offset = posicionActual - posicionInicial;

                // Actualiza la posición de la ventana
                this.Left += offset.X;
                this.Top += offset.Y;
            }
        }

        // Manejador del evento MouseUp de la ventana
        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (estaArrastrando)
            {
                estaArrastrando = false;
                this.ReleaseMouseCapture();     // Libera la captura del mouse
                MostrarMenuEmergente();         // Muestra el menú popup
            }
        }

        // Método para mostrar el menú emergente
        private void MostrarMenuEmergente()
        {
            menuEmergente.PlacementTarget = this;     // Define la ventana como objetivo del popup
            menuEmergente.Placement = PlacementMode.Right;  // Coloca el popup a la derecha
            menuEmergente.IsOpen = true;              // Muestra el popup
        }
    }
}