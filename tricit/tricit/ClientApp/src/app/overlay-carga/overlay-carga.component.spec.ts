import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OverlayCargaComponent } from './overlay-carga.component';

describe('OverlayCargaComponent', () => {
  let component: OverlayCargaComponent;
  let fixture: ComponentFixture<OverlayCargaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OverlayCargaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OverlayCargaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
