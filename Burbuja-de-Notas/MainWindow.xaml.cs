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
        private NotaWindow ventanaNota;         // nueva entana para las notas
        private bool NotasAbiertas = false;     // para controlar si esta abierta o no las notas

        // Constructor de la clase
        public MainWindow()
        {
            InitializeComponent();              // Inicializa los componentes definidos en XAML
            InicializarMenuEmergente();         // Configura el menú emergente
        }

        // Método que crea y configura el menú emergente
        private void InicializarMenuEmergente()
        {
            // Crear el popup
            menuEmergente = new Popup
            {
                AllowsTransparency = true,
                PopupAnimation = PopupAnimation.Fade
            };

            // Crear el borde principal
            var bordePrincipal = new Border
            {
                Background = new SolidColorBrush(Colors.White),
                CornerRadius = new CornerRadius(15),
                BorderBrush = new SolidColorBrush(Colors.LightGray),
                BorderThickness = new Thickness(1),
                Width = 160,
                Height = 160,
                Margin = new Thickness(10)
            };

            // Agregar efecto de sombra
            bordePrincipal.Effect = new System.Windows.Media.Effects.DropShadowEffect
            {
                BlurRadius = 10,
                ShadowDepth = 5,
                Opacity = 0.3
            };

            // Crear el grid principal que contendrá todo
            var gridPrincipal = new Grid();

            // Crear la cuadrícula para los botones
            var cuadriculaBotones = new Grid();

            // Configurar filas y columnas
            for (int i = 0; i < 2; i++)
            {
                cuadriculaBotones.RowDefinitions.Add(new RowDefinition());
                cuadriculaBotones.ColumnDefinitions.Add(new ColumnDefinition());
            }

            // Crear y agregar los botones
            string[] opcionesMenu = { "📝", "📅", "📷", "⚙️" };
            for (int i = 0; i < 4; i++)
            {
                var boton = CrearBotonCircular(opcionesMenu[i]);
                Grid.SetRow(boton, i / 2);
                Grid.SetColumn(boton, i % 2);
                cuadriculaBotones.Children.Add(boton);
            }

            // Agregar todos los elementos al grid principal
            gridPrincipal.Children.Add(cuadriculaBotones);
        

            // Ensamblar todo
            bordePrincipal.Child = gridPrincipal;
            menuEmergente.Child = bordePrincipal;
        }
        // metodo para el boton cerrar
        private void BotonCerrar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        // Método para crear botones circulares 
        private Button CrearBotonCircular(string contenido)
        {
            // Crear el botón con sus propiedades
            var boton = new Button
            {
                Content = contenido,            // El emoji que se mostrará
                Width = 60,
                Height = 60,
                Margin = new Thickness(5),
                Style = FindResource("EstiloBotonCircular") as Style,
                Tag = contenido                 // Guardamos el emoji para identificar el botón
            };

            // Agregamos el manejador del clic
            
                boton.Click += BotonMenu_Click;
                
            return boton;
            
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
            menuEmergente.Placement = PlacementMode.Top;  // Coloca el popup a la derecha
            menuEmergente.IsOpen = true;              // Muestra el popup
        }
        //--------------------
        

        // Nuevo método para manejar los clics en los botones
        private void BotonMenu_Click(object sender, RoutedEventArgs e)
        {
            // Obtener el botón que fue clickeado
            var boton = sender as Button;

            // Verificar si es el botón de nota (emoji 📝)
            if (boton?.Tag.ToString() == "📝")
            {
                if (NotasAbiertas)
                {
                    return;
                }
                // Crear nueva ventana de nota
                ventanaNota = new NotaWindow();

                NotasAbiertas = true;

                // Posicionar la ventana cerca de donde se hizo clic
                ventanaNota.Left = this.Left + this.Width + menuEmergente.HorizontalOffset;
                ventanaNota.Top = this.Top;

                //ahora para cambiamos el estado de NotasAbiertas cuando se cierra

                ventanaNota.Closed += (s, args) => NotasAbiertas = false; // funcion lambda o de flasha en JS (nota: los argumentos son obligatorios a si no se utilicen)

                // Mostrar la ventana
                ventanaNota.Show();
                
                // Cerrar el menú emergente
                menuEmergente.IsOpen = false;
            }
        }
    }
}