import * as ProjectSelectors from './project.selectors';
import { ProjectState } from '../reducers/project.reducer';
import { ProjectCode } from '../../core/models/project.model';

describe('Project Selectors', () => {
  const project: ProjectCode = { id: 1, code: 'P1', projectName: 'Proj', clientName: 'C', isBillable: true, status: 'Active', createdAt: new Date(), createdBy: 1 };
  const state: { project: ProjectState } = {
    project: {
      ids: [1],
      entities: { 1: project },
      selectedProjectId: 1,
      loading: false,
      error: null
    }
  } as any;

  it('should select project state', () => {
    expect(ProjectSelectors.selectProjectState(state)).toEqual(state.project);
  });

  it('should select all projects', () => {
    expect(ProjectSelectors.selectProjects.projector(state.project)).toEqual([project]);
  });

  it('should select projects dictionary', () => {
    expect(ProjectSelectors.selectProjectsDictionary.projector(state.project)).toEqual({ 1: project });
  });

  it('should select selected project id', () => {
    expect(ProjectSelectors.selectSelectedProjectId.projector(state.project)).toBe(1);
  });

  it('should select selected project', () => {
    expect(ProjectSelectors.selectSelectedProject.projector({ 1: project }, 1)).toEqual(project);
    expect(ProjectSelectors.selectSelectedProject.projector({ 1: project }, null)).toBeNull();
  });

  it('should select loading', () => {
    expect(ProjectSelectors.selectProjectLoading.projector(state.project)).toBe(false);
  });

  it('should select error', () => {
    expect(ProjectSelectors.selectProjectError.projector(state.project)).toBeNull();
  });

  it('should select active projects', () => {
    expect(ProjectSelectors.selectActiveProjects.projector([project])).toEqual([project]);
  });

  it('should select billable projects', () => {
    expect(ProjectSelectors.selectBillableProjects.projector([project])).toEqual([project]);
  });

  it('should select project by id', () => {
    expect(ProjectSelectors.selectProjectById(1).projector({ 1: project })).toEqual(project);
  });
});
