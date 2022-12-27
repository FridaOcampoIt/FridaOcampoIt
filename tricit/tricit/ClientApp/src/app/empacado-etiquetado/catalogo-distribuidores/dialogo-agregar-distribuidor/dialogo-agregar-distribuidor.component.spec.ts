import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogoAgregarDistribuidorComponent } from './dialogo-agregar-distribuidor.component';

describe('DialogoAgregarDistribuidorComponent', () => {
  let component: DialogoAgregarDistribuidorComponent;
  let fixture: ComponentFixture<DialogoAgregarDistribuidorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogoAgregarDistribuidorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogoAgregarDistribuidorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
