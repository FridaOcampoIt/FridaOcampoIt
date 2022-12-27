import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogoSolicitarComponent } from './dialogo-solicitar.component';

describe('DialogoSolicitarComponent', () => {
  let component: DialogoSolicitarComponent;
  let fixture: ComponentFixture<DialogoSolicitarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogoSolicitarComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogoSolicitarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
