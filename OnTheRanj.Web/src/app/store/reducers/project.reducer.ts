import { createReducer, on } from '@ngrx/store';
import { EntityState, EntityAdapter, createEntityAdapter } from '@ngrx/entity';
import { ProjectCode } from '../../core/models/project.model';
import * as ProjectActions from '../actions/project.actions';

/**
 * Project state interface using NgRx Entity
 */
export interface ProjectState extends EntityState<ProjectCode> {
  selectedProjectId: number | null;
  loading: boolean;
  error: string | null;
}

/**
 * Entity adapter for project management
 */
export const projectAdapter: EntityAdapter<ProjectCode> = createEntityAdapter<ProjectCode>();

/**
 * Initial project state
 */
export const initialState: ProjectState = projectAdapter.getInitialState({
  selectedProjectId: null,
  loading: false,
  error: null
});

/**
 * Project reducer
 */
export const projectReducer = createReducer(
  initialState,

  // Load all projects
  on(ProjectActions.loadProjects, (state) => ({
    ...state,
    loading: true,
    error: null
  })),

  on(ProjectActions.loadProjectsSuccess, (state, { projects }) =>
    projectAdapter.setAll(projects, {
      ...state,
      loading: false,
      error: null
    })
  ),

  on(ProjectActions.loadProjectsFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  })),

  // Load single project
  on(ProjectActions.loadProject, (state) => ({
    ...state,
    loading: true,
    error: null
  })),

  on(ProjectActions.loadProjectSuccess, (state, { project }) =>
    projectAdapter.upsertOne(project, {
      ...state,
      selectedProjectId: project.id,
      loading: false,
      error: null
    })
  ),

  on(ProjectActions.loadProjectFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  })),

  // Create project
  on(ProjectActions.createProject, (state) => ({
    ...state,
    loading: true,
    error: null
  })),

  on(ProjectActions.createProjectSuccess, (state, { project }) =>
    projectAdapter.addOne(project, {
      ...state,
      loading: false,
      error: null
    })
  ),

  on(ProjectActions.createProjectFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  })),

  // Update project
  on(ProjectActions.updateProject, (state) => ({
    ...state,
    loading: true,
    error: null
  })),

  on(ProjectActions.updateProjectSuccess, (state, { project }) =>
    projectAdapter.updateOne(
      { id: project.id, changes: project },
      {
        ...state,
        loading: false,
        error: null
      }
    )
  ),

  on(ProjectActions.updateProjectFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  })),

  // Delete project
  on(ProjectActions.deleteProject, (state) => ({
    ...state,
    loading: true,
    error: null
  })),

  on(ProjectActions.deleteProjectSuccess, (state, { id }) =>
    projectAdapter.removeOne(id, {
      ...state,
      loading: false,
      error: null
    })
  ),

  on(ProjectActions.deleteProjectFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  }))
);

/**
 * Export entity selectors
 */
export const {
  selectIds: selectProjectIds,
  selectEntities: selectProjectEntities,
  selectAll: selectAllProjects,
  selectTotal: selectProjectTotal
} = projectAdapter.getSelectors();
