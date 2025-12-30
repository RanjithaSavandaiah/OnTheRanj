import { TestBed } from '@angular/core/testing';
import { TimesheetApprovalService } from './timesheet-approval.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

describe('TimesheetApprovalService', () => {
  let service: TimesheetApprovalService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [TimesheetApprovalService]
    });
    service = TestBed.inject(TimesheetApprovalService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should call getPendingTimesheets()', () => {
    const mock = [{ id: 1 }];
    service.getPendingTimesheets().subscribe(result => {
      expect(result).toEqual(mock);
    });
    const req = httpMock.expectOne('/api/timesheetapprovals/pending');
    expect(req.request.method).toBe('GET');
    req.flush(mock);
  });

  it('should call approveTimesheet()', () => {
    const mock = { success: true };
    service.approveTimesheet(2).subscribe(result => {
      expect(result).toEqual(mock);
    });
    const req = httpMock.expectOne('/api/timesheetapprovals/approve/2');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual({});
    req.flush(mock);
  });

  it('should call getTimesheetsByStatus()', () => {
    const mock = [{ id: 3 }];
    service.getTimesheetsByStatus('Submitted').subscribe(result => {
      expect(result).toEqual(mock);
    });
    const req = httpMock.expectOne('/api/timesheetapprovals/status/Submitted');
    expect(req.request.method).toBe('GET');
    req.flush(mock);
  });

  it('should call rejectTimesheet()', () => {
    const mock = { success: true };
    service.rejectTimesheet(4, 'Reason').subscribe(result => {
      expect(result).toEqual(mock);
    });
    const req = httpMock.expectOne('/api/timesheetapprovals/reject/4');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual({ comments: 'Reason' });
    req.flush(mock);
  });
});
