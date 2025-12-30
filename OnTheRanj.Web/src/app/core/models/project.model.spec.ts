import { ProjectCode, CreateProjectRequest, UpdateProjectRequest } from './project.model';

describe('ProjectCode Model', () => {
  it('should create a valid ProjectCode object', () => {
    const project: ProjectCode = {
      id: 1,
      code: 'P001',
      projectName: 'Website Redesign',
      clientName: 'Acme Corp',
      isBillable: true,
      status: 'Active',
      createdAt: new Date('2025-01-01T10:00:00Z'),
      updatedAt: new Date('2025-01-02T10:00:00Z'),
      createdBy: 42
    };
    expect(project.id).toBe(1);
    expect(project.code).toBe('P001');
    expect(project.projectName).toBe('Website Redesign');
    expect(project.clientName).toBe('Acme Corp');
    expect(project.isBillable).toBe(true);
    expect(project.status).toBe('Active');
    expect(project.createdAt).toEqual(new Date('2025-01-01T10:00:00Z'));
    expect(project.updatedAt).toEqual(new Date('2025-01-02T10:00:00Z'));
    expect(project.createdBy).toBe(42);
  });

  it('should allow optional updatedAt', () => {
    const project: ProjectCode = {
      id: 2,
      code: 'P002',
      projectName: 'Mobile App',
      clientName: 'Beta Inc',
      isBillable: false,
      status: 'Inactive',
      createdAt: new Date('2025-02-01T10:00:00Z'),
      createdBy: 99
    };
    expect(project.updatedAt).toBeUndefined();
  });
});

describe('CreateProjectRequest Model', () => {
  it('should create a valid CreateProjectRequest object', () => {
    const req: CreateProjectRequest = {
      code: 'P003',
      projectName: 'API Integration',
      clientName: 'Gamma LLC',
      isBillable: true
    };
    expect(req.code).toBe('P003');
    expect(req.projectName).toBe('API Integration');
    expect(req.clientName).toBe('Gamma LLC');
    expect(req.isBillable).toBe(true);
  });
});

describe('UpdateProjectRequest Model', () => {
  it('should create a valid UpdateProjectRequest object', () => {
    const req: UpdateProjectRequest = {
      id: 3,
      code: 'P004',
      projectName: 'Cloud Migration',
      clientName: 'Delta Ltd',
      isBillable: false,
      status: 'Active'
    };
    expect(req.id).toBe(3);
    expect(req.code).toBe('P004');
    expect(req.projectName).toBe('Cloud Migration');
    expect(req.clientName).toBe('Delta Ltd');
    expect(req.isBillable).toBe(false);
  });
});
