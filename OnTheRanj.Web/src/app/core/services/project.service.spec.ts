import { TestBed } from '@angular/core/testing';
import { ProjectService } from './project.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ProjectCode, CreateProjectRequest, UpdateProjectRequest } from '../models/project.model';

describe('ProjectService', () => {
  let service: ProjectService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ProjectService]
    });
    service = TestBed.inject(ProjectService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should call getAllProjects()', () => {
    const mockProjects: ProjectCode[] = [{ id: 1, code: 'P001', projectName: 'Test', clientName: 'Client', isBillable: true, status: 'Active', createdAt: new Date(), createdBy: 1 } as ProjectCode];
    service.getAllProjects().subscribe(projects => {
      expect(projects).toEqual(mockProjects);
    });
    const req = httpMock.expectOne(req => req.url.endsWith('/projects'));
    expect(req.request.method).toBe('GET');
    req.flush(mockProjects);
  });

  it('should call getProjectById()', () => {
    const mockProject: ProjectCode = { id: 2, code: 'P002', projectName: 'Test2', clientName: 'Client2', isBillable: false, status: 'Inactive', createdAt: new Date(), createdBy: 2 } as ProjectCode;
    service.getProjectById(2).subscribe(project => {
      expect(project).toEqual(mockProject);
    });
    const req = httpMock.expectOne(req => req.url.endsWith('/projects/2'));
    expect(req.request.method).toBe('GET');
    req.flush(mockProject);
  });

  it('should call createProject()', () => {
    const request: CreateProjectRequest = { code: 'P003', projectName: 'Test3', clientName: 'Client3', isBillable: true } as CreateProjectRequest;
    const mockProject: ProjectCode = { id: 3, code: 'P003', projectName: 'Test3', clientName: 'Client3', isBillable: true, status: 'Active', createdAt: new Date(), createdBy: 3 } as ProjectCode;
    service.createProject(request).subscribe(project => {
      expect(project).toEqual(mockProject);
    });
    const req = httpMock.expectOne(req => req.url.endsWith('/projects'));
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(request);
    req.flush(mockProject);
  });

  it('should call updateProject()', () => {
    const request: UpdateProjectRequest = { projectName: 'Updated' } as UpdateProjectRequest;
    const mockProject: ProjectCode = { id: 4, code: 'P004', projectName: 'Updated', clientName: 'Client4', isBillable: false, status: 'Inactive', createdAt: new Date(), createdBy: 4 } as ProjectCode;
    service.updateProject(4, request).subscribe(project => {
      expect(project).toEqual(mockProject);
    });
    const req = httpMock.expectOne(req => req.url.endsWith('/projects/4'));
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(request);
    req.flush(mockProject);
  });

  it('should call deactivateProject()', () => {
    service.deactivateProject(5).subscribe(result => {
      expect(result).toBeNull();
    });
    const req = httpMock.expectOne(req => req.url.endsWith('/projects/5'));
    expect(req.request.method).toBe('DELETE');
    req.flush(null);
  });

  it('should call getBillableProjects()', () => {
    const mockProjects: ProjectCode[] = [{ id: 6, code: 'P006', projectName: 'Billable', clientName: 'Client6', isBillable: true, status: 'Active', createdAt: new Date(), createdBy: 6 } as ProjectCode];
    service.getBillableProjects().subscribe(projects => {
      expect(projects).toEqual(mockProjects);
    });
    const req = httpMock.expectOne(req => req.url.endsWith('/projects/billable'));
    expect(req.request.method).toBe('GET');
    req.flush(mockProjects);
  });
});
