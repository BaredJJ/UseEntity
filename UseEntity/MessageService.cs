using System.Windows;

namespace UseEntity
{
    public interface IMessageService
    {
        void ShowMessage(string message);
        MessageBoxResult ShowExclametion(string exclemention);
        void ShowError(string error);
    }

    class MessageService : IMessageService
    {
        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public MessageBoxResult ShowExclametion(string exclamation)
        {
            return MessageBox.Show(exclamation, "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
        }

        public void ShowError(string error)
        {
            MessageBox.Show(error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
