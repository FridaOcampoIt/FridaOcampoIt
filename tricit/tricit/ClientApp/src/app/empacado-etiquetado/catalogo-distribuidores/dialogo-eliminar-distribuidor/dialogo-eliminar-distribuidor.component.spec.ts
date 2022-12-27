import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogoEliminarDistribuidorComponent } from './dialogo-eliminar-distribuidor.component';

describe('DialogoEliminarDistribuidorComponent', () => {
  let component: DialogoEliminarDistribuidorComponent;
  let fixture: ComponentFixture<DialogoEliminarDistribuidorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogoEliminarDistribuidorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogoEliminarDistribuidorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
