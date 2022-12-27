import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogoPreguntasComponent } from './dialogo-preguntas.component';

describe('DialogoPreguntasComponent', () => {
  let component: DialogoPreguntasComponent;
  let fixture: ComponentFixture<DialogoPreguntasComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogoPreguntasComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogoPreguntasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
