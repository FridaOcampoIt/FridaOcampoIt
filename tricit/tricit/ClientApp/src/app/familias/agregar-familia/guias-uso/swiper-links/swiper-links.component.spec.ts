import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SwiperLinksGUComponent } from './swiper-links.component';

describe('SwiperLinksComponent', () => {
  let component: SwiperLinksGUComponent;
  let fixture: ComponentFixture<SwiperLinksGUComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SwiperLinksGUComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SwiperLinksGUComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
