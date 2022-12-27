import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgregarFamiliaComponent } from './agregar-familia.component';

describe('AgregarFamiliaComponent', () => {
  let component: AgregarFamiliaComponent;
  let fixture: ComponentFixture<AgregarFamiliaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgregarFamiliaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgregarFamiliaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
