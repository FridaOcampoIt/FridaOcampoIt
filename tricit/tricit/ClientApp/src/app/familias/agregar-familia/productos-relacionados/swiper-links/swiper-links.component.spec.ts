import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SwiperLinksPRComponent } from './swiper-links.component';

describe('SwiperLinksComponent', () => {
  let component: SwiperLinksPRComponent;
  let fixture: ComponentFixture<SwiperLinksPRComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SwiperLinksPRComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SwiperLinksPRComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
