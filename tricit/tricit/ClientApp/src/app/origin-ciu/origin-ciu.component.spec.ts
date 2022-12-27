import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OriginCiuComponent } from './origin-ciu.component';

describe('OriginCiuComponent', () => {
  let component: OriginCiuComponent;
  let fixture: ComponentFixture<OriginCiuComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OriginCiuComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OriginCiuComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
