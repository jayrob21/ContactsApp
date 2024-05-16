import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule,ReactiveFormsModule } from '@angular/forms'
import { AppRoutingModule } from './app-routing.module';
import { NewComponent } from './NewContact/app.addComponent';
import { AppComponent } from './app.component';
import { UpdateContact } from './EditContact/app.editContact'
import { AgGridAngular } from 'ag-grid-angular'
import { EditContactRenderer } from './EditContact/editContactRenderer'
import { DeleteContactRendered } from './deleteRenderer'

@NgModule({
  declarations: [
    AppComponent, NewComponent, UpdateContact,EditContactRenderer,DeleteContactRendered
  ],
  imports: [
    BrowserModule, HttpClientModule,
    AppRoutingModule,FormsModule, ReactiveFormsModule,AgGridAngular
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
