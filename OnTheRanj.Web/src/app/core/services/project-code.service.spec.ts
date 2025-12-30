import { TestBed } from '@angular/core/testing';
import { ProjectCodeService } from './project-code.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ProjectCode } from '../models/project.model';

describe('ProjectCodeService', () => {
  let service: ProjectCodeService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ProjectCodeService]
    });
    service = TestBed.inject(ProjectCodeService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should call getAll()', () => {
    const mockCodes: ProjectCode[] = [{ id: 1, code: 'P001', projectName: 'Test', clientName: 'Client', isBillable: true, status: 'Active', createdAt: new Date(), createdBy: 1 } as ProjectCode];
    service.getAll().subscribe(codes => {
      expect(codes).toEqual(mockCodes);
    });
    const req = httpMock.expectOne('/api/Projects');
    expect(req.request.method).toBe('GET');
    req.flush(mockCodes);
  });
});
