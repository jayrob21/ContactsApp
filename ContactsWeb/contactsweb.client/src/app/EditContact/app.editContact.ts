import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ElementRef, ViewChild, Input, Output, EventEmitter, input } from '@angular/core';
import { ModalDismissReasons, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ContactModel } from '../contact'
import { FormControl, ReactiveFormsModule, FormGroup, Validators } from '@angular/forms'
import Swal from 'sweetalert2'

@Component({
  selector: 'app-update-component',
  templateUrl: './app.editContact.html',
  styleUrl: '../app.component.css'
})

export class UpdateContact implements OnInit {
  constructor(private http: HttpClient, private modalService: NgbModal) { }
  @Input() item: ContactModel = new ContactModel();
  @Output() updateContactEvent = new EventEmitter<ContactModel[]>();
  @ViewChild('updateContactModal') mymodal: ElementRef | undefined;
  updateContactForm: any

  ngOnInit() {
    this.updateContactForm = new FormGroup({
      firstName: new FormControl(this.item.firstName, [
        Validators.required
      ]),
      lastName: new FormControl(this.item.lastName, [
        Validators.required
      ]),
      email: new FormControl(this.item.email, [
        Validators.required,
        Validators.email
      ]),
      id: new FormControl(this.item.id)
    });
  }
  

  update() {
    this.open(this.mymodal);
  }

  open(content: any) {
    this.modalService
      .open(content, { ariaLabelledBy: 'modal-basic-title' })
      .result.then(
        (result) => {
          if (result != 'Dismiss') {
            this.updateContactEvent.emit(result);
          }
          else {
            console.log(result);
          }

        },
        (reason) => {
          if (reason != 'Dismiss') {
            this.updateContactEvent.emit(reason);
          }
        }
      );
  }

  updateContact() {
    var updatedContactVal = this.updateContactForm.value;
    this.http.put<ContactModel>('/api/Contacts/EditContact', updatedContactVal).subscribe({
      next: (data: any) => {
        Swal.fire({
          title: 'Success!',
          text: 'Contact Info Updated!',
          icon: 'success',
          showCancelButton: true,
          confirmButtonText: 'OK',
          buttonsStyling: false,
          customClass: {
            confirmButton: 'btn btn-primary px-4',
            cancelButton: 'btn btn-secondary ms-2 px-4',

          },
        });
        this.modalService.dismissAll(data);
      },
      error: (error: any) => {
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
}
