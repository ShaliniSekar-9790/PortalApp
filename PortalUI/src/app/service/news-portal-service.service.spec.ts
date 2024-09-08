import { TestBed } from '@angular/core/testing';
import { NewsPortalService } from './news-portal.service';


describe('NewsPortalServiceService', () => {
  let service: NewsPortalService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(NewsPortalService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
