using ContactsWeb.Server;

namespace ContactsApp.Test
{
    [TestClass]
    public class ContactDsJsonManagerTest
    {
        [TestMethod]
        public void TestAddContact()
        {
            ContactDsJsonManager.ResetContactDs();
            Assert.IsTrue(ContactDsJsonManager.GetAllContacts().Count == 0);
            ContactDsJsonManager.AddNewContact(GetDummyContactItem());
            Assert.IsTrue(ContactDsJsonManager.GetAllContacts().Count > 0);
        }

        [TestMethod]
        public void TestDeleteContact() {
            ContactDsJsonManager.ResetContactDs();
            Assert.IsTrue(ContactDsJsonManager.GetAllContacts().Count == 0);
            ContactDsJsonManager.AddNewContact(GetDummyContactItem());
            Assert.IsTrue(ContactDsJsonManager.GetAllContacts().Count == 1);
            int id = ContactDsJsonManager.GetAllContacts().First().Id;
            ContactDsJsonManager.DeleteContact(id);
            Assert.IsTrue(ContactDsJsonManager.GetAllContacts().Count == 0);
        }

        [TestMethod]
        public void TestUpdateContact()
        {
            ContactDsJsonManager.ResetContactDs();
            Assert.IsTrue(ContactDsJsonManager.GetAllContacts().Count == 0);
            ContactModel contact = GetDummyContactItem();
            ContactDsJsonManager.AddNewContact(contact);
            Assert.IsTrue(ContactDsJsonManager.GetAllContacts().Count == 1);
            ContactModel updatedItem = GetDummyContactItem();
            updatedItem.FirstName = "mark";
            updatedItem.Id = contact.Id;
            updatedItem.Email = "mark.strong@usagov.com";
            ContactDsJsonManager.EditContact(updatedItem);
            var fetchedItem = ContactDsJsonManager.GetAllContacts().First();
            Assert.IsTrue(fetchedItem.FirstName==updatedItem.FirstName&&fetchedItem.Email==fetchedItem.Email&&fetchedItem.LastName==fetchedItem.LastName);
        }

        [TestMethod]
        public void TestGetNextId() {
            ContactDsJsonManager.ResetContactDs();
            Assert.IsTrue(ContactDsJsonManager.GetAllContacts().Count == 0);
            ContactModel contact = GetDummyContactItem();
            ContactDsJsonManager.AddNewContact(contact);
            Assert.IsTrue(ContactDsJsonManager.GetAllContacts().Count == 1);
            int maxId = ContactDsJsonManager.GetAllContacts().OrderByDescending(x => x.Id).First().Id;
            int nextId = ContactDsJsonManager.GetNextId();
            Assert.IsTrue(nextId == maxId + 1);
        }

        [TestMethod]
        public void TestHasAnyId() {
            ContactDsJsonManager.ResetContactDs();
            Assert.IsTrue(ContactDsJsonManager.GetAllContacts().Count == 0);
            ContactModel contact = GetDummyContactItem();
            ContactDsJsonManager.AddNewContact(contact);
            Assert.IsTrue(ContactDsJsonManager.GetAllContacts().Count == 1);
            Assert.IsTrue(ContactDsJsonManager.HasAnyId(1));
            Assert.IsFalse(ContactDsJsonManager.HasAnyId(5));
        }

        private ContactModel GetDummyContactItem()
        {
            return new()
            {
                Email = "em.strong@example.com",
                FirstName = "emily",
                LastName = "Strong"
            };
        }
    }
}