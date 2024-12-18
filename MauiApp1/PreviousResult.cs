using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp1
{
    public class PreviousResult
    {

        // Below 3 are for sp Set
        public int CorrectAnswers { get; set; }
        public int IncorrectAnswers { get; set; }
        public string PlayerName { get; set; }
        
        // Below is for any mp Game

        public List<String> PlayerNames { get; set; }

        // Any timed Game
        public int Timer { get; set; }
        public string Winner { get; set; }

        public string HotPotato { get; set; }
    }

  
}
