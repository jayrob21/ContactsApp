import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Component } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';

describe('AppComponent', () => {
  let component: AppComponent;
  let fixture: ComponentFixture<AppComponent>;
  let httpMock: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AppComponent],
      imports: [HttpClientTestingModule]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AppComponent);
    component = fixture.componentInstance;
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should create the app', () => {
    expect(component).toBeTruthy();
  });

  it('should retrieve contact list from the server', () => {
    const mockContactList = [
      { id: 0, firstName: 'mark', lastName: 'strong', email: 'mark.strong@example.com' },
      { id: 0, firstName: 'emily', lastName: 'strong', email: 'emily.strong@example.com' }
    ];

    component.ngOnInit();

    const req = httpMock.expectOne('/api/Contacts/GetContactList');
    expect(req.request.method).toEqual('GET');
    req.flush(mockContactList);

    expect(component.contactList).toEqual(mockContactList);
  });
});
