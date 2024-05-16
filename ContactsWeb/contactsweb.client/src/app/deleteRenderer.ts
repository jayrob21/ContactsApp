import { ICellRendererAngularComp } from '@ag-grid-community/angular';
import { ICellRendererParams } from "@ag-grid-community/core";
import Swal from 'sweetalert2'
import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { ContactDataService } from './DataService'

@Component({
  templateUrl: './deleteRenderer.html',
  styleUrl: './app.component.css'
})

export class DeleteContactRendered implements ICellRendererAngularComp {
  constructor(private http: HttpClient, private contactDataService: ContactDataService) { }
  id: number = -1;

  agInit(params: ICellRendererParams): void {
    this.id = params.data.id;
  }
  refresh(params: ICellRendererParams) {
    return true;
  }

  delete(id: number) {
    Swal.fire({
      title: "Are you want to delete this contact?",
      text: "You won't be able to revert this!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: '#80140f',
      cancelButtonColor: "#5a6268",
      confirmButtonText: "Delete Contact"
    }).then((result) => {
      if (result.isConfirmed) {
        this.http.delete('/api/Contacts/DeleteContact?id=' + id).subscribe({
          next: (data: any) => {
            this.contactDataService.updateContacts(data);
            Swal.fire({
              title: "Deleted!",
              text: "Contact was deleted.",
              icon: "success"
            });
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
    });
  }

}
