import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogoUbicacionComponent } from './dialogo-ubicacion.component';

describe('DialogoUbicacionComponent', () => {
  let component: DialogoUbicacionComponent;
  let fixture: ComponentFixture<DialogoUbicacionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogoUbicacionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogoUbicacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
