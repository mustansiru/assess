using System;
using Microsoft.AspNetCore.Http;

namespace assess.ViewModels
{
    public class ChoiceViewModel
    {
        public int Order { get; set; }
        public string ChoiceText { get; set; }
        public IFormFile ChoiceImage { get; set; }
    }
}
