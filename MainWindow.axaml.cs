using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System;
using System.Data;

namespace CalculatorApp
{
    public partial class MainWindow : Window
    {
        private string currentInput = "";

        public MainWindow()
        {
            InitializeComponent();
            DrawToCanvas();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                string value = button.Content?.ToString() ?? "";

                // Cegah dua titik berurutan
                if (value == "." && currentInput.EndsWith(".")) return;

                // Cegah dua operator berurutan (misalnya ++, --)
                if ("+-*/".Contains(value) && currentInput.Length > 0 &&
                    "+-*/".Contains(currentInput[^1]))
                    return;

                currentInput += value;
                DrawToCanvas();
            }
        }

        private void Clear_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            currentInput = "";
            DrawToCanvas();
        }

        private void Equals_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            try
            {
                // Evaluasi ekspresi (ganti koma dengan titik jika perlu)
                string expr = currentInput.Replace(",", ".");
                var result = new DataTable().Compute(expr, null);
                currentInput = result.ToString();
            }
            catch
            {
                currentInput = "Error";
            }
            DrawToCanvas();
        }

        private void DrawToCanvas()
        {
            var canvas = this.FindControl<Canvas>("DisplayCanvas");
            if (canvas == null)
                return;

            canvas.Children.Clear();

            var text = new TextBlock
            {
                Text = currentInput,
                FontSize = 28,
                Foreground = Brushes.Black,
                FontWeight = FontWeight.Bold
            };

            Canvas.SetLeft(text, 10);
            Canvas.SetTop(text, 15);
            canvas.Children.Add(text);
        }
    }
}
