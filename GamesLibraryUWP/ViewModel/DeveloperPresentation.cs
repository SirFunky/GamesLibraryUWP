using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesLibraryUWP.ViewModel
{
    public class DeveloperPresentation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string developerName
        {
            get { return Name + " " + Role + " "; }
        }
        public string Role { get; set; }
    }
}
