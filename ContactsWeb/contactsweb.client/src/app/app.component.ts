import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ElementRef, ViewChild, Input, Output, EventEmitter } from '@angular/core';
import { ContactModel } from './contact'
import { ContactDataService } from './DataService'
import { AgGridAngular } from 'ag-grid-angular'
import { ColDef } from 'ag-grid-community';
import Swal from 'sweetalert2'
import { EditContactRenderer } from './EditContact/editContactRenderer'
import { DeleteContactRendered} from './deleteRenderer'

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})


export class AppComponent implements OnInit{
  constructor(private http: HttpClient, private contactDataService: ContactDataService) { }

  columns: ColDef[] = [
    { field: 'id', filter: true, floatingFilter: true },
    { field: 'firstName', filter: true, floatingFilter: true },
    { field: 'lastName', filter: true, floatingFilter: true },
    { field: 'email', filter: true, floatingFilter: true },
    { field: 'edit',  cellRenderer: EditContactRenderer },
    {
      field: 'delete', cellRenderer:DeleteContactRendered
    }
  ];
  public paginationPageSize = 10;
  public paginationPageSizeSelector: number[] | boolean = [10, 25, 50];


  ngOnInit() {
    this.getData();
  }

  handleContacts(contacts: ContactModel[]) {
    this.contactDataService.updateContacts(contacts);
  }

  getContactList() {
    return this.contactDataService.getContactList();
  }

  getData() {
    this.http.get<ContactModel[]>('/api/Contacts/GetContactList').subscribe({
      next: (data: any) => {
        this.contactDataService.updateContacts(data);
      },
      error(error: any) {
        Swal.fire({
          title: 'Error!',
          icon: 'error',
          text: error.error,
          buttonsStyling: false,
          confirmButtonText: 'OK',
          customClass: {
            confirmButton: 'btn btn-primary px-4',
          }
        });
      },
      complete: () => { }
    });
  }

  title = 'contactsweb.client';
}
