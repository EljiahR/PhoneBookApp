using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook
{
    internal class Query
    {
        PhoneBookContext db { get; set; }

        public Query(PhoneBookContext context)
        {
            db = context;
        }

        public List<Category> AllCategories()
        {
            //using var db = new PhoneBookContext();

            return db.Categories
                    .OrderBy(c => c.Name).ToList();
        }

        public void AddCategory(string name)
        {
            //using var db = new PhoneBookContext();

            db.Categories.Add(new Category { Name = name });
        }
    }
}
