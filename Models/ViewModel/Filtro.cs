using System.Collections.Generic;

namespace CyberLibrary2.Models.ViewModel
{
    public class Filtro
    {
        public List<Livro> RecommendedBooks { get; set; } = new List<Livro>();
        public List<Livro> RecentBooks { get; set; } = new List<Livro>();
        public List<Livro> ExploreBooks { get; set; } = new List<Livro>();
        public List<Livro> MostBorrowedBooks { get; set; } = new List<Livro>();
        public List<Livro> TopRatedBooks { get; set; } = new List<Livro>();
        public List<Livro> Mangas { get; set; } = new List<Livro>();
        public List<Livro> MachadoDeAssisBooks { get; set; } = new List<Livro>();
    }
}
