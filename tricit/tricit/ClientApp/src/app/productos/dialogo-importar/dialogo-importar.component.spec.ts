import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogoImportarComponent } from './dialogo-importar.component';

describe('DialogoImportarComponent', () => {
  let component: DialogoImportarComponent;
  let fixture: ComponentFixture<DialogoImportarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogoImportarComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogoImportarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
