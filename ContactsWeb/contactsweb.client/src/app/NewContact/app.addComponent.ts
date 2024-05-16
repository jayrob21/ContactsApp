import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ElementRef, ViewChild, Input, Output, EventEmitter } from '@angular/core';
import { ModalDismissReasons, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ContactModel } from '../contact'
import { FormControl, ReactiveFormsModule, FormGroup, Validators } from '@angular/forms'
import Swal from 'sweetalert2'

@Component({
  selector: 'app-add-component',
  templateUrl: './app.addComponent.html',
  styleUrl: '../app.component.css'
})

export class NewComponent {
  constructor(private http: HttpClient, private modalService: NgbModal) { }

  @Output() newContactEvent = new EventEmitter<ContactModel[]>();
  @ViewChild('newContactModal') mymodal: ElementRef | undefined;

  contactForm = new FormGroup({
    firstName: new FormControl('', [
      Validators.required
    ]),
    lastName: new FormControl('', [
      Validators.required
    ]),
    email: new FormControl('', [
      Validators.required,
      Validators.email
    ]),
    id: new FormControl(0)
  });

  add() {
    this.open(this.mymodal);
  }
 
   open(content: any) {
    this.modalService
      .open(content, { ariaLabelledBy: 'modal-basic-title' })
      .result.then(
        (result) => {
          if (result != 'Dismiss') {
            this.newContactEvent.emit(result);
          }
          else {
            console.log(result);
          }
          
        },
        (reason) => {
          if (reason != 'Dismiss') {
            this.newContactEvent.emit(reason);
          }
        }
      );
  }

  addNewContact() {
    var newContact = this.contactForm.value;
    this.http.post<ContactModel>('/api/Contacts/AddContact', newContact).subscribe({
      next: (data: any) => {
        Swal.fire({
          title: 'Success!',
          text: 'New Contact Added!',
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
