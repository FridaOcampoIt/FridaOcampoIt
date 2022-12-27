import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogoAgregarComponent } from './dialogo-agregar.component';

describe('DialogoAgregarComponent', () => {
  let component: DialogoAgregarComponent;
  let fixture: ComponentFixture<DialogoAgregarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogoAgregarComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogoAgregarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
