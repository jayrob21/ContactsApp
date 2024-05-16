using Microsoft.AspNetCore.Mvc;

namespace ContactsWeb.Server.Controllers
{
    [ApiController, Route("api/Contacts")]
    public class ContactsController : ControllerBase
    {
        [HttpGet, Route("Ping")]
        public IActionResult Ping() => Ok();

        [HttpGet, Route("GetContactList")]
        public IActionResult GetContactList() => Ok(ContactDsJsonManager.GetAllContacts());

        [HttpGet, Route("Count")]
        public IActionResult Count() => Ok(ContactDsJsonManager.GetAllContacts().Count);

        [HttpPost, Route("AddContact")]
        public IActionResult AddContact([FromBody]ContactModel contact)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            bool res = ContactDsJsonManager.AddNewContact(contact);
            Utilities.LogInfo($"New contact creation for Id: {contact.Id} FirstName: {contact.FirstName} LastName: {contact.LastName}" +
                $" Email: {contact.Email} Success: {res}");
            return Ok(ContactDsJsonManager.GetAllContacts());
        }

        [HttpDelete, Route("DeleteContact")]
        public IActionResult DeleteContact(int id)
        {
            if (!ContactDsJsonManager.HasAnyId(id))
                return BadRequest($"No contact found matching id: {id}");
            bool res = ContactDsJsonManager.DeleteContact(id);
            Utilities.LogInfo($"Contact deletion for Id: {id} Success: {res}");
            return Ok(ContactDsJsonManager.GetAllContacts());
        }

        [HttpPut, Route("EditContact")]
        public IActionResult EditContact([FromBody] ContactModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            bool res = ContactDsJsonManager.EditContact(model);
            Utilities.LogInfo($"Contact edit for Id: {model.Id} Success: {res}");
            return Ok(ContactDsJsonManager.GetAllContacts());
        }
    }
}