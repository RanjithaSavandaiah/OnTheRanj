import { TestBed } from '@angular/core/testing';
import { ReportService } from './report.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

describe('ReportService', () => {
  let service: ReportService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ReportService]
    });
    service = TestBed.inject(ReportService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should call getEmployeeHoursSummary()', () => {
    const mock = [{ employeeId: 1, totalHours: 40 }];
    const start = new Date('2025-01-01');
    const end = new Date('2025-01-31');
    service.getEmployeeHoursSummary(start, end).subscribe(result => {
      expect(result).toEqual(mock);
    });
    const req = httpMock.expectOne(r => r.url.includes('/reports/employee-hours'));
    expect(req.request.method).toBe('GET');
    req.flush(mock);
  });

  it('should call getProjectHoursSummary()', () => {
    const mock = [{ projectId: 1, totalHours: 100 }];
    const start = new Date('2025-01-01');
    const end = new Date('2025-01-31');
    service.getProjectHoursSummary(start, end).subscribe(result => {
      expect(result).toEqual(mock);
    });
    const req = httpMock.expectOne(r => r.url.includes('/reports/project-hours'));
    expect(req.request.method).toBe('GET');
    req.flush(mock);
  });

  it('should call getBillableHoursSummary()', () => {
    const mock = { billable: 80, nonBillable: 20 };
    const start = new Date('2025-01-01');
    const end = new Date('2025-01-31');
    service.getBillableHoursSummary(start, end).subscribe(result => {
      expect(result).toEqual(mock);
    });
    const req = httpMock.expectOne(r => r.url.includes('/reports/billable-hours'));
    expect(req.request.method).toBe('GET');
    req.flush(mock);
  });

  it('should call getEmployeeDetailReport()', () => {
    const mock = { employeeId: 2, totalHours: 50 };
    const start = new Date('2025-01-01');
    const end = new Date('2025-01-31');
    service.getEmployeeDetailReport(2, start, end).subscribe(result => {
      expect(result).toEqual(mock);
    });
    const req = httpMock.expectOne(r => r.url.includes('/reports/employee/2'));
    expect(req.request.method).toBe('GET');
    req.flush(mock);
  });

  it('should call exportReport()', () => {
    const mockBlob = new Blob(['test'], { type: 'application/vnd.ms-excel' });
    const start = new Date('2025-01-01');
    const end = new Date('2025-01-31');
    service.exportReport('employee', start, end).subscribe(result => {
      expect(result).toEqual(mockBlob);
    });
    const req = httpMock.expectOne(r => r.url.includes('/reports/export/employee'));
    expect(req.request.method).toBe('GET');
    expect(req.request.responseType).toBe('blob');
    req.flush(mockBlob);
  });

  it('should call getDashboardSummary()', () => {
    const mock = {
      totalHours: 128,
      billableHours: 90,
      nonBillableHours: 38,
      topProjects: [],
      topEmployees: []
    };
    service.getDashboardSummary().subscribe(result => {
      expect(result.totalHours).toBe(128);
      expect(result.billableHours).toBe(90);
      expect(result.nonBillableHours).toBe(38);
      expect(Array.isArray(result.topProjects)).toBe(true);
      expect(Array.isArray(result.topEmployees)).toBe(true);
    });
    const req = httpMock.expectOne(r => r.url.includes('/dashboard-summary'));
    expect(req.request.method).toBe('GET');
    req.flush(mock);
  });
});
