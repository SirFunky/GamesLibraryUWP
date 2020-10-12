using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesLibraryUWP.Model
{
    public class Studio
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Game> Games { get; set; }
        public virtual ICollection<GameDeveloper> GameDevelopers { get; set; }
    }
}
