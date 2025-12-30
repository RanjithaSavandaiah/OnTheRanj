import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { TimesheetWeekService, TimesheetWeekSubmitRequest } from './timesheet-week.service';


describe('TimesheetWeekService', () => {
  let service: TimesheetWeekService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [TimesheetWeekService]
    });
    service = TestBed.inject(TimesheetWeekService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should POST week submission to API', () => {
    const request: TimesheetWeekSubmitRequest = {
      employeeId: 1,
      entries: [
        { projectCodeId: 2, date: '2025-12-29', hoursWorked: 8, description: 'Worked on feature X' }
      ]
    };
    const mockResponse = { success: true };

    service.submitWeek(request).subscribe(res => {
      expect(res).toEqual(mockResponse);
    });

    const req = httpMock.expectOne('/api/timesheets/week');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(request);
    req.flush(mockResponse);
  });
});
