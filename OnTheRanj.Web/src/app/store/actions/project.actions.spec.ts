  it('should create loadProjectsSuccess action with empty array', () => {
    const projects: ProjectCode[] = [];
    const action = ProjectActions.loadProjectsSuccess({ projects });
    expect(action.type).toBe('[Project] Load Projects Success');
    expect(action.projects).toEqual([]);
  });

  it('should create loadProjectsFailure action with empty error', () => {
    const error = '';
    const action = ProjectActions.loadProjectsFailure({ error });
    expect(action.type).toBe('[Project] Load Projects Failure');
    expect(action.error).toBe('');
  });

  it('should create loadProjectFailure action with undefined error', () => {
    const action = ProjectActions.loadProjectFailure({ error: undefined as any });
    expect(action.type).toBe('[Project] Load Project Failure');
    expect(action.error).toBeUndefined();
  });

  it('should create createProjectFailure action with null error', () => {
    const action = ProjectActions.createProjectFailure({ error: null as any });
    expect(action.type).toBe('[Project] Create Project Failure');
    expect(action.error).toBeNull();
  });

  it('should create updateProjectFailure action with empty error', () => {
    const action = ProjectActions.updateProjectFailure({ error: '' });
    expect(action.type).toBe('[Project] Update Project Failure');
    expect(action.error).toBe('');
  });

  it('should create deleteProjectFailure action with undefined error', () => {
    const action = ProjectActions.deleteProjectFailure({ error: undefined as any });
    expect(action.type).toBe('[Project] Delete Project Failure');
    expect(action.error).toBeUndefined();
  });
import * as ProjectActions from './project.actions';
import { ProjectCode, CreateProjectRequest, UpdateProjectRequest } from '../../core/models/project.model';

describe('Project Actions', () => {
  it('should create loadProjects action', () => {
    const action = ProjectActions.loadProjects();
    expect(action.type).toBe('[Project] Load Projects');
  });

  it('should create loadProjectsSuccess action', () => {
    const projects: ProjectCode[] = [{ id: 1, code: 'P1', projectName: 'Proj', clientName: 'C', isBillable: true, status: 'Active', createdAt: new Date(), createdBy: 1 }];
    const action = ProjectActions.loadProjectsSuccess({ projects });
    expect(action.type).toBe('[Project] Load Projects Success');
    expect(action.projects).toEqual(projects);
  });

  it('should create loadProjectsFailure action', () => {
    const error = 'Failed';
    const action = ProjectActions.loadProjectsFailure({ error });
    expect(action.type).toBe('[Project] Load Projects Failure');
    expect(action.error).toBe(error);
  });

  it('should create loadProject action', () => {
    const action = ProjectActions.loadProject({ id: 1 });
    expect(action.type).toBe('[Project] Load Project');
    expect(action.id).toBe(1);
  });

  it('should create loadProjectSuccess action', () => {
    const project: ProjectCode = { id: 1, code: 'P1', projectName: 'Proj', clientName: 'C', isBillable: true, status: 'Active', createdAt: new Date(), createdBy: 1 };
    const action = ProjectActions.loadProjectSuccess({ project });
    expect(action.type).toBe('[Project] Load Project Success');
    expect(action.project).toEqual(project);
  });

  it('should create loadProjectFailure action', () => {
    const error = 'Failed';
    const action = ProjectActions.loadProjectFailure({ error });
    expect(action.type).toBe('[Project] Load Project Failure');
    expect(action.error).toBe(error);
  });

  it('should create createProject action', () => {
    const request: CreateProjectRequest = { code: 'P1', projectName: 'Proj', clientName: 'C', isBillable: true };
    const action = ProjectActions.createProject({ request });
    expect(action.type).toBe('[Project] Create Project');
    expect(action.request).toEqual(request);
  });

  it('should create createProjectSuccess action', () => {
    const project: ProjectCode = { id: 1, code: 'P1', projectName: 'Proj', clientName: 'C', isBillable: true, status: 'Active', createdAt: new Date(), createdBy: 1 };
    const action = ProjectActions.createProjectSuccess({ project });
    expect(action.type).toBe('[Project] Create Project Success');
    expect(action.project).toEqual(project);
  });

  it('should create createProjectFailure action', () => {
    const error = 'Failed';
    const action = ProjectActions.createProjectFailure({ error });
    expect(action.type).toBe('[Project] Create Project Failure');
    expect(action.error).toBe(error);
  });

  it('should create updateProject action', () => {
    const request: UpdateProjectRequest = { id: 2, code: 'P2', projectName: 'New', clientName: 'C', isBillable: false, status: 'Inactive' };
    const action = ProjectActions.updateProject({ id: 2, request });
    expect(action.type).toBe('[Project] Update Project');
    expect(action.id).toBe(2);
    expect(action.request).toEqual(request);
  });

  it('should create updateProjectSuccess action', () => {
    const project: ProjectCode = { id: 2, code: 'P2', projectName: 'New', clientName: 'C', isBillable: false, status: 'Inactive', createdAt: new Date(), createdBy: 1 };
    const action = ProjectActions.updateProjectSuccess({ project });
    expect(action.type).toBe('[Project] Update Project Success');
    expect(action.project).toEqual(project);
  });

  it('should create updateProjectFailure action', () => {
    const error = 'Failed';
    const action = ProjectActions.updateProjectFailure({ error });
    expect(action.type).toBe('[Project] Update Project Failure');
    expect(action.error).toBe(error);
  });

  it('should create deleteProject action', () => {
    const action = ProjectActions.deleteProject({ id: 3 });
    expect(action.type).toBe('[Project] Delete Project');
    expect(action.id).toBe(3);
  });

  it('should create deleteProjectSuccess action', () => {
    const action = ProjectActions.deleteProjectSuccess({ id: 3 });
    expect(action.type).toBe('[Project] Delete Project Success');
    expect(action.id).toBe(3);
  });

  it('should create deleteProjectFailure action', () => {
    const error = 'Failed';
    const action = ProjectActions.deleteProjectFailure({ error });
    expect(action.type).toBe('[Project] Delete Project Failure');
    expect(action.error).toBe(error);
  });
});
