import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogoNotificacionesComponent } from './dialogo-notificaciones.component';

describe('DialogoNotificacionesComponent', () => {
  let component: DialogoNotificacionesComponent;
  let fixture: ComponentFixture<DialogoNotificacionesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogoNotificacionesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogoNotificacionesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
