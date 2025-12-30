import { ProjectAssignment, CreateAssignmentRequest } from './project-assignment.model';

describe('ProjectAssignment Model', () => {
  it('should create a valid ProjectAssignment object', () => {
    const assignment: ProjectAssignment = {
      id: 1,
      employeeId: 2,
      projectCodeId: 3,
      startDate: new Date('2025-01-01'),
      endDate: new Date('2025-12-31'),
      createdAt: new Date('2025-01-01T10:00:00Z'),
      createdBy: 99,
      employee: undefined,
      projectCode: undefined
    };
    expect(assignment.id).toBe(1);
    expect(assignment.employeeId).toBe(2);
    expect(assignment.projectCodeId).toBe(3);
    expect(assignment.startDate).toEqual(new Date('2025-01-01'));
    expect(assignment.endDate).toEqual(new Date('2025-12-31'));
    expect(assignment.createdAt).toEqual(new Date('2025-01-01T10:00:00Z'));
    expect(assignment.createdBy).toBe(99);
    expect(assignment.employee).toBeUndefined();
    expect(assignment.projectCode).toBeUndefined();
  });

  it('should allow optional endDate, employee, and projectCode', () => {
    const assignment: ProjectAssignment = {
      id: 2,
      employeeId: 3,
      projectCodeId: 4,
      startDate: new Date('2025-02-01'),
      createdAt: new Date('2025-02-01T10:00:00Z'),
      createdBy: 100
    };
    expect(assignment.endDate).toBeUndefined();
    expect(assignment.employee).toBeUndefined();
    expect(assignment.projectCode).toBeUndefined();
  });
});


describe('CreateAssignmentRequest Model', () => {
  it('should create a valid CreateAssignmentRequest object', () => {
    const req: CreateAssignmentRequest = {
      employeeId: 5,
      projectCodeId: 6,
      startDate: '2025-03-01',
      endDate: '2025-03-31'
    };
    expect(req.employeeId).toBe(5);
    expect(req.projectCodeId).toBe(6);
    expect(req.startDate).toBe('2025-03-01');
    expect(req.endDate).toBe('2025-03-31');
  });

  it('should allow optional endDate', () => {
    const req: CreateAssignmentRequest = {
      employeeId: 7,
      projectCodeId: 8,
      startDate: '2025-04-01'
    };
    expect(req.endDate).toBeUndefined();
  });
});
