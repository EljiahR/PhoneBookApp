namespace PhoneBook
{
    internal class Query
    {
        PhoneBookContext db { get; set; }

        public Query(PhoneBookContext context)
        {
            db = context;
        }

        public List<Contact> AllContacts()
        {
            return db.Contacts
                    .OrderBy(c => c.Name).ToList();   
        }

        public List<Contact> ContactByCategory(int id)
        {
            return db.Contacts.Where(c => c.CategoryId == id).ToList();
        }

        public List<Category> AllCategories()
        {

            return db.Categories
                    .OrderBy(c => c.Name).ToList();
        }

        public int AddCategory(string name)
        {
            Category newCategory = new Category { Name = name };
            db.Categories.Add(newCategory);
            db.SaveChanges();
            return newCategory.Id;
        }

        public void AddContact(string name, string email, string phone, int categoryId)
        {
            Contact newContact = new Contact { Name = name, Email = email, PhoneNumber = phone };
            if(categoryId > 0)
            { 
                newContact.CategoryId = categoryId; 
                newContact.Category = db.Categories.Where(c => c.Id == categoryId).FirstOrDefault();
            }
            db.Contacts.Add(new Contact { Name = name, Email = email, PhoneNumber = phone, CategoryId = categoryId });
            db.SaveChanges();
        }
        
        public void UpdateCategory(int id, string newName)
        {
            Category categoryToUpdate = db.Categories
                                            .Where(c => c.Id == id).First();
            categoryToUpdate.Name = newName;
            db.SaveChanges();
        }

        public void DeleteCategory(int id) 
        {
            Category categoryToDelete = db.Categories
                                            .Where(c => c.Id == id).First();
            db.Remove(categoryToDelete);
            db.SaveChanges();
        }
    }
}
