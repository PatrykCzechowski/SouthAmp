import { TestBed } from '@angular/core/testing';
import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import { DiscountService, Discount } from './discount.service';

describe('DiscountService', () => {
  let service: DiscountService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [DiscountService],
    });
    service = TestBed.inject(DiscountService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should fetch discounts', () => {
    const mockDiscounts: Discount[] = [
      {
        id: 1,
        code: 'TEST',
        percent: 10,
        validFrom: '2024-01-01',
        validTo: '2024-12-31',
        usageLimit: 100,
        usedCount: 0,
        active: true,
      },
    ];
    service.getDiscounts().subscribe((discounts) => {
      expect(discounts.length).toBe(1);
      expect(discounts[0].code).toBe('TEST');
    });
    const req = httpMock.expectOne('https://api.example.com/discounts');
    expect(req.request.method).toBe('GET');
    req.flush(mockDiscounts);
  });

  it('should validate discount code', () => {
    const mockDiscount: Discount = {
      id: 2,
      code: 'CODE20',
      percent: 20,
      validFrom: '2024-01-01',
      validTo: '2024-12-31',
      usageLimit: 50,
      usedCount: 10,
      active: true,
    };
    service.validateCode('CODE20').subscribe((discount) => {
      expect(discount.code).toBe('CODE20');
      expect(discount.percent).toBe(20);
    });
    const req = httpMock.expectOne(
      'https://api.example.com/discounts/validate/CODE20'
    );
    expect(req.request.method).toBe('GET');
    req.flush(mockDiscount);
  });
});
