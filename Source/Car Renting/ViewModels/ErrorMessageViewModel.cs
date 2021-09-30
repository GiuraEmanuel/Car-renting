using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Car_Renting.ViewModels
{
    public class ErrorMessageViewModel
    {
        public string Message { get;}

        public ErrorMessageViewModel(string message)
        {
            Message = message;
        }
    }
}
