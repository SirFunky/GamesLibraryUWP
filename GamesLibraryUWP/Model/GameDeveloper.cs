using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesLibraryUWP.Model
{
    public class GameDeveloper
    {
        public int GameId { get; set; }
        public int DeveloperId { get; set; }
        public int StudioId { get; set; }

        public virtual Developer Developer { get; set; }
        public virtual Game Game { get; set; }
        public virtual Studio Studio { get; set; }
    }
}
