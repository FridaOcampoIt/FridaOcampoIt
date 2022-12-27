import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogoAgregarLinksComponent } from './dialogo-agregar-links.component';

describe('DialogoAgregarLinksComponent', () => {
  let component: DialogoAgregarLinksComponent;
  let fixture: ComponentFixture<DialogoAgregarLinksComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DialogoAgregarLinksComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogoAgregarLinksComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
