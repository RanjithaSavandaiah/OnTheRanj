import { TestBed } from '@angular/core/testing';
import { TimesheetService } from './timesheet.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { Timesheet, TimesheetRequest, TimesheetReviewRequest } from '../models/timesheet.model';

describe('TimesheetService', () => {
  let service: TimesheetService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [TimesheetService]
    });
    service = TestBed.inject(TimesheetService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should call submitDraftTimesheet()', () => {
    service.submitDraftTimesheet(1).subscribe();
    const req = httpMock.expectOne(req => req.url.includes('/timesheets/1/submit'));
    expect(req.request.method).toBe('PATCH');
    req.flush({});
  });

  it('should call getEmployeeTimesheets()', () => {
    const mock: Timesheet[] = [{ id: 1 } as Timesheet];
    service.getEmployeeTimesheets(2).subscribe(result => {
      expect(result).toEqual(mock);
    });
    const req = httpMock.expectOne(req => req.url.includes('/timesheets/employee/2'));
    expect(req.request.method).toBe('GET');
    req.flush(mock);
  });

  it('should call getTimesheetById()', () => {
    const mock: Timesheet = { id: 3 } as Timesheet;
    service.getTimesheetById(3).subscribe(result => {
      expect(result).toEqual(mock);
    });
    const req = httpMock.expectOne(req => req.url.includes('/timesheets/3'));
    expect(req.request.method).toBe('GET');
    req.flush(mock);
  });

  it('should call submitTimesheet()', () => {
    const request: TimesheetRequest = { employeeId: 1 } as TimesheetRequest;
    const mock: Timesheet = { id: 4 } as Timesheet;
    service.submitTimesheet(request).subscribe(result => {
      expect(result).toEqual(mock);
    });
    const req = httpMock.expectOne(req => req.url.includes('/timesheets'));
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(request);
    req.flush(mock);
  });

  it('should call updateTimesheet()', () => {
    const request: TimesheetRequest = { employeeId: 2 } as TimesheetRequest;
    const mock: Timesheet = { id: 5 } as Timesheet;
    service.updateTimesheet(5, request).subscribe(result => {
      expect(result).toEqual(mock);
    });
    const req = httpMock.expectOne(req => req.url.includes('/timesheets/5'));
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(request);
    req.flush(mock);
  });

  it('should call deleteTimesheet()', () => {
    service.deleteTimesheet(6).subscribe(result => {
      expect(result).toBeNull();
    });
    const req = httpMock.expectOne(req => req.url.includes('/timesheets/6'));
    expect(req.request.method).toBe('DELETE');
    req.flush(null);
  });

  it('should call getPendingTimesheets()', () => {
    const mock: Timesheet[] = [{ id: 7 } as Timesheet];
    service.getPendingTimesheets().subscribe(result => {
      expect(result).toEqual(mock);
    });
    const req = httpMock.expectOne(req => req.url.includes('/timesheets/pending'));
    expect(req.request.method).toBe('GET');
    req.flush(mock);
  });

  it('should call reviewTimesheet()', () => {
    const review: TimesheetReviewRequest = { approved: true } as TimesheetReviewRequest;
    const mock: Timesheet = { id: 8 } as Timesheet;
    service.reviewTimesheet(8, review).subscribe(result => {
      expect(result).toEqual(mock);
    });
    const req = httpMock.expectOne(req => req.url.includes('/timesheets/8/review'));
    expect(req.request.method).toBe('PATCH');
    expect(req.request.body).toEqual(review);
    req.flush(mock);
  });

  it('should call getAllTimesheets()', () => {
    const mock: Timesheet[] = [{ id: 9 } as Timesheet];
    service.getAllTimesheets().subscribe(result => {
      expect(result).toEqual(mock);
    });
    const req = httpMock.expectOne(req => req.url.endsWith('/timesheets'));
    expect(req.request.method).toBe('GET');
    req.flush(mock);
  });

  it('should call getTimesheetsByProject()', () => {
    const mock: Timesheet[] = [{ id: 10 } as Timesheet];
    service.getTimesheetsByProject(11).subscribe(result => {
      expect(result).toEqual(mock);
    });
    const req = httpMock.expectOne(req => req.url.includes('/timesheets/project/11'));
    expect(req.request.method).toBe('GET');
    req.flush(mock);
  });
});
