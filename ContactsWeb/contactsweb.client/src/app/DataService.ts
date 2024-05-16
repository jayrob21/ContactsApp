import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { ContactModel } from './contact'

@Injectable({
  providedIn: 'root'
})

export class ContactDataService {
  private contactSubject = new BehaviorSubject<ContactModel[]>([]);
  contacts = this.contactSubject.asObservable();

  updateContacts(contactList: ContactModel[]) {
    this.contactSubject.next(contactList);
  }

  getContactList() {
    return this.contactSubject.getValue();
  }
}
