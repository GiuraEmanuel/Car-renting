namespace Car_Renting.Models
{
    public class SuccessModalContentViewModel
    {
        public string Message { get; set; }

        public SuccessModalContentViewModel(string message)
        {
            Message = message;
        }
    }
}
