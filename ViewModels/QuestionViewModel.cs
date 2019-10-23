using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace assess.ViewModels
{
    public class QuestionViewModel
    {
        public string Id { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string QuestionHeading { get; set; }
        public string Content { get; set; }
        public IFormFile ContentImage { get; set; }
        public decimal Score { get; set; }
    }
}
