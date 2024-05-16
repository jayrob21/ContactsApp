using Newtonsoft.Json;

namespace ContactsWeb.Server
{
    public class ContactDsJsonManager
    {
        private readonly static string DsFilePath = Path.Combine(AppContext.BaseDirectory, "ContactDs.json");

        public static void CreateDsFile()
        {
            try
            {
                if (!File.Exists(DsFilePath))
                    File.Create(DsFilePath);
            }
            catch (Exception ex)
            {
                Utilities.LogError(ex);
            }
        }

        public static bool WriteToDsFileSuccessful(List<ContactModel> contacts)
        {
            File.WriteAllText(DsFilePath, JsonConvert.SerializeObject(contacts, Newtonsoft.Json.Formatting.Indented));
            if (File.Exists(DsFilePath) && GetAllContacts()?.Count == contacts.Count)
                return true;
            else
                return false;
        }

        public static bool HasAnyId(int id) => GetAllContacts().Where(x => x.Id == id).Any();

        public static List<ContactModel> GetAllContacts()
        {
            List<ContactModel> contacts = [];
            string contactsJson = File.ReadAllText(DsFilePath);
            if (!string.IsNullOrWhiteSpace(contactsJson))
                contacts = JsonConvert.DeserializeObject<List<ContactModel>>(contactsJson) ?? [];
            return contacts;
        }

        public static int GetNextId()
        {
            int newId = 1;
            try
            {
                List<ContactModel> contacts = GetAllContacts();
                if (contacts.Count > 0)
                {
                    ContactModel? lastItem = contacts.OrderByDescending(x => x.Id).FirstOrDefault();
                    if (lastItem != null)
                        newId = lastItem.Id + 1;
                }
            }
            catch (Exception ex)
            {
                Utilities.LogError(ex);
                throw;
            }
            return newId;
        }

        public static bool AddNewContact(ContactModel contact)
        {
            try
            {
                int newId = GetNextId();
                contact.Id = newId;
                List<ContactModel> contacts = GetAllContacts();
                contacts.Add(contact);
                return WriteToDsFileSuccessful(contacts);
            }
            catch (Exception ex)
            {
                Utilities.LogError(ex);
                throw;
            }
        }

        public static bool DeleteContact(int id)
        {
            try
            {
                List<ContactModel> contacts = GetAllContacts();
                var contact = contacts.FirstOrDefault(x => x.Id == id);
                if (contact != null)
                {
                    contacts.Remove(contact);
                    return WriteToDsFileSuccessful(contacts);
                }
                else
                    throw new Exception($"No contact found matching Id: {id}");

            }
            catch (Exception ex)
            {
                Utilities.LogError(ex);
                throw;
            }
        }

        public static bool EditContact(ContactModel contact)
        {
            try
            {
                List<ContactModel> contacts = GetAllContacts();
                var matchItem = contacts.Where(x => x.Id == contact.Id).FirstOrDefault();
                if (matchItem != null)
                {
                    contacts.Remove(matchItem);
                    contacts.Add(contact);
                    return WriteToDsFileSuccessful(contacts);
                }
                else
                {
                    Utilities.LogInfo($"No contact found with id: {contact.Id}. Adding new contact to list.");
                    contact.Id = GetNextId();
                    contacts.Add(contact);
                    return WriteToDsFileSuccessful(contacts);
                }
            }
            catch (Exception ex)
            {
                Utilities.LogError(ex);
                throw;
            }
        }

        public static void ResetContactDs() {
            List<ContactModel> contacts = [];
            File.WriteAllText(DsFilePath, JsonConvert.SerializeObject(contacts, Newtonsoft.Json.Formatting.Indented));
        }
    }
}
