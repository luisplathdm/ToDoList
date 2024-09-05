using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace ToDoList
{
    public partial class ColorPickerPage : ContentPage
    {
        public TaskCompletionSource<Color> ColorSelectedTaskCompletionSource { get; set; }

        public ColorPickerPage()
        {
            InitializeComponent();
            ColorSelectedTaskCompletionSource = new TaskCompletionSource<Color>();
        }

        private async void OnColorSelected(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                string colorName = button.CommandParameter.ToString();
                Color selectedColor = Colors.Black; // Default color

                switch (colorName)
                {
                    case "Amarelo":
                        selectedColor = Colors.Yellow;
                        break;
                    case "Laranja":
                        selectedColor = Colors.Orange;
                        break;
                    case "Vermelho":
                        selectedColor = Colors.Red;
                        break;
                }

                // Complete the task with the selected color
                ColorSelectedTaskCompletionSource.SetResult(selectedColor);

                // Close the modal page
                await Navigation.PopModalAsync();
            }
        }
    }
}
