using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace BurbujasDeNotas
{
    public partial class MainWindow : Window
    {
        
        private bool estaArrastrando = false;
        private Point posicionInicial;
        private Popup menuEmergente;

        public MainWindow()
        {
            InitializeComponent();
            InicializarMenuEmergente();
        }

        
        /// Inicializa el menú emergente (popup) con sus botones y estilos
        
        private void InicializarMenuEmergente()
        {
            // Crear el popup principal
            menuEmergente = new Popup
            {
                AllowsTransparency = true,
                PopupAnimation = PopupAnimation.Fade
            };

            // Crear el borde que contendrá todo el contenido del popup
            var bordePrincipal = new Border
            {
                Background = new SolidColorBrush(Colors.White),
                CornerRadius = new CornerRadius(15),
                BorderBrush = new SolidColorBrush(Colors.LightGray),
                BorderThickness = new Thickness(1),
                Width = 160,
                Height = 160,
                Margin = new Thickness(10) // Espacio entre la burbuja principal y el popup
            };

            // Agregar sombra al borde para dar efecto de elevación
            bordePrincipal.Effect = new System.Windows.Media.Effects.DropShadowEffect
            {
                BlurRadius = 10,
                ShadowDepth = 5,
                Opacity = 0.3
            };

            // Crear la cuadrícula para organizar los botones
            var cuadricula = new Grid();

            // Configurar filas y columnas de la cuadrícula
            for (int i = 0; i < 2; i++)
            {
                cuadricula.RowDefinitions.Add(new RowDefinition());
                cuadricula.ColumnDefinitions.Add(new ColumnDefinition());
            }

            // Agregar botones a la cuadrícula
            string[] opcionesMenu = { "📝", "📅", "📷","⚙️" };
            for (int i = 0; i < 4; i++)
            {
                
                var boton = CrearBotonCircular(opcionesMenu[i]);
                //se define donde van a ir los botones
                Grid.SetRow(boton, i / 2);
                Grid.SetColumn(boton, i % 2);
                cuadricula.Children.Add(boton);
            }
            // unimos todo
            bordePrincipal.Child = cuadricula;
            menuEmergente.Child = bordePrincipal;
        }

        
        /// Crea un botón circular con el contenido especificado
        
        private Button CrearBotonCircular(string contenido)
        {
            return new Button
            {
                Content = contenido,
                Width = 60,
                Height = 60,
                Margin = new Thickness(5),
                Style = FindResource("EstiloBotonCircular") as Style
            };
        }

        
        /// Maneja el evento cuando se presiona el botón del mouse
        
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // Si el menú está abierto, lo cerramos
                if (menuEmergente.IsOpen)
                {
                    menuEmergente.IsOpen = false;
                    return;
                }

                // Iniciamos el arrastre
                estaArrastrando = true;
                posicionInicial = e.GetPosition(this);
                this.CaptureMouse();
            }
        }

        
        /// Maneja el movimiento del mouse para arrastrar la ventana
        
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (estaArrastrando)
            {
                if (e.LeftButton == MouseButtonState.Released)
                {
                    Window_MouseUp(sender, null);
                    return;
                }

                // Calcular la nueva posición de la ventana
                var posicionActual = e.GetPosition(this);
                var offset = posicionActual - posicionInicial;

                // Mover la ventana
                this.Left += offset.X;
                this.Top += offset.Y;
            }
        }

        
        /// Maneja cuando se suelta el botón del mouse
        
        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (estaArrastrando)
            {
                estaArrastrando = false;
                this.ReleaseMouseCapture();
                MostrarMenuEmergente();
            }
        }

        
        /// Muestra el menú emergente al lado de la burbuja principal
        
        private void MostrarMenuEmergente()
        {
            menuEmergente.PlacementTarget = this;
            menuEmergente.Placement = PlacementMode.Right;
            menuEmergente.IsOpen = true;
        }
    }
}