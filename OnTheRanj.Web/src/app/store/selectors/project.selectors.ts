import { createFeatureSelector, createSelector } from '@ngrx/store';
import { ProjectState, selectAllProjects, selectProjectEntities } from '../reducers/project.reducer';

/**
 * Project selectors for accessing project state
 */

// Feature selector
export const selectProjectState = createFeatureSelector<ProjectState>('project');

// Select all projects using entity adapter selector
export const selectProjects = createSelector(
  selectProjectState,
  selectAllProjects
);

// Select project entities
export const selectProjectsDictionary = createSelector(
  selectProjectState,
  selectProjectEntities
);

// Select selected project ID
export const selectSelectedProjectId = createSelector(
  selectProjectState,
  (state: ProjectState) => state.selectedProjectId
);

// Select selected project
export const selectSelectedProject = createSelector(
  selectProjectsDictionary,
  selectSelectedProjectId,
  (entities, selectedId) => (selectedId ? entities[selectedId] : null)
);

// Select loading state
export const selectProjectLoading = createSelector(
  selectProjectState,
  (state: ProjectState) => state.loading
);

// Select error
export const selectProjectError = createSelector(
  selectProjectState,
  (state: ProjectState) => state.error
);

// Select active projects
export const selectActiveProjects = createSelector(
  selectProjects,
  (projects) => projects.filter(project => project.status === 'Active')
);

// Select billable projects
export const selectBillableProjects = createSelector(
  selectActiveProjects,
  (projects) => projects.filter(project => project.isBillable)
);

// Select project by ID
export const selectProjectById = (id: number) =>
  createSelector(
    selectProjectsDictionary,
    (entities) => entities[id]
  );
