import { projectReducer, initialState } from './project.reducer';
import * as ProjectActions from '../actions/project.actions';
import { ProjectCode } from '../../core/models/project.model';

describe('Project Reducer', () => {
  const project: ProjectCode = { id: 1, code: 'P1', projectName: 'Proj', clientName: 'C', isBillable: true, status: 'Active', createdAt: new Date(), createdBy: 1 };

  it('should return initial state', () => {
    expect(projectReducer(undefined, { type: '@@init' } as any)).toEqual(initialState);
  });

  it('should set loading true on loadProjects', () => {
    const state = projectReducer(initialState, ProjectActions.loadProjects());
    expect(state.loading).toBe(true);
    expect(state.error).toBeNull();
  });

  it('should set all projects on loadProjectsSuccess', () => {
    const state = projectReducer(initialState, ProjectActions.loadProjectsSuccess({ projects: [project] }));
    expect(state.ids).toContain(1);
    expect(state.entities[1]).toEqual(project);
    expect(state.loading).toBe(false);
    expect(state.error).toBeNull();
  });

  it('should set error on loadProjectsFailure', () => {
    const state = projectReducer(initialState, ProjectActions.loadProjectsFailure({ error: 'fail' }));
    expect(state.loading).toBe(false);
    expect(state.error).toBe('fail');
  });

  it('should set selectedProjectId on loadProjectSuccess', () => {
    const state = projectReducer(initialState, ProjectActions.loadProjectSuccess({ project }));
    expect(state.selectedProjectId).toBe(1);
    expect(state.entities[1]).toEqual(project);
    expect(state.loading).toBe(false);
    expect(state.error).toBeNull();
  });

  it('should add project on createProjectSuccess', () => {
    const state = projectReducer(initialState, ProjectActions.createProjectSuccess({ project }));
    expect(state.entities[1]).toEqual(project);
    expect(state.loading).toBe(false);
    expect(state.error).toBeNull();
  });

  it('should update project on updateProjectSuccess', () => {
    const updated = { ...project, projectName: 'New' };
    const state = projectReducer({ ...initialState, entities: { 1: project }, ids: [1] }, ProjectActions.updateProjectSuccess({ project: updated }));
    expect(state.entities[1]).toEqual(updated);
    expect(state.loading).toBe(false);
    expect(state.error).toBeNull();
  });

  it('should remove project on deleteProjectSuccess', () => {
    const prev = projectReducer(initialState, ProjectActions.createProjectSuccess({ project }));
    const state = projectReducer(prev, ProjectActions.deleteProjectSuccess({ id: 1 }));
    expect(state.entities[1]).toBeUndefined();
    expect(state.loading).toBe(false);
    expect(state.error).toBeNull();
  });
});
