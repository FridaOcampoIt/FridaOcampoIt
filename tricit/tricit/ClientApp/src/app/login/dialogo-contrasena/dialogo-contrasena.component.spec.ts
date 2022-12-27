import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogoContrasenaComponent } from './dialogo-contrasena.component';

describe('DialogoContrasenaComponent', () => {
  let component: DialogoContrasenaComponent;
  let fixture: ComponentFixture<DialogoContrasenaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogoContrasenaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogoContrasenaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
