import { TestBed } from '@angular/core/testing';
import { ProjectAssignmentService } from './project-assignment.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ProjectAssignment, CreateAssignmentRequest } from './project-assignment.service';

describe('ProjectAssignmentService', () => {
  let service: ProjectAssignmentService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ProjectAssignmentService]
    });
    service = TestBed.inject(ProjectAssignmentService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should call getActiveAssignments()', () => {
    const mockAssignments: ProjectAssignment[] = [{ id: 1, employeeId: 2, projectCodeId: 3, startDate: '2025-01-01', endDate: null }];
    service.getActiveAssignments(2).subscribe(assignments => {
      expect(assignments).toEqual(mockAssignments);
    });
    const req = httpMock.expectOne('/api/ProjectAssignments/employee/2/active');
    expect(req.request.method).toBe('GET');
    req.flush(mockAssignments);
  });

  it('should call getAll()', () => {
    const assignmentService = TestBed.inject(ProjectAssignmentService);
    const mockAssignments: ProjectAssignment[] = [{ id: 1 } as ProjectAssignment];
    assignmentService.getAll().subscribe(assignments => {
      expect(assignments).toEqual(mockAssignments);
    });
    const req = httpMock.expectOne('/api/ProjectAssignments');
    expect(req.request.method).toBe('GET');
    req.flush(mockAssignments);
  });

  it('should call getAllByEmployee()', () => {
    const assignmentService = TestBed.inject(ProjectAssignmentService);
    const mockAssignments: ProjectAssignment[] = [{ id: 2 } as ProjectAssignment];
    assignmentService.getAllByEmployee(5).subscribe(assignments => {
      expect(assignments).toEqual(mockAssignments);
    });
    const req = httpMock.expectOne('/api/ProjectAssignments/employee/5');
    expect(req.request.method).toBe('GET');
    req.flush(mockAssignments);
  });

  it('should call getAllByProject()', () => {
    const assignmentService = TestBed.inject(ProjectAssignmentService);
    const mockAssignments: ProjectAssignment[] = [{ id: 3 } as ProjectAssignment];
    assignmentService.getAllByProject(7).subscribe(assignments => {
      expect(assignments).toEqual(mockAssignments);
    });
    const req = httpMock.expectOne('/api/ProjectAssignments/project/7');
    expect(req.request.method).toBe('GET');
    req.flush(mockAssignments);
  });

  it('should call getById()', () => {
    const assignmentService = TestBed.inject(ProjectAssignmentService);
    const mockAssignment: ProjectAssignment = { id: 4 } as ProjectAssignment;
    assignmentService.getById(4).subscribe(assignment => {
      expect(assignment).toEqual(mockAssignment);
    });
    const req = httpMock.expectOne('/api/ProjectAssignments/4');
    expect(req.request.method).toBe('GET');
    req.flush(mockAssignment);
  });

  it('should call create()', () => {
    const assignmentService = TestBed.inject(ProjectAssignmentService);
    const request: CreateAssignmentRequest = { employeeId: 1, projectCodeId: 2 } as CreateAssignmentRequest;
    const mockAssignment: ProjectAssignment = { id: 5 } as ProjectAssignment;
    assignmentService.create(request).subscribe(assignment => {
      expect(assignment).toEqual(mockAssignment);
    });
    const req = httpMock.expectOne('/api/ProjectAssignments');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(request);
    req.flush(mockAssignment);
  });

  it('should call update()', () => {
    const assignmentService = TestBed.inject(ProjectAssignmentService);
    // Use string for date fields to match the expected type
    const update: Partial<ProjectAssignment> = { endDate: '2025-12-31' };
    const mockAssignment: ProjectAssignment = {
      id: 6,
      employeeId: 1,
      projectCodeId: 2,
      startDate: '2025-01-01',
      endDate: '2025-12-31',
      createdAt: '2025-01-01',
      createdBy: 1
    };
    assignmentService.update(6, update).subscribe(assignment => {
      expect(assignment).toEqual(mockAssignment);
    });
    const req = httpMock.expectOne('/api/ProjectAssignments/6');
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(update);
    req.flush(mockAssignment);
  });

  it('should call delete()', () => {
    const assignmentService = TestBed.inject(ProjectAssignmentService);
    assignmentService.delete(7).subscribe(result => {
      expect(result).toBeNull();
    });
    const req = httpMock.expectOne('/api/ProjectAssignments/7');
    expect(req.request.method).toBe('DELETE');
    req.flush(null);
  });
});
