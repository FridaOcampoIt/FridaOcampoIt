import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SwiperLinksGIComponent } from './swiper-links.component';

describe('SwiperLinksComponent', () => {
  let component: SwiperLinksGIComponent;
  let fixture: ComponentFixture<SwiperLinksGIComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
        declarations: [SwiperLinksGIComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SwiperLinksGIComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
