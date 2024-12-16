using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1
{
    public class Question
    {
        public string Category { get; set; }
        public string Type { get; set; }
        public string Difficulty { get; set; }
        public string question { get; set; }
        public string Correct_answer { get; set; }
        public List<string> Incorrect_answers { get; set; }
    }

    public class QuestionResponse
    {
        public List<Question> Results { get; set; }
    }


}
