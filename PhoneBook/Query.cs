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

            return db.Categories
                    .OrderBy(c => c.Name).ToList();
        }

        public void AddCategory(string name)
        {

            db.Categories.Add(new Category { Name = name });
            db.SaveChanges();
        }
    }
}
