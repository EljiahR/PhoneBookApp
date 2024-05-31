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

        public Category AddCategory(string name)
        {
            Category newCategory = new Category { Name = name };
            db.Categories.Add(newCategory);
            db.SaveChanges();
            return newCategory;
        }

        public void AddContact(string name, string email, string phone, int categoryId)
        {
            Contact newContact = new Contact { Name = name, Email = email, PhoneNumber = phone };
            if(categoryId > 0)
            { 
                newContact.CategoryId = categoryId; 
                newContact.Category = db.Categories.Where(c => c.Id == categoryId).FirstOrDefault();
            }
            db.Contacts.Add(newContact);
            db.SaveChanges();
        }

        public void UpdateContact(int id, string newName, string newEmail, string newPhone, int newCategoryId)
        {
            Contact contactToUpdate = db.Contacts.Where(c => c.Id == id).FirstOrDefault();
            if(!string.IsNullOrEmpty(newName)) contactToUpdate.Name = newName;
            if (!string.IsNullOrEmpty(newEmail)) contactToUpdate.Email = newEmail;
            if (!string.IsNullOrEmpty(newPhone)) contactToUpdate.PhoneNumber = newPhone;
            if(newCategoryId != 0)
            {
                if(newCategoryId == -1)
                {
                    contactToUpdate.CategoryId = null;
                    contactToUpdate.Category = null;
                }
                else
                {
                    contactToUpdate.CategoryId = newCategoryId;
                    contactToUpdate.Category = db.Categories.Where(c => c.Id == newCategoryId).FirstOrDefault();
                }
            }
            db.SaveChanges();
        }

        public void DeleteContact(int id)
        {
            Contact contactToDelete = db.Contacts
                                            .Where(c => c.Id == id).First();
            db.Remove(contactToDelete);
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
