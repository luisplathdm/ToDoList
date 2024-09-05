using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Microsoft.Maui.Controls;

namespace ToDoList
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<TaskModel> Tasks { get; set; }

        public MainPage()
        {
            InitializeComponent();
            Tasks = new ObservableCollection<TaskModel>();
            TaskListView.ItemsSource = Tasks;
        }

        // Método para adicionar uma nova tarefa
        private async void OnAddTaskClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TaskEntry.Text))
            {
                // Mostrar DisplayActionSheet para escolher a cor
                string colorChoice = await DisplayActionSheet("Escolha a cor da tarefa:", "Cancelar", null,
                    "Amarelo - Necessário", "Laranja - Importante", "Vermelho - Urgente");

                if (colorChoice == "Cancelar")
                {
                    return;
                }

                // Navegar para a página de escolha de cor
                var colorPickerPage = new ColorPickerPage();
                await Navigation.PushModalAsync(colorPickerPage);

                // Aguardar até que o usuário selecione uma cor
                Color selectedColor = await colorPickerPage.ColorSelectedTaskCompletionSource.Task;

                // Verificar a cor escolhida
                Color taskColor = Colors.Black; // Default color
                switch (colorChoice)
                {
                    case "Amarelo - Necessário":
                        taskColor = Colors.Yellow;
                        break;
                    case "Laranja - Importante":
                        taskColor = Colors.Orange;
                        break;
                    case "Vermelho - Urgente":
                        taskColor = Colors.Red;
                        break;
                }

                Tasks.Add(new TaskModel
                {
                    Name = TaskEntry.Text,
                    IsCompleted = false,
                    TaskColor = taskColor,
                    OriginalColor = taskColor
                });

                TaskEntry.Text = string.Empty;
            }
        }

        // Método para deletar uma tarefa
        private void OnDeleteTaskSwiped(object sender, EventArgs e)
        {
            var swipeItem = sender as SwipeItem;
            if (swipeItem?.BindingContext is TaskModel task)
            {
                Tasks.Remove(task);
            }
        }

        // Método chamado quando uma tarefa é clicada
        private async void OnTaskTapped(object sender, EventArgs e)
        {
            var task = (sender as Grid)?.BindingContext as TaskModel;
            if (task != null)
            {
                bool isCompleted = task.IsCompleted;
                string action = await DisplayActionSheet(
                    $"Ação para a Tarefa '{task.Name}'",
                    "Cancelar",
                    null,
                    isCompleted ? "Desmarcar como Não Concluída" : "Marcar como Concluída",
                    "Excluir");

                if (action == "Excluir")
                {
                    Tasks.Remove(task);
                }
                else if (action == (isCompleted ? "Desmarcar como Não Concluída" : "Marcar como Concluída"))
                {
                    if (isCompleted)
                    {
                        task.IsCompleted = false;
                        // Voltar à cor original
                        task.TaskColor = task.OriginalColor;
                    }
                    else
                    {
                        task.IsCompleted = true;
                        // Mudar a cor para verde quando concluído
                        task.TaskColor = Colors.Green;
                    }
                }
            }
        }
    }

    // Modelo de Tarefa
    public class TaskModel : INotifyPropertyChanged
    {
        private bool isCompleted;
        private Color taskColor;
        private Color originalColor;

        public string Name { get; set; }

        public bool IsCompleted
        {
            get => isCompleted;
            set
            {
                if (isCompleted != value)
                {
                    isCompleted = value;
                    OnPropertyChanged(nameof(IsCompleted));
                }
            }
        }

        public Color TaskColor
        {
            get => taskColor;
            set
            {
                if (taskColor != value)
                {
                    taskColor = value;
                    OnPropertyChanged(nameof(TaskColor));
                }
            }
        }

        public Color OriginalColor
        {
            get => originalColor;
            set
            {
                if (originalColor != value)
                {
                    originalColor = value;
                    OnPropertyChanged(nameof(OriginalColor));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
