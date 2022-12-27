import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogEliminarSectorComponent } from './dialog-eliminar-sector.component';

describe('DialogEliminarSectorComponent', () => {
  let component: DialogEliminarSectorComponent;
  let fixture: ComponentFixture<DialogEliminarSectorComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogEliminarSectorComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogEliminarSectorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
